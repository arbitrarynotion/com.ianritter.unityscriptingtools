using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.Scene;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Runtime;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;

namespace Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Editor
{
    /// <summary>
    ///     Extend this class if you want to customize the Inspector window of a script<br />
    ///     that is extending PrefabSpawnerRoot. Note that the extending class still<br />
    ///     needs to include a `CustomEditor` for its target class even though this editor's<br/>
    ///     custom editor is set to also be the editor for child classes.
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

        private FormattedLogger _logger;
        
        /// <summary> Called at the start of OnEnable. </summary>
        protected virtual void OnEnableFirst() {}
        /// <summary> Called at the end of OnEnable. </summary>
        protected virtual void OnEnableLast() {}
        
        /// <summary> Called at the start of OnDisable. </summary>
        protected virtual void OnDisableFirst() {}
        /// <summary> Called at the end of OnDisable. </summary>
        protected virtual void OnDisableLast() {}
        
        /// <summary> Called at the start of OnInspectorGUI. </summary>
        protected virtual void OnInspectorGUIFirst() {}
        /// <summary> Called at the end of OnInspectorGUI. </summary>
        protected virtual void OnInspectorGUILast() {}
        
        /// <summary>
        /// Called at the start of DuringSceneGUI in response to SceneView.duringSceneGUI. Note that the DuringSceneGUI method
        /// will filter out all but Repaint events.
        /// </summary>
        protected virtual void DuringSceneGUIFirst() {}
        
        /// <summary>
        /// Called at the end of DuringSceneGUI in response to SceneView.duringSceneGUI. Note that the DuringSceneGUI method
        /// will filter out all but Repaint events.
        /// </summary>
        protected virtual void DuringSceneGUILast() {}

#endregion


#region LifeCycle

        private void OnEnable()
        {
            if ( _logger == null )
                _logger = FormattedLoggerLoader.GetLogger( PrefabSpawnerELoggerName, DefaultLoggerPath );
            
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
                _logger.Log( $"Can't find {_prefabSpawnerRoot}.", FormattedLogType.Warning );
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
            _logger.LogObjectAssignmentResult( nameof( _prefabToSpawnProp ), _prefabToSpawnProp == null, FormattedLogType.Standard );
            _logger.LogObjectAssignmentResult( "PrefabToSpawnProp's object reference: ", _prefabToSpawnProp.objectReferenceValue == null, FormattedLogType.Standard );
            _spawnPointsProp = serializedObject.FindProperty( _spawnPointsVarName );
            _logger.LogObjectAssignmentResult( nameof( _spawnPointsProp ), _spawnPointsProp == null, FormattedLogType.Standard );

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
            if ( Event.current.type != EventType.Repaint ) return;
            
            DuringSceneGUIFirst();
            
            if ( _sceneViewVisualsModeProp.intValue != 2 ) return;
            
            // Refresh the scene view any time the mouse is moved while in the view window.
            // if ( Event.current.type == EventType.MouseMove )
            //     sceneView.Repaint();

            if ( _prefabToSpawnProp.objectReferenceValue == null )
            {
                _logger.Log( "Couldn't find the prefab!", FormattedLogType.Warning );
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