using Services.CustomLogger;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(CustomLoggerSymbol))]
    public class CustomLoggerSymbolDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );
            {

                const float divider = 2f;
                const float verticalPadding = 2f;
                float labelWidth = EditorGUIUtility.labelWidth;
                // float remainingWidth = position.width - labelWidth;
                const float boolWidth = 20f;
                // const float colorFieldWidth = 70f;
                // float buttonWidth = remainingWidth - boolWidth - colorFieldWidth;

                // Label
                // Split the remaining space between the string and the color fields.

                var labelRect = new Rect( position )
                {
                    height = EditorGUIUtility.singleLineHeight + verticalPadding,
                    width = EditorGUIUtility.labelWidth
                };
                // DrawRectOutline( labelRect, Orange.color );
                
                // EditorGUI.PropertyField( stringRect, property.FindPropertyRelative( "Name" ), label );
                EditorGUI.LabelField( labelRect, label );
                
                int cachedIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                
                // float singleFieldWidth = ( position.width - EditorGUIUtility.labelWidth);
                
                EditorGUIUtility.labelWidth = 0.01f;
                
                // Data field
                var dataRect = new Rect( position );
                dataRect.xMin += labelWidth + divider;

                // Bool
                var boolRect = new Rect( dataRect ) { width = boolWidth };
                // DrawRectOutline( boolRect, Blue.color );
                EditorGUI.PropertyField( boolRect, property.FindPropertyRelative( "toggle" ), GUIContent.none );

                // Symbol
                var symbolRect = new Rect( dataRect );
                symbolRect.xMin += boolWidth + divider;
                symbolRect.width = ( dataRect.width - boolWidth - divider );
                // DrawRectOutline( symbolRect, Yellow.color );
                EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "customColor" ), GUIContent.none );

                
                // // Symbol
                // var symbolRect = new Rect( dataRect );
                // symbolRect.xMin += boolWidth + divider;
                // symbolRect.width = ( dataRect.width - boolWidth - divider ) - colorFieldWidth;
                // DrawRectOutline( symbolRect, Yellow.color );
                
                // EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "symbol" ), GUIContent.none );
                //
                // // Color
                // var colorRect = new Rect( dataRect );
                // colorRect.xMin += boolWidth + divider + symbolRect.width + divider;
                // // DrawRectOutline( colorRect, Purple.color );
                // EditorGUI.PropertyField( colorRect, property.FindPropertyRelative( "customColor" ), GUIContent.none );
                
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.indentLevel = cachedIndentLevel;

            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;

    }
}