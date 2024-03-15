using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Editor;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
#region DataMembers

        // Serialized Properties
        private SerializedProperty _totalObjectsProp;

        private SerializedProperty _settingsSOProp;

        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _mapGenerationModeProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;

        // Debug
        private FormattedLogger _localLogger;

        // These are used to draw a frame around the editor area.
        private float _editorStartPosition;
        private float _editorEndPosition;

        // Embedded Editor
        private UnityEditor.Editor _settingsSOEditor;
        private Object _settingsSOEditorTarget;
        private EmbedSOEditor _settingsEmbeddedSOEditor;

        private SerializedObject _settingsSerializedObject;

#endregion
        
#region FoldoutToggles

        private bool StackerSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( StackerSettingsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( StackerSettingsFoldoutToggle )}", value );
        }

        private bool VisualisationsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( VisualisationsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( VisualisationsFoldoutToggle )}", value );
        }

        private bool ObjectStackerSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( ObjectStackerSettingsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( ObjectStackerSettingsFoldoutToggle )}", value );
        }

        private bool DebugFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( DebugFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{name}_{nameof( DebugFoldoutToggle )}", value );
        }

#endregion

#region EventMethods

        protected override void DuringSceneGUILast()
        {
            // Todo: This solved the problem of the scene view not repainting until the next frame but it means the scene is nearly constantly being repainted.
            // _localLogger.Log( "DuringSceneGUILast was called. Repainting scene views." );
            SceneView.RepaintAll();
        }

#endregion


#region LifeCycle

        protected override void OnEnableFirst()
        {
            if( _localLogger == null )
                _localLogger = FormattedLoggerLoader.GetLogger( ObjectStackerELoggerName, DefaultLoggerPath );

            _localLogger.LogStart();

            LoadObjectStackProperties();
            InitializeEmbeddedEditors();

            _localLogger.LogEnd();
        }

        protected override void OnDisableLast()
        {
            _localLogger.LogStart();

            _settingsEmbeddedSOEditor.OnDisable();

            _localLogger.LogEnd();
        }

        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();

            DrawStackerSection();
            DrawSettingsSOSection();
            DrawVisualizationSection();
            DrawDebugSection();

            if( !serializedObject.ApplyModifiedProperties() ) return;

            _localLogger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }

#endregion

        private void DrawSettingsSOSection()
        {
            // Draw the embedded editor.
            _settingsEmbeddedSOEditor.DrawSettingsSoInspector( "Object Stacker Settings SO" );
            if( ( _settingsSOProp.objectReferenceValue != null ) && ObjectStackerSettingsFoldoutToggle )
                Space( VerticalSeparator );
        }


#region Initialization

        private void InitializeEmbeddedEditors()
        {
            _settingsEmbeddedSOEditor.OnEnable();
        }

        private void LoadObjectStackProperties()
        {
            _totalObjectsProp = serializedObject.FindProperty( "totalObjects" );

            _settingsSOProp = serializedObject.FindProperty( "objectStackerSettingsSO" );
            _settingsEmbeddedSOEditor = new EmbedSOEditor( _settingsSOProp );

            _sceneViewVisualsModeProp = serializedObject.FindProperty( "sceneViewVisualsMode" );
            _mapGenerationModeProp = serializedObject.FindProperty( "mapGenerationMode" );
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );
        }

#endregion


#region DrawInspectorGUI

        private void DrawStackerSection()
        {
            StackerSettingsFoldoutToggle = DrawFoldoutSection( "Stacker Settings", FoldoutFrameType, StackerSettingsFoldoutToggle );
            if( !StackerSettingsFoldoutToggle ) return;

            EditorGUI.indentLevel++;
            {
                PropertyField( _totalObjectsProp );
                PropertyField( _settingsSOProp );
            }
            EditorGUI.indentLevel--;
            
            Space( BetweenSectionPadding );
        }

        private void DrawVisualizationSection()
        {
            VisualisationsFoldoutToggle = DrawFoldoutSection( "Visualizations", FoldoutFrameType, VisualisationsFoldoutToggle );
            if( !VisualisationsFoldoutToggle ) return;

            EditorGUI.indentLevel++;
            {
                PropertyField( _sceneViewVisualsModeProp, new GUIContent( "Visualization Mode" ) );
                PropertyField( _mapGenerationModeProp, new GUIContent( "Map Dimension Mode" ) );
                PropertyField( _prefabProp, new GUIContent( "Model Mode Prefab" ) );
            }
            EditorGUI.indentLevel--;
            
            Space( BetweenSectionPadding );
        }

        private void DrawDebugSection()
        {
            DebugFoldoutToggle = DrawFoldoutSection( "Debug", FoldoutFrameType, DebugFoldoutToggle );
            if( !DebugFoldoutToggle ) return;

            EditorGUI.indentLevel++;
            {
                PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;
        }

#endregion
    }
}