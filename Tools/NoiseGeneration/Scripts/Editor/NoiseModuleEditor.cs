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
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.UI.EditorDividerGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( NoiseModule ) )]
    public class NoiseModuleEditor : UnityEditor.Editor
    {
        private const float MinRightMargin = 40f;
        private const float LabelSidePadding = 4f;
        private const float FrameWidth = 2f;
        private const string Title = "Noise Meter";
        // Keep the image preview scaling from getting any skinner than this.
        private const float ImageMinWidth = 20f;

        private SerializedProperty _noiseSettingsSOProp;
        private SerializedProperty _noiseMapWidthProp;
        private SerializedProperty _noiseMapHeightProp;
        private SerializedProperty _loggerProp;
        
        // Noise map visualization
        private SerializedProperty _showNoiseMeterProp;
        private SerializedProperty _mapPreviewTopMarginProp;
        private SerializedProperty _mapPreviewBottomMarginProp;
        private SerializedProperty _mapPreviewRightMarginProp;
        private SerializedProperty _mapPreviewWidthProp;
        private SerializedProperty _mapPreviewHeightProp;

        private NoiseModule _noiseModule;
        private NoiseSettingsSO _noiseSettingsSO;
        
        private UnityEditor.Editor _noiseSettingsSOEditor;
        private Object _noiseSettingsSOEditorTarget;
        private EmbedSOEditor _noiseSettingsEmbeddedSOEditor;
        private FormattedLogger _logger;

        private bool _showNoiseSettings = true;
        private bool _noisePreviewSettingsToggle = true;

#region LifeCycle

        private void OnEnable()
        {
            if( _logger == null )
                _logger = FormattedLoggerLoader.GetLogger( NoiseModuleELoggerName, DefaultLoggerPath );

            _logger.LogStart();
            
            _noiseModule = target as NoiseModule;

            LoadProperties();
            
            _noiseSettingsEmbeddedSOEditor = new EmbedSOEditor( _noiseSettingsSOProp );
            
            InitializeEmbeddedEditors();

            _logger.LogEnd();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            PropertyField( _noiseSettingsSOProp );
            DrawSettingsSoInspector( ref _showNoiseSettings, "Noise Settings SO", _noiseSettingsEmbeddedSOEditor );
            if ( _noiseSettingsSOProp.objectReferenceValue != null && _showNoiseSettings )
                Space( VerticalSeparator );
            
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
                _mapPreviewWidthProp.floatValue,
                _mapPreviewHeightProp.floatValue,
                _mapPreviewTopMarginProp.floatValue,
                _mapPreviewBottomMarginProp.floatValue,
                _mapPreviewRightMarginProp.floatValue
            );
        }

#endregion


#region Initialization

        private void LoadProperties()
        {
            _noiseSettingsSOProp = serializedObject.FindProperty( "noiseSettingsSo" );
            _noiseMapWidthProp = serializedObject.FindProperty( "noiseMapWidth" );
            _noiseMapHeightProp = serializedObject.FindProperty( "noiseMapHeight" );
            _loggerProp = serializedObject.FindProperty( "logger" );
            
            
            _showNoiseMeterProp = serializedObject.FindProperty( "showNoiseMeter" );
            _mapPreviewTopMarginProp = serializedObject.FindProperty( "mapPreviewTopMargin" );
            _mapPreviewBottomMarginProp = serializedObject.FindProperty( "mapPreviewBottomMargin" );
            _mapPreviewRightMarginProp = serializedObject.FindProperty( "mapPreviewRightMargin" );
            _mapPreviewWidthProp = serializedObject.FindProperty( "mapPreviewWidth" );
            _mapPreviewHeightProp = serializedObject.FindProperty( "mapPreviewHeight" );
        }

        private void InitializeEmbeddedEditors()
        {
            _noiseSettingsEmbeddedSOEditor.OnEnable();
        }

#endregion


#region InspectorDrawing

        private void DrawDebugSections()
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
            foldoutFrameRect.xMin += ParentFrameWidth;
            foldoutFrameRect.yMax += EditorFrameBottomPadding;
            DrawRect( foldoutFrameRect, EditorFrameType, Color.gray, Color.gray, ChildFrameWidth, false );

            Space( VerticalSeparator );
        }
        
        private void DrawNoisePreviewSettingsSection()
        {
            _noisePreviewSettingsToggle = DrawFoldoutSection( "Noise Meter Settings", SubFoldoutFrameType, _noisePreviewSettingsToggle );
            if( !_noisePreviewSettingsToggle ) return;
            EditorGUI.indentLevel++;
            {
                PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
                DrawLabelSection( "Size", SubLabelHeadingFrameType );
                EditorGUI.indentLevel++;
                {
                    PropertyField( _mapPreviewWidthProp, new GUIContent( "Width", "This is the percentage of the height." ) );
                    PropertyField( _mapPreviewHeightProp, new GUIContent( "Height", "This is the percentage of the available screen height, minus both top and bottom margins." ) );
                }
                EditorGUI.indentLevel--;
                
                Space(BetweenSectionPadding);
                
                DrawLabelSection( "Margins", SubLabelHeadingFrameType );
                EditorGUI.indentLevel++;
                {
                    PropertyField( _mapPreviewTopMarginProp, new GUIContent( "Top" ) );
                    PropertyField( _mapPreviewBottomMarginProp, new GUIContent( "Bottom" ) );
                    PropertyField( _mapPreviewRightMarginProp, new GUIContent( "Right" ) );
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        
            Space( BetweenSectionPadding );
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
                float previewWidth,
                float previewHeight,
                float topMargin,
                float bottomMargin,
                float rightMargin
            )
        {
            Rect currentViewPortRect = Camera.current.pixelRect;

            // Calculate the height of the preview image.
            float height = currentViewPortRect.height - topMargin - bottomMargin;
            float previewImageHeight = Mathf.Min( height, height * previewHeight );

            float value = 0.25f;
            var outlineColor = new Color( value, value, value );
            value = 0.15f;
            var backgroundColor = new Color( value, value, value );

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
                Color.black,
                Color.white,
                outlineColor,
                FrameWidth,
                ScaleMode.StretchToFill
                // ScaleMode.ScaleAndCrop
            );

            Rect labelRect = GetPreviewLabelRect
            ( 
                Title,
                currentViewPortRect, 
                previewImageHeight,
                topMargin,
                rightMargin,
                // labelWidth,
                FrameWidth
            );
        
            Handles.BeginGUI();
            {
                GUIUtility.RotateAroundPivot( 90, labelRect.center );
                DrawSolidRectWithOutline( labelRect, outlineColor, backgroundColor, 2f );
                labelRect.xMin += LabelSidePadding;
                GUI.Label( labelRect, Title );
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
            float labelRectWidth = titleSize.x + ( 2 * ( LabelSidePadding + 2f ) );
            
            var labelPositionVector = new Vector2(
                currentViewPortRect.width - labelRectWidth - rightMargin + MinRightMargin,
                topMargin + previewImageHeight / 2f - ( EditorGUIUtility.singleLineHeight / 2f) - frameWidth
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
