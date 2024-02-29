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
        [SerializeField] [HideInInspector]
        private List<PseudoTransform> objectStack = new List<PseudoTransform>();
        
        [SerializeField] 
        private ObjectStackerSettingsSO settingsSo;

        [SerializeField] private bool showWireFrames = false;
        [SerializeField] private GameObject prefab;
        [SerializeField] private CustomLogger logger;

        private int _currentStackPosition;

        private float[] _noiseMap;
        private float _yRotAdjustment;
        private float _yRotCorrectedNoiseValue;

        private bool _updateRequired = false;
        
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
            
            _noiseMap = NoiseGenerator.GetPerlinNoiseArray(
                totalObjects,
                settingsSo.seed,
                settingsSo.noiseScale,
                settingsSo.octaves,
                settingsSo.persistence,
                settingsSo.lacunarity,
                settingsSo.noiseOffsetHorizontal,
                settingsSo.noiseOffsetVertical
            );
            
            logger.LogEnd();
        }

        private void UpdatePseudoTransforms()
        {
            logger.LogStart();
            
            float currentOffset = 0f;

            // Flip the z axis if face up is checked.
            float zAxisRot = settingsSo.faceUp ? 0f : 180f;
            
            for (int i = 0; i < totalObjects; i++)
            {
                // Get the percent of the deck traversed so far.
                float percentProgress = ( i / (float)totalObjects );
                float noiseCurveValue = settingsSo.noiseDampeningCurve.Evaluate( percentProgress );
                
                SetRotationEffects( i, noiseCurveValue );

                PseudoTransform pseudoTransform = objectStack[i];
                pseudoTransform.position = transform.position + new Vector3( 0f, currentOffset, 0f );
                pseudoTransform.rotation = Quaternion.Euler( 0, _yRotCorrectedNoiseValue + _yRotAdjustment, zAxisRot );
                pseudoTransform.scale = Vector3.one;

                // If spawning debug models, apply the noise effects to the transform of the model.
                // if ( spawnModels ) 
                //     pseudoTransform.ApplyPositionAndRotation( _debugModels[i].transform );

                currentOffset += settingsSo.verticalOffset;
            }
            
            logger.LogEnd();
        }

        private void SetRotationEffects( int i, float noiseCurveValue )
        {
            float firstCardYRot = _noiseMap[0];

            // Apply an increasing skew to the y rotation to all cards.
            float ySkew = i * ( settingsSo.deckYRotSkew );
                
            // Apply an increasing skew to only the top cards.
            float topCardsYSkew = 0;
            int currentCard = ( totalObjects - settingsSo.topSkewCardCount );
            if ( i > currentCard )
            {
                // Get the percent traversed from the top card range.
                int relativeIndex = ( i - currentCard );
                int totalCardsInTop = ( totalObjects - currentCard );
                float percentOfTopCardsTraversed = ( relativeIndex / (float) totalCardsInTop );
                topCardsYSkew = settingsSo.rotationDampeningCurve.Evaluate( percentOfTopCardsTraversed ) * settingsSo.topCardsYRotSkew;
            }
                
            // Keep the deck from rotating by locking the bottom card in placed and subtracting its rotation from all other cards.
            _yRotCorrectedNoiseValue = ( ( _noiseMap[i] * noiseCurveValue ) - firstCardYRot );
            // _yRotCorrectedNoiseValue = _noiseMap[i];

            // Combine all other y rotation adjustments;
            _yRotAdjustment = settingsSo.yRotOffset + ySkew + topCardsYSkew;
        }

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

        private void OnDrawGizmos()
        {
            if ( !showWireFrames ) return;
            // Gizmos.DrawSphere( transform.position, 0.001f );
            
            // Gizmos.DrawIcon( Vector3.zero, "warning" );
            
            Matrix4x4 cachedMatrix = Gizmos.matrix;
            foreach ( PseudoTransform pseudoTransform in objectStack )
            {
                // if ( pseudoTransform.rotation != new Quaternion( 0.000000f, 0.000000f, 0.000000f, 0.000000f ) )
                // {
                // var scale = new Vector3( pseudoTransform.scale.x, 1, pseudoTransform.scale.z );
                Matrix4x4 rotationMatrix = Matrix4x4.TRS( pseudoTransform.position, pseudoTransform.rotation, Vector3.one );
                Gizmos.matrix = rotationMatrix;
                // }
                
                // Gizmos.DrawWireCube( Vector3.zero, new Vector3( 0.032f, 0.0002f, 0.0445f ) );
                // Gizmos.DrawWireCube( Vector3.zero, new Vector3( 0.1f, 0.002f, 0.1f ) );
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere( pseudoTransform.position, 0.0002f );
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube( pseudoTransform.position, new Vector3( 0.064f, 0.0002f, 0.0889f ) );
                // Gizmos.DrawWireCube( pseudoTransform.position, new Vector3( 0.032f, 0.0002f, 0.0445f ) );
            }
        
            Gizmos.matrix = cachedMatrix;
        }

    }
}