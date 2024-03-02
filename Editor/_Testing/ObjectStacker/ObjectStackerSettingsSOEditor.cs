using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.UIEditorOnlyRectGraphics;

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

        private SerializedProperty _verticalOffsetProp;

        private SerializedProperty _yRotOffsetProp;
        private SerializedProperty _deckYRotSkewProp;
        private SerializedProperty _topSkewCardCountProp;
        private SerializedProperty _topCardsYRotSkewProp;
        private SerializedProperty _rotationDampeningCurveProp;

        private SerializedProperty _positionAffectedProp;

        private SerializedProperty _faceUpProp;

        private const ElementFrameType ParentFrameType = ElementFrameType.BottomOnly;
        private const ElementFrameType ChildFrameType = ElementFrameType.None;
        private const float TitleFrameBottomPadding = 2f;
        private const float BetweenSectionPadding = 4f;

        // private bool _noiseGenToggle = true;
        // private bool _offsetsToggle = true;
        // private bool _rotationEffectsToggle = true;
        // private bool _positionEffectsToggle = true;
        // private bool _orientationToggle = true;


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


            _verticalOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.verticalOffset ) );

            _yRotOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.yRotOffset ) );
            _deckYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.deckYRotSkew ) );
            _topSkewCardCountProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topDownSkewPercent ) );
            _topCardsYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topCardsYRotSkew ) );
            _rotationDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.rotationDampeningCurve ) );

            _positionAffectedProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.positionAffected ) );

            _faceUpProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.faceUp ) );
        }

        private void StartLabelSection( string titleText, ElementFrameType frameType )
        {
            EditorGUI.LabelField( 
                GetFramedControlRect( Color.gray, frameType ), 
                titleText, 
                EditorStyles.boldLabel 
            );
            EditorGUILayout.Space( TitleFrameBottomPadding );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // base.DrawDefaultInspector();
            // EditorGUILayout.Space( BetweenSectionPadding );
            // serializedObject.DrawScriptField();
            
            StartLabelSection( "Noise Drive Rotation Effects", ParentFrameType );
            EditorGUI.indentLevel++;
            {
                // StartLabelSection( "Rotation", ChildFrameType );
                // EditorGUI.indentLevel++;
                // {
                    EditorGUILayout.PropertyField( _seedProp );
                    EditorGUILayout.PropertyField( _showNoiseMeterProp, new GUIContent( "Noise Meter" ) );
                    EditorGUILayout.PropertyField( _noiseOffsetHorizontalProp, new GUIContent( "Horizontal Offset" ) );
                    EditorGUILayout.PropertyField( _noiseOffsetVerticalProp, new GUIContent( "Vertical Offset" ) );
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
                    EditorGUILayout.PropertyField( _noiseDampeningCurveProp, new GUIContent( "Intensity Curve" ) );
                    EditorGUILayout.PropertyField( _noiseMultiplierProp, new GUIContent( "Multiplier") );
                // }
                // EditorGUI.indentLevel--;
                
                // EditorGUILayout.Space( BetweenSectionPadding );
                //
                // StartLabelSection( "Position",ChildFrameType );
                // EditorGUI.indentLevel++;
                // {
                //     EditorGUILayout.PropertyField( _positionAffectedProp );
                // }
                // EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space( BetweenSectionPadding );
            
            StartLabelSection( "Manual Rotation Effects", ParentFrameType );
            EditorGUI.indentLevel++;
            {
                // StartLabelSection( "Rotation", ChildFrameType );
                // EditorGUI.indentLevel++;
                // {
                    EditorGUILayout.PropertyField( _faceUpProp );
                    EditorGUILayout.PropertyField( _deckYRotSkewProp, new GUIContent( "Progressive Skew" ) );
                    // EditorGUILayout.LabelField( "Top Objects Skew", EditorStyles.boldLabel );
                    StartLabelSection( "Isolated Skew", ParentFrameType );
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
                // }
                // EditorGUI.indentLevel--;
                
                // EditorGUILayout.Space( BetweenSectionPadding );
                //
                // StartLabelSection( "Position", ChildFrameType );
                // EditorGUI.indentLevel++;
                // {
                //     
                // }
                // EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            
            
            //
            // EditorGUILayout.LabelField( "Noise Generation", EditorStyles.boldLabel );
            // EditorGUI.indentLevel++;
            // {
            //     EditorGUILayout.PropertyField( _seedProp );
            //     EditorGUILayout.PropertyField( _showNoiseMeterProp, new GUIContent( "Show Noise Meter" ) );
            //     EditorGUILayout.PropertyField( _noiseOffsetHorizontalProp, new GUIContent( "Horizontal Offset" ) );
            //     EditorGUILayout.PropertyField( _noiseOffsetVerticalProp, new GUIContent( "Vertical Offset" ) );
            //     EditorGUILayout.PropertyField( _noiseScaleProp, new GUIContent( "Scale") );
            //     EditorGUILayout.PropertyField( _octavesProp );
            //     using ( new EditorGUI.DisabledScope( _octavesProp.intValue <= 1 ) )
            //     {
            //         EditorGUI.indentLevel++;
            //         {
            //             EditorGUILayout.PropertyField( _persistenceProp );
            //             EditorGUILayout.PropertyField( _lacunarityProp );
            //         }
            //         EditorGUI.indentLevel--;
            //     }
            //     EditorGUILayout.PropertyField( _noiseDampeningCurveProp, new GUIContent( "Dampening Curve" ) );
            //     EditorGUILayout.PropertyField( _noiseMultiplierProp );
            //
            // }
            // EditorGUI.indentLevel--;
            //
            // EditorGUILayout.Space( 4f );
            //
            // EditorGUILayout.LabelField( "Offsets", EditorStyles.boldLabel );
            // EditorGUI.indentLevel++;
            // {
            //     EditorGUILayout.PropertyField( _verticalOffsetProp );
            // }
            // EditorGUI.indentLevel--;
            //
            // EditorGUILayout.Space( 4f );
            //
            // EditorGUILayout.LabelField( "Rotation Effects", EditorStyles.boldLabel );
            // EditorGUI.indentLevel++;
            // {
            //     EditorGUILayout.PropertyField( _deckYRotSkewProp );
            //     EditorGUILayout.LabelField( "Top Objects Skew", EditorStyles.boldLabel );
            //     EditorGUI.indentLevel++;
            //     {
            //         EditorGUILayout.PropertyField( _topSkewCardCountProp, new GUIContent( "Percent in Group") );
            //         if ( _topSkewCardCountProp.floatValue >= 1f )
            //             EditorGUILayout.HelpBox( "Note: if all cards are included in skew group, the rotation lock one the first object will negate whole-stack rotations." +
            //                                      "You can still effect the upper cards using a dampening curve.", MessageType.Info );
            //         using ( new EditorGUI.DisabledScope( _topSkewCardCountProp.floatValue <= 0f ) )
            //         {
            //             EditorGUI.indentLevel++;
            //             {
            //                 EditorGUILayout.PropertyField( _topCardsYRotSkewProp, new GUIContent( "Amount") );
            //                 EditorGUILayout.PropertyField( _rotationDampeningCurveProp, new GUIContent( "Dampener" ) );
            //             }
            //             EditorGUI.indentLevel--;
            //         }
            //     }
            //     EditorGUI.indentLevel--;
            // }
            // EditorGUI.indentLevel--;
            //
            // EditorGUILayout.Space( 4f );
            //
            // EditorGUILayout.LabelField( "Position Effects", EditorStyles.boldLabel );
            // EditorGUI.indentLevel++;
            // {
            //     EditorGUILayout.PropertyField( _positionAffectedProp );
            // }
            // EditorGUI.indentLevel--;
            //
            // EditorGUILayout.Space( 4f );
            //
            // EditorGUILayout.LabelField( "Orientation", EditorStyles.boldLabel );
            // EditorGUI.indentLevel++;
            // {
            //     EditorGUILayout.PropertyField( _faceUpProp );
            // }
            // EditorGUI.indentLevel--;

            if ( serializedObject.ApplyModifiedProperties() )
            {
                Debug.Log( "Change in Settings SO detected. Repainting Scene Views." );
                SceneView.RepaintAll();
            }
        }
    }
}