using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors;

using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors.PresetColors;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.GUIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PopupWindows.CustomColorPicker
{
    /// <summary>
    ///     <para>Creates a popup window that lists all of the colors in PresetColors.cs as buttons.</para>
    ///     <para>Can be used with either a UnityEngine.Color as a SerializedProperty or a CustomColor directly. Which one it's using
    ///     is determined by the SetTarget method.</para>
    /// </summary>
    public class CustomColorPicker : PopupWindowContent
    {
        private class CustomColorButton
        {
            public readonly CustomColor CustomColor;
            public readonly Texture2D Texture2D;
            public readonly Rect ButtonRect;

            public CustomColorButton( 
                CustomColor customColor,
                Rect positionRect, 
                Texture2D texture2D )
            {
                CustomColor = customColor;
                Texture2D = texture2D;
                ButtonRect = positionRect;
            }
        }

        private CustomColor[] _colorList;
        private Vector2 _windowSize;
        
        private int _buttonsPerLine;
        private float _buttonWidth;
        private float _buttonHeight;
        private List<CustomColorButton> _buttons = new List<CustomColorButton>();
        
        private const float Separator = 2f;
        private const float VerticalSeparator = 2f;
        private const float EdgePadding = 5f;

        private Vector2 _scrollPosition;
        private Rect _labelRect;
        
        private CustomColor _targetColor;
        private bool _useProperty;
        private SerializedProperty _targetColorProperty;
        
        public delegate void ButtonPressed( CustomColor buttonCustomColor );
        public event ButtonPressed OnButtonPressed;
        private void OnButtonPressedNotify( CustomColor buttonCustomColor )
        {
            if ( _useProperty )
            {
                // _targetColorProperty.FindPropertyRelative( "color" ).colorValue = buttonCustomColor.color;
                _targetColorProperty.colorValue = buttonCustomColor.color;
                _targetColorProperty.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                _targetColor.color = buttonCustomColor.color;
            }
            OnButtonPressed?.Invoke( buttonCustomColor );
        }
        
        
        public CustomColorPicker( Vector2 windowSize, int buttonsPerLine = 8 )
        {
            _windowSize = windowSize;
            _buttonsPerLine = buttonsPerLine;
            
            BuildButtonList();
        }
        

        public void SetTargetCustomColor( CustomColor targetCustomColor )
        {
            _targetColor = targetCustomColor;
            
            // Clear previous target property.
            _targetColorProperty = null;
            _useProperty = false;
        }
        
        public void SetTargetCustomColor( SerializedProperty customColorProperty )
        {
            _targetColorProperty = customColorProperty;
            _useProperty = true;
        }

        public int GetButtonsPerLine() => _buttonsPerLine;
        
        public void SetButtonsPerLine( int buttonsPerLine )
        {
            if ( _buttonsPerLine == buttonsPerLine ) return;

            _buttonsPerLine = buttonsPerLine;
            BuildButtonList();
        }
        
        // Todo: Added the option to set the window size but didn't have time to test it thoroughly.
        public void SetWindowSize( Vector2 windowSize )
        {
            if ( Math.Abs( _windowSize.x - windowSize.x ) <= 0.01f && Math.Abs( _windowSize.y - windowSize.y ) <= 0.01f ) return;
            
            _windowSize = windowSize;
            BuildButtonList();
        }
        
        private void BuildButtonList()
        {
            // _logger.LogStart( MethodBase.GetCurrentMethod(), true )
            _buttons = new List<CustomColorButton>();
            
            // Todo: Add option to provide a custom list of colors but default to the PresetColors class.
            _colorList = GetAllColors().ToArray();

            // Define the position available for this line.
            var firstLine = new Rect( 0f, 0f, _windowSize.x, _windowSize.y );
            firstLine.yMin += EdgePadding;
            firstLine.yMax -= EdgePadding;
            firstLine.xMin += EdgePadding;
            firstLine.xMax -= EdgePadding;

            _labelRect = new Rect( firstLine ) { height = EditorGUIUtility.singleLineHeight };
            // GUI.Label( labelRect, new GUIContent( "Color Picker") );

            // Shift down beyond the label.
            firstLine.yMin += EditorGUIUtility.singleLineHeight + VerticalSeparator;
            
            PopulateButtons( GetSingleButtonRect( firstLine ) );
            // _logger.LogEnd( MethodBase.GetCurrentMethod(), true );
        }
        
        private Rect GetSingleButtonRect( Rect position )
        {
            float widthForSeparators = ( _buttonsPerLine - 1 ) * Separator;
            float remainingWidth = position.width - widthForSeparators - 15f;
            // float remainingWidth = position.width;
            _buttonWidth = remainingWidth / _buttonsPerLine;

            // All buttons are square for now. The reduction commented out below was for the purpose of
            // adding a label below the button. However, it's less intrusive to just use the color name
            // as the tooltip.
            // _buttonHeight = _buttonWidth - ( 0.3f * _buttonWidth );
            _buttonHeight = _buttonWidth;
            
            return new Rect( position )
            {
                width = _buttonWidth, 
                height = _buttonHeight
            };

            // Todo: Allow number of buttons per line to be determined by a set width? So far, it doesn't seem necessary from my experience with the window.
            // Otherwise, set all buttons to the cached size and set buttons per line based on the set width vs the window width.
        }
        
        private void PopulateButtons( Rect singleButtonRect )
        {
            // _logger.LogStart( MethodBase.GetCurrentMethod() );
            
            // _logger.Log( $"Button size set to: {GetColoredStringGreen( $"{singleButtonRect.width.ToString()} x {singleButtonRect.height.ToString()}" )}" );
            
            // Draw each element in the position provided, adding a separator between each.
            var buttonRect = new Rect( singleButtonRect );
            int buttonCount = 1;
            foreach ( CustomColor customColor in _colorList )
            {
                Texture2D buttonBackground = GenerateTexture( (int) _buttonWidth, (int) _buttonWidth, customColor.color );
                
                // If at the end up the line, move back to the front on the next line like a carriage return.
                if ( buttonCount > _buttonsPerLine )
                {
                    buttonCount = 1;
                    buttonRect.x = singleButtonRect.x;
                    buttonRect.y += _buttonWidth + Separator;
                }

                _buttons.Add( 
                    new CustomColorButton( 
                        customColor, 
                        new Rect( buttonRect.x, buttonRect.y, _buttonWidth, _buttonHeight ), 
                        buttonBackground 
                    ) 
                );
                
                buttonRect.x += _buttonWidth + Separator;
                buttonCount++;
            }
            // _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }
        
        public override Vector2 GetWindowSize() => _windowSize;

        public override void OnGUI( Rect position )
        {
            GUI.Label( _labelRect, new GUIContent( "Color Picker") );

            // Apply scrollbar if space required for buttons exceeds window size.
            var scrollArea = new Rect( position );
            scrollArea.yMin += EdgePadding + EditorGUIUtility.singleLineHeight + VerticalSeparator;
            _scrollPosition = GUI.BeginScrollView( scrollArea, _scrollPosition, GetTotalPositionRequired( scrollArea ) );
            {
                DrawColorButtons();
            }
            GUI.EndScrollView();
        }
        
        private Rect GetTotalPositionRequired( Rect basePosition )
        {
            // Determine height required to fit all buttons.
            var returnRect = new Rect( basePosition );
            // Remove space on right side to fit scroll bar. (13f + 2f of padding)
            returnRect.xMax -= 15f;
            float totalHeightRequired = 0;
            // Using buttonWidth for height because the buttons are square.
            float totalLinesRequired = Mathf.CeilToInt( _buttons.Count / (float) _buttonsPerLine );
            for (int i = 0; i < totalLinesRequired; i++)
                totalHeightRequired += _buttonWidth + VerticalSeparator;

            returnRect.height = totalHeightRequired +  ( 2 * EdgePadding );
            return new Rect( returnRect );
        }

        private void DrawColorButtons()
        {
            foreach ( CustomColorButton customColorButton in _buttons )
            {
                DrawColorGUIButton( customColorButton );
            }
        }

        private void DrawColorGUIButton( CustomColorButton customColorButton )
        {
            Color cacheColor = GUI.color;
            GUI.color = customColorButton.CustomColor.color;

            if ( GUI.Button( customColorButton.ButtonRect, new GUIContent( customColorButton.Texture2D, customColorButton.CustomColor.name ) ) )
                OnButtonPressedNotify( customColorButton.CustomColor );
            
            GUI.color = cacheColor;
        }
    }
}