﻿using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Editor.Services.AssetLoader;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.Graphics
{
    public static class SceneGUIMeshes
    {
        private static CustomLogger Logger
        {
            get
            {
                if ( _logger == null )
                    _logger = CustomLoggerLoader.GetLogger( "SceneGUIMeshesLogger", "Assets/_Testing/ScriptableObjects/Loggers/" );

                return _logger;
            }
        }
        private static CustomLogger _logger;
        
        /// <summary>
        /// This method will spawn the prefab once for each spawn point. The spawned meshes will share the transforms of their
        /// respective spawn points.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parentTransform"></param>
        /// <param name="spawnPoints"></param>
        /// <param name="sceneView"></param>
        public static void SpawnPrefabsInSceneView( GameObject prefab, Transform parentTransform, SerializedProperty spawnPoints, SceneView sceneView )
        {
            if ( prefab == null )
                return;

            // Logger.LogStart();

            // If we're in prefab isolation mode, don't process/render tool.
            if ( PrefabStageUtility.GetCurrentPrefabStage() != null )
                return;
        
            Handles.zTest = CompareFunction.LessEqual;
            
            DrawPrefabsForEachSpawnPoint( prefab, parentTransform, spawnPoints, sceneView );

            // Logger.LogEnd();
        }

        private static void DrawPrefabsForEachSpawnPoint( GameObject prefab, Transform parentTransform, SerializedProperty spawnPoints, SceneView sceneView )
        {
            // Logger.Log( $"Processing {GetColoredStringYellow( spawnPoints.arraySize.ToString() )} spawn points." );

            for (int i = 0; i < spawnPoints.arraySize; i++)
            {
                SerializedProperty currentPseudoTransform = spawnPoints.GetArrayElementAtIndex( i );
                if ( currentPseudoTransform == null )
                {
                    Debug.LogWarning( $"Found no PseudoTransform for spawn point {GetColoredStringGoldenrod( i.ToString() )}!" );
                    continue;
                }
                
                DrawPrefab( prefab, parentTransform, currentPseudoTransform, sceneView.camera );
            }
        }

        private static void DrawPrefab( GameObject prefab, Transform parentTransform, SerializedProperty currentPseudoTransform, Camera sceneViewCamera )
        {
            // Grab the first transform in the array for testing.
            // var transformToMatch = spawnPointsProp.GetArrayElementAtIndex( 0 ).objectReferenceValue as Transform;
            
            // Get the array of filters.
            MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>();
            // Logger.Log( $"Processing {GetColoredStringYellow( filters.Length.ToString() )} filters." );
            if ( filters.Length == 0 )
                Debug.LogWarning( $"Found no MeshFilters on the prefab {prefab.name}!" );

            DrawEachMeshFilter( parentTransform, currentPseudoTransform, filters, sceneViewCamera );
        }

        private static void DrawEachMeshFilter( Transform parentTransform, SerializedProperty currentPseudoTransform, MeshFilter[] filters, Camera sceneViewCamera )
        {
            // This step is necessary in case prefab consists of more than one mesh.
            foreach ( MeshFilter filter in filters )
            {
                var renderer = filter.GetComponent<MeshRenderer>();
                if ( renderer == null )
                {
                    Logger.Log( $"MeshFilter {filter.name} has no MeshRenderer component!", CustomLogType.Warning );
                    continue;
                }

                DrawMesh( parentTransform, currentPseudoTransform, filter, renderer, sceneViewCamera );
            }
        }
        
        private static void DrawMesh( Transform parentTransform, SerializedProperty currentPseudoTransform, MeshFilter filter, MeshRenderer renderer, Camera sceneViewCamera )
        {
            Mesh mesh = filter.sharedMesh;
            Material[] materials = renderer.sharedMaterials;

            SerializedProperty position = currentPseudoTransform.FindPropertyRelative( nameof( PseudoTransform.position ) );
            SerializedProperty rotation = currentPseudoTransform.FindPropertyRelative( nameof( PseudoTransform.rotation ) );
            SerializedProperty scale = currentPseudoTransform.FindPropertyRelative( nameof( PseudoTransform.scale ) );

            // Generate a localToWorldMatrix from the pseudoTransform.
            Matrix4x4 spawnPointLocalToWorldMtx = MatrixUtilities.CreateLocalToWorldMatrix(
                position.vector3Value,
                rotation.quaternionValue,
                scale.vector3Value
            );

            // Calculate the local to world matrix relative to the parent transform.
            // Matrix4x4 localToWorldRelativeMtx = parentTransform.LocalToWorldRelativeMatrix( filter.transform );
            Matrix4x4 localToWorldRelativeMtx = MatrixUtilities.LocalToWorldRelativeMatrix( spawnPointLocalToWorldMtx, filter.transform );

            localToWorldRelativeMtx = parentTransform.localToWorldMatrix * localToWorldRelativeMtx;
            
            for (int i = 0; i < mesh.subMeshCount && i < materials.Length; i++)
            {
                UnityEngine.Graphics.DrawMesh( mesh, localToWorldRelativeMtx, materials[i], 0, sceneViewCamera , i);
            }
        }
        
    }
}