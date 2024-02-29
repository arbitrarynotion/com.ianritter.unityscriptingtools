using Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows.SerializedPropertyExplorerWindow;
using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.SOPropertyDrawer;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.EditorDividerGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.SOPropertyDrawer
{
    [CustomPropertyDrawer( typeof( TestScriptableObjectWrapper ) )]
    public class TestSoWrapperPropertyDrawer : PropertyDrawer
    {
        private const int ElementCount = 2;
        private const float VerticalSeparator = 2f;
        
        
        private UnityEditor.Editor _currentEditor;
        private Object _soObject;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            var currentPosRect = new Rect( position )
            {
                height = EditorGUIUtility.singleLineHeight
            };

            EditorGUI.LabelField( currentPosRect, "This is the SoWrapper's property drawer.", EditorStyles.boldLabel );

            currentPosRect.y += EditorGUIUtility.singleLineHeight + VerticalSeparator;
            
            SerializedProperty soWrapperProp = property.FindPropertyRelative( nameof( TestScriptableObjectWrapper.so ) );
            _soObject = soWrapperProp.objectReferenceValue;

            if ( _soObject == null )
            {
                EditorGUI.PropertyField( currentPosRect, soWrapperProp );
                return;
            }
            
            // DrawEditor();
        }


        private void DrawEditor()
        {
            _currentEditor = UnityEditor.Editor.CreateEditor( _soObject );

            EditorGUI.indentLevel++;
            {
                _currentEditor.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            float totalElementHeight = ElementCount * EditorGUIUtility.singleLineHeight;
            const float combinedVerticalSeparators = ( ElementCount - 1 ) * VerticalSeparator;
            return ( totalElementHeight + combinedVerticalSeparators );
        }
        
        ~TestSoWrapperPropertyDrawer()
        {
            Debug.Log( "~~~~ObjectStackPropertyDrawer's deconstructor was called." );
            if ( _currentEditor == null ) return;
            
            Object.DestroyImmediate( _currentEditor );
            _currentEditor = null;
        }
    }
}
