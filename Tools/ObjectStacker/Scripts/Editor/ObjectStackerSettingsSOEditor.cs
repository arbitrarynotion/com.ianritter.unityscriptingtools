using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUtilities;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.SubscribableScriptableObject;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( ObjectStackerSettingsSO ) ), CanEditMultipleObjects]
    public class ObjectStackerSettingsSOEditor : SubscribableSOEditor
    {
        private const float SliderScaleMultiplier = 10f;
        
#region DataMembers
        
        // Noise Driven Effects
        // Y Axis Rotation
        private SerializedProperty _lockBottomObjectProp;
        private SerializedProperty _noiseMultiplierProp;
        private SerializedProperty _noiseDampeningCurveProp;

        // Position Shift
        private SerializedProperty _xAxisNoiseProp;
        private SerializedProperty _yAxisNoiseProp;
        private SerializedProperty _xAxisCurveProp;
        private SerializedProperty _yAxisCurveProp;


        // Manual Adjustments
        // Rotation
        private SerializedProperty _faceUpProp;
        private SerializedProperty _deckYRotSkewProp;
        private SerializedProperty _topSkewCardCountProp;
        private SerializedProperty _topCardsYRotSkewProp;
        private SerializedProperty _rotationDampeningCurveProp;

        // Position
        private SerializedProperty _modelHeightProp;
        private SerializedProperty _verticalOffsetProp;

        // Foldout Toggles
        private bool _stackHeightAndOffsetFoldoutToggle = true;
        private bool _noiseDrivenEffectsFoldoutToggle = false;
        private bool _manualAdjustmentFoldoutToggle = false;
        
#endregion
        
        
#region FoldoutToggles

        private bool StackHeightAndOffsetFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{GetInstanceID()}_{nameof( _stackHeightAndOffsetFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{GetInstanceID()}_{nameof( _stackHeightAndOffsetFoldoutToggle )}", value );
        }

        private bool NoiseDrivenEffectsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{GetInstanceID()}_{nameof( _noiseDrivenEffectsFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{GetInstanceID()}_{nameof( _noiseDrivenEffectsFoldoutToggle )}", value );
        }
        
        private bool ManualAdjustmentFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{GetInstanceID()}_{nameof( _manualAdjustmentFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{GetInstanceID()}_{nameof( _manualAdjustmentFoldoutToggle )}", value );
        }

#endregion
        

#region LifeCycle
        
        protected override void OnEnableLast()
        {
            LoadProperties();
        }

        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

#endregion


#region Initilization

        private void LoadProperties()
        {
            _lockBottomObjectProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.lockBottomObject ) );
            _noiseMultiplierProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMultiplier ) );
            _noiseDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseDampeningCurve ) );
            
            _xAxisNoiseProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posXNoiseShift ) );
            _yAxisNoiseProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posZNoiseShift ) );
            _xAxisCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posXNoiseCurve ) );
            _yAxisCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.posZNoiseCurve ) );
            
            _faceUpProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.faceUp ) );
            _deckYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.deckYRotSkew ) );
            _topSkewCardCountProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topDownSkewPercent ) );
            _topCardsYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topObjectsYRotSkew ) );
            _rotationDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.rotationDampeningCurve ) );

            _modelHeightProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.modelHeight ) );
            _verticalOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.verticalOffset ) );
        }

#endregion


#region DrawInspectorUI

        private static void DrawLabeledSection( string titleText, ElementFrameType frameType ) => 
            DrawLabelSection( titleText, frameType );

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                serializedObject.DrawScriptField();
                DrawManualPositionSection();
                DrawNoiseDrivenEffectsSection();
                DrawManualAdjustmentsSection();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawManualPositionSection()
        {
            StackHeightAndOffsetFoldoutToggle = DrawFoldoutSection( "Stack Height & Offset", SubFoldoutFrameType, StackHeightAndOffsetFoldoutToggle );
            if( !StackHeightAndOffsetFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _modelHeightProp, new GUIContent( "Model Height" ) );
                DrawVariableMinMaxFloatSlider( _verticalOffsetProp, _modelHeightProp );
            }
            EditorGUI.indentLevel--;
        }

#region NoiseDrivenEffects

        private void DrawNoiseDrivenEffectsSection()
        {
            NoiseDrivenEffectsFoldoutToggle = DrawFoldoutSection( "Noise-Driven Effects", SubFoldoutFrameType, NoiseDrivenEffectsFoldoutToggle );
            if( !NoiseDrivenEffectsFoldoutToggle ) return;

            EditorGUI.indentLevel++;
            {
                DrawYAxisRotationalSection();
                Space( BetweenSectionPadding );
                DrawPositionShiftSection();

                // Display note when noise effects are all set to 0 so the use knows why nothing is happening when they change the noise settings.
                if( _noiseMultiplierProp.floatValue <= 0f &&
                    _xAxisNoiseProp.floatValue <= 0f &&
                    _yAxisNoiseProp.floatValue <= 0f )
                    HelpBox( "Note: Noise has no effect when all intensities are set to 0.", MessageType.Info );

                Space( BetweenSectionPadding );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawYAxisRotationalSection()
        {
            DrawLabeledSection( "Y Axis Rotationa", LabelHeadingFrameType );
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _lockBottomObjectProp, new GUIContent( "Lock Bottom", "Forces the bottom object to remain in place. " +
                                                                                     "This will cause all rotation and position shift to center their effect over the bottom object." ) );
                PropertyField( _noiseMultiplierProp, new GUIContent( "Intensity" ) );
                using ( new EditorGUI.DisabledScope( _noiseMultiplierProp.floatValue <= 0f ) )
                {
                    EditorGUI.indentLevel++;
                    {
                        PropertyField( _noiseDampeningCurveProp, new GUIContent( "Dampener" ) );
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel--;
        }

        private void DrawPositionShiftSection()
        {
            DrawLabeledSection( "Position Shift", LabelHeadingFrameType );
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _xAxisNoiseProp, new GUIContent( "X Intensity" ) );
                using ( new EditorGUI.DisabledScope( _xAxisNoiseProp.floatValue <= 0f ) )
                {
                    EditorGUI.indentLevel++;
                    {
                        PropertyField( _xAxisCurveProp, new GUIContent( "Dampener" ) );
                    }
                    EditorGUI.indentLevel--;
                }

                PropertyField( _yAxisNoiseProp, new GUIContent( "Z Intensity" ) );
                using ( new EditorGUI.DisabledScope( _yAxisNoiseProp.floatValue <= 0f ) )
                {
                    EditorGUI.indentLevel++;
                    {
                        PropertyField( _yAxisCurveProp, new GUIContent( "Dampener" ) );
                    }
                    EditorGUI.indentLevel--;

                }
            }
            EditorGUI.indentLevel--;
        }

#endregion


#region ManualAdjustments

        private void DrawManualAdjustmentsSection()
        {
            ManualAdjustmentFoldoutToggle = DrawFoldoutSection( "Manual Adjustments", SubFoldoutFrameType, ManualAdjustmentFoldoutToggle );
            if( !ManualAdjustmentFoldoutToggle ) return;
            EditorGUI.indentLevel++;
            {
                DrawManualRotationSection();
                Space( BetweenSectionPadding );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawManualRotationSection()
        {
            DrawLabeledSection( "Rotation", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _faceUpProp, new GUIContent( "Flip Over" ) );
                PropertyField( _deckYRotSkewProp, new GUIContent( "Y Axis Twist" ) );
                DrawLabeledSection( "Subgroup Skew (Top-down)", SubLabelHeadingFrameType );
                EditorGUI.indentLevel++;
                {
                    PropertyField( _topSkewCardCountProp, new GUIContent( "Group Size" ) );
                    if( _topSkewCardCountProp.floatValue >= 1f )
                    {
                        HelpBox( "Note: if all cards are included in skew group, the rotation lock one the first object will negate whole-stack rotations." +
                                 "You can still effect the upper cards using a dampening curve.", MessageType.Info );
                    }

                    using ( new EditorGUI.DisabledScope( _topSkewCardCountProp.floatValue <= 0f ) )
                    {
                        PropertyField( _topCardsYRotSkewProp, new GUIContent( "Intensity" ) );
                        using ( new EditorGUI.DisabledScope( Math.Abs( _topCardsYRotSkewProp.floatValue ) < 0.0001f ) )
                        {
                            EditorGUI.indentLevel++;
                            {
                                PropertyField( _rotationDampeningCurveProp, new GUIContent( "Dampener" ) );
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

#endregion
        

        private void DrawVariableMinMaxFloatSlider( SerializedProperty valueProperty, SerializedProperty maxProperty )
        {
            // PropertyField( _verticalOffsetProp, new GUIContent( "Vertical Gap" ) );
            Rect sliderRect = GetControlRect();
            valueProperty.floatValue = EditorGUI.Slider(
                sliderRect,
                "Vertical Gap",
                valueProperty.floatValue,
                0f,
                maxProperty.floatValue * SliderScaleMultiplier
            );
        }

        private void DrawVariableMinMaxFloatSlider( SerializedProperty valueProperty, SerializedProperty minProperty, SerializedProperty maxProperty )
        {
            // PropertyField( _verticalOffsetProp, new GUIContent( "Vertical Gap" ) );
            Rect sliderRect = GetControlRect();
            valueProperty.floatValue = EditorGUI.Slider(
                sliderRect,
                "Vertical Gap",
                valueProperty.floatValue,
                minProperty.floatValue * SliderScaleMultiplier,
                maxProperty.floatValue * SliderScaleMultiplier
            );
        }

#endregion
    }
}