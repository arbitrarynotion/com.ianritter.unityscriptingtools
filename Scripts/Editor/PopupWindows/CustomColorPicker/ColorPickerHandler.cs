using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.PopupWindows.CustomColorPicker
{
    public static class ColorPickerHandler
    {
        private static readonly CustomColorPicker CustomColorPicker;
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
            Close();
        }

        static ColorPickerHandler()
        {
            ButtonTextureAsset = AssetLoader.GetAssetByName<Texture>( "color-circle" );
            // string result = _buttonTextureAsset == null ? "failed" : "succeeded";
            // Debug.Log( $"ColorPickerHandler: loading of button texture {TextFormat.GetColoredStringYellow( result )}" );
            _position = new Rect( Vector2.zero, Vector2.zero );
            
            CustomColorPicker = new CustomColorPicker( new Vector2( 350f, 400f ), 5 );
            CustomColorPicker.OnButtonPressed += OnColorSelection;
        }
        
        public static float GetColorPickerButtonWidth() => _buttonWidth;
        public static void SetColorPickerButtonWidth( float buttonWidth ) => _buttonWidth = buttonWidth;

        public static void SetWindowPosition( Vector2 position ) => _position = new Rect( position, Vector2.zero );
        public static void SetWindowSize( float windowWidth, float windowHeight ) => CustomColorPicker.SetWindowSize( new Vector2( windowWidth, windowHeight ));
        
        public static int GetButtonsPerLine() => CustomColorPicker.GetButtonsPerLine();
        public static void SetButtonsPerLine( int buttonsPerLine ) => CustomColorPicker.SetButtonsPerLine( buttonsPerLine );


        private static void Close() => CustomColorPicker.editorWindow.Close();

        /// <summary>
        ///     Use this as the button callback to trigger the color picker popup window.
        /// </summary>
        private static void ColorPickerButtonPressed( CustomColor targetColor )
        {
            CustomColorPicker.SetTargetCustomColor( targetColor );
            PopupWindow.Show( _position, CustomColorPicker );
        }

        private static void ColorPickerPropertyButtonPressed( SerializedProperty customColorProperty )
        {
            CustomColorPicker.SetTargetCustomColor( customColorProperty );
            PopupWindow.Show( _position, CustomColorPicker );
        }
        
        public static bool DrawCustomColorField( CustomColor targetColor )
        {
            Rect lineRect = EditorGUILayout.GetControlRect( true );
            SetWindowPosition( new Vector2( lineRect.x, lineRect.y ) );
            // float availableWidth = lineRect.width - _buttonWidth;
            const float colorFieldWidth = 250f;

            var colorFieldRect = new Rect( lineRect )
            {
                width = colorFieldWidth
            };
            // DrawRectOutline( colorFieldRect, Color.cyan );
            EditorGUI.BeginChangeCheck();
            {
                targetColor.color = EditorGUI.ColorField( colorFieldRect, targetColor.name, targetColor.color );
                var buttonRect = new Rect( lineRect ) { width = _buttonWidth };
                buttonRect.x += colorFieldWidth;
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
            CustomColorPicker.editorWindow.Close();
            OnColorSelectedNotify( color );
        }
        
        private static void DrawPropertyWithColorPicker( 
            Rect positionRect, 
            SerializedProperty fieldProperty, 
            SerializedProperty targetProperty, 
            GUIContent guiContent )
        {
            SetWindowPosition( positionRect.position );
        
            // Exclude color picker button width from available width.
            float availableWidth = positionRect.width - GetColorPickerButtonWidth();
        
            var lineRect = new Rect( positionRect )
            {
                width = availableWidth
            };
            // DrawRectOutline( lineRect, Color.grey );
        
            // Draw Property label and field.
            var propertyFieldRect = new Rect( lineRect );
            // DrawRectOutline( propertyField, Color.magenta );
        
            EditorGUI.PropertyField( propertyFieldRect, fieldProperty, guiContent );
            
            // Set the color picker button rect to start at the end of the available space plus a spacer.
            // Then get the width from the color picker handler.
            var buttonRect = new Rect( positionRect );
            buttonRect.xMin += availableWidth + 2f;
            buttonRect.width = GetColorPickerButtonWidth();
            // DrawRectOutline( buttonRect, Color.yellow );
            
            // Finally, pass the button rect and the color property to the color picker handler.
            // This can either be a direct color property via serializedObject.FindProperty or an indirect one via property.FindPropertyRelative.
            DrawColorPickerPropertyButton( buttonRect, targetProperty );
        }
        
        /// <summary>
        ///     Draw UnityEngine.Color serialized property and add a color picker button at the end.
        /// </summary>
        public static void DrawPropertyWithColorPicker( SerializedProperty property, GUIContent guiContent = null ) => 
            DrawPropertyWithColorPicker( EditorGUILayout.GetControlRect(), property, property, guiContent ?? GUIContent.none );
        
        /// <summary>
        ///     Draw UnityEngine.Color serialized property and add a color picker button at the end.
        /// </summary>
        public static void DrawPropertyWithColorPicker( Rect positionRect, SerializedProperty property, GUIContent guiContent = null ) => 
            DrawPropertyWithColorPicker( positionRect, property, property, guiContent ?? GUIContent.none );



        // public static void DrawPropertyWithColorPicker( Rect positionRect, SerializedProperty fieldProperty, SerializedProperty targetProperty, GUIContent guiContent )
        // {
        //     SetWindowPosition( positionRect.position );
        //
        //     // Exclude color picker button width from available width.
        //     float availableWidth = positionRect.width - GetColorPickerButtonWidth();
        //
        //     var lineRect = new Rect( positionRect )
        //     {
        //         width = availableWidth
        //     };
        //     // DrawRectOutline( lineRect, Color.grey );
        //
        //     // Draw Property label and field.
        //     var propertyFieldRect = new Rect( lineRect );
        //     // DrawRectOutline( propertyField, Color.magenta );
        //
        //     EditorGUI.PropertyField( propertyFieldRect, fieldProperty, guiContent );
        //     
        //     // Set the color picker button rect to start at the end of the available space plus a spacer.
        //     // Then get the width from the color picker handler.
        //     var buttonRect = new Rect( positionRect );
        //     buttonRect.xMin += availableWidth + 2f;
        //     buttonRect.width = GetColorPickerButtonWidth();
        //     // DrawRectOutline( buttonRect, Color.yellow );
        //     
        //     // Finally, pass the button rect and the color property to the color picker handler.
        //     // This can either be a direct color property via serializedObject.FindProperty or an indirect one via property.FindPropertyRelative.
        //     DrawColorPickerPropertyButton( buttonRect, targetProperty );
        // }
        //
        // // 
        // public static void DrawPropertyWithColorPicker( SerializedProperty property, GUIContent guiContent = null ) => 
        //     DrawPropertyWithColorPicker( EditorGUILayout.GetControlRect(), property, property, guiContent ?? GUIContent.none );
        //
        // public static void DrawPropertyWithColorPicker( Rect positionRect, SerializedProperty property, GUIContent guiContent = null ) => 
        //     DrawPropertyWithColorPicker( positionRect, property, property, guiContent ?? GUIContent.none );
        //
        // public static void DrawCustomColorProperty( SerializedProperty property, GUIContent guiContent = null )
        // {
        //     DrawCustomColorProperty( EditorGUILayout.GetControlRect(), property, guiContent ?? GUIContent.none );
        // }
        //
        //
        // public static void DrawCustomColorProperty( Rect positionRect, SerializedProperty property, GUIContent guiContent = null )
        // {
        //     SerializedProperty colorProperty = property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "color" );
        //     if ( colorProperty == null )
        //     {
        //         Debug.LogError( "Failed to find 'customColor' property or its 'color' property." );
        //         return;
        //     }
        //     DrawPropertyWithColorPicker( positionRect, property, colorProperty, guiContent ?? GUIContent.none );
        // }
        
        
        // private static void ResolveColorProperty( Rect positionRect, SerializedProperty property, GUIContent guiContent )
        // {
        //     SerializedProperty colorProperty = null;
        //     // Case 1: property is a color value.
        //     if ( property.propertyType == SerializedPropertyType.Color )
        //     {
        //         // colorProperty = property;
        //         DrawPropertyWithColorPicker( positionRect, property, property, guiContent ?? GUIContent.none );
        //         return;
        //     }
        //     
        //     // Case 2: property is a custom color
        //     colorProperty = property.FindPropertyRelative( "color" );
        //     if ( colorProperty != null )
        //     {
        //         DrawPropertyWithColorPicker( positionRect, property, colorProperty, guiContent ?? GUIContent.none );
        //         return;
        //     }
        //     
        //     // Case 3: property contains a custom color property
        //     colorProperty = property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "color" );
        //     SerializedProperty nameProperty = property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "name" );
        //     if ( colorProperty != null )
        //     {
        //         // Split the remaining space between the string and the color fields.
        //         var stringRect = new Rect( positionRect )
        //         {
        //             height = EditorGUIUtility.singleLineHeight + 2f, 
        //             width = positionRect.width * 0.7f
        //         };
        //         // float singleFieldWidth = ( position.width / 2f ) - 4f;
        //         // stringRect.width = singleFieldWidth;
        //
        //         var colorFieldRect = new Rect( positionRect )
        //         {
        //             height = EditorGUIUtility.singleLineHeight + 2f
        //         };
        //         // colorFieldRect.xMin += singleFieldWidth + 4f;
        //         colorFieldRect.xMin += stringRect.width + 2f;
        //
        //         EditorGUI.PropertyField( stringRect, nameProperty, GUIContent.none );
        //         float labelWidth = EditorGUIUtility.labelWidth;
        //         
        //         int cachedIndentLevel = EditorGUI.indentLevel;
        //         EditorGUI.indentLevel = 0;
        //         
        //         EditorGUIUtility.labelWidth = 0.01f;
        //         EditorGUI.PropertyField( colorFieldRect, colorProperty, GUIContent.none );
        //         
        //         EditorGUIUtility.labelWidth = labelWidth;
        //         EditorGUI.indentLevel = cachedIndentLevel;
        //         
        //         // DrawPropertyWithColorPicker( positionRect, property, colorProperty, guiContent ?? GUIContent.none );
        //         
        //         return;
        //     }
        //     
        //     Debug.LogError( "Failed to find 'customColor' property or its 'color' property." );
        //
        //     // DrawPropertyWithColorPicker( EditorGUILayout.GetControlRect(), property, colorProperty, guiContent ?? GUIContent.none );
        // }
        //
        // // Draw cases:
        // // Case 1: property has to get its own position rect.
        // // Case 2: property is drawn in provided position rect.
        //
        // public static void DrawColorPickerProperty( SerializedProperty property, GUIContent guiContent = null ) =>
        //     ResolveColorProperty( EditorGUILayout.GetControlRect(), property, guiContent ?? GUIContent.none );
        // // DrawPropertyWithColorPicker( EditorGUILayout.GetControlRect(), property, ResolveColorProperty( property ), guiContent ?? GUIContent.none );
        //
        // public static void DrawColorPickerProperty( Rect positionRect, SerializedProperty property, GUIContent guiContent = null ) => 
        //     ResolveColorProperty( positionRect, property, guiContent ?? GUIContent.none );
        //     // DrawPropertyWithColorPicker( positionRect, property, ResolveColorProperty( property ), guiContent ?? GUIContent.none );
    }
}
