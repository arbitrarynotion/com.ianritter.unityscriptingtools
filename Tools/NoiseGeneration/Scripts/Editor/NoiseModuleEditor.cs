using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( NoiseModule ) )]
    public class NoiseModuleEditor : UnityEditor.Editor
    {
        private SerializedProperty _noiseSettingsSOProp;
        
        private UnityEditor.Editor _noiseSettingsSOEditor;
        private Object _noiseSettingsSOEditorTarget;
        private EmbedSOEditor _noiseSettingsEmbeddedSOEditor;
        private FormattedLogger _logger;

        private bool _showNoiseSettings = true;

        // UI Formatting
        // private const float VerticalSeparator = 8f;
        private const float ParentFrameWidth = 2f;
        private const float ChildFrameWidth = 2f;
        // private const float FoldoutFramePadding = 2f;
        private const float EditorFrameBottomPadding = 10f;
        private const ElementFrameType FoldoutFrameType = ElementFrameType.LeftOnly;
        private const ElementFrameType EditorFrameType = ElementFrameType.PartialLeftFullBottom;

        private void OnEnable()
        {
            if( _logger == null )
                _logger = FormattedLoggerLoader.GetLogger( NoiseModuleELoggerName, DefaultLoggerPath );

            _logger.LogStart();
            
            _noiseSettingsSOProp = serializedObject.FindProperty( "settingsSo" );
            _noiseSettingsEmbeddedSOEditor = new EmbedSOEditor( _noiseSettingsSOProp );
            
            InitializeEmbeddedEditors();
        }
        
        private void InitializeEmbeddedEditors()
        {
            _noiseSettingsEmbeddedSOEditor.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            
            PropertyField( _noiseSettingsSOProp );
            DrawSettingsSoInspector( ref _showNoiseSettings, "Noise Settings", _noiseSettingsEmbeddedSOEditor );
            if ( _noiseSettingsSOProp.objectReferenceValue != null && _showNoiseSettings )
                Space( VerticalSeparator );
            
            if( !serializedObject.ApplyModifiedProperties() ) return;

            _logger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }
        
        private static void DrawSettingsSoInspector( ref bool toggle, string title, EmbedSOEditor editor )
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !editor.SettingsSOEditorIsValid() )
            {
                HelpBox( "Populate the object field to display the object's settings.", MessageType.Info );
                return;
            }

            // Draw Foldout with frame.
            // EditorGUI.indentLevel++;
            // Rect labelRect = GetFramedControlRect( Color.gray, FoldoutFrameType, 0f, true );
            // toggle = EditorGUI.Foldout( labelRect, toggle, title, true );
            // if ( !toggle ) return;
            toggle = DrawFoldoutSection( title, FoldoutFrameType, toggle );
            if( !toggle ) return;

            // EditorGUI.indentLevel--;


            // Space( VerticalSeparator );

            Rect foldoutFrameRect = editor.DrawSettingsSoInspector();
            foldoutFrameRect.xMin += ParentFrameWidth;
            foldoutFrameRect.yMax += EditorFrameBottomPadding;
            DrawRect( foldoutFrameRect, EditorFrameType, Color.gray, Color.gray, ChildFrameWidth, false );

            Space( VerticalSeparator );
        }
    }
}
