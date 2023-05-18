using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker
{
    public static class ColorPickerHandler
    {
        private static readonly CustomColorPicker _customColorPicker;
        private static Rect _position;

        // private SerializedProperty _buttonTexture;

        private static float _buttonWidth = 22f;
        private static readonly Texture ButtonTextureAsset;
        
        public delegate void ColorSelected( CustomColor color );
        /// <summary>
        ///     Subscribe to this event with the method that will receive the chosen custom color.
        /// </summary>
        public static event ColorSelected OnColorSelected;
        private static void OnColorSelectedNotify( CustomColor color )
        {
            OnColorSelected?.Invoke( color );
        }

        static ColorPickerHandler()
        {
            ButtonTextureAsset = AssetLoader.GetAssetByName<Texture>( "color-circle" );
            // string result = _buttonTextureAsset == null ? "failed" : "succeeded";
            // Debug.Log( $"ColorPickerHandler: loading of button texture {TextFormat.GetColoredStringYellow( result )}" );
            _position = new Rect( Vector2.zero, Vector2.zero );
            
            _customColorPicker = new CustomColorPicker( new Vector2( 350f, 400f ), 5 );
            _customColorPicker.OnButtonPressed += OnColorSelection;
        }
        
        public static float GetColorPickerButtonWidth() => _buttonWidth;
        public static void SetColorPickerButtonWidth( float buttonWidth ) => _buttonWidth = buttonWidth;

        public static void SetWindowPosition( Vector2 position ) => _position = new Rect( position, Vector2.zero );
        public static void SetWindowSize( float windowWidth, float windowHeight ) => _customColorPicker.SetWindowSize( new Vector2( windowWidth, windowHeight ));
        
        public static int GetButtonsPerLine() => _customColorPicker.GetButtonsPerLine();
        public static void SetButtonsPerLine( int buttonsPerLine ) => _customColorPicker.SetButtonsPerLine( buttonsPerLine );


        public static void Close() => _customColorPicker.editorWindow.Close();

        /// <summary>
        ///     Use this as the button callback to trigger the color picker popup window.
        /// </summary>
        private static void ColorPickerButtonPressed( CustomColor targetColor )
        {
            _customColorPicker.SetTargetCustomColor( targetColor );
            PopupWindow.Show( _position, _customColorPicker );
        }

        private static void ColorPickerPropertyButtonPressed( SerializedProperty customColorProperty )
        {
            _customColorPicker.SetTargetCustomColor( customColorProperty );
            PopupWindow.Show( _position, _customColorPicker );
        }
        
        public static bool DrawCustomColorField( CustomColor targetColor )
        {
            Rect lineRect = EditorGUILayout.GetControlRect( true );
            float availableWidth = lineRect.width - _buttonWidth;

            var colorFieldRect = new Rect( lineRect )
            {
                width = availableWidth
            };
            // DrawRectOutline( colorFieldRect, Color.cyan );
            EditorGUI.BeginChangeCheck();
            {
                targetColor.color = EditorGUI.ColorField( colorFieldRect, targetColor.name, targetColor.color );
                var buttonRect = new Rect( lineRect ) { width = _buttonWidth };
                buttonRect.x += availableWidth;
                buttonRect.xMin += 2f;
                // DrawRectOutline( buttonRect, Color.green );
                DrawColorPickerButton( buttonRect, targetColor );
            }
            return EditorGUI.EndChangeCheck();
        }
        
        public static void DrawColorPickerButton( Rect position, CustomColor targetColor )
        {
            Vector2 cachedIconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize( new Vector2( 13, 13 ) );
            if ( GUI.Button( position, new GUIContent( ButtonTextureAsset ) ))
                ColorPickerButtonPressed( targetColor );
            
            EditorGUIUtility.SetIconSize( cachedIconSize );
        }
        
        public static void DrawColorPickerPropertyButton( Rect position, SerializedProperty customColorProperty )
        {
            // if ( GUI.Button( position, "..." ))

            // var texture = _buttonTexture.objectReferenceValue as Texture;
            Vector2 cachedIconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize( new Vector2( 13, 13 ) );
            if ( GUI.Button( position, new GUIContent( ButtonTextureAsset ) ))
                ColorPickerPropertyButtonPressed( customColorProperty );
            
            EditorGUIUtility.SetIconSize( cachedIconSize );
        }

        private static void OnColorSelection( CustomColor color )
        {
            _customColorPicker.editorWindow.Close();
            OnColorSelectedNotify( color );
        }
    }
}
