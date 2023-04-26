using Services.CustomColors;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(CustomColor))]
    public class CustomColorDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );
            {
                // Split the remaining space between the string and the color fields.
                var stringRect = new Rect( position )
                {
                    height = EditorGUIUtility.singleLineHeight + 2f, 
                    width = position.width * 0.7f
                };
                // float singleFieldWidth = ( position.width / 2f ) - 4f;
                // stringRect.width = singleFieldWidth;

                var colorFieldRect = new Rect( position )
                {
                    height = EditorGUIUtility.singleLineHeight + 2f
                };
                // colorFieldRect.xMin += singleFieldWidth + 4f;
                colorFieldRect.xMin += stringRect.width + 2f;

                EditorGUI.PropertyField( stringRect, property.FindPropertyRelative( "name" ), GUIContent.none );
                float labelWidth = EditorGUIUtility.labelWidth;
                
                int cachedIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                
                EditorGUIUtility.labelWidth = 0.01f;
                EditorGUI.PropertyField( colorFieldRect, property.FindPropertyRelative( "color" ), GUIContent.none );
                
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.indentLevel = cachedIndentLevel;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;
    }
}