using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.GUIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( SoUser ) )]
    public class SoUserCustomEditor : UnityEditor.Editor
    {
        private SerializedProperty _testSoProp;
        private UnityEditor.Editor _editor;
        private bool _showSoSettings = true;

        private void OnEnable()
        {
            // Get the so prop.
            _testSoProp = serializedObject.FindProperty( nameof( SoUser.testSo ) );
        }

        private void OnDisable()
        {
            if ( _editor != null )
                DestroyImmediate( _editor );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField( _testSoProp );
            
            if ( _testSoProp == null ) return;

            // Make sure that the object reference is not null.
            if ( _testSoProp.objectReferenceValue == null ) return;
            
            // Create an editor for the so.
            _editor = CreateEditor( _testSoProp.objectReferenceValue );
            
            if (_editor == null ) return;

            EditorGUI.indentLevel++;
            {
                Rect labelRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight + 6f );
                DrawRectOutline( labelRect, Color.gray, 2f );
                _showSoSettings = EditorGUI.Foldout( labelRect, _showSoSettings, "Object Stacker Settings", true );
                if ( _showSoSettings )
                {
                    // EditorGUILayout.LabelField( "Object Stacker Settings" );
                    // DrawDivider( Color.white, 1, 15, 0, 2, 2 );
                    _editor.OnInspectorGUI();
                }
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
