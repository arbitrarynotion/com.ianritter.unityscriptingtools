using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.CustomEditors;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( NoiseSettingsSO ) )]
    public class NoiseSettingsSOEditor : SubscribableSOEditor
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

        private bool _noiseSettingsToggle = true;

        private NoiseSettingsSO _noiseSettingsSO;

#endregion

#region LifeCycle

        protected override void OnEnableLast()
        {
            _noiseSettingsSO = target as NoiseSettingsSO;

            _seedProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.seed ) );
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( NoiseSettingsSO.lacunarity ) );
        }

        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

#endregion


#region DrawInspectorUI

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                serializedObject.DrawScriptField();

                DrawNoiseSettings();

                // DrawNoisePreviewSettingsSection();
            }
            EditorGUI.indentLevel--;
        }


        private void DrawNoiseSettings()
        {
            _noiseSettingsToggle = DrawFoldoutSection( "Noise Settings", SubFoldoutFrameType, _noiseSettingsToggle );
            if( !_noiseSettingsToggle ) return;

            EditorGUI.indentLevel++;
            {
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
            }
            EditorGUI.indentLevel--;

            Space( BetweenSectionPadding );
        }

#endregion
    }
}