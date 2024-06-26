using System;
using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.ExtensionMethods.AssetDatabaseWrapper;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.ColorEntryHelperWindow
{
    /// <summary>
    ///     The purpose of this tool is to help convert colors into a usable code format.
    ///     I primarily used it to copy large sections of swatch color values and convert
    ///     them into a format that I could paste into the color presets class with minimal
    ///     additional formatting required.
    /// </summary>
    public class CustomColorEntryHelper : EditorWindow
    {
        private const float Separator = 2f;
        private const float VerticalSeparator = 2f;
        private const float EdgePadding = 5f;

        private readonly Vector2 _windowSize = new Vector2( 350f, 150f );

        private int _charSequenceIndex;

        // For building the output string.
        private string _completeListOfColors = "";
        private char[] _currentSequence = { '0', '0', '0' };
        private int _numberOfEntries;
        private List<char> _outputCharList = new List<char>();

        // These are for the text conversion.
        [TextArea] private string _rbgConversionText = "";

        [SerializeField] public CustomColorLibrary[] colorLibraries;
        [SerializeField] private SerializedProperty colorLibrariesProperty;

        [SerializeField] public CustomColor[] colorList;
        [SerializeField] private SerializedProperty colorListProperty;

        public CustomColorLibrary customColorLibrary;

        // Single-line entry field.
        [SerializeField] private CustomColor customColorSingleEntry = new CustomColor( "Name_Here", Color.black );
        [SerializeField] private SerializedProperty customColorSingleEntryProperty;

        [SerializeField] public SerializedObject serializedObject;

        [MenuItem( CustomColorEntryHelperMenuItemName )]
        private static void Init()
        {
            var window = (CustomColorEntryHelper) GetWindow( typeof( CustomColorEntryHelper ) );
            window.titleContent = new GUIContent( CustomColorEntryHelperMenuTitle );
            window.Show();
        }

        private void OnEnable()
        {
            List<CustomColorLibrary> assets = AssetLoader.GetAssetsByType<CustomColorLibrary>();
            if( assets.Count > 0 )
            {
                Debug.Log( "Assets lookup returns results." );
                customColorLibrary = assets.FirstOrDefault();
                Debug.Log(
                    $"Loading of existing color entry helper database: {( customColorLibrary == null ? $"{GetColoredStringMaroon( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}" )}." );
            }

            if( customColorLibrary == null )
            {
                const string fileName = "CustomColorLibrary";
                Debug.Log( "Creating new color entry helper database." );
                customColorLibrary = CreateInstance<CustomColorLibrary>();
                if( !CreateAssetSafely( customColorLibrary, CustomColorLibraryPath, fileName ) )
                    Debug.Log( $"Asset creation aborted. Asset with file name '{fileName}' already exists." );
            }

            Debug.Log(
                $"After initialization, loading of color entry helper database: {( customColorLibrary == null ? $"{GetColoredStringMaroon( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}" )}." );


            minSize = _windowSize;

            // maxSize = _windowSize;
            serializedObject = new SerializedObject( this );
            colorListProperty = serializedObject.FindProperty( "colorList" );
            colorLibrariesProperty = serializedObject.FindProperty( "colorLibraries" );
            customColorSingleEntryProperty = serializedObject.FindProperty( "customColorSingleEntry" );
        }

        public void OnGUI()
        {
            EditorGUILayout.PropertyField( colorListProperty );
            float singleLineHeight = EditorGUIUtility.singleLineHeight;

            using ( new EditorGUI.DisabledScope( true ) )
            {
                EditorGUILayout.PropertyField( colorLibrariesProperty );
            }

            EditorGUILayout.PropertyField( customColorSingleEntryProperty );

            // Rect customColorArea = EditorGUILayout.GetControlRect();
            //
            // float halfWidthOfCustomColorArea = ( customColorArea.width / 2f );
            //
            // var customColorLabelRect = new Rect( customColorArea )
            // {
            //     width = halfWidthOfCustomColorArea, 
            //     height = singleLineHeight
            // };
            // // DrawRectOutline( customColorLabelRect, Color.cyan );
            // customColorSingleEntry.name = EditorGUI.TextField( customColorLabelRect, customColorSingleEntry.name );
            //
            // var customColorFieldRect = new Rect( customColorLabelRect );
            // customColorFieldRect.x += customColorFieldRect.width + Separator;
            // customColorFieldRect.width -= Separator;
            // // DrawRectOutline( customColorFieldRect, Color.white );
            // customColorSingleEntry.color = EditorGUI.ColorField( customColorFieldRect, customColorSingleEntry.color );

            EditorGUILayout.LabelField( $"Total Entries: {_numberOfEntries.ToString()}" );

            Rect recordColorButton = EditorGUILayout.GetControlRect();
            if( GUI.Button( recordColorButton, "Record Color" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColorSingleEntry.name}, {_customColorSingleEntry.color.ToString()}" );

                _completeListOfColors += $"public static CustomColor {customColorSingleEntry.name} = " +
                                         $"new CustomColor( nameof( {customColorSingleEntry.name} ), new Color(" +
                                         $" {customColorSingleEntry.color.r.ToString( "0.00" )}f, " +
                                         $"{customColorSingleEntry.color.g.ToString( "0.00" )}f, " +
                                         $"{customColorSingleEntry.color.b.ToString( "0.00" )}f ) );\n";
                _numberOfEntries++;
            }

            Rect resetButtonRect = EditorGUILayout.GetControlRect();
            if( GUI.Button( resetButtonRect, "Reset List" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColorSingleEntry.name}, {_customColorSingleEntry.color.ToString()}" );

                _completeListOfColors = "";
                _numberOfEntries = 0;

                Debug.Log( "Colors List was reset." );
            }

            Rect printButtonRect = EditorGUILayout.GetControlRect();
            if( GUI.Button( printButtonRect, "Print List to Console" ) )
            {
                OutputColorList();

                // Debug.Log( _completeListOfColors );

                SerializedPropertyInfoExtractor.PrintSerializedPropertyInfo( serializedObject, nameof( colorLibraries ), true, false );

                // string serializedColorListDatabase = JsonUtility.ToJson( customColorLibrary );
                // Debug.Log( serializedColorListDatabase );
            }


            EditorGUILayout.Space();

            EditorGUILayout.LabelField( "Convert RGB 0-255 to RGB 0-1.0" );
            EditorGUILayout.HelpBox( "This is useful if you have a list of colors in 0-255 format and you need 0-1.0 format. \n" +
                                     "This will search the pasted text, find each color entry in '(0, 0, 0)' format, and convert it to `(0.00f, 0.00f. 0.00f)` format, in place." +
                                     "The justification for this is that Rider only shows a color preview for a Unity color if it is in 0-1.0 format.", MessageType.None );

            Rect convertTextButtonRect = EditorGUILayout.GetControlRect();
            if( GUI.Button( convertTextButtonRect, "Convert Text" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColorSingleEntry.name}, {_customColorSingleEntry.color.ToString()}" );
                ConvertRgbText();

                // Debug.Log( _rbgConversionText.Equals( "" ) ? "No text entered." : _rbgConversionText );
                Debug.Log( new string( _outputCharList.ToArray() ) );
                ResetValues();
            }

            _rbgConversionText = EditorGUILayout.TextArea( _rbgConversionText );
        }

        private void OutputColorList()
        {
            string outputString = "Color List:\n";

            serializedObject.Update();
            for ( int x = 0; x < colorListProperty.arraySize; x++ )
            {
                SerializedProperty customColorProperty = colorListProperty.GetArrayElementAtIndex( x );
                SerializedProperty nameProperty = customColorProperty.FindPropertyRelative( "name" );
                SerializedProperty colorProperty = customColorProperty.FindPropertyRelative( "color" );

                outputString += $"public static CustomColor {nameProperty.stringValue} = " +
                                $"new CustomColor( nameof( {nameProperty.stringValue} ), new Color(" +
                                $" {colorProperty.colorValue.r:0.00}f, " +
                                $"{colorProperty.colorValue.g:0.00}f, " +
                                $"{colorProperty.colorValue.b:0.00}f ) );\n";
            }


            // foreach ( CustomColor customColor in colorList )
            // {
            //     outputString += $"public static CustomColor {customColor.name} = " +
            //                              $"new CustomColor( nameof( {customColor.name} ), new Color(" +
            //                              $" {customColor.color.r.ToString( "0.00" )}f, " +
            //                              $"{customColor.color.g.ToString( "0.00" )}f, " +
            //                              $"{customColor.color.b.ToString( "0.00" )}f ) );\n";
            // }

            Debug.Log( outputString );
        }

        private void ResetValues()
        {
            _currentSequence = new[] { '0', '0', '0' };
            _charSequenceIndex = 0;
            _outputCharList = new List<char>();
        }


        private void ConvertRgbText()
        {
            _outputCharList.Clear();
            foreach ( char c in _rbgConversionText )
            {
                // Debug.Log( $"Char: {c.ToString()}" );
                // Is char a number between 0 (ascii 48) and 9 (ascii 57)?
                if( ( c >= 48 ) &&
                    ( c <= 57 ) )
                {
                    _currentSequence[_charSequenceIndex++] = c;
                    if( _charSequenceIndex <= 2 ) continue;

                    CommitSequence();
                    continue;
                }

                // Is there a sequence in progress that isn't full?
                if( _charSequenceIndex != 0 ) CommitSequence();

                _outputCharList.Add( c );
            }
        }

        private void CommitSequence()
        {
            string finalSequence = new string( _currentSequence ).Substring( 0, _charSequenceIndex );

            string convertedSequence = ConvertTextIntToFloat( finalSequence ).ToString( "0.00" );

            // Debug.Log( $"Found number: {new string( _currentSequence )} -> {finalSequence} = {convertedSequence}" );

            _outputCharList.AddRange( convertedSequence );
            _outputCharList.Add( 'f' );

            _currentSequence = new[] { '0', '0', '0' };
            _charSequenceIndex = 0;
        }

        private float ConvertTextIntToFloat( string textInt )
        {
            if( int.TryParse( textInt, out int numValue ) )
                Console.WriteLine( numValue );
            else
                Console.WriteLine( $"Int32.TryParse could not parse '{textInt}' to an int." );

            return GetPercentageOfColor( numValue );
        }

        private float GetPercentageOfColor( int colorChannelValue ) => Mathf.InverseLerp( 0, 255, colorChannelValue );
    }
}