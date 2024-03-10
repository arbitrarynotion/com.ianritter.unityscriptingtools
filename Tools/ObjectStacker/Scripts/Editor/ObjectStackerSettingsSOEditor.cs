using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( ObjectStackerSettingsSO ) )]
    public class ObjectStackerSettingsSOEditor : UnityEditor.Editor
    {
#region DataMembers

        // Noise map visualization
        // private SerializedProperty _showNoiseMeterProp;
        // private SerializedProperty _noiseMapTopMarginProp;
        // private SerializedProperty _noiseMapRightMarginProp;
        // private SerializedProperty _noiseMapWidthProp;
        // private SerializedProperty _noiseMapLabelWidthProp;
        // private SerializedProperty _noiseMapLabelRightMarginProp;


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
        private bool _noisePreviewSettingsToggle = true;
        private bool _noiseDrivenEffectsToggle = true;
        private bool _manualAdjustmentToggle = true;

#endregion

#region LifeCycle

        private void OnEnable()
        {
            // _showNoiseMeterProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.showNoiseMeter ) );
            // _noiseMapTopMarginProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMapTopMargin ) );
            // _noiseMapRightMarginProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMapRightMargin ) );
            // _noiseMapWidthProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMapWidth ) );
            // _noiseMapLabelWidthProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMapLabelWidth ) );
            // _noiseMapLabelRightMarginProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseMapLabelRightMargin ) );
            
            
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

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

#endregion


#region DrawUI

        private static void DrawLabeledSection( string titleText, ElementFrameType frameType ) => 
            DrawLabelSection( titleText, frameType );

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                serializedObject.DrawScriptField();
                DrawNoiseDrivenEffectsSection();
                DrawManualAdjustmentsSection();
            }
            EditorGUI.indentLevel--;
        }
        
        private void DrawNoiseDrivenEffectsSection()
        {
            _noiseDrivenEffectsToggle = DrawFoldoutSection( "Noise-Driven Effects", SubFoldoutFrameType, _noiseDrivenEffectsToggle );
            if( !_noiseDrivenEffectsToggle ) return;

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

        private void DrawManualAdjustmentsSection()
        {
            _manualAdjustmentToggle = DrawFoldoutSection( "Manual Adjustments", SubFoldoutFrameType, _manualAdjustmentToggle );
            if( !_manualAdjustmentToggle ) return;

            DrawManualRotationSection();
            Space( BetweenSectionPadding );
            DrawManualPositionSection();
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

                    // PropertyField( _yAxisCurveProp, new GUIContent( " " ) );
                }
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

        private void DrawManualPositionSection()
        {
            DrawLabeledSection( "Position", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _modelHeightProp, new GUIContent( "Model Height" ) );
                PropertyField( _verticalOffsetProp, new GUIContent( "Vertical Gap" ) );
            }
            EditorGUI.indentLevel--;
        }
        
        
        // private void DrawNoisePreviewSettingsSection()
        // {
        //     _noisePreviewSettingsToggle = DrawFoldoutSection( "Noise Meter Settings", SubFoldoutFrameType, _noisePreviewSettingsToggle );
        //     if( !_noisePreviewSettingsToggle ) return;
        //     PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
        //     PropertyField( _noiseMapTopMarginProp, new GUIContent( "Top Margin" ) );
        //     PropertyField( _noiseMapRightMarginProp, new GUIContent( "Right Margin" ) );
        //     PropertyField( _noiseMapWidthProp, new GUIContent( "Width" ) );
        //     PropertyField( _noiseMapLabelWidthProp, new GUIContent( "Label Width" ) );
        //     PropertyField( _noiseMapLabelRightMarginProp, new GUIContent( "Label Right Margin" ) );
        //
        //     Space( BetweenSectionPadding );
        // }

#endregion
    }
}