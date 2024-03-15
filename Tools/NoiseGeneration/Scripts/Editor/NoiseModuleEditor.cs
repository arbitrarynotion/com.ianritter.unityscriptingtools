using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
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

#region DataMembers

        private SerializedProperty _noiseSettingsSOProp;
        private SerializedProperty _targetLoggerProp;

        private UnityEditor.Editor _noiseSettingsSOEditor;
        private Object _noiseSettingsSOEditorTarget;
        private EmbedSOEditor _settingsEmbeddedSOEditor;
        private FormattedLogger _logger;
        
#endregion


#region FoldoutToggles

        private bool NoiseSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( NoiseSettingsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( NoiseSettingsFoldoutToggle )}", value );
        }

        private bool DebugFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( DebugFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{name}_{nameof( DebugFoldoutToggle )}", value );
        }

#endregion


#region LifeCycle

        private void OnEnable()
        {
            if( _logger == null )
                _logger = FormattedLoggerLoader.GetLogger( NoiseModuleELoggerName, DefaultLoggerPath );

            _logger.LogStart();

            // _noiseModule = target as NoiseModule;

            LoadProperties();

            _settingsEmbeddedSOEditor = new EmbedSOEditor( _noiseSettingsSOProp );

            InitializeEmbeddedEditors();

            _logger.LogEnd();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            DrawNoiseSettingsSOSection();
            // DrawNoisePreviewSettingsSection();
            DrawDebugSections();

            if( !serializedObject.ApplyModifiedProperties() ) return;

            // _noiseModule.OnDataChanged();

            // _logger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }

#endregion


#region Initialization

        private void LoadProperties()
        {
            _noiseSettingsSOProp = serializedObject.FindProperty( "noiseSettingsSo" );
            _targetLoggerProp = serializedObject.FindProperty( "logger" );
        }

        private void InitializeEmbeddedEditors() => _settingsEmbeddedSOEditor.OnEnable();

#endregion


#region DrawInspectorGUI

        private void DrawNoiseSettingsSOSection()
        {
            PropertyField( _noiseSettingsSOProp );
            _settingsEmbeddedSOEditor.DrawSettingsSoInspector( "Noise Settings SO" );
            if( ( _noiseSettingsSOProp.objectReferenceValue != null ) && NoiseSettingsFoldoutToggle )
                Space( VerticalSeparator );
        }

        private void DrawDebugSections()
        {
            DebugFoldoutToggle = DrawFoldoutSection( "Debug", FoldoutFrameType, DebugFoldoutToggle );
            if( !DebugFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _targetLoggerProp );
            }
            EditorGUI.indentLevel--;
        }

#endregion
        
    }
}