using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Editor;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.UI.EditorDividerGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
#region DataMembers

        // Target
        private ObjectStack _objectStack;

        // Serialized Properties
        private SerializedProperty _totalObjectsProp;

        private SerializedProperty _settingsSOProp;

        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;

        // Foldout bools
        private bool _showObjectStackerSettings = true;

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


#region LifeCycle

        protected override void OnEnableFirst()
        {
            if( _localLogger == null )
                _localLogger = FormattedLoggerLoader.GetLogger( ObjectStackerELoggerName, DefaultLoggerPath );

            _localLogger.LogStart();

            _objectStack = target as ObjectStack;

            LoadObjectStackProperties();
            InitializeEmbeddedEditors();

            _localLogger.LogEnd();
        }

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
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );
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

            // Draw the embedded editor.
            DrawSettingsSoInspector( ref _showObjectStackerSettings, "Object Stacker Settings SO", _settingsEmbeddedSOEditor );
            if( _settingsSOProp.objectReferenceValue != null && _showObjectStackerSettings )
                Space( VerticalSeparator );

            Space( BetweenSectionPadding );

            DrawVisualizationSection();

            Space( BetweenSectionPadding );

            DrawDebugSection();

            if( !serializedObject.ApplyModifiedProperties() ) return;

            _localLogger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }

        private void DrawStackerSection()
        {
            DrawLabelSection( "Stacker Settings", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _totalObjectsProp );
                PropertyField( _settingsSOProp );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawVisualizationSection()
        {
            DrawLabelSection( "Visualizations", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _sceneViewVisualsModeProp, new GUIContent( "Visualization Mode" ) );
                PropertyField( _prefabProp, new GUIContent( "Model Mode Prefab" ) );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawDebugSection()
        {
            DrawLabelSection( "Debug", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;
        }

        private static void DrawSettingsSoInspector( ref bool toggle, string title, EmbedSOEditor editor )
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !editor.SettingsSOEditorIsValid() )
            {
                HelpBox( "Populate the object field to display the object's settings.", MessageType.Info );
                return;
            }

            DrawDivider( Color.gray, 2f, 19f, 3f, 12f, 8f );

            toggle = DrawFoldoutSection( title, FoldoutFrameType, toggle );
            if( !toggle ) return;

            // Space( VerticalSeparator );

            Rect foldoutFrameRect = editor.DrawSettingsSoInspector();

            // Draw editor frame
            foldoutFrameRect.xMin += ParentFrameWidth;
            foldoutFrameRect.yMax += EditorFrameBottomPadding;
            DrawRect( foldoutFrameRect, EditorFrameType, Color.gray, Color.gray, ParentFrameWidth, false );

            Space( VerticalSeparator );
        }

        protected override void DuringSceneGUILast()
        {
            // This solved the problem of the scene view not repainting until the next frame.
            // _localLogger.Log( "DuringSceneGUILast was called. Repainting scene views." );
            SceneView.RepaintAll();
        }

#endregion
    }
}