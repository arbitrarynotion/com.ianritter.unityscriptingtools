using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.PropertyDrawers
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
                float remainingWidth = position.width - labelWidth;
                const float boolWidth = 20f;
                const float colorFieldWidth = 70f;
                // float buttonWidth = remainingWidth - boolWidth - colorFieldWidth;

                // Label
                // Split the remaining space between the string and the color fields.

                var labelRect = new Rect( position )
                {
                    height = EditorGUIUtility.singleLineHeight + verticalPadding,
                    width = EditorGUIUtility.labelWidth
                };
                // DrawRectOutline( labelRect, Orange );
                
                // EditorGUI.PropertyField( stringRect, property.FindPropertyRelative( "Name" ), label );
                EditorGUI.LabelField( labelRect, label );
                
                // float singleFieldWidth = ( position.width - EditorGUIUtility.labelWidth);
                
                EditorGUIUtility.labelWidth = 0.01f;
                
                // Data field
                var dataRect = new Rect( position );
                dataRect.xMin += labelWidth + divider;

                // Bool
                var boolRect = new Rect( dataRect ) { width = boolWidth };
                // DrawRectOutline( boolRect, Blue );
                EditorGUI.PropertyField( boolRect, property.FindPropertyRelative( "toggle" ), GUIContent.none );
                
                // Symbol
                var symbolRect = new Rect( dataRect );
                symbolRect.xMin += boolWidth + divider;
                // symbolRect.width = ( dataRect.width - boolWidth - divider ) / symbolColorDivisor;
                symbolRect.width = ( dataRect.width - boolWidth - divider ) - colorFieldWidth;
                // DrawRectOutline( symbolRect, Yellow );
                
                EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "symbol" ), GUIContent.none );

                // Color
                var colorRect = new Rect( dataRect );
                colorRect.xMin += boolWidth + divider + symbolRect.width + divider;
                // DrawRectOutline( colorRect, Purple );
                EditorGUI.PropertyField( colorRect, property.FindPropertyRelative( "color" ), GUIContent.none );
                
                EditorGUIUtility.labelWidth = labelWidth;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;

    }
}