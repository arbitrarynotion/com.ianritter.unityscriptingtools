using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    public class SceneViewNoiseMapPreview : MonoBehaviour
    {
        
        private SerializedProperty _noiseSettingsSOProp;
        // Noise Settings
        private SerializedProperty _seedProp;
        private SerializedProperty _noiseOffsetHorizontalProp;
        private SerializedProperty _noiseOffsetVerticalProp;
        private SerializedProperty _noiseScaleProp;
        private SerializedProperty _octavesProp;
        private SerializedProperty _persistenceProp;
        private SerializedProperty _lacunarityProp;

        private PerlinNoiseSettingsSO _noiseSettingsSO;

        private void Initialize( SerializedProperty noiseSettingsSO )
        {
            _noiseSettingsSOProp = noiseSettingsSO;
            SerializedObject so = _noiseSettingsSOProp.serializedObject;
            _noiseSettingsSO = (PerlinNoiseSettingsSO) so.targetObject;
            
            _seedProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.seed ) );
            _noiseOffsetHorizontalProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.noiseScale ) );
            _octavesProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.octaves ) );
            _persistenceProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.persistence ) );
            _lacunarityProp = so.FindProperty( nameof( PerlinNoiseSettingsSO.lacunarity ) );
        }
        
#region SceneViewNoiseMapPreview

        // /// <summary>
        // ///     Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
        // ///     left side that indicate where the objects are sampling the noise.
        // /// </summary>
        // public void DrawSceneViewNoiseMapPreview
        // ( 
        //     SerializedProperty _noiseSettingsSOProp, 
        //     SerializedProperty _totalObjectsProp, 
        //     float NoiseMapRightMargin,
        //     float NoiseMapWidth,
        //     float NoiseMapTopMargin,
        //     float NoiseMapLabelWidth,
        //     float NoiseMapLabelRightMargin,
        //     
        // )
        // {
        //     if( _noiseSettingsSOProp.objectReferenceValue == null ) return;
        //
        //     Rect currentViewPortRect = Camera.current.pixelRect;
        //     float previewImageHeight = currentViewPortRect.height / 2f;
        //     float value = 0.25f;
        //     var outlineColor = new Color( value, value, value );
        //
        //     value = 0.15f;
        //     var backgroundColor = new Color( value, value, value );
        //
        //     var positionVector = new Vector2(
        //         currentViewPortRect.width - NoiseMapRightMargin - NoiseMapWidth,
        //
        //         // NoiseMapTopMargin + NoiseMapRightMargin + 35f
        //         NoiseMapTopMargin
        //     );
        //     var sizeVector = new Vector2(
        //         NoiseMapWidth,
        //         previewImageHeight
        //     );
        //     var positionRect = new Rect( positionVector, sizeVector );
        //
        //     DrawRectOutline( positionRect, Color.black );
        //
        //     NoiseEditorUtilities.DrawSceneViewNoiseMapPreview(
        //         positionRect,
        //         _noiseSettingsSO.GetNoiseMap2D( ),
        //         Color.black,
        //         Color.white,
        //         _totalObjectsProp.intValue,
        //         outlineColor,
        //         2f,
        //         ScaleMode.StretchToFill
        //     );
        //
        //     var labelPositionVector = new Vector2(
        //         currentViewPortRect.width - NoiseMapLabelWidth + NoiseMapLabelRightMargin,
        //
        //         // NoiseMapTopMargin + singleLineHeight + standardVerticalSpacing
        //         NoiseMapTopMargin + previewImageHeight / 2f
        //     );
        //     var labelSizeRect = new Vector2(
        //         NoiseMapLabelWidth,
        //         EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
        //     );
        //     var labelRect = new Rect( labelPositionVector, labelSizeRect );
        //
        //     Handles.BeginGUI();
        //     {
        //         GUIUtility.RotateAroundPivot( 90, labelRect.center );
        //         DrawSolidRectWithOutline( labelRect, outlineColor, backgroundColor, 2f );
        //         GUI.Label( labelRect, " Noise Meter" );
        //         GUIUtility.RotateAroundPivot( -90, labelRect.center );
        //     }
        //     Handles.EndGUI();
        // }

#endregion
    }
}
