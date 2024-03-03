using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PropertyDrawers.CustomLogger
{
    [CustomPropertyDrawer( typeof( CustomLoggerSymbol ) )]
    public class CustomLoggerSymbolDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            // Disable all field when toggle is off.
            SerializedProperty toggleProperty = property.FindPropertyRelative( "toggle" );

            EditorGUI.BeginProperty( position, label, property );
            {
                const float divider = 2f;
                const float verticalPadding = 2f;
                float labelWidth = EditorGUIUtility.labelWidth;
                const float boolWidth = 20f;

                EditorGUIUtility.labelWidth = 0.01f;

                // Data field
                var dataRect = new Rect( position );

                if ( label != GUIContent.none )
                {
                    var labelRect = new Rect( position )
                    {
                        height = EditorGUIUtility.singleLineHeight + verticalPadding,
                        width = labelWidth
                    };
                    // DrawRectOutline( labelRect, Orange.color );
                    EditorGUI.LabelField( labelRect, label );

                    dataRect.xMin += labelWidth + divider;
                }

                // Bool
                var boolRect = new Rect( dataRect )
                {
                    width = boolWidth
                };
                // DrawRectOutline( boolRect, Blue.color );
                EditorGUI.PropertyField( boolRect, toggleProperty, GUIContent.none );

                dataRect.xMin += boolRect.width + divider;

                // Symbol
                var symbolRect = new Rect( dataRect )
                {
                    width = dataRect.width - ColorPickerHandler.GetColorPickerButtonWidth() - divider
                };
                // DrawRectOutline( symbolRect, Yellow.color );

                dataRect.xMin += symbolRect.width + divider;

                // Add color picker at the end.
                var colorPickerRect = new Rect( dataRect );

                using ( new EditorGUI.DisabledScope( !toggleProperty.boolValue ) )
                {
                    EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "customColor" ), GUIContent.none );
                    ColorPickerHandler.SetWindowPosition( new Vector2( position.x, position.y ) );
                    ColorPickerHandler.DrawColorPickerPropertyButton( colorPickerRect, property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "color" ) );

                    EditorGUIUtility.labelWidth = labelWidth;
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) =>
            EditorGUIUtility.singleLineHeight + 2f;
    }
}