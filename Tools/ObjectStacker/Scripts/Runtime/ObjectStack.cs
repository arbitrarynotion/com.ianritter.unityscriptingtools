using System;
using System.Collections.Generic;
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
    public class ObjectStack : PrefabSpawnerRoot, IObjectStack
    {
#region DataMembers
        [SerializeField] [Delayed] private int totalObjects = 3;

        // The list that hold the noise transform effects for all positions in the stack.
        [HideInInspector] [SerializeField] private List<PseudoTransform> objectStack = new List<PseudoTransform>();

        [SerializeField] private ObjectStackerSettingsSO settingsSo;
        [SerializeField] private NoiseSettingsSO noiseSettingsSO;

        [SerializeField] private SceneViewDebugVisualsMode sceneViewVisualsMode = SceneViewDebugVisualsMode.Off;
        [SerializeField] private GameObject prefab;
        [SerializeField] private FormattedLogger logger;

        private int _currentStackPosition;

        [SerializeField] [HideInInspector] private float[,] noiseMap2D;
        private const int NoiseMapWidth = 1;

        /// <summary>
        ///     As changes in settings are handled by an event subscribed to the settings so, the only reason this class should<br/>
        ///     update the stack is when the object total is changed. The var should be set to true when that occurs and set to<br/>
        ///     false after the update is performed. This is an optimization to avoid recreating the stack every frame.
        /// </summary>
        private bool _updateRequired;
        private ObjectStackerSettingsSO _previousSettingsSo;
        private const float NoisePosShiftIncreaseFactor = 100f;
        // private SubscriptionsHandler _subscriptionsHandler;
#endregion


#region PrefabSpawnerOverrides

        public override string GetPrefabVarName()
        {
            logger.Log( "GetPrefabVarName was called." );
            return nameof( prefab );
        }

        public override string GetSpawnPointsVarName()
        {
            logger.Log( "GetSpawnPointsVarName was called." );
            return nameof( objectStack );
        }
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
        
        public ObjectStackerSettingsSO GetSettingsSO() => settingsSo;

#endregion
        

#region LifeCycle

        private void OnEnable()
        {
            logger.LogStart( true );

            // _subscriptionsHandler.Initialize( logger );
            // _subscriptionsHandler.UpdateSubscriptions( ref _previousSettingsSo, ref settingsSo, OnSettingsSOUpdated );
            
            UpdateSettingsSoSubscriptions();

            UpdateStack();

            logger.LogEnd();
        }

        private void OnDisable()
        {
            logger.LogStart( true );

            UpdateSettingsSoSubscriptions();

            logger.LogEnd();
        }

        private void OnValidate()
        {
            logger.LogStart( true );

            _updateRequired = true;
            UpdateSettingsSoSubscriptions();
            UpdateStack();

            logger.LogEnd();
        }

#endregion


#region EventMethods

        /// <summary>
        ///     This class handles when to subscribe and unsubscribe from settings SOs. It should be called during any Unity<br/>
        ///     event in which the settings SO object reference could be changed (either to null or to another settings SO).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateSettingsSoSubscriptions()
        {
            logger.LogStart();

            // Let: previousSettingsSO = P (for previous), and settingsSO = C (for current)

            if( _previousSettingsSo == null &&
                settingsSo == null )
            {
                // Case 1: P is null and C is null.
                //     This means settings have not been set so we do nothing.
                logger.LogEnd( "Case 1: No settings ref, aborting." );
                return;
            }

            if( _previousSettingsSo == null &&
                settingsSo != null )
            {
                // Case 2: P is null and C is null.
                //     This means this is the first run so no subscriptions have taken place so we need to:
                //         - set P equal to C
                //         - subscribe to C

                _previousSettingsSo = settingsSo;
                SubscribeToEvents( settingsSo );
                logger.LogEnd( $"Case 2: New settings set, subscribing to {GetColoredStringYellow( settingsSo.name )}." );
                return;
            }

            if( _previousSettingsSo != null &&
                settingsSo == null )
            {
                // Case 3: P is not null but C is null.
                //     This means that C has been set to null in the editor so we need to:
                //         - unsubscribe from P
                //         - set P to null
                UnsubscribeToEvents( _previousSettingsSo );
                logger.LogEnd( $"Settings cleared. Unsubscribing from {GetColoredStringYellow( _previousSettingsSo.name )}" );
                _previousSettingsSo = null;
                return;
            }

            if( _previousSettingsSo != null &&
                settingsSo != null )
            {
                // Case 4: neither P or C are null
                //     This means we're in one of two states:
                //         Case 4a: P is equal to C meaning no change has occured so we do nothing.
                //         Case 4b: P is not equal to C so we need to:
                //             - unsubscribe from P
                //             - set P equal to C
                //             - subscribe to C
                UnsubscribeToEvents( _previousSettingsSo );
                _previousSettingsSo = settingsSo;
                SubscribeToEvents( settingsSo );
                logger.LogEnd( $"Settings changed. Unsubscribing from {GetColoredStringYellow( _previousSettingsSo.name )} " +
                               $"and subscribing to {GetColoredStringYellow( settingsSo.name )}." );
                return;
            }

            // If we get to this point, it means that P is null but C is not null. As P is always set equal to C, this would be an erroneous state.
            throw new ArgumentOutOfRangeException();
        }

        private void SubscribeToEvents( ObjectStackerSettingsSO so )
        {
            logger.LogStart();

            so.onSettingsUpdated += OnSettingsSOUpdated;

            logger.LogEnd();
        }

        private void UnsubscribeToEvents( ObjectStackerSettingsSO so )
        {
            logger.LogStart( false, $"Unsubscribing from {GetColoredStringYellow( so.name )}" );

            if( so.onSettingsUpdated == null ) return;
            so.onSettingsUpdated -= OnSettingsSOUpdated;

            logger.LogEnd();
        }

        /// <summary>
        ///     This method should only be called in response to a change in the settingsSO's settings. That is triggered<br/>
        ///     in the settingsSO class's OnValidate() method.
        /// </summary>
        private void OnSettingsSOUpdated()
        {
            logger.LogStart( true );

            // This is to show stack adjustments in realtime during play mode.
            UpdateStackTransforms();

            logger.LogEnd();
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
            if( !_updateRequired )
            {
                logger.LogEnd( "No update required." );
                return;
            }

            UpdateStackTransforms();
            _updateRequired = false;

            logger.LogEnd();
        }

        private void UpdateStackTransforms()
        {
            GenerateNoiseMap();
            UpdatePseudoTransforms();
        }

#endregion


#region StackGeneration

        /// <summary>
        ///     Ensure stack size is correct, otherwise shrink or expand to appropriate size.
        /// </summary>
        private void ReevaluateStack()
        {
            logger.LogStart( false, $"stack.count: {GetColoredStringYellow( objectStack.Count.ToString() )}, " +
                                    $"objectCount: {GetColoredStringYellow( totalObjects.ToString() )}." );
            if( objectStack.Count == totalObjects )
            {
                logger.LogEnd( "Object stack is are the correct size." );
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


        public float[,] GetNoiseMap2D() =>
            NoiseGenerator.GenerateNoiseMap(
                NoiseMapWidth,
                totalObjects,
                settingsSo.seed,
                settingsSo.noiseScale,
                settingsSo.octaves,
                settingsSo.persistence,
                settingsSo.lacunarity,
                new Vector2( settingsSo.noiseOffsetHorizontal, settingsSo.noiseOffsetVertical )
            );

        private void UpdatePseudoTransforms()
        {
            // logger.LogStart();

            float currentOffset = 0f;

            float firstObjectYRot = 0;
            float firstObjectXShift = 0;
            float firstObjectYShift = 0;

            if( settingsSo.lockBottomObject )
            {
                firstObjectYRot = GetNoiseDrivenRotationValue( 0 );
                firstObjectXShift = GetNoiseDrivePositionValue( 0, settingsSo.posXNoiseCurve, settingsSo.posXNoiseShift ) / NoisePosShiftIncreaseFactor;
                firstObjectYShift = GetNoiseDrivePositionValue( 0, settingsSo.posZNoiseCurve, settingsSo.posZNoiseShift ) / NoisePosShiftIncreaseFactor;
            }


            // Flip the z axis if face up is checked.
            float zAxisRot = settingsSo.faceUp ? 0f : 180f;

            // logger.LogIndentStart( "Getting noise values:" );
            for ( int i = 0; i < totalObjects; i++ )
            {
                PseudoTransform pseudoTransform = objectStack[i];
                var noisePosShift = new Vector3(
                    GetNoisePositionShiftValue( i, settingsSo.posXNoiseCurve, settingsSo.posXNoiseShift, firstObjectXShift ),
                    currentOffset,
                    GetNoisePositionShiftValue( i, settingsSo.posZNoiseCurve, settingsSo.posZNoiseShift, firstObjectYShift )
                );
                pseudoTransform.position = transform.position + noisePosShift;
                pseudoTransform.rotation = Quaternion.Euler( 0, GetNoiseDrivenRotationValue( i ) - firstObjectYRot, zAxisRot );
                pseudoTransform.scale = Vector3.one;

                currentOffset += settingsSo.modelHeight + settingsSo.verticalOffset;
            }

            // logger.DecrementMethodIndent();
            // logger.LogEnd();
        }


        private float GetDeckYRotSkew( int i ) => i * settingsSo.deckYRotSkew;

        /// <summary>
        ///     Applies a relative rotation to the top object group. The group size is determined by the topDownSkewPercent<br/>
        ///     which represents the percent of object, starting from the top, that will be in the group. The topObjectsYRotSkew<br/>
        ///     and the dampening curve are then applied to that group only.
        /// </summary>
        private float GetTopObjectsYRotSkew( int i )
        {
            // The threshold index represents the point where the group starts. All objects above this index are in the group.
            // Note that the topDownSkewPercent is inverted to allow the UI slider to increase from left to right.
            int thresholdIndex = Mathf.RoundToInt( ( 1 - settingsSo.topDownSkewPercent ) * totalObjects );

            // The clamp is just a failsafe to ensure the rounding doesn't result in one digit to high or low.
            thresholdIndex = Mathf.Clamp( thresholdIndex, 0, totalObjects );

            // Filter out all objects below the threshold index.
            if( i < thresholdIndex ) return 0f;

            // Get the index relative to this group.
            int relativeIndex = i - thresholdIndex;

            // Get the group size.
            int totalObjectsInTop = totalObjects - thresholdIndex;

            // Use the relative index and the group size to determine what percent of the group has been traversed.
            float percentOfTopObjectssTraversed = relativeIndex / (float) totalObjectsInTop;

            // Use the percent to get the value on the dampening curve and multiply it by the topObjectsYRotSkew setting.
            return settingsSo.rotationDampeningCurve.Evaluate( percentOfTopObjectssTraversed ) * settingsSo.topObjectsYRotSkew;
        }


#region Noise

        private void GenerateNoiseMap()
        {
            logger.LogStart();

            noiseMap2D = GetNoiseMap2D();

            logger.LogEnd();
        }

        private float GetNoisePositionShiftValue( int i, AnimationCurve noiseCurve, float posShift, float firstObjectShiftValue ) => GetNoiseDrivePositionValue( i, noiseCurve, posShift ) / NoisePosShiftIncreaseFactor - firstObjectShiftValue;

        private float GetNoiseDrivePositionValue( int i, AnimationCurve curve, float axisNoise ) => GetCurveDampenedNoiseValueAtIndex( i, curve ) * axisNoise;

        private float GetNoiseDrivenRotationValue( int i )
        {
            // Get manual y rotation skews.
            float ySkew = GetDeckYRotSkew( i );
            float topObjectsYSkew = GetTopObjectsYRotSkew( i );

            // Get noise rotation skew.
            float noiseValue = GetCurveDampenedNoiseValueAtIndex( i, settingsSo.noiseDampeningCurve ) * settingsSo.noiseMultiplier;

            // Return combined y rotation adjustment.
            return noiseValue + ySkew + topObjectsYSkew + settingsSo.yRotOffset;
        }

        private float GetCurveDampenedNoiseValueAtIndex( int i, AnimationCurve curve )
        {
            // Get the percent of the deck traversed so far and use the percentage to get a sample from the curve.
            float percentProgress = i / (float) totalObjects;
            float noiseCurveValue = curve.Evaluate( percentProgress );

            // Re-scope the noise sample from [0,1] to [-1,1] (This effectively centers the noise effect) then apply the curve value.
            return noiseMap2D[0, i].Convert0ToMaxToNegMaxToPosMax() * noiseCurveValue;
        }

#endregion

#endregion


// #region PrefabSpawning
//
//         private void HandlePrefabSpawning()
//         {
//             if( Event.current != null &&
//                 Event.current.type != EventType.Repaint )
//                 return;
//
//             logger.LogStart();
//
//             if( objectStack.Count == 0 )
//                 logger.Log( "objectStack is empty!", FormattedLogType.Warning );
//
//             foreach ( PseudoTransform placementData in objectStack )
//             {
//                 DrawPrefab( placementData );
//             }
//
//             logger.LogEnd();
//         }
//
//         // This is modified from my Prefab Placement Brush Tool's DrawPrefab() method (originally from Freya Holmer Game Tools tutorial).
//         private void DrawPrefab( PseudoTransform placementData )
//         {
//             logger.LogStart();
//
//             MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>();
//
//             Matrix4x4 localToWorldMtx = Matrix4x4.TRS( placementData.position, placementData.rotation, Vector3.one );
//
//             logger.Log( $"Found {GetColoredStringYellow( filters.Length.ToString() )} mesh filters." );
//
//             // This step is necessary in case prefab consists of more than one mesh.
//             foreach ( MeshFilter filter in filters )
//             {
//                 // Calculate child object's local position in world space.
//                 Matrix4x4 childToParentMtx = filter.transform.localToWorldMatrix;
//                 Matrix4x4 childToWorldMtx = localToWorldMtx * childToParentMtx;
//
//                 // Material mat = spawnPoint.IsValid ? filter.GetComponent<MeshRenderer>().sharedMaterial : hologramMaterial;
//                 Material mat = filter.GetComponent<MeshRenderer>().sharedMaterial;
//
//                 Graphics.DrawMesh( filter.sharedMesh, childToWorldMtx, mat, 0, Camera.main );
//
//                 // Graphics.DrawMesh( filter.sharedMesh, Matrix4x4.identity, mat, 0, Camera.main );
//             }
//
//             logger.LogEnd();
//         }
//
// #endregion

        private void OnDrawGizmosSelected()
        {
            if( sceneViewVisualsMode != SceneViewDebugVisualsMode.WireFrames ) return;

            // Gizmos.DrawSphere( transform.position, 0.001f );

            // Gizmos.DrawIcon( Vector3.zero, "warning" );

            if( noiseMap2D == null ) return;

            Matrix4x4 cachedMatrix = Gizmos.matrix;
            for ( int i = 0; i < objectStack.Count; i++ )
            {
                PseudoTransform pseudoTransform = objectStack[i];

                Matrix4x4 matrix = Matrix4x4.TRS( pseudoTransform.position, pseudoTransform.rotation, Vector3.one );
                Gizmos.matrix = matrix;

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere( transform.position, 0.0002f );

                // float noiseSample = noiseMap[i];
                // float noiseSample = _noiseMap2D[0, i];
                float noiseSample = noiseMap2D[0, i];

                // Debug.Log( $"Noisemap[{i.ToString()}]: {GetColoredStringYellow( noiseSample.ToString( "0.00" ) )}" );

                // Note that we don't need to use the PseudoTransform's position here because we've already transformed the Gizmos' space.
                Gizmos.color = new Color( 1f, noiseSample, noiseSample );
                Gizmos.DrawWireCube( transform.position, new Vector3( 0.064f, 0.0002f, 0.0889f ) );
            }

            Gizmos.matrix = cachedMatrix;
        }
    }
}