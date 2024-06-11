using System.Collections.Generic;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.CustomColorPicker.ColorPickerPopupWindow;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer.SerializedPropertyInfoExtractor;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer
{
    public class SerializedPropertyExplorer : EditorWindow
    {
        [MenuItem( SerializedPropertyExplorerMenuItemName )]
        private static void Init()
        {
            var window = (SerializedPropertyExplorer) GetWindow( typeof( SerializedPropertyExplorer ) );
            window.titleContent = new GUIContent( SerializedPropertyExplorerWindowTitle );
            window.Show();
        }


#region EventMethods

        private void OnColorSelection( CustomColor color )
        {
            // Debug.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            UpdateHexColors();
            Repaint();
        }

#endregion


#region DataMembers

        public bool expandArrays = true;
        public bool simplifyPaths = true;
        public CustomColor titleHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTitleText, Color.grey );
        public CustomColor pathHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutPathText, new Color( 0.13f, 0.7f, 0.67f ) );
        public CustomColor typeHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTypeText, new Color( 1f, 0.65f, 0f ) );
        public CustomColor objectHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutObjectIDText, new Color( 0.2f, 0.8f, 0.2f ) );
        public CustomColor valueHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutValueText, new Color( 1f, 1f, 0f ) );
        public CustomColor searchHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutSearchText, new Color( 0f, 1f, 0f ) );

        public SerializedProperty expandArraysProp;
        public SerializedProperty simplifyPathsProp;
        public SerializedProperty titleHighlightColorProp;
        public SerializedProperty pathHighlightColorProp;
        public SerializedProperty typeHighlightColorProp;
        public SerializedProperty objectHighlightColorProp;
        public SerializedProperty valueHighlightColorProp;
        public SerializedProperty searchHighlightColorProp;

        private string _titleHighlightHexColor;
        private string _pathHighlightHexColor;
        private string _typeHighlightHexColor;
        private string _objectHighlightHexColor;
        private string _valueHighlightHexColor;
        private string _searchHighlightHexColor;

        private Object _object;
        private List<SerializedPropertyData> _extractedData;
        private string _searchStr;
        private string _searchStrRep;
        private GUIStyle _richTextStyle;
        private Vector2 _scrollPos;
        private Rect _dataOutputOutlineRect;
        private Rect _optionsRect;
        private bool _optionsFoldoutToggle;
        private float _dataEntryControlRectWidth;
        private float _topOfDataBlock;
        private float _bottomOfDataBlock;

        private const float DataEntryLeftPadding = 15f;
        private const float DataEntryRightPadding = 4f;
        private const float DataBlockRightPadding = 4f;
        private const float DataBlockBottomPadding = 44f;

#endregion


#region LifeCycle

        private void OnEnable()
        {
            _object = null;

            UpdateHexColors();

            ColorPickerHandler.OnColorSelected += OnColorSelection;
        }

        private void OnDisable()
        {
            ColorPickerHandler.OnColorSelected -= OnColorSelection;
        }

        private void OnGUI()
        {
            if( _richTextStyle == null )
                _richTextStyle = new GUIStyle( EditorStyles.label ) { richText = true };

            // Ensure existing object has been analyzed.
            if( _object != null &&
                _extractedData == null )
                AnalyzeObject();

            EditorGUI.indentLevel++;
            {
                DrawOptions();
                DrawColorSettingsToSection();
                DrawButtons();
                DrawSearchField();
                DrawDataBlock();
            }
            EditorGUI.indentLevel--;
        }

#endregion


#region DrawEditorWindow

        private void DrawDataBlock()
        {
            if( _object == null ||
                _extractedData == null ) return;

            EditorGUILayout.Space( 4f );

            Rect lastOptionsRect = GUILayoutUtility.GetLastRect();
            _optionsRect.yMax = lastOptionsRect.yMax;
            _dataOutputOutlineRect = new Rect( lastOptionsRect );

            // if ( _object != null && _extractedData != null )
            DrawDataToWindow();

            float optionsRectHeight = _optionsRect.height;
            _optionsRect.xMin += DataEntryLeftPadding;
            _optionsRect.yMin = _optionsRect.yMax;
            _optionsRect.width = _dataEntryControlRectWidth + DataBlockRightPadding;
            _optionsRect.yMax = _dataOutputOutlineRect.yMax + optionsRectHeight + DataBlockBottomPadding;

            DrawRectOutline( _optionsRect, Color.grey );
        }

        private void DrawOptions()
        {
            EditorGUI.BeginChangeCheck();
            {
                _object = EditorGUILayout.ObjectField( SerializedPropertyExplorerMenuObjectText, _object, typeof( Object ), true );
                _optionsRect = GUILayoutUtility.GetLastRect();
                simplifyPaths = EditorGUILayout.Toggle( SerializedPropertyExplorerMenuSimplifyPathsText, simplifyPaths );
                expandArrays = EditorGUILayout.Toggle( SerializedPropertyExplorerMenuExpandArraysText, expandArrays );
            }
            if( EditorGUI.EndChangeCheck() )
                AnalyzeObject();
        }

        private void DrawSearchField()
        {
            EditorGUI.BeginChangeCheck();
            {
                _searchStr = $@"{EditorGUILayout.TextField( SerializedPropertyExplorerMenuExpandSearchText, _searchStr )}";
                _topOfDataBlock = GUILayoutUtility.GetLastRect().yMax;
            }
            if( EditorGUI.EndChangeCheck() )
                _searchStrRep = $@"{GetColoredString( _searchStr, _searchHighlightHexColor )}";
        }

        private void DrawButtons()
        {
            if( DrawButton( "Print SO to Console" ) )
            {
                if( _object != null )
                    PrintSerializedPropertyInfo( new SerializedObject( _object ) );
            }

            if( DrawButton( "Copy To Clipboard" ) )
                EditorGUIUtility.systemCopyBuffer = GetSerializedPropertyInfo( new SerializedObject( _object ) );
        }

        private bool DrawButton( string buttonText )
        {
            Rect buttonRect = EditorGUILayout.GetControlRect();
            buttonRect.xMin = buttonRect.width - 150f;
            return GUI.Button( buttonRect, new GUIContent( buttonText ) );
        }

        private void DrawColorSettingsToSection()
        {
            _optionsFoldoutToggle = EditorGUILayout.Foldout( _optionsFoldoutToggle, new GUIContent( SerializedPropertyExplorerMenuHighlightColorsText ), true );
            if( !_optionsFoldoutToggle ) return;

            bool colorChanged = false;
            EditorGUI.indentLevel++;
            {
                colorChanged |= ColorPickerHandler.DrawCustomColorField( titleHighlightColor );
                colorChanged |= ColorPickerHandler.DrawCustomColorField( pathHighlightColor );
                colorChanged |= ColorPickerHandler.DrawCustomColorField( typeHighlightColor );
                colorChanged |= ColorPickerHandler.DrawCustomColorField( objectHighlightColor );
                colorChanged |= ColorPickerHandler.DrawCustomColorField( valueHighlightColor );
                colorChanged |= ColorPickerHandler.DrawCustomColorField( searchHighlightColor );
            }
            EditorGUI.indentLevel--;
            if( !colorChanged ) return;

            // Debug.Log( "SerializedPropertyExplorer: Color change was registered." );

            UpdateHexColors();
            Repaint();
        }

        private void DrawDataToWindow()
        {
            string outputTabString = $"{GetColoredString( SerializedPropertyExplorerMenuReadoutDividerText, Color.grey )}    ";

            _scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );
            {
                var controlRect = new Rect();
                for ( int index = 0; index < _extractedData.Count; index++ )
                {
                    if( index == _extractedData.Count - 2 )

                        // _bottomOfDataBlock = GUILayoutUtility.GetLastRect().yMax - _scrollPos.y;
                        _dataOutputOutlineRect.yMax = GUILayoutUtility.GetLastRect().yMax - _scrollPos.y;

                    SerializedPropertyData line = _extractedData[index];

                    string outputString = "";
                    for ( int i = 0; i < line.Depth; i++ )
                    {
                        outputString += outputTabString;
                    }

                    string propDesc = _searchStr.Equals( "" )
                        ? line.GetColorizedInfoLine( _pathHighlightHexColor, _titleHighlightHexColor,
                            _typeHighlightHexColor, _objectHighlightHexColor, _valueHighlightHexColor, simplifyPaths )
                        : line.GetInfoLine( simplifyPaths );

                    outputString = HighlightSearchedWord( $"{outputString}{propDesc}" );

                    controlRect = EditorGUILayout.GetControlRect();
                    controlRect.xMin += DataEntryLeftPadding;
                    controlRect.xMax -= DataEntryRightPadding;

                    // If line is not an object, just print its data.
                    if( line.ObjectId <= 0 ||
                        line.Type.Equals( "PPtr<Prefab>" ) )
                    {
                        EditorGUI.LabelField( controlRect, outputString, _richTextStyle );
                        continue;
                    }

                    // This is an object so include a button to focus it in the editor.
                    GUILayout.BeginHorizontal();
                    {
                        // Draw the entry.
                        var labelRect = new Rect( controlRect );
                        labelRect.width -= 45;
                        EditorGUI.LabelField( labelRect, outputString, _richTextStyle );

                        var buttonRect = new Rect( controlRect );
                        buttonRect.xMin += labelRect.width + 2f;
                        Color cachedColor = GUI.color;
                        GUI.color = objectHighlightColor.color;
                        if( GUI.Button( buttonRect, SerializedPropertyExplorerReadoutButtonText ) )
                        {
                            EditorGUIUtility.PingObject( line.ObjectId );
                            Selection.activeInstanceID = line.ObjectId;
                        }

                        GUI.color = cachedColor;
                    }
                    GUILayout.EndHorizontal();
                }

                _dataEntryControlRectWidth = controlRect.width;
                EditorGUILayout.Space( 10f );
            }
            EditorGUILayout.EndScrollView();
        }

#endregion


#region PropertyDataExtraction

        private void AnalyzeObject()
        {
            _extractedData = new List<SerializedPropertyData>();

            if( _object == null )
                return;

            PerformDataExtraction( new SerializedObject( _object ).GetIterator() );
        }

        private void PerformDataExtraction( SerializedProperty prop )
        {
            ExtractPropertyData( prop );
            prop.Next( true );

            bool enterChildren = true;
            bool foundEndProperty = true;
            SerializedProperty endProp = prop;

            do
            {
                // Handle Arrays
                if( prop.isArray &&
                    prop.propertyType == SerializedPropertyType.Generic )
                {
                    // Debug.Log( $"{GetColoredStringOrange(prop.name)} is an array of type {prop.arrayElementType} and is {GetColoredStringYellow( IsExplorableType( prop ).ToString() )} explorable." );

                    if( SerializedProperty.EqualContents( prop, endProp ) &&
                        !foundEndProperty )
                    {
                        // Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property {GetColoredStringGreen(endProp.name)}, which is an array." );
                        foundEndProperty = true;
                    }

                    enterChildren = expandArrays;

                    if( foundEndProperty )
                    {
                        endProp = prop.GetEndProperty();

                        // Debug.Log( $"{GetColoredStringBlue( "-->" )} End property set to {GetColoredStringGreen(endProp.name)} for array {GetColoredStringOrange(prop.name)}" );
                        foundEndProperty = false;
                    }
                }
                else
                {
                    enterChildren = true;

                    if( SerializedProperty.EqualContents( prop, endProp ) &&
                        !foundEndProperty )
                    {
                        // Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property: {GetColoredStringGreen(endProp.name)}" );
                        foundEndProperty = true;
                    }
                }

                ExtractPropertyData( prop );
            } while ( prop.Next( IsExplorableType( prop ) && enterChildren ) );
        }

        private void ExtractPropertyData( SerializedProperty prop ) => _extractedData.Add( new SerializedPropertyData( prop ) );

#endregion


#region Helpers

        private void UpdateHexColors()
        {
            _titleHighlightHexColor = titleHighlightColor.GetHex();
            _pathHighlightHexColor = pathHighlightColor.GetHex();
            _typeHighlightHexColor = typeHighlightColor.GetHex();
            _objectHighlightHexColor = objectHighlightColor.GetHex();
            _valueHighlightHexColor = valueHighlightColor.GetHex();
            _searchHighlightHexColor = searchHighlightColor.GetHex();
        }

        private string HighlightSearchedWord( string propDesc ) => !string.IsNullOrEmpty( _searchStr ) ? propDesc.Replace( _searchStr, _searchStrRep ) : propDesc;

        private int GetLengthOfLongestStringInSet()
        {
            int longestString = 0;
            foreach ( SerializedPropertyData serializedPropertyData in _extractedData )
            {
                longestString = Mathf.Max( longestString, serializedPropertyData.Path.Length );
            }

            return longestString;
        }

        /// <summary>
        ///     Returns true if this property has child elements.
        ///     The only ones I'm including are Arrays (Generic) and Rects.
        /// </summary>
        private static bool IsExplorableType( SerializedProperty property )
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Rect:
                    return true;

                // case SerializedPropertyType.Integer:
                // case SerializedPropertyType.Boolean:
                // case SerializedPropertyType.Float:
                // case SerializedPropertyType.String:
                // case SerializedPropertyType.Color:
                // case SerializedPropertyType.ObjectReference:
                // case SerializedPropertyType.LayerMask:
                // case SerializedPropertyType.Enum:
                // case SerializedPropertyType.Vector2:
                // case SerializedPropertyType.Vector3:
                // case SerializedPropertyType.Vector4:
                // case SerializedPropertyType.ArraySize:
                // case SerializedPropertyType.Character:
                // case SerializedPropertyType.AnimationCurve:
                // case SerializedPropertyType.Bounds:
                // case SerializedPropertyType.Gradient:
                // case SerializedPropertyType.Quaternion:
                // case SerializedPropertyType.ExposedReference:
                // case SerializedPropertyType.FixedBufferSize:
                // case SerializedPropertyType.Vector2Int:
                // case SerializedPropertyType.Vector3Int:
                // case SerializedPropertyType.RectInt:
                // case SerializedPropertyType.BoundsInt:
                // case SerializedPropertyType.ManagedReference:

                // Had to remove this to make this tool compatible with Unity 2020. 
                // case SerializedPropertyType.Hash128:

                // return false;

                default:
                    return false;

                // Debug.Log( $"{property.propertyType.ToString()} is not supported!" );
                // throw new ArgumentOutOfRangeException(property.propertyType.ToString());
            }
        }

#endregion


        // private void PerformDataExtraction( SerializedProperty prop )
        // {
        //     // ExtractPropertyData( prop );
        //     prop.Next( true );
        //     // bool enterChildren = IsExplorableType(prop ) && ( !prop.isArray || ( prop.isArray && expandArrays ) );
        //     bool enterChildren = IsExplorableType(prop );
        //     do
        //     {
        //         ExtractPropertyData( prop );
        //     } while ( prop.Next( enterChildren ) );
        // }

        // private void DrawCustomColorField( CustomColor targetColor )
        // {
        //     // return EditorGUILayout.ColorField( dataTitle, targetColor, GUILayout.MaxWidth( 350 ) );
        //     Rect lineRect = EditorGUILayout.GetControlRect( true );
        //     float availableWidth = lineRect.width;
        //     const float buttonWidth = 40f;
        //
        //     // float colorFieldWidth = availableWidth * 0.9f;
        //     float colorFieldWidth = availableWidth - buttonWidth;
        //     float startOfButton = colorFieldWidth;
        //     
        //     var colorFieldRect = new Rect( lineRect )
        //     {
        //         width = startOfButton
        //     };
        //     // DrawRectOutline( colorFieldRect, Color.cyan );
        //     targetColor.color = EditorGUI.ColorField( colorFieldRect, targetColor.name, targetColor.color );
        //     
        //     var buttonRect = new Rect( lineRect ) { width = buttonWidth };
        //     buttonRect.x += startOfButton;
        //     buttonRect.xMin += 2f;
        //     // DrawRectOutline( buttonRect, Color.green );
        //     
        //     _colorPickerHandler.DrawColorPickerButton( buttonRect, targetColor );

        // }
    }
}