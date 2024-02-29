using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.ObjectStacker
{
    [CustomEditor( typeof( ObjectStackerSettingsSO ) )]
    public class ObjectStackerSettingsSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _noiseOffsetHorizontalProp;
        private SerializedProperty _noiseOffsetVerticalProp;
        private SerializedProperty _noiseScaleProp;
        private SerializedProperty _octavesProp;
        private SerializedProperty _persistenceProp;
        private SerializedProperty _lacunarityProp;
        private SerializedProperty _noiseDampeningCurveProp;

        private SerializedProperty _verticalOffsetProp;

        private SerializedProperty _yRotOffsetProp;
        private SerializedProperty _deckYRotSkewProp;
        private SerializedProperty _topSkewCardCountProp;
        private SerializedProperty _topCardsYRotSkewProp;
        private SerializedProperty _rotationDampeningCurveProp;

        private SerializedProperty _positionAffectedProp;

        private SerializedProperty _faceUpProp;

        private bool _noiseGenToggle = true;
        private bool _offsetsToggle = true;
        private bool _rotationEffectsToggle = true;
        private bool _positionEffectsToggle = true;
        private bool _orientationToggle = true;


        private void OnEnable()
        {
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.lacunarity ) );
            _noiseDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.noiseDampeningCurve ) );

            _verticalOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.verticalOffset ) );

            _yRotOffsetProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.yRotOffset ) );
            _deckYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.deckYRotSkew ) );
            _topSkewCardCountProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topSkewCardCount ) );
            _topCardsYRotSkewProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.topCardsYRotSkew ) );
            _rotationDampeningCurveProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.rotationDampeningCurve ) );

            _positionAffectedProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.positionAffected ) );

            _faceUpProp = serializedObject.FindProperty( nameof( ObjectStackerSettingsSO.faceUp ) );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // base.DrawDefaultInspector();
            EditorGUILayout.Space( 4f );
            
            _noiseGenToggle = EditorGUILayout.Foldout( _noiseGenToggle, "Noise Generation", true );
            if ( _noiseGenToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _noiseOffsetHorizontalProp );
                    EditorGUILayout.PropertyField( _noiseOffsetVerticalProp );
                    EditorGUILayout.PropertyField( _noiseScaleProp );
                    EditorGUILayout.PropertyField( _octavesProp );
                    EditorGUILayout.PropertyField( _persistenceProp );
                    EditorGUILayout.PropertyField( _lacunarityProp );
                    EditorGUILayout.PropertyField( _noiseDampeningCurveProp );
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space( 4f );
            
            _offsetsToggle = EditorGUILayout.Foldout( _offsetsToggle, "Offsets", true );
            if ( _offsetsToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _verticalOffsetProp );
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space( 4f );
            
            _rotationEffectsToggle = EditorGUILayout.Foldout( _rotationEffectsToggle, "Rotation Effects", true );
            if ( _rotationEffectsToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _yRotOffsetProp );
                    EditorGUILayout.PropertyField( _deckYRotSkewProp );
                    EditorGUILayout.PropertyField( _topSkewCardCountProp );
                    EditorGUILayout.PropertyField( _topCardsYRotSkewProp );
                    EditorGUILayout.PropertyField( _rotationDampeningCurveProp );
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space( 4f );
            
            _positionEffectsToggle = EditorGUILayout.Foldout( _positionEffectsToggle, "Position Effects", true );
            if ( _positionEffectsToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _positionAffectedProp );
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space( 4f );
            
            _orientationToggle = EditorGUILayout.Foldout( _orientationToggle, "Orientation", true );
            if ( _orientationToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _faceUpProp );
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}