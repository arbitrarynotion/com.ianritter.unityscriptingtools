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
        
        private readonly Vector2 _windowSize = new Vector2( 350f, 75f);
        
        private const float Separator = 2f;
        private const float VerticalSeparator = 2f;
        private const float EdgePadding = 5f;

        private CustomColor _customColor = new CustomColor( "Name_Here", Color.black );

        private string _completeListOfColors = "";

        private void OnEnable()
        {
            minSize = _windowSize;
            maxSize = _windowSize;
        }

        public void OnGUI()
        {
            // Define the position available for this line.
            var drawableArea = new Rect( 0f, 0f, _windowSize.x, _windowSize.y );
            drawableArea.yMin += EdgePadding;
            drawableArea.yMax -= EdgePadding;
            drawableArea.xMin += EdgePadding;
            drawableArea.xMax -= EdgePadding;

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

            EditorGUILayout.Space();
            
            Rect recordColorButton = EditorGUILayout.GetControlRect();
            if ( GUI.Button( recordColorButton, "Record Color" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColor.name}, {_customColor.color.ToString()}" );

                _completeListOfColors += $"public static CustomColor {_customColor.name} = " +
                                         $"new CustomColor( nameof( {_customColor.name} ), new Color(" +
                                         $" {_customColor.color.r.ToString( "0.00" )}f, " +
                                         $"{_customColor.color.g.ToString( "0.00" )}f, " +
                                         $"{_customColor.color.b.ToString( "0.00" )}f ) );\n";
                
                Debug.Log( _completeListOfColors );
            }
            
            Rect resetButtonRect = EditorGUILayout.GetControlRect();
            if ( GUI.Button( resetButtonRect, "Reset List" ) )
            {
                // Debug.Log( $"Color Recorded: {_customColor.name}, {_customColor.color.ToString()}" );

                _completeListOfColors = "";
                
                Debug.Log( "Colors List was reset." );
            }
        }
    }
}
