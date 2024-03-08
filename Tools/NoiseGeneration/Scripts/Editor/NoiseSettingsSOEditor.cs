using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( NoiseSettingsSO ) )]
    public class NoiseSettingsSOEditor : UnityEditor.Editor
    {
#region DataMembers

        // Noise Settings
        private SerializedProperty _seedProp;
        private SerializedProperty _noiseOffsetHorizontalProp;
        private SerializedProperty _noiseOffsetVerticalProp;
        private SerializedProperty _noiseScaleProp;
        private SerializedProperty _octavesProp;
        private SerializedProperty _persistenceProp;
        private SerializedProperty _lacunarityProp;
        
        // Noise map visualization
        private SerializedProperty _showNoiseMeterProp;
        private SerializedProperty _noiseMapTopMarginProp;
        private SerializedProperty _noiseMapRightMarginProp;
        private SerializedProperty _noiseMapWidthProp;
        private SerializedProperty _noiseMapLabelWidthProp;
        private SerializedProperty _noiseMapLabelRightMarginProp;

        private bool _noiseSettingsToggle = true;
        private bool _noisePreviewSettingsToggle = true;

        private NoiseSettingsSO _noiseSettingsSO;

#endregion

#region LifeCycle

        private void OnEnable()
        {
            _noiseSettingsSO = target as NoiseSettingsSO;

            _seedProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.seed ) );
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.lacunarity ) );
            
            _showNoiseMeterProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.showNoiseMeter ) );
            _noiseMapTopMarginProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseMapTopMargin ) );
            _noiseMapRightMarginProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseMapRightMargin ) );
            _noiseMapWidthProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseMapWidth ) );
            _noiseMapLabelWidthProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseMapLabelWidth ) );
            _noiseMapLabelRightMarginProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseMapLabelRightMargin ) );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }
        
        // protected void OnSceneGUI()
        // {
        //     if( _noiseSettingsSO == null ) return;
        //
        //     if( !_showNoiseMeterProp.boolValue ) return;
        //
        //     DrawSceneViewNoiseMapPreview();
        // }

#endregion


#region DrawInspectorUI

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                DrawNoiseSettings();
                // DrawNoisePreviewSettingsSection();
            }
            EditorGUI.indentLevel--;
        }


        private void DrawNoiseSettings()
        {
            _noiseSettingsToggle = DrawFoldoutSection( "Noise Settings", Level0FrameType, _noiseSettingsToggle );
            if( !_noiseSettingsToggle ) return;

            PropertyField( _seedProp );
            PropertyField( _noiseOffsetHorizontalProp, new GUIContent( "X Offset" ) );
            PropertyField( _noiseOffsetVerticalProp, new GUIContent( "Y Offset" ) );
            PropertyField( _noiseScaleProp, new GUIContent( "Scale" ) );
            PropertyField( _octavesProp );
            using ( new EditorGUI.DisabledScope( _octavesProp.intValue <= 1 ) )
            {
                EditorGUI.indentLevel++;
                {
                    PropertyField( _persistenceProp );
                    PropertyField( _lacunarityProp );
                }
                EditorGUI.indentLevel--;
            }

            Space( BetweenSectionPadding );
        }
        
        private void DrawNoisePreviewSettingsSection()
        {
            _noisePreviewSettingsToggle = DrawFoldoutSection( "Noise Meter Settings", Level0FrameType, _noisePreviewSettingsToggle );
            if( !_noisePreviewSettingsToggle ) return;
            PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
            PropertyField( _noiseMapTopMarginProp, new GUIContent( "Top Margin" ) );
            PropertyField( _noiseMapRightMarginProp, new GUIContent( "Right Margin" ) );
            PropertyField( _noiseMapWidthProp, new GUIContent( "Width" ) );
            PropertyField( _noiseMapLabelWidthProp, new GUIContent( "Label Width" ) );
            PropertyField( _noiseMapLabelRightMarginProp, new GUIContent( "Label Right Margin" ) );

            Space( BetweenSectionPadding );
        }

#endregion


// #region SceneViewNoiseMapPreview
//
//         /// <summary>
//         ///     Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
//         ///     left side that indicate where the objects are sampling the noise.
//         /// </summary>
//         private void DrawSceneViewNoiseMapPreview()
//         {
//             Rect currentViewPortRect = Camera.current.pixelRect;
//             float previewImageHeight = currentViewPortRect.height / 2f;
//             float value = 0.25f;
//             var outlineColor = new Color( value, value, value );
//
//             value = 0.15f;
//             var backgroundColor = new Color( value, value, value );
//
//             var positionVector = new Vector2(
//                 // currentViewPortRect.width - NoiseMapRightMargin - NoiseMapWidth,
//                 currentViewPortRect.width - _noiseMapRightMarginProp.floatValue - _noiseMapWidthProp.floatValue,
//
//                 // NoiseMapTopMargin
//                 _noiseMapTopMarginProp.floatValue
//             );
//             var sizeVector = new Vector2(
//                 // NoiseMapWidth,
//                 _noiseMapWidthProp.floatValue,
//                 previewImageHeight
//             );
//             var positionRect = new Rect( positionVector, sizeVector );
//
//             DrawRectOutline( positionRect, Color.black );
//
//             NoiseEditorUtilities.DrawSceneViewNoiseMapPreview(
//                 positionRect,
//                 _noiseSettingsSO.GetNoiseMap2D(),
//                 Color.black,
//                 Color.white,
//                 _totalObjectsProp.intValue,
//                 outlineColor,
//                 2f,
//                 ScaleMode.StretchToFill
//             );
//
//             var labelPositionVector = new Vector2(
//                 // currentViewPortRect.width - NoiseMapLabelWidth + NoiseMapLabelRightMargin,
//                 currentViewPortRect.width - _noiseMapLabelWidthProp.floatValue + _noiseMapLabelRightMarginProp.floatValue,
//
//                 // NoiseMapTopMargin + singleLineHeight + standardVerticalSpacing
//                 // NoiseMapTopMargin + previewImageHeight / 2f
//                 _noiseMapTopMarginProp.floatValue + previewImageHeight / 2f
//             );
//             var labelSizeRect = new Vector2(
//                 // NoiseMapLabelWidth,
//                 _noiseMapLabelWidthProp.floatValue,
//                 EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
//             );
//             var labelRect = new Rect( labelPositionVector, labelSizeRect );
//
//             Handles.BeginGUI();
//             {
//                 GUIUtility.RotateAroundPivot( 90, labelRect.center );
//                 DrawSolidRectWithOutline( labelRect, outlineColor, backgroundColor, 2f );
//                 GUI.Label( labelRect, " Noise Meter" );
//                 GUIUtility.RotateAroundPivot( -90, labelRect.center );
//             }
//             Handles.EndGUI();
//         }
//
// #endregion
    }
}