using Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows.ColorEntryHelperWindow;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Editor.PropertyDrawers.CustomColors
{
    [CustomPropertyDrawer(typeof(CustomColorLibrary))]
    public class CustomColorLibraryDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            Debug.Log( "CustomColorLibrary's property drawer was called." );
            
            SerializedProperty nameProperty = property.FindPropertyRelative( "libraryName" );
            SerializedProperty colorListProperty = property.FindPropertyRelative( "customColors" );
            Debug.Log( $"Loading of libraryName property: {(nameProperty == null ? $"{GetColoredStringMaroon( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}")}." );
            Debug.Log( $"Loading of colorListProperty property: {(colorListProperty == null ? $"{GetColoredStringMaroon( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}")}." );

            // EditorGUI.PropertyField( position, nameProperty );
            // position.yMin += EditorGUI.GetPropertyHeight( nameProperty ) + 2f;
            // EditorGUI.PropertyField( position, colorListProperty );
        }

        // public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        // {
        //     SerializedProperty nameProperty = property.FindPropertyRelative( "libraryName" );
        //     SerializedProperty colorListProperty = property.FindPropertyRelative( "customColors" );
        //     float totalHeight =  EditorGUI.GetPropertyHeight( nameProperty ) + EditorGUI.GetPropertyHeight( colorListProperty );
        //     Debug.Log( $"Height of custom color library is {totalHeight.ToString()}" );
        //     return 40f;
        //
        //     // return base.GetPropertyHeight( property, label );
        // }
    }
}
