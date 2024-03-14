using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.CustomEditors;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    [CustomEditor( typeof( PerlinNoiseSettingsSO ) )]
    public class PerlinNoiseSettingsSOEditor : SubscribableSOEditor
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
        
        private PerlinNoiseSettingsSO _noiseSettingsSO;

#endregion
        
#region FoldoutToggles

        private bool NoiseSettingsFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( NoiseSettingsFoldoutToggle )}", true );
            set => EditorPrefs.SetBool( $"{name}_{nameof( NoiseSettingsFoldoutToggle )}", value );
        }
        
#endregion

#region LifeCycle

        protected override void OnEnableLast()
        {
            _noiseSettingsSO = target as PerlinNoiseSettingsSO;

            // LoadFoldoutToggles();
            LoadProperties();
        }

        protected override void OnDisableLast()
        {
            // SaveFoldoutToggles();
        }

        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            DrawSettings();

            serializedObject.ApplyModifiedProperties();
        }

#endregion


#region Initialization

        // private void LoadFoldoutToggles()
        // {
        //     // Load the foldout states from EditorPrefs.
        //     _noiseSettingsToggle = EditorPrefs.GetBool( nameof( _noiseSettingsToggle ), true );
        // }
        //
        // private void SaveFoldoutToggles()
        // {
        //     // Save the foldout states to EditorPrefs to preserve their state.
        //     EditorPrefs.SetBool( nameof( _noiseSettingsToggle ), _noiseSettingsToggle );
        // }
        
        private void LoadProperties()
        {
            _seedProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.seed ) );
            _noiseOffsetHorizontalProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.noiseOffsetHorizontal ) );
            _noiseOffsetVerticalProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.noiseOffsetVertical ) );
            _noiseScaleProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.noiseScale ) );
            _octavesProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.octaves ) );
            _persistenceProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.persistence ) );
            _lacunarityProp = serializedObject.FindProperty( nameof( PerlinNoiseSettingsSO.lacunarity ) );
        }

#endregion


#region DrawInspectorUI

        private void DrawSettings()
        {
            EditorGUI.indentLevel++;
            {
                serializedObject.DrawScriptField();

                DrawNoiseSettings();
            }
            EditorGUI.indentLevel--;
        }


        private void DrawNoiseSettings()
        {
            NoiseSettingsFoldoutToggle = DrawFoldoutSection( "Noise Settings", SubFoldoutFrameType, NoiseSettingsFoldoutToggle );
            if( !NoiseSettingsFoldoutToggle ) return;

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