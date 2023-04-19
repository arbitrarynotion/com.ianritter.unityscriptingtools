using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(CustomColor))]
    public class CustomColorDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );
            {
                // Split the remaining space between the string and the color fields.
                var stringRect = new Rect( position);
                stringRect.height = EditorGUIUtility.singleLineHeight + 2f;
                float singleFieldWidth = ( position.width / 2f ) - 4f;
                stringRect.width = singleFieldWidth;
                
                var colorFieldRect = new Rect( position );
                colorFieldRect.height = EditorGUIUtility.singleLineHeight + 2f;
                colorFieldRect.xMin += singleFieldWidth + 4f;

                EditorGUI.PropertyField( stringRect, property.FindPropertyRelative( "name" ), GUIContent.none );
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 0.01f;
                EditorGUI.PropertyField( colorFieldRect, property.FindPropertyRelative( "color" ), GUIContent.none );
                EditorGUIUtility.labelWidth = labelWidth;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;
    }
}