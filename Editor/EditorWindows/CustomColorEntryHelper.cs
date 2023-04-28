using System;
using System.Collections.Generic;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.ToolingConstants;

namespace Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows
{
    public class CustomColorEntryHelper : EditorWindow
    {
        [MenuItem( CustomColorEntryHelperMenuItemName )]
        private static void Init()
        {
            var window = (CustomColorEntryHelper) GetWindow( typeof( CustomColorEntryHelper ) );
            window.titleContent = new GUIContent( CustomColorEntryHelperMenuTitle );
            window.Show();
        }
        
        private readonly Vector2 _windowSize = new Vector2( 350f, 150f);
        
        private const float Separator = 2f;
        private const float VerticalSeparator = 2f;
        private const float EdgePadding = 5f;

        private CustomColor _customColor = new CustomColor( "Name_Here", Color.black );

        private string _completeListOfColors = "";
        private int _numberOfEntries = 0;

        private string _rbgConversionText = "";
        private char[] _currentSequence = new[] { '0', '0', '0' };
        private int _charSequenceIndex = 0;
        private List<char> _outputCharList = new List<char>();

        private void OnEnable()
        {
            minSize = _windowSize;
            // maxSize = _windowSize;
        }

        public void OnGUI()
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight;

            Rect customColorArea = EditorGUILayout.GetControlRect();
            
            float halfWidthOfCustomColorArea = ( customColorArea.width / 2f );

            var customColorLabelRect = new Rect( customColorArea )
            {
                width = halfWidthOfCustomColorArea, 
                height = singleLineHeight
            };
            // DrawRectOutline( customColorLabelRect, Color.cyan );
            _customColor.name = EditorGUI.TextField( customColorLabelRect, _customColor.name );

            var customColorFieldRect = new Rect( customColorLabelRect );
            customColorFieldRect.x += customColorFieldRect.width + Separator;
            customColorFieldRect.width -= Separator;
            // DrawRectOutline( customColorFieldRect, Color.white );
            _customColor.color = EditorGUI.ColorField( customColorFieldRect, _customColor.color );
            EditorGUILayout.LabelField( $"Total Entries: {_numberOfEntries.ToString()}" );
            
            Rect recordColorButton = EditorGUILayout.GetControlRect();
            if ( GUI.Button( recordColorButton, "Record Color" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColor.name}, {_customColor.color.ToString()}" );

                _completeListOfColors += $"public static CustomColor {_customColor.name} = " +
                                         $"new CustomColor( nameof( {_customColor.name} ), new Color(" +
                                         $" {_customColor.color.r.ToString( "0.00" )}f, " +
                                         $"{_customColor.color.g.ToString( "0.00" )}f, " +
                                         $"{_customColor.color.b.ToString( "0.00" )}f ) );\n";
                _numberOfEntries++;
            }
            
            Rect resetButtonRect = EditorGUILayout.GetControlRect();
            if ( GUI.Button( resetButtonRect, "Reset List" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColor.name}, {_customColor.color.ToString()}" );

                _completeListOfColors = "";
                _numberOfEntries = 0;
                
                Debug.Log( "Colors List was reset." );
            }
            
            Rect printButtonRect = EditorGUILayout.GetControlRect();
            if ( GUI.Button( printButtonRect, "Print List to Console" ) )
            {
                Debug.Log( _completeListOfColors );
            }
            
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField( "Convert RGB 0-255 to RGB 0-1.0" );
            
            Rect convertTextButtonRect = EditorGUILayout.GetControlRect();
            if ( GUI.Button( convertTextButtonRect, "Convert Text" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColor.name}, {_customColor.color.ToString()}" );
                ConvertRgbText();
                // Debug.Log( _rbgConversionText.Equals( "" ) ? "No text entered." : _rbgConversionText );
                Debug.Log( new string ( _outputCharList.ToArray() ) );
                ResetValues();
            }
            _rbgConversionText = EditorGUILayout.TextArea( _rbgConversionText );
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
                if ( c >= 48 && c <= 57 )
                {
                    _currentSequence[_charSequenceIndex++] = c;
                    if ( _charSequenceIndex <= 2 ) continue;

                    CommitSequence();
                    continue;
                }
                
                // Is there a sequence in progress that isn't full?
                if ( _charSequenceIndex != 0 ) CommitSequence();

                _outputCharList.Add( c );
            }
        }

        private void CommitSequence()
        {
            string convertedSequence = ConvertTextIntToFloat( new string( _currentSequence ) ).ToString( "0.00" );
            // Debug.Log( $"Found number: {new string( currentSequence )} -> {convertedSequence}" );

            _outputCharList.AddRange( convertedSequence );
            _outputCharList.Add( 'f' );

            _currentSequence = new[] { '0', '0', '0' };
            _charSequenceIndex = 0;
        }

        private float ConvertTextIntToFloat( string textInt )
        {
            if (int.TryParse(textInt, out int numValue))
            {
                Console.WriteLine(numValue);
            }
            else
            {
                Console.WriteLine($"Int32.TryParse could not parse '{textInt}' to an int.");
            }

            return GetPercentageOfColor( numValue );
        }
        
        private float GetPercentageOfColor( int colorChannelValue ) => Mathf.InverseLerp( 0, 255, colorChannelValue );
    }
}
