using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( NoiseSettingsSO ) )]
    public class NoiseSettingsSOEditor : UnityEditor.Editor
    {
        // Noise Settings
        private SerializedProperty _seedProp;
        private SerializedProperty _noiseOffsetHorizontalProp;
        private SerializedProperty _noiseOffsetVerticalProp;
        private SerializedProperty _noiseScaleProp;
        private SerializedProperty _octavesProp;
        private SerializedProperty _persistenceProp;
        private SerializedProperty _lacunarityProp;

        private bool _noiseSettingsToggle = true;


        private void OnEnable()
        {
            _seedProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.seed ) );
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.lacunarity ) );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();
            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSettings()
        {
            // EditorGUI.indentLevel++;
            // {
            DrawNoiseSettings();
            // }
            // EditorGUI.indentLevel--;
        }


        private void DrawNoiseSettings()
        {
            // _noiseSettingsToggle = DrawFoldoutSection( "Noise Settings", Level0FrameType, _noiseSettingsToggle );
            // if( !_noiseSettingsToggle ) return;

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
    }
}