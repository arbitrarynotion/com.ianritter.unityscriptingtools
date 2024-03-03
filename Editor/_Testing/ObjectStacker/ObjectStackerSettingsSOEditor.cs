using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.UIEditorOnlyRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors.PresetColors;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.ObjectStacker
{
    [CustomEditor( typeof( ObjectStackerSettingsSO ) )]
    public class ObjectStackerSettingsSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _seedProp;
        // private SerializedProperty _noiseMapResolutionProp;
        private SerializedProperty _showNoiseMeterProp;
        private SerializedProperty _noiseOffsetHorizontalProp;
        private SerializedProperty _noiseOffsetVerticalProp;
        private SerializedProperty _noiseScaleProp;
        private SerializedProperty _octavesProp;
        private SerializedProperty _persistenceProp;
        private SerializedProperty _lacunarityProp;
        private SerializedProperty _noiseDampeningCurveProp;
        private SerializedProperty _noiseMultiplierProp;

        private SerializedProperty _modelHeightProp;
        private SerializedProperty _verticalOffsetProp;

        private SerializedProperty _yRotOffsetProp;
        private SerializedProperty _deckYRotSkewProp;
        private SerializedProperty _topSkewCardCountProp;
        private SerializedProperty _topCardsYRotSkewProp;
        private SerializedProperty _rotationDampeningCurveProp;

        private SerializedProperty _xAxisNoiseProp;
        private SerializedProperty _yAxisNoiseProp;
        private SerializedProperty _xAxisCurveProp;
        private SerializedProperty _yAxisCurveProp;

        private SerializedProperty _faceUpProp;

        private const ElementFrameType Level0FrameType = ElementFrameType.FullOutline;
        private const ElementFrameType Level1FrameType = ElementFrameType.BottomOnly;
        private const ElementFrameType Level2FrameType = ElementFrameType.None;
        private const float TitleFrameBottomPadding = 2f;
        private const float BetweenSectionPadding = 4f;
        private const float TitleLeftEdgePadding = 4f;

        private bool _toggle = true;
        
        private bool _noiseSettingsToggle = true;
        private bool _noiseDrivenEffectsToggle = true;
        private bool _manualAdjustmentToggle = true;
        // private bool _rotationEffectsToggle = true;
        // private bool _positionEffectsToggle = true;
        // private bool _orientationToggle = true;

#region LifeCycle

        private void OnEnable()
        {
            _seedProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.seed ) );
            _showNoiseMeterProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.showNoiseMeter ) );
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.lacunarity ) );
            _noiseDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseDampeningCurve ) );
            _noiseMultiplierProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMultiplier ) );

            _modelHeightProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.modelHeight ) );
            _verticalOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.verticalOffset ) );

            _yRotOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.yRotOffset ) );
            _deckYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.deckYRotSkew ) );
            _topSkewCardCountProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topDownSkewPercent ) );
            _topCardsYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topCardsYRotSkew ) );
            _rotationDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.rotationDampeningCurve ) );

            _xAxisNoiseProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posXNoiseShift ) );
            _yAxisNoiseProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posZNoiseShift ) );            
            _xAxisCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posXNoiseCurve ) );
            _yAxisCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posZNoiseCurve ) );

            _faceUpProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.faceUp ) );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

#endregion

        private void DrawLabelSection( string titleText, ElementFrameType frameType )
        {
            DrawLabelSection( titleText, frameType, TitleLeftEdgePadding, TitleFrameBottomPadding );
        }
        
        private bool DrawFoldoutSection( string titleText, ElementFrameType frameType, bool toggle )
        {
            // Rect labelRect = GetFramedControlRect
            // ( 
            //     Color.gray, 
            //     frameType, 
            //     TitleLeftEdgePadding, 
            //     true 
            // );
            const float value = 0.19f;
            return EditorGUI.Foldout
            ( 
                GetFramedControlRect
                ( 
                    Color.gray, 
                    new Color( value, value, value ),
                    frameType, 
                    0f,
                    true
                ), 
                toggle, 
                titleText, 
                true 
            );
            // return DrawFoldoutSection( titleText, toggle, frameType, TitleLeftEdgePadding, TitleLeftEdgePadding );
        }

        private static void DrawLabelSection( string titleText, ElementFrameType frameType, float titleLeftEdgePadding, float titleUnderFramePadding )
        {
            const float value = 0.19f;
            EditorGUI.LabelField( 
                GetFramedControlRect
                ( 
                    Color.gray, 
                    new Color( value, value, value ),
                    frameType, 
                    titleLeftEdgePadding
                ), 
                titleText, 
                EditorStyles.boldLabel 
            );
            EditorGUILayout.Space( titleUnderFramePadding );
        }
        
        private static bool DrawFoldoutSection( string titleText, bool toggle, ElementFrameType frameType, float titleLeftEdgePadding, float titleUnderFramePadding )
        {
            const float value = 0.19f;
            bool result = EditorGUI.Foldout( 
                GetFramedControlRect
                ( 
                    Color.gray, 
                    new Color( value, value, value ),
                    frameType, 
                    titleLeftEdgePadding
                ), 
                toggle,
                titleText, 
                EditorStyles.boldLabel 
            );
            EditorGUILayout.Space( titleUnderFramePadding );
            return result;
        }

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                DrawNoiseSettings();
                DrawNoiseDrivenEffectsSection();
                DrawManualAdjustmentsSection();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawNoiseSettings()
        {
            _noiseSettingsToggle = DrawFoldoutSection( "Noise Settings", Level0FrameType, _noiseSettingsToggle );
            if ( !_noiseSettingsToggle ) return;
            
            EditorGUILayout.PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
            EditorGUILayout.PropertyField( _seedProp );
            EditorGUILayout.PropertyField( _noiseOffsetHorizontalProp, new GUIContent( "X Offset" ) );
            EditorGUILayout.PropertyField( _noiseOffsetVerticalProp, new GUIContent( "Y Offset" ) );
            EditorGUILayout.PropertyField( _noiseScaleProp, new GUIContent( "Scale") );
            EditorGUILayout.PropertyField( _octavesProp );
            using ( new EditorGUI.DisabledScope( _octavesProp.intValue <= 1 ) )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _persistenceProp );
                    EditorGUILayout.PropertyField( _lacunarityProp );
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space( BetweenSectionPadding );
        }

        private void DrawNoiseDrivenEffectsSection()
        {
            _noiseDrivenEffectsToggle = DrawFoldoutSection( "Noise Driven Effects", Level0FrameType, _noiseDrivenEffectsToggle );
            if ( !_noiseDrivenEffectsToggle ) return;
            
            DrawLabelSection( "Rotational Skew", Level2FrameType );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _noiseDampeningCurveProp, new GUIContent( "Intensity Curve" ) );
                EditorGUILayout.PropertyField( _noiseMultiplierProp, new GUIContent( "Multiplier") );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space( BetweenSectionPadding );
            
            DrawLabelSection( "Position Shift",Level2FrameType );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _xAxisNoiseProp, new GUIContent( "X Axis" ) );
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _xAxisCurveProp, new GUIContent( "X Curve" ) );
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.PropertyField( _yAxisNoiseProp, new GUIContent( "Z Axis" ) );
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _yAxisCurveProp, new GUIContent( "Z Curve" ) );
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space( BetweenSectionPadding );
        }

        private void DrawManualAdjustmentsSection()
        {
            _manualAdjustmentToggle = DrawFoldoutSection( "Manual Adjustments", Level0FrameType, _manualAdjustmentToggle );
            if ( !_manualAdjustmentToggle ) return;
            
            DrawLabelSection( "Rotation", Level2FrameType );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _deckYRotSkewProp, new GUIContent( "Progressive Skew" ) );
                // EditorGUILayout.LabelField( "Top Objects Skew", EditorStyles.boldLabel );
                DrawLabelSection( "Isolated Skew", Level2FrameType );
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _topSkewCardCountProp, new GUIContent( "Percent in Group") );
                    if ( _topSkewCardCountProp.floatValue >= 1f )
                        EditorGUILayout.HelpBox( "Note: if all cards are included in skew group, the rotation lock one the first object will negate whole-stack rotations." +
                                                 "You can still effect the upper cards using a dampening curve.", MessageType.Info );
                    using ( new EditorGUI.DisabledScope( _topSkewCardCountProp.floatValue <= 0f ) )
                    {
                        EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField( _topCardsYRotSkewProp, new GUIContent( "Amount") );
                            EditorGUILayout.PropertyField( _rotationDampeningCurveProp, new GUIContent( "Intensity Curve" ) );
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.PropertyField( _faceUpProp );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space( BetweenSectionPadding );
            
            DrawLabelSection( "Position", Level2FrameType );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _modelHeightProp, new GUIContent( "Model Height" ) );
                EditorGUILayout.PropertyField( _verticalOffsetProp, new GUIContent( "Vertical Gap" ) );
            }
            EditorGUI.indentLevel--;
        }
    }
}