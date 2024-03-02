using UnityEngine;
using UnityEditor;
using Packages.com.ianritter.unityscriptingtools.Editor._Testing.Graphics;
using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.PrefabSpawner
{
    /// <summary>
    ///     Extend this class if you want to customize the Inspector window of a script<br />
    ///     that is extending PrefabSpawnerRoot. Note that the extending class does not<br />
    ///     need to include a `CustomEditor` attribute as this parent class has<br />
    ///     `editorForChildClasses` set to true.
    /// </summary>
    [CustomEditor( typeof( PrefabSpawnerRoot ), true )]
    public class PrefabSpawnerEditor : UnityEditor.Editor
    {
#region DataMembers
        // Note that this is a variable of a child of PrefabSpawnerRoot. We can access it because the serialized object
        // essentially combines the parent and child class data so it looks like it's all coming from the same level.
        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _prefabToSpawnProp;
        private SerializedProperty _spawnPointsProp;

        private PrefabSpawnerRoot _prefabSpawnerRoot;
        private string _prefabVarName;
        private string _spawnPointsVarName;

        private CustomLogger _logger;
        
        protected virtual void OnEnableFirst() {}
        protected virtual void OnEnableLast() {}
        
        protected virtual void OnDisableFirst() {}
        protected virtual void OnDisableLast() {}
        
        protected virtual void OnInspectorGUIFirst() {}
        
        protected virtual void DuringSceneGUIFirst() {}
        protected virtual void DuringSceneGUILast() {}

#endregion


#region LifeCycle

        private void OnEnable()
        {
            if ( _logger == null )
                // _logger = CustomLoggerLoader.GetLogger( "PrefabSpawnerEditorLogger", "Assets/_Testing/ScriptableObjects/Loggers/" );
            // _logger = CustomLoggerLoader.GetLogger( "PrefabSpawnerELogger", "Packages/Unity Scripting Tools/ScriptableObjects/CustomLoggers/" );
            _logger = CustomLoggerLoader.GetLogger( "PrefabSpawnerELogger", "Packages/com.ianritter.unityscriptingtools/ScriptableObjects/CustomLoggers/" );
            
            _logger.LogStart( true );
            
            _logger.Log( "Calling OnEnableFirst..." );
            OnEnableFirst();
            
            LoadTarget();
            LoadVarNames();
            LoadSerializedProperties();
            SubscribeToEvents();
            
            OnEnableLast();

            _logger.LogEnd();
        }
        
        private void OnDisable()
        {
            _logger.LogStart( true );

            OnDisableFirst();
            
            UnsubscribeFromEvents();
            
            OnDisableLast();
            
            _logger.LogEnd();
        }

        public override void OnInspectorGUI()
        {
            // _logger.LogStart( true );
            OnInspectorGUIFirst();
            // _logger.LogEnd();
        }

#endregion


#region Initialization

        private void LoadTarget()
        {
            _prefabSpawnerRoot = target as PrefabSpawnerRoot;
            if ( _prefabSpawnerRoot == null )
            {
                _logger.Log( $"Can't find {_prefabSpawnerRoot}.", CustomLogType.Warning );
                // Debug.LogWarning( $"Can't find {_prefabSpawnerRoot}." );
            }
        }

        private void LoadVarNames()
        {
            _logger.LogStart();
            _prefabVarName = _prefabSpawnerRoot.GetPrefabVarName();
            _spawnPointsVarName = _prefabSpawnerRoot.GetSpawnPointsVarName();
            _logger.LogEnd( $"Loaded: _prefabVarName: {_prefabVarName}, _spawnPointsVarName: {_spawnPointsVarName}." );
        }

        private void LoadSerializedProperties()
        {
            _sceneViewVisualsModeProp = serializedObject.FindProperty( "sceneViewVisualsMode" );
            
            _prefabToSpawnProp = serializedObject.FindProperty( _prefabVarName );
            _logger.LogObjectAssignmentResult( nameof( _prefabToSpawnProp ), _prefabToSpawnProp == null, CustomLogType.Standard );
            _logger.LogObjectAssignmentResult( "PrefabToSpawnProp's object reference: ", _prefabToSpawnProp.objectReferenceValue == null, CustomLogType.Standard );
            _spawnPointsProp = serializedObject.FindProperty( _spawnPointsVarName );
            _logger.LogObjectAssignmentResult( nameof( _spawnPointsProp ), _spawnPointsProp == null, CustomLogType.Standard );

            if ( _spawnPointsProp == null )
            {
                Debug.LogWarning( $"Can't find {_prefabToSpawnProp}." );
            }
            if ( _spawnPointsProp == null )
            {
                Debug.LogWarning( $"Can't find {_spawnPointsVarName}." );
            }
        }

        private void SubscribeToEvents() => SceneView.duringSceneGui += DuringSceneGUI;

        private void UnsubscribeFromEvents() => SceneView.duringSceneGui -= DuringSceneGUI;

#endregion
        

#region EventMethods

        private void DuringSceneGUI( SceneView sceneView )
        {
            DuringSceneGUIFirst();
            
            if ( _sceneViewVisualsModeProp.intValue != 2 ) return;
            
            // Refresh the scene view any time the mouse is moved while in the view window.
            // if ( Event.current.type == EventType.MouseMove )
            //     sceneView.Repaint();

            if ( _prefabToSpawnProp.objectReferenceValue == null )
            {
                _logger.Log( "Couldn't find the prefab!", CustomLogType.Warning );
                return;
            }

            SceneGUIMeshes.SpawnPrefabsInSceneView(
                (GameObject) _prefabToSpawnProp.objectReferenceValue,
                _prefabSpawnerRoot.transform,
                _spawnPointsProp,
                sceneView
            );
            
            DuringSceneGUILast();
        }

#endregion
    }
}