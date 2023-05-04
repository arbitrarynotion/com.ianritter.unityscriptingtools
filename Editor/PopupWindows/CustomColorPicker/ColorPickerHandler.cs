using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker
{
    public class ColorPickerHandler
    {
        private readonly CustomColorPicker _customColorPicker;
        private readonly Rect _position;

        // private SerializedProperty _buttonTexture;

        private const float buttonWidth = 22f;
        private Texture _buttonTextureAsset;
        
        public delegate void ColorSelected( CustomColor color );
        /// <summary>
        ///     Subscribe to this event with the method that will receive the chosen custom color.
        /// </summary>
        public event ColorSelected OnColorSelected;
        private void OnColorSelectedNotify( CustomColor color )
        {
            OnColorSelected?.Invoke( color );
        }

        
        private ColorPickerHandler( Vector2 position )
        {
            _buttonTextureAsset = AssetLoader.LoadScriptableObject<Texture>( "color-circle" );
            string result = _buttonTextureAsset == null ? "failed" : "succeeded";
            Debug.Log( $"ColorPickerHandler: loading of button texture {TextFormat.GetColoredStringYellow( result )}" );
            _position = new Rect( position, Vector2.zero );
        }


        public float GetColorPickerButtonWidth() => buttonWidth;
        
        /// <summary>
        /// Handles the color picker window.
        /// </summary>
        /// <param name="position">This is the top left corner of the window.</param>
        /// <param name="windowSize">The size of the color picker window.</param>
        /// <param name="buttonsPerLine">How many colors will displayed per line within the window. The buttons are square and will auto-size to fit that many per line.</param>
        public ColorPickerHandler( Vector2 position, Vector2 windowSize, int buttonsPerLine = 8 ) : this( position )
        {
            _customColorPicker = new CustomColorPicker( windowSize, buttonsPerLine );
            _customColorPicker.OnButtonPressed += OnColorSelection;
        }

        // public ColorPickerHandler( CustomLogger logger, Rect position, Vector2 windowSize, Vector2 buttonSize ) : this( logger, position )
        // {
        //     _customColorPicker = new CustomColorPicker( logger, windowSize, buttonSize );
        //     _customColorPicker.OnButtonPressed += OnColorSelection;
        // }

        public void Close() => _customColorPicker.editorWindow.Close();

        /// <summary>
        ///     Use this as the button callback to trigger the color picker popup window.
        /// </summary>
        private void ColorPickerButtonPressed( CustomColor targetColor )
        {
            _customColorPicker.SetTargetCustomColor( targetColor );
            PopupWindow.Show( _position, _customColorPicker );
        }

        private void ColorPickerPropertyButtonPressed( SerializedProperty customColorProperty )
        {
            _customColorPicker.SetTargetCustomColor( customColorProperty );
            PopupWindow.Show( _position, _customColorPicker );
        }
        
        public void DrawCustomColorField( CustomColor targetColor )
        {
            Rect lineRect = EditorGUILayout.GetControlRect( true );
            float availableWidth = lineRect.width - buttonWidth;

            // float colorFieldWidth = availableWidth * 0.9f;
            // float colorFieldWidth = availableWidth;
            // float startOfButton = colorFieldWidth;
            
            var colorFieldRect = new Rect( lineRect )
            {
                width = availableWidth
            };
            // DrawRectOutline( colorFieldRect, Color.cyan );
            targetColor.color = EditorGUI.ColorField( colorFieldRect, targetColor.name, targetColor.color );
            
            var buttonRect = new Rect( lineRect ) { width = buttonWidth };
            buttonRect.x += availableWidth;
            buttonRect.xMin += 2f;
            // DrawRectOutline( buttonRect, Color.green );
            DrawColorPickerButton( buttonRect, targetColor );
        }
        
        public void DrawColorPickerButton( Rect position, CustomColor targetColor )
        {
            Vector2 cachedIconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize( new Vector2( 13, 13 ) );
            if ( GUI.Button( position, new GUIContent( _buttonTextureAsset ) ))
                ColorPickerButtonPressed( targetColor );
            
            EditorGUIUtility.SetIconSize( cachedIconSize );
        }
        
        public void DrawColorPickerPropertyButton( Rect position, SerializedProperty customColorProperty )
        {
            // if ( GUI.Button( position, "..." ))

            // var texture = _buttonTexture.objectReferenceValue as Texture;
            Vector2 cachedIconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize( new Vector2( 13, 13 ) );
            if ( GUI.Button( position, new GUIContent( _buttonTextureAsset ) ))
                ColorPickerPropertyButtonPressed( customColorProperty );
            
            EditorGUIUtility.SetIconSize( cachedIconSize );
        }

        private void OnColorSelection( CustomColor color )
        {
            _customColorPicker.editorWindow.Close();
            OnColorSelectedNotify( color );
        }
    }
}
