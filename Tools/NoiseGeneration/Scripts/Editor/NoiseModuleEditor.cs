using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
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
#region Constants

        private const float MinRightMargin = 40f;
        private const float LabelSidePadding = 4f;
        private const float FrameWidth = 2f;

        private const string Title = "Noise Meter";

        // Keep the image preview scaling from getting any skinner than this.
        private const float ImageMinWidth = 20f;

#endregion


#region DataMembers

        private SerializedProperty _noiseSettingsSOProp;

        private SerializedProperty _loggerProp;

        // Noise map visualization
        private SerializedProperty _showNoiseMeterProp;
        private SerializedProperty _mapScaleModeProp;
        private SerializedProperty _highColorProp;
        private SerializedProperty _lowColorProp;
        private SerializedProperty _textColorProp;
        private SerializedProperty _frameColorProp;
        private SerializedProperty _backgroundColorProp;
        
        
        private SerializedProperty _frameWidthProp;
        private SerializedProperty _mapPreviewTopMarginProp;
        private SerializedProperty _mapPreviewBottomMarginProp;
        private SerializedProperty _mapPreviewRightMarginProp;
        private SerializedProperty _mapPreviewWidthProp;
        private SerializedProperty _mapPreviewHeightProp;

        private NoiseModule _noiseModule;
        private PerlinNoiseSettingsSO _noiseSettingsSO;

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

        private bool NoisePreviewSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( NoisePreviewSettingsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( NoisePreviewSettingsFoldoutToggle )}", value );
        }

        private bool AdvancedSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( AdvancedSettingsFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{name}_{nameof( AdvancedSettingsFoldoutToggle )}", value );
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

            _noiseModule = target as NoiseModule;

            // LoadFoldoutToggles();
            LoadProperties();

            _settingsEmbeddedSOEditor = new EmbedSOEditor( _noiseSettingsSOProp );

            InitializeEmbeddedEditors();

            _logger.LogEnd();
        }

        private void OnDisable()
        {
            // SaveFoldoutToggles();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            DrawNoiseSettingsSOSection();
            DrawNoisePreviewSettingsSection();
            DrawDebugSections();

            if( !serializedObject.ApplyModifiedProperties() ) return;

            // _noiseModule.OnDataChanged();

            // _logger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }

        protected void OnSceneGUI()
        {
            if( _noiseModule == null ) return;

            if( _noiseSettingsSOProp?.objectReferenceValue == null ) return;

            if( _showNoiseMeterProp == null ) return;

            if( !_showNoiseMeterProp.boolValue ) return;

            DrawSceneViewNoiseMapPreview
            (
                _noiseModule.Get2DNoiseMap(),
                (ScaleMode)_mapScaleModeProp.intValue,
                _mapPreviewWidthProp.floatValue,
                _mapPreviewHeightProp.floatValue,
                _frameWidthProp.floatValue,
                _mapPreviewTopMarginProp.floatValue,
                _mapPreviewBottomMarginProp.floatValue,
                _mapPreviewRightMarginProp.floatValue,
                _highColorProp.colorValue,
                _lowColorProp.colorValue,
                _textColorProp.colorValue,
                _frameColorProp.colorValue,
                _backgroundColorProp.colorValue
            );
        }

#endregion


#region Initialization

        // private void LoadFoldoutToggles()
        // {
        //     // Load the foldout states from EditorPrefs.
        //     _showNoiseSettings = EditorPrefs.GetBool( nameof( _showNoiseSettings ), true );
        //     _noisePreviewSettingsToggle = EditorPrefs.GetBool( nameof( _noisePreviewSettingsToggle ), true );
        //     _debugToggle = EditorPrefs.GetBool( nameof( _debugToggle ), false );
        //     _advancedSettingsToggle = EditorPrefs.GetBool( nameof( _advancedSettingsToggle ), false );
        // }
        //
        // private void SaveFoldoutToggles()
        // {
        //     // Save the foldout states to EditorPrefs to preserve their state.
        //     EditorPrefs.SetBool( nameof( _showNoiseSettings ), _showNoiseSettings );
        //     EditorPrefs.SetBool( nameof( _noisePreviewSettingsToggle ), _noisePreviewSettingsToggle );
        //     EditorPrefs.SetBool( nameof( _debugToggle ), _debugToggle );
        //     EditorPrefs.SetBool( nameof( _advancedSettingsToggle ), _advancedSettingsToggle );
        // }

        private void LoadProperties()
        {
            _noiseSettingsSOProp = serializedObject.FindProperty( "noiseSettingsSo" );
            _loggerProp = serializedObject.FindProperty( "logger" );

            _showNoiseMeterProp = serializedObject.FindProperty( "showNoiseMeter" );
            _mapScaleModeProp = serializedObject.FindProperty( "mapScaleMode" );
            _highColorProp = serializedObject.FindProperty( "highColor" );
            _lowColorProp = serializedObject.FindProperty( "lowColor" );
            _textColorProp = serializedObject.FindProperty( "textColor" );
            _frameColorProp = serializedObject.FindProperty( "frameColor" );
            _backgroundColorProp = serializedObject.FindProperty( "backgroundColor" );

            _frameWidthProp = serializedObject.FindProperty( "frameWidth" );
            _mapPreviewTopMarginProp = serializedObject.FindProperty( "mapPreviewTopMargin" );
            _mapPreviewBottomMarginProp = serializedObject.FindProperty( "mapPreviewBottomMargin" );
            _mapPreviewRightMarginProp = serializedObject.FindProperty( "mapPreviewRightMargin" );
            _mapPreviewWidthProp = serializedObject.FindProperty( "mapPreviewWidth" );
            _mapPreviewHeightProp = serializedObject.FindProperty( "mapPreviewHeight" );
        }

        private void InitializeEmbeddedEditors() => _settingsEmbeddedSOEditor.OnEnable();

#endregion


#region DrawInspectorGUI

        private void DrawDebugSections()
        {
            DebugFoldoutToggle = DrawFoldoutSection( "Debug", FoldoutFrameType, DebugFoldoutToggle );
            if( !DebugFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawNoiseSettingsSOSection()
        {
            PropertyField( _noiseSettingsSOProp );
            _settingsEmbeddedSOEditor.DrawSettingsSoInspector( "Noise Settings SO" );
            if( ( _noiseSettingsSOProp.objectReferenceValue != null ) && NoiseSettingsFoldoutToggle )
                Space( VerticalSeparator );
        }

        private void DrawNoisePreviewSettingsSection()
        {
            NoisePreviewSettingsFoldoutToggle = DrawFoldoutSection( "Noise Meter Settings", FoldoutFrameType, NoisePreviewSettingsFoldoutToggle );
            if( !NoisePreviewSettingsFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
                
                DrawPreviewSizeSection();
                Space( BetweenSectionPadding );
                DrawAdvancedSettingsSection();
                Space( BetweenSectionPadding );
            }
            EditorGUI.indentLevel--;

        }

        private void DrawAdvancedSettingsSection()
        {
            AdvancedSettingsFoldoutToggle = DrawFoldoutSection( "Advanced Settings", SubFoldoutFrameType, AdvancedSettingsFoldoutToggle );
            if( !AdvancedSettingsFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                DrawPreviewColorsSection();

                Space( BetweenSectionPadding );

                DrawPreviewMarginsSection();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawPreviewColorsSection()
        {
            DrawLabelSection( "Colors", SubLabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                ColorPickerHandler.DrawPropertyWithColorPicker( _highColorProp, new GUIContent( "High" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _lowColorProp, new GUIContent( "Low" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _textColorProp, new GUIContent( "Text" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _frameColorProp, new GUIContent( "Frame" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _backgroundColorProp, new GUIContent( "Background" ) );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawPreviewSizeSection()
        {
            DrawLabelSection( "Size", SubLabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _mapPreviewWidthProp, new GUIContent( "Width", "This is the percentage of the height." ) );
                PropertyField( _mapPreviewHeightProp, new GUIContent( "Height", "This is the percentage of the available screen height, minus both top and bottom margins." ) );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawPreviewMarginsSection()
        {
            DrawLabelSection( "Layout", SubLabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _frameWidthProp, new GUIContent( "Frame Width" ) );
                PropertyField( _mapPreviewTopMarginProp, new GUIContent( "Top" ) );
                PropertyField( _mapPreviewBottomMarginProp, new GUIContent( "Bottom" ) );
                PropertyField( _mapPreviewRightMarginProp, new GUIContent( "Right" ) );
            }
            EditorGUI.indentLevel--;
        }

#endregion


#region SceneViewNoiseMapPreview

        /// <summary>
        ///     Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
        ///     left side that indicate where the objects are sampling the noise.
        /// </summary>
        private static void DrawSceneViewNoiseMapPreview
            (
                float[,] noiseMap,
                ScaleMode scaleMode,
                float previewWidth,
                float previewHeight,
                float frameWidth,
                float topMargin,
                float bottomMargin,
                float rightMargin,
                Color highColor,
                Color lowColor,
                Color textColor,
                Color frameColor,
                Color backgroundColor
            )
        {
            Rect currentViewPortRect = Camera.current.pixelRect;

            // Calculate the height of the preview image.
            float height = currentViewPortRect.height - topMargin - bottomMargin;
            float previewImageHeight = Mathf.Min( height, height * previewHeight );

            Rect previewImageRect = GetPreviewImageRect
            (
                currentViewPortRect,
                previewImageHeight,
                previewWidth,
                topMargin,
                rightMargin
            );

            DrawRectOutline( previewImageRect, Color.black );

            NoiseEditorUtilities.DrawSceneViewNoiseMapPreview(
                previewImageRect,
                noiseMap,
                lowColor,
                highColor,
                frameColor,
                frameWidth,
                scaleMode
            );

            Rect labelRect = GetPreviewLabelRect
            (
                Title,
                currentViewPortRect,
                previewImageHeight,
                topMargin,
                rightMargin,
                frameWidth
            );

            Handles.BeginGUI();
            {
                GUIUtility.RotateAroundPivot( 90, labelRect.center );
                DrawSolidRectWithOutline( labelRect, frameColor, backgroundColor, FrameWidth );
                labelRect.xMin += LabelSidePadding;
                var style = new GUIStyle( EditorStyles.label )
                {
                    normal = { textColor = textColor }
                };
                GUI.Label( labelRect, Title, style );
                GUIUtility.RotateAroundPivot( -90, labelRect.center );
            }
            Handles.EndGUI();
        }

        private static Rect GetPreviewImageRect
            (
                Rect currentViewPortRect,
                float previewImageHeight,
                float previewWidth,
                float topMargin,
                float rightMargin
            )
        {
            float width = Mathf.Max( previewImageHeight * previewWidth, ImageMinWidth );

            var positionVector = new Vector2(
                currentViewPortRect.width - rightMargin - width - 2f,
                topMargin
            );
            var sizeVector = new Vector2(
                width,
                previewImageHeight
            );
            return new Rect( positionVector, sizeVector );
        }

        private static Rect GetPreviewLabelRect
            (
                string title,
                Rect currentViewPortRect,
                float previewImageHeight,
                float topMargin,
                float rightMargin,
                float frameWidth
            )
        {
            var myStyle = new GUIStyle( EditorStyles.label );
            Vector2 titleSize = myStyle.CalcSize( new GUIContent( title ) );
            float labelRectWidth = titleSize.x + ( 2 * LabelSidePadding );

            var labelPositionVector = new Vector2(
                ( currentViewPortRect.width - labelRectWidth - rightMargin ) + MinRightMargin,
                ( topMargin + ( previewImageHeight / 2f ) ) - ( EditorGUIUtility.singleLineHeight / 2f ) - frameWidth
            );

            var labelSizeRect = new Vector2
            (
                labelRectWidth,
                titleSize.y
            );

            return new Rect( labelPositionVector, labelSizeRect );
        }

#endregion
    }
}