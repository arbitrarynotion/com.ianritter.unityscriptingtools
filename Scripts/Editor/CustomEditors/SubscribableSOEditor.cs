using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.CustomEditors
{
    [CustomEditor( typeof( SubscribableSO ), true )]
    public abstract class SubscribableSOEditor : UnityEditor.Editor
    {
        private SubscribableSO _subscribableSO;
        private SerializedProperty _formattedLoggerProp;

        protected virtual void OnEnableFirst()
        {
        }

        protected virtual void OnEnableLast()
        {
        }

        protected virtual void OnInspectorGUIFirst()
        {
        }

        protected virtual void OnInspectorGUILast()
        {
        }

        private void OnEnable()
        {
            _subscribableSO = target as SubscribableSO;

            // Debug.Log( $"SubscribableSO OnEnable was called for {GetColoredStringYellow( _subscribableSO.name )}." );

            _formattedLoggerProp = serializedObject.FindProperty( "logger" );

            OnEnableLast();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnInspectorGUIFirst();

            EditorGUI.indentLevel++;
            {
                PropertyField( _formattedLoggerProp );
                Rect buttonRect = GetControlRect();
                buttonRect.xMin += 15f;
                if( GUI.Button( buttonRect, "Print my Subscribers" ) )
                    _subscribableSO.PrintMyEventSubscribers();
            }
            EditorGUI.indentLevel--;

            OnInspectorGUILast();

            serializedObject.ApplyModifiedProperties();
        }
    }
}