using System;
using System.Collections.Generic;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Runtime;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime
{
    [ExecuteInEditMode]
    [RequireComponent( typeof( NoiseModule ) )]
    public class ObjectStack : PrefabSpawnerRoot, IObjectStack
    {
#region DataMembers

        private const float NoisePosShiftIncreaseFactor = 100f;
        private const int NoiseMapWidth = 1;

        [SerializeField] [Delayed] private int totalObjects = 52;

        // The list that hold the noise transform effects for all positions in the stack.
        [HideInInspector] [SerializeField] private List<PseudoTransform> objectStack = new List<PseudoTransform>();
        [SerializeField] private ObjectStackerSettingsSO objectStackerSettingsSO;
        [SerializeField] private SceneViewDebugVisualsMode sceneViewVisualsMode = SceneViewDebugVisualsMode.Off;
        [SerializeField] private NoiseMapDimensions mapGenerationMode = NoiseMapDimensions.OneDimensional;
        [SerializeField] private GameObject prefab;
        [SerializeField] private FormattedLogger logger;

        /// <summary>
        ///     As changes in settings are handled by an event subscribed to the settings so, the only reason this class should<br/>
        ///     update the stack is when the object total is changed. The var should be set to true when that occurs and set to<br/>
        ///     false after the update is performed. This is an optimization to avoid recreating the stack every frame.
        /// </summary>
        private bool _updateRequired;

        private NoiseMapDimensions _previousNoiseMapDimensions;

        private int _currentStackPosition;

        // SOs and subscriptions
        private ObjectStackerSettingsSO _previousObjectStackerSettingsSo;

        // Conversion to Noise Module
        private INoiseSource _noiseModule;

#endregion


#region PrefabSpawnerOverrides

        public override string GetPrefabVarName() => nameof( prefab );
        public override string GetSpawnPointsVarName() => nameof( objectStack );

#endregion


#region Interface

        public PseudoTransform GetNextPosition()
        {
            if( _currentStackPosition >= objectStack.Count )
                throw new ArgumentOutOfRangeException( nameof( objectStack ), "Stack is full!" );

            return objectStack[_currentStackPosition++];
        }

        public int GetCurrentStackPosition() => _currentStackPosition;
        public void ResetStackCounter() => _currentStackPosition = 0;

#endregion


#region LifeCycle

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            logger.LogStart( true );

            Initialize();
            UpdateSubscriptions();
            UpdateStack();

            logger.LogEnd();
        }

        private void OnDisable()
        {
            logger.LogStart( true );

            UpdateSubscriptions();

            logger.LogEnd();
        }

        private void OnValidate()
        {
            _updateRequired = true;
            UpdateSubscriptions();
            UpdateStack();
        }

#endregion


#region EventMethods

        private void Initialize()
        {
            // Load the noise module attached to this object.
            _noiseModule = GetComponent<INoiseSource>();
            logger.LogObjectAssignmentResult( nameof( _noiseModule ), _noiseModule == null, FormattedLogType.Standard );

            int mapWidth = mapGenerationMode == NoiseMapDimensions.TwoDimensional ? totalObjects : NoiseMapWidth;
            _noiseModule.Initialize( mapWidth, totalObjects, OnNoiseModuleUpdated, OnSettingsSOUpdated );
        }

        private void OnNoiseModuleUpdated()
        {
            logger.LogStart();

            // Update the subscriptions.
            UpdateSubscriptions();
            UpdateStackTransforms();

            logger.LogEnd();
        }

        private void UpdateSubscriptions()
        {
            _previousObjectStackerSettingsSo = (ObjectStackerSettingsSO) SubscriptionsHandler.UpdateSubscriptions
            (
                _previousObjectStackerSettingsSo,
                objectStackerSettingsSO,
                OnSettingsSOUpdated,
                logger
            );
        }

        /// <summary>
        ///     This method should only be called in response to a change in the objectStackerSettingsSO's settings. That is triggered<br/>
        ///     in the settingsSO class's OnValidate() method.
        /// </summary>
        private void OnSettingsSOUpdated()
        {
            // logger.LogStart( true );

            UpdateStackTransforms();

            // logger.LogEnd();
        }

#endregion


#region StackManagement

        /// <summary>
        ///     Checks to see if the stack size is still correct. If not, it rebuilds the stack and regenerates<br/>
        ///     the noise map and pseudoTransforms. This should only be called when the stack size variable has been changed.
        /// </summary>
        private void UpdateStack()
        {
            logger.LogStart();

            ReevaluateStack();
            if( !_updateRequired &&
                !NoiseMapDimensionsHaveChanged() )
            {
                logger.LogEnd( "No update required." );
                return;
            }

            UpdateStackTransforms();
            _updateRequired = false;

            logger.LogEnd();
        }

        /// <summary>
        ///     Rebuilds the noise map and the updates the pseudoTransforms. This should be called any time a stack or
        ///     noise setting has been changed, which will occur either in this class's OnValidate or in response to
        ///     the settings SO's event being raised when its OnValidate is called.
        /// </summary>
        private void UpdateStackTransforms()
        {
            if( _noiseModule == null )
            {
                logger.Log( "Warning! Noise module was null!", FormattedLogType.Warning );
                return;
            }

            // GenerateNoiseMap();
            int mapWidth = mapGenerationMode == NoiseMapDimensions.TwoDimensional ? totalObjects : NoiseMapWidth;
            _noiseModule.UpdateNoiseMapSize( mapWidth, totalObjects );
            UpdatePseudoTransforms();
        }

#endregion


#region StackGeneration

        private bool NoiseMapDimensionsHaveChanged()
        {
            if( _previousNoiseMapDimensions == mapGenerationMode ) return false;

            _previousNoiseMapDimensions = mapGenerationMode;
            return true;
        }

        /// <summary>
        ///     Ensure stack size is correct, otherwise shrink or expand to appropriate size.
        /// </summary>
        private void ReevaluateStack()
        {
            logger.LogStart( $"stack.count: {GetColoredStringYellow( objectStack.Count.ToString() )}, " +
                             $"objectCount: {GetColoredStringYellow( totalObjects.ToString() )}." );
            if( objectStack.Count == totalObjects )
            {
                logger.LogEnd( "Object stack is at the correct size." );
                _updateRequired = false;
                return;
            }

            // If the stack is too big, remove the extra entries from the end of the list.
            if( objectStack.Count > totalObjects )
            {
                int extras = objectStack.Count - totalObjects;
                for ( int i = extras - 1; i >= 0; i-- )
                {
                    objectStack.RemoveAt( i );
                }

                logger.LogEnd( $"Removed {GetColoredStringMaroon( extras.ToString() )} objects." );
                return;
            }

            // If the stack is too small, add the missing entries.
            int deficit = totalObjects - objectStack.Count;
            for ( int i = 0; i < deficit; i++ )
            {
                objectStack.Add( new PseudoTransform() );
            }

            logger.LogEnd( $"Added {GetColoredStringGreenYellow( deficit.ToString() )} objects." );
        }

        private void UpdatePseudoTransforms()
        {
            // This is required when a noise module update is triggered when no object stacker settings SO is set.
            if( objectStackerSettingsSO == null ) return;

            // logger.LogStart();

            float currentOffset = 0f;

            float firstObjectYRot = 0;
            float firstObjectXShift = 0;
            float firstObjectYShift = 0;

            if( objectStackerSettingsSO.lockBottomObject )
            {
                firstObjectYRot = GetNoiseDrivenRotationValue( 0 );
                firstObjectXShift = GetNoiseDrivePositionValue( 0, objectStackerSettingsSO.posXNoiseCurve, objectStackerSettingsSO.posXNoiseShift ) / NoisePosShiftIncreaseFactor;
                firstObjectYShift = GetNoiseDrivePositionValue( 0, objectStackerSettingsSO.posZNoiseCurve, objectStackerSettingsSO.posZNoiseShift ) / NoisePosShiftIncreaseFactor;
            }


            // Flip the z axis if face up is checked.
            float zAxisRot = objectStackerSettingsSO.faceUp ? 0f : 180f;

            // logger.LogIndentStart( "Getting noise values:" );
            for ( int i = 0; i < totalObjects; i++ )
            {
                PseudoTransform pseudoTransform = objectStack[i];
                var noisePosShift = new Vector3(
                    GetNoisePositionShiftValue( i, objectStackerSettingsSO.posXNoiseCurve, objectStackerSettingsSO.posXNoiseShift, firstObjectXShift ),
                    currentOffset,
                    GetNoisePositionShiftValue( i, objectStackerSettingsSO.posZNoiseCurve, objectStackerSettingsSO.posZNoiseShift, firstObjectYShift )
                );
                pseudoTransform.position = transform.position + noisePosShift;
                pseudoTransform.rotation = Quaternion.Euler( 0, GetNoiseDrivenRotationValue( i ) - firstObjectYRot, zAxisRot );
                pseudoTransform.scale = Vector3.one;

                currentOffset += objectStackerSettingsSO.modelHeight + objectStackerSettingsSO.verticalOffset;
            }

            // logger.DecrementMethodIndent();
            // logger.LogEnd();
        }


        private float GetDeckYRotSkew( int i ) => i * objectStackerSettingsSO.deckYRotSkew;

        /// <summary>
        ///     Applies a relative rotation to the top object group. The group size is determined by the topDownSkewPercent<br/>
        ///     which represents the percent of object, starting from the top, that will be in the group. The topObjectsYRotSkew<br/>
        ///     and the dampening curve are then applied to that group only.
        /// </summary>
        private float GetTopObjectsYRotSkew( int i )
        {
            // The threshold index represents the point where the group starts. All objects above this index are in the group.
            // Note that the topDownSkewPercent is inverted to allow the UI slider to increase from left to right.
            int thresholdIndex = Mathf.RoundToInt( ( 1 - objectStackerSettingsSO.topDownSkewPercent ) * totalObjects );

            // The clamp is just a failsafe to ensure the rounding doesn't result in one digit to high or low.
            thresholdIndex = Mathf.Clamp( thresholdIndex, 0, totalObjects );

            // Filter out all objects below the threshold index.
            if( i < thresholdIndex ) return 0f;

            // Get the index relative to this group.
            int relativeIndex = i - thresholdIndex;

            // Get the group size.
            int totalObjectsInTop = totalObjects - thresholdIndex;

            // Use the relative index and the group size to determine what percent of the group has been traversed.
            float percentOfTopObjectsTraversed = relativeIndex / (float) totalObjectsInTop;

            // Use the percent to get the value on the dampening curve and multiply it by the topObjectsYRotSkew setting.
            return objectStackerSettingsSO.rotationDampeningCurve.Evaluate( percentOfTopObjectsTraversed ) * objectStackerSettingsSO.topObjectsYRotSkew;
        }

#endregion


#region Noise

        private float GetNoisePositionShiftValue
            ( int i, AnimationCurve noiseCurve, float posShift, float firstObjectShiftValue ) =>
            ( GetNoiseDrivePositionValue( i, noiseCurve, posShift ) / NoisePosShiftIncreaseFactor ) - firstObjectShiftValue;

        private float GetNoiseDrivePositionValue( int i, AnimationCurve curve, float axisNoise ) => GetCurveDampenedNoiseValueAtIndex( i, curve ) * axisNoise;

        private float GetNoiseDrivenRotationValue( int i )
        {
            // Get manual y rotation skews.
            float ySkew = GetDeckYRotSkew( i );
            float topObjectsYSkew = GetTopObjectsYRotSkew( i );

            // Get noise rotation skew.
            float noiseValue = GetCurveDampenedNoiseValueAtIndex( i, objectStackerSettingsSO.noiseDampeningCurve ) * objectStackerSettingsSO.noiseMultiplier;

            // Return combined y rotation adjustment.
            return noiseValue + ySkew + topObjectsYSkew + objectStackerSettingsSO.yRotOffset;
        }

        private float GetCurveDampenedNoiseValueAtIndex( int i, AnimationCurve curve )
        {
            // Get the percent of the deck traversed so far and use the percentage to get a sample from the curve.
            float percentProgress = i / (float) totalObjects;
            float noiseCurveValue = curve.Evaluate( percentProgress );

            // Re-scope the noise sample from [0,1] to [-1,1] (This effectively centers the noise effect) then apply the curve value.
            return _noiseModule.GetNoiseAtIndex( 0, i ).Convert0ToMaxToNegMaxToPosMax() * noiseCurveValue;
        }

#endregion


#region Debug

        // Draws the noise-colorized wire frame object previews.
        private void OnDrawGizmosSelected()
        {
            if( sceneViewVisualsMode != SceneViewDebugVisualsMode.WireFrames ) return;

            if( _noiseModule?.Get2DNoiseMap() == null ) return;

            Matrix4x4 cachedMatrix = Gizmos.matrix;
            for ( int i = 0; i < objectStack.Count; i++ )
            {
                PseudoTransform pseudoTransform = objectStack[i];

                Matrix4x4 matrix = Matrix4x4.TRS( pseudoTransform.position, pseudoTransform.rotation, Vector3.one );
                Gizmos.matrix = matrix;

                Gizmos.color = Color.cyan;
                Vector3 parentsPosition = transform.position;
                Gizmos.DrawSphere( parentsPosition, 0.0002f );

                float noiseSample = _noiseModule.GetNoiseAtIndex( 0, i );

                // Note that we don't need to use the PseudoTransform's position here because we've already transformed the Gizmos' space.
                Gizmos.color = new Color( 1f, noiseSample, noiseSample );
                Gizmos.DrawWireCube( parentsPosition, new Vector3( 0.064f, 0.0002f, 0.0889f ) );
            }

            Gizmos.matrix = cachedMatrix;
        }

#endregion
    }
}