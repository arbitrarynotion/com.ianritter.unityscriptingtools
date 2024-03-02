using System;
using System.Collections.Generic;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks
{
    [ExecuteInEditMode]
    public class ObjectStack : PrefabSpawnerRoot
    {
        [SerializeField] [Delayed] private int totalObjects = 3;

        // The list that hold the noise transform effects for all positions in the stack.
        [HideInInspector]
        [SerializeField] private List<PseudoTransform> objectStack = new List<PseudoTransform>();


        [SerializeField] private ObjectStackerSettingsSO settingsSo;

        [SerializeField] private SceneViewDebugVisualsMode sceneViewVisualsMode = SceneViewDebugVisualsMode.Off;
        [SerializeField] private GameObject prefab;
        [SerializeField] private CustomLogger logger;

        private int _currentStackPosition;

        [SerializeField] [HideInInspector]
        // private float[] noiseMap;
        private float[,] _noiseMap2D;
        // private float[,] _noiseMap2DScaled;
        // private float _yRotAdjustment;
        // private float _yRotCorrectedNoiseValue;
        private float _noiseDrivenRotationValue;

        private bool _updateRequired = false;


        // public float[,] Get2DNoiseMap() => _noiseMap2D;
        // public float[] GetNoiseMap() => noiseMap;
        
        
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

#region LifeCycle

        private void Awake()
        {
            logger.LogStart( true );
            logger.LogEnd();
        }

        private void OnEnable()
        {
            logger.LogStart( true );
            
            SubscribeToEvents();
            
            UpdateStack();
            
            logger.LogEnd();
        }

        private void OnDisable()
        {
            logger.LogStart( true );
            
            UnsubscribeToEvents();
            
            logger.LogEnd();
        }

        private void OnValidate()
        {
            logger.LogStart( true );

            _updateRequired = true;
            UpdateStack();
            
            logger.LogEnd();
        }

#endregion


#region EventFunctions
        
        private void SubscribeToEvents()
        {
            logger.LogStart();
            
            settingsSo.onSettingsUpdated += OnSettingsSOUpdated;
            
            logger.LogEnd();
        }
        
        private void UnsubscribeToEvents()
        {
            logger.LogStart();
            
            settingsSo.onSettingsUpdated -= OnSettingsSOUpdated;
            
            logger.LogEnd();
        }

        private void OnSettingsSOUpdated()
        {
            logger.LogStart( true );
            
            // This is to show stack adjustments in realtime during play mode.
            UpdateStackTransforms();
            
            logger.LogEnd();
        }

#endregion


#region StandManagement

        private void UpdateStack()
        {
            logger.LogStart();
            
            ReevaluateStack();
            if ( !_updateRequired )
            {
                logger.LogEnd( "No update required." );
                return;
            }
            
            GenerateNoiseMap();
            UpdatePseudoTransforms();
            _updateRequired = false;

            logger.LogEnd();
        }

        private void UpdateStackTransforms()
        {
            GenerateNoiseMap();
            UpdatePseudoTransforms();
        }



#endregion


#region Interface
        
        public PseudoTransform GetNextPosition()
        {
            if ( _currentStackPosition >= objectStack.Count )
                throw new ArgumentOutOfRangeException( nameof(objectStack), "Stack is full!");

            return objectStack[_currentStackPosition++];
        }

        public int GetCurrentStackPosition() => _currentStackPosition;

        public void ResetStackCounter() => _currentStackPosition = 0;

#endregion


#region StackGeneration

        private void ReevaluateStack()
        {
            logger.LogStart( false, $"stack.count: {GetColoredStringYellow( objectStack.Count.ToString() )}, " +
                                    $"objectCount: {GetColoredStringYellow( totalObjects.ToString() )}.");
            if ( objectStack.Count == totalObjects )
            {
                logger.LogEnd( "Object stack is are the correct size." );
                _updateRequired = false;
                return;
            }
            
            // If the stack is too big, remove the extra entries from the end of the list.
            if ( objectStack.Count > totalObjects )
            {
                int extras = objectStack.Count - totalObjects;
                for (int i = extras - 1; i >= 0; i--)
                {
                    objectStack.RemoveAt( i );
                }
                logger.LogEnd( $"Removed {GetColoredStringMaroon( extras.ToString() )} objects." );
                return;
            }

            // If the stack is too small, add the missing entries.
            int deficit = totalObjects - objectStack.Count;
            for (int i = 0; i < deficit; i++)
            {
                objectStack.Add( new PseudoTransform() );
            }
            logger.LogEnd( $"Added {GetColoredStringGreenYellow( deficit.ToString() )} objects." );
        }

        private void GenerateNoiseMap()
        {
            logger.LogStart();
            
            // noiseMap = NoiseGenerator.GetPerlinNoiseArray(
            //     totalObjects,
            //     settingsSo.seed,
            //     settingsSo.noiseScale,
            //     settingsSo.octaves,
            //     settingsSo.persistence,
            //     settingsSo.lacunarity,
            //     settingsSo.noiseOffsetHorizontal,
            //     settingsSo.noiseOffsetVertical
            // );

            _noiseMap2D = NoiseGenerator.GenerateNoiseMap(
                totalObjects,
                totalObjects,
                settingsSo.seed,
                settingsSo.noiseScale,
                settingsSo.octaves,
                settingsSo.persistence,
                settingsSo.lacunarity,
                new Vector2( settingsSo.noiseOffsetHorizontal,  settingsSo.noiseOffsetVertical )
            );

            // _noiseMap2DScaled = NoiseGenerator.GetNoiseMapScaledToGrid( 
            //     _noiseMap2D, 
            //     totalObjects, 
            //     totalObjects, 
            //     totalObjects, 
            //     totalObjects 
            // );
            
            // Debug.Log( $"Noise Map 2D is [{GetColoredStringYellow( _noiseMap2D.GetLength( 0 ).ToString() )}, " +
            //            $"{GetColoredStringYellow( _noiseMap2D.GetLength( 1 ).ToString() )}]" );
            // Debug.Log( $"Noise Map 2D Scaled is [{GetColoredStringYellow( _noiseMap2DScaled.GetLength( 0 ).ToString() )}, " +
            //            $"{GetColoredStringYellow( _noiseMap2DScaled.GetLength( 1 ).ToString() )}]" );
            
            logger.LogEnd();
        }

        // public float[,] GetNoiseMap2DScaled()
        // {
        //     return NoiseGenerator.GenerateNoiseMap(
        //         totalObjects,
        //         totalObjects,
        //         settingsSo.seed,
        //         settingsSo.noiseScale,
        //         settingsSo.octaves,
        //         settingsSo.persistence,
        //         settingsSo.lacunarity,
        //         new Vector2( settingsSo.noiseOffsetHorizontal,  settingsSo.noiseOffsetVertical )
        //     );
        // }
        
        public float[,] GetNoiseMap2D()
        {
            return NoiseGenerator.GenerateNoiseMap(
                5,
                totalObjects,
                settingsSo.seed,
                settingsSo.noiseScale,
                settingsSo.octaves,
                settingsSo.persistence,
                settingsSo.lacunarity,
                new Vector2( settingsSo.noiseOffsetHorizontal,  settingsSo.noiseOffsetVertical )
            );
        }

        private void UpdatePseudoTransforms()
        {
            // logger.LogStart();
            
            float currentOffset = 0f;

            float firstCardYRot = GetNoiseDrivenRotationValue( 0 );

            // Flip the z axis if face up is checked.
            float zAxisRot = settingsSo.faceUp ? 0f : 180f;
            
            // logger.LogIndentStart( "Getting noise values:" );
            for (int i = 0; i < totalObjects; i++)
            {
                PseudoTransform pseudoTransform = objectStack[i];
                pseudoTransform.position = transform.position + new Vector3( 0f, currentOffset, 0f );
                pseudoTransform.rotation = Quaternion.Euler( 0, GetNoiseDrivenRotationValue( i ) - firstCardYRot, zAxisRot );
                pseudoTransform.scale = Vector3.one;

                currentOffset += settingsSo.verticalOffset;
            }

            // logger.DecrementMethodIndent();
            // logger.LogEnd();
        }

        private float GetTotalRotationAdjustment( int i, float firstCardYRot )
        {
            return GetNoiseDrivenRotationValue( i ) - firstCardYRot;
        }

        private float GetCurveDampenedNoiseValueAtIndex( int i )
        {
            // Get the percent of the deck traversed so far.
            float percentProgress = ( i / (float)totalObjects );
            float noiseCurveValue = settingsSo.noiseDampeningCurve.Evaluate( percentProgress );
            return _noiseMap2D[0, i] * noiseCurveValue;
        }

        private float GetNoiseDrivenRotationValue( int i )
        {
            // Get manual y rotation skews.
            float ySkew = GetDeckYRotSkew( i );
            float topCardsYSkew = GetTopCardsYRotSkew( i );

            // Get noise rotation skew.
            float noiseValue = GetCurveDampenedNoiseValueAtIndex( i ) * settingsSo.noiseMultiplier;

            // Return combined y rotation adjustment.
            return ( noiseValue + ySkew + topCardsYSkew + settingsSo.yRotOffset );
        }

        private float GetDeckYRotSkew( int i )
        {
            return i * ( settingsSo.deckYRotSkew );
        }

        private float GetTopCardsYRotSkew( int i )
        {
            // Apply an increasing skew to only the top cards.
            
            // int currentCard = ( totalObjects - settingsSo.topDownSkewPercent );
            int thresholdIndex = Mathf.RoundToInt( ( 1 - settingsSo.topDownSkewPercent ) * totalObjects );
            // int thresholdIndex = Mathf.RoundToInt( settingsSo.topDownSkewPercent * totalObjects );
            thresholdIndex = Mathf.Clamp( thresholdIndex, 0, totalObjects );
            // thresholdIndex = 0;
            if ( i < thresholdIndex ) return 0f;
            
            // Get the percent traversed from the top card range.
            int relativeIndex = ( i - thresholdIndex );
            int totalCardsInTop = ( totalObjects - thresholdIndex );
            float percentOfTopCardsTraversed = ( relativeIndex / (float) totalCardsInTop );
            float result = settingsSo.rotationDampeningCurve.Evaluate( percentOfTopCardsTraversed ) * settingsSo.topCardsYRotSkew;

            // if ( i < 2 )
            // logger.Log( $"i: {GetColoredStringYellow( i.ToString() )}, " +
            //             $"thresholdIndex: {GetColoredStringYellow( thresholdIndex.ToString() )}, " +
            //             $"relativeIndex: {GetColoredStringYellow( relativeIndex.ToString() )}, " +
            //             $"totalCardsInTop: {GetColoredStringYellow( totalCardsInTop.ToString() )}, " +
            //             $"percentOfTopCardsTraversed: {GetColoredStringYellow( percentOfTopCardsTraversed.ToString("0.00") )}, " +
            //             $"result: {GetColoredStringYellow( result.ToString("0.00") )}" );
            return result;
        }
        
        // private float GetTopCardsYRotSkew( int i )
        // {
        //     // Apply an increasing skew to only the top cards.
        //     float topCardsYSkew = 0;
        //     
        //     // Is this index within the top down percent?
        //     float currentPercentOfTotal = i / (float) totalObjects;
        //     int thresholdObject = Mathf.RoundToInt( settingsSo.topDownSkewPercent * totalObjects );
        //     
        //     if ( currentPercentOfTotal < ( 1f - settingsSo.topDownSkewPercent ) ) return topCardsYSkew;
        //     
        //     // The range of objects to apply the rotation to is from threshold to total.
        //     // So the number of objects in the skew group is total - threshold.
        //     int totalInSkewGroup = ( totalObjects - thresholdObject );
        //     
        //     // Get the percent traversed from the top card range.
        //     // Relative index will always be greater than 0 because otherwise 
        //     int relativeIndex = ( i - totalInSkewGroup );
        //     int totalCardsInTop = ( totalObjects - totalInSkewGroup );
        //     
        //     
        //     float percentOfTopCardsTraversed = ( relativeIndex / (float) totalCardsInTop );
        //     topCardsYSkew = settingsSo.rotationDampeningCurve.Evaluate( percentOfTopCardsTraversed ) * settingsSo.topCardsYRotSkew;
        //
        //     return topCardsYSkew;
        // }

#endregion
        

#region PrefabSpawning
        
        private void HandlePrefabSpawning()
        {

            if ( Event.current != null && Event.current.type != EventType.Repaint )
                return;

            logger.LogStart();
            
            if ( objectStack.Count == 0 )
                logger.Log( "objectStack is empty!", CustomLogType.Warning );
            
            foreach ( PseudoTransform placementData in objectStack )
            {
                DrawPrefab( placementData );
            }

            logger.LogEnd();
        }

        // This is modified from my Prefab Placement Brush Tool's DrawPrefab() method (originally from Freya Holmer Game Tools tutorial).
        private void DrawPrefab( PseudoTransform placementData )
        {
            logger.LogStart();
            
            MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>();
        
            Matrix4x4 localToWorldMtx = Matrix4x4.TRS( placementData.position, placementData.rotation, Vector3.one );
        
            logger.Log( $"Found {GetColoredStringYellow( filters.Length.ToString())} mesh filters." );
            
            // This step is necessary in case prefab consists of more than one mesh.
            foreach ( MeshFilter filter in filters )
            {
                // Calculate child object's local position in world space.
                Matrix4x4 childToParentMtx = filter.transform.localToWorldMatrix;
                Matrix4x4 childToWorldMtx = localToWorldMtx * childToParentMtx;
        
                // Material mat = spawnPoint.IsValid ? filter.GetComponent<MeshRenderer>().sharedMaterial : hologramMaterial;
                Material mat = filter.GetComponent<MeshRenderer>().sharedMaterial;
        
                Graphics.DrawMesh( filter.sharedMesh, childToWorldMtx, mat, 0, Camera.main );
                // Graphics.DrawMesh( filter.sharedMesh, Matrix4x4.identity, mat, 0, Camera.main );
            }
            
            logger.LogEnd();
        }

#endregion

        private void OnDrawGizmosSelected()
        {
            if ( sceneViewVisualsMode != SceneViewDebugVisualsMode.WireFrames ) return;
            // Gizmos.DrawSphere( transform.position, 0.001f );
            
            // Gizmos.DrawIcon( Vector3.zero, "warning" );

            if ( _noiseMap2D == null ) return;
            
            Matrix4x4 cachedMatrix = Gizmos.matrix;
            for (int i = 0; i < objectStack.Count; i++)
            {
                PseudoTransform pseudoTransform = objectStack[i];

                Matrix4x4 matrix = Matrix4x4.TRS( pseudoTransform.position, pseudoTransform.rotation, Vector3.one );
                Gizmos.matrix = matrix;
                
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere( pseudoTransform.position, 0.0002f );
                // float noiseSample = noiseMap[i];
                // float noiseSample = _noiseMap2D[0, i];
                float noiseSample = _noiseMap2D[0, i];
                // Debug.Log( $"Noisemap[{i.ToString()}]: {GetColoredStringYellow( noiseSample.ToString( "0.00" ) )}" );
                
                // Note that we don't need to use the PseudoTransform's position here because we've already transformed the Gizmos' space.
                Gizmos.color = new Color( 1f, noiseSample, noiseSample );
                Gizmos.DrawWireCube( transform.position, new Vector3( 0.064f, 0.0002f, 0.0889f ) );
            }

            Gizmos.matrix = cachedMatrix;
        }

    }
}