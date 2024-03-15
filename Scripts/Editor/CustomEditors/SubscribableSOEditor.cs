using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.CustomEditors
{
    [CustomEditor( typeof( SubscribableSO ), true )]
    public abstract class SubscribableSOEditor : UnityEditor.Editor
    {
        private SubscribableSO _subscribableSO;
        private SerializedProperty _formattedLoggerProp;

        protected virtual void OnEnableFirst() {}
        protected virtual void OnEnableLast() {}
        protected virtual void OnDisableFirst() {}
        protected virtual void OnDisableLast() {}
        protected virtual void OnInspectorGUIFirst() {}
        protected virtual void OnInspectorGUILast() {}

        // private bool _debugToggle = true;
        
#region FoldoutToggles
        
        private bool DebugFoldoutToggle
        {
            get => EditorPrefs.GetBool( $"{name}_{nameof( DebugFoldoutToggle )}", false );
            set => EditorPrefs.SetBool( $"{name}_{nameof( DebugFoldoutToggle )}", value );
        }

#endregion

        private void OnEnable()
        {
            _subscribableSO = target as SubscribableSO;
            
            OnEnableFirst();

            // Debug.Log( $"SubscribableSO OnEnable was called for {GetColoredStringYellow( _subscribableSO.name )}." );

            _formattedLoggerProp = serializedObject.FindProperty( "logger" );
            
            OnEnableLast();
        }

        private void OnDisable()
        {
            OnDisableFirst();
            OnDisableLast();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnInspectorGUIFirst();

            EditorGUI.indentLevel++;
            {
                DrawDebugSection();
            }
            EditorGUI.indentLevel--;

            OnInspectorGUILast();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDebugSection()
        {
            DebugFoldoutToggle = DrawFoldoutSection( "Debug", SubFoldoutFrameType, DebugFoldoutToggle );
            if( !DebugFoldoutToggle ) return;
            
            EditorGUI.indentLevel++;
            {
                PropertyField( _formattedLoggerProp );
                // Space( VerticalSeparator );
                DrawPrintSubscribersButton();
            }
            EditorGUI.indentLevel--;
        }

        private void DrawPrintSubscribersButton()
        {
            Rect buttonRect = GetControlRect();
            buttonRect.xMin += EditorGUI.indentLevel * 15f;
            if( GUI.Button( buttonRect, "Print my Subscribers" ) )
                _subscribableSO.PrintMyEventSubscribers();
        }
    }
}