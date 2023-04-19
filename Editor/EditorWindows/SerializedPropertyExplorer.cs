using System;
using System.Collections.Generic;
using Packages.com.ianritter.unityscriptingtools.Editor.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Runtime.ToolingConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows
{
    public class SerializedPropertyExplorer : EditorWindow
    {
        private class SerializedPropertyData
        {
            public SerializedProperty Property;
            public int Depth { get; }
            public int ObjectID { get; }
            public readonly string Type;
            public readonly string Path;
            
            private readonly string _name;
            private readonly string _value;

            public SerializedPropertyData( SerializedProperty property )
            {
                Property = property;
                Depth = Mathf.Max( property.depth, 0 );
                Path = property.propertyPath.Equals( "" ) ? "ROOT: " : property.propertyPath;
                _name = property.name;
                Type = property.type;
                _value = ( property.isArray && ( property.propertyType != SerializedPropertyType.String ) ) 
                    ? $"array of {property.arraySize.ToString()} {property.arrayElementType}(s)." 
                    : property.GetValueAsString();
                ObjectID = property.GetUnityObjectID();
            }

            public string GetColorizedInfoLine( 
                string pathHighlightHexColor,
                string titleHighlightHexColor,
                string typeHighlightHexColor,
                string nameHighlightHexColor,
                string valueHighlightHexColor,
                bool removeParentsFromPath = true)
            {
                string outputPath = Path;
                // Strip parents from path.
                if ( ( Depth > 0 ) && removeParentsFromPath )
                {
                    outputPath = "";
                    string[] pathMembers = Path.Split( '.' );
                    for (int i = Depth; i < pathMembers.Length; i++)
                    {
                        outputPath += pathMembers[i];
                        if ( i < pathMembers.Length - 1 )
                            outputPath += ".";
                    }
                }
                
                // Add path to output string.
                string outputString = $"{GetColoredString( outputPath, pathHighlightHexColor )} = ";
                
                // Add objectID to output string.
                if ( ObjectID > 0 )
                    outputString += $"{GetColoredString( "objectID", titleHighlightHexColor )}: {GetColoredString( ObjectID.ToString(), nameHighlightHexColor )}, ";
                
                // Add type to output string.
                outputString += $"{GetColoredString( "type", titleHighlightHexColor )}: {GetColoredString( Type, typeHighlightHexColor )}";
                
                // Add value to output string.
                if ( !_value.Equals( "" ))
                    outputString += $"{GetColoredString( ", value", titleHighlightHexColor )}: {GetColoredString( _value, valueHighlightHexColor )}";
                
                return outputString;
            }
            
            public string GetInfoLine( bool removeParentsFromPath = true )
            {
                string outputPath = Path;
                if ( ( Depth > 0 ) && removeParentsFromPath )
                {
                    outputPath = "";
                    string[] pathMembers = Path.Split( '.' );
                    for (int i = Depth; i < pathMembers.Length; i++)
                    {
                        outputPath += pathMembers[i];
                        if ( i < pathMembers.Length - 1 )
                            outputPath += ".";
                    }
                }
                
                string outputString = $"{outputPath} = ";
                if ( ObjectID > 0 )
                    outputString += $"objectID: {ObjectID.ToString()}, ";
                outputString += $"type: {Type}";
                if ( !_value.Equals( "" ))
                    outputString += $", value: {_value}";
                return outputString;
            }
        }

        [MenuItem( SerializedPropertyExplorerMenuItemName )]
        private static void Init()
        {
            var window = (SerializedPropertyExplorer) GetWindow( typeof( SerializedPropertyExplorer ) );
            window.titleContent = new GUIContent( SerializedPropertyExplorerWindowTitle );
            window.Show();
        }

        public bool expandArrays = true;
        public bool simplifyPaths = true;
        public Color titleHighlightColor = Color.grey;
        public Color pathHighlightColor = Color.blue;
        public Color typeHighlightColor = Color.yellow;
        public Color objectHighlightColor = Color.yellow;
        public Color valueHighlightColor = Color.yellow;
        public Color searchHighlightColor = Color.green;
        
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
        private Rect _outputOutlineRect;
        private Rect _optionsRect;
        private bool _optionsFoldoutToggle = false;
        private float _width;

        private void OnEnable() => UpdateHexColors();

        private void UpdateHexColors()
        {
            _titleHighlightHexColor = ColorUtility.ToHtmlStringRGBA( titleHighlightColor );
            _pathHighlightHexColor = ColorUtility.ToHtmlStringRGBA( pathHighlightColor );
            _typeHighlightHexColor = ColorUtility.ToHtmlStringRGBA( typeHighlightColor );
            _objectHighlightHexColor = ColorUtility.ToHtmlStringRGBA( objectHighlightColor );
            _valueHighlightHexColor = ColorUtility.ToHtmlStringRGBA( valueHighlightColor );
            _searchHighlightHexColor = ColorUtility.ToHtmlStringRGBA( searchHighlightColor );
        }
        
        private void OnGUI()
        {
            if ( _richTextStyle == null )
                _richTextStyle = new GUIStyle( EditorStyles.label ) { richText = true };

            EditorGUI.indentLevel++;
            {
                // Ensure existing object has been analyzed.
                if ( _object != null && _extractedData == null )
                    AnalyzeObject();
                
                DrawOptions();

                EditorGUILayout.Space( 4f );
                Rect lastOptionsRect = GUILayoutUtility.GetLastRect();
                // DrawRectOutline( lastOptionsRect, Color.green );

                
                _optionsRect.yMax = lastOptionsRect.yMax;
                _outputOutlineRect = new Rect( lastOptionsRect );

                if ( _object != null && _extractedData != null )
                    DrawDataToWindow();


                float optionsRectHeight = _optionsRect.height;
                
                _optionsRect.xMin += 15f;
                _optionsRect.yMin = _optionsRect.yMax;
                _optionsRect.width = _width - 6f;
                _optionsRect.yMax = _outputOutlineRect.yMax + optionsRectHeight + 44f;
                DrawRectOutline( _optionsRect, Color.grey );

            }
            EditorGUI.indentLevel--;
        }

        private void DrawDataSectionFrame()
        {
            
        }

        private void DrawOptions()
        {
            EditorGUI.BeginChangeCheck();
            {
                _object = EditorGUILayout.ObjectField( SerializedPropertyExplorerMenuObjectText, _object, typeof( Object ), false );
            }
            if ( EditorGUI.EndChangeCheck() ) 
                AnalyzeObject();

            _optionsRect = GUILayoutUtility.GetLastRect();

            EditorGUI.BeginChangeCheck();
            {
                simplifyPaths = EditorGUILayout.Toggle( SerializedPropertyExplorerMenuSimplifyPathsText, simplifyPaths );
            }
            if ( EditorGUI.EndChangeCheck() ) 
                AnalyzeObject();
            
            EditorGUI.BeginChangeCheck();
            {
                expandArrays = EditorGUILayout.Toggle( SerializedPropertyExplorerMenuExpandArraysText, expandArrays );
            }
            if ( EditorGUI.EndChangeCheck() ) 
                AnalyzeObject();
            

            DrawColorSettingsToWindow();
            
            EditorGUI.BeginChangeCheck();
            {
                _searchStr = $@"{EditorGUILayout.TextField( SerializedPropertyExplorerMenuExpandSearchText, _searchStr )}";
            }
            if ( EditorGUI.EndChangeCheck() ) 
                _searchStrRep = $@"{GetColoredString( _searchStr, _searchHighlightHexColor )}";
        }
        
        private void DrawColorSettingsToWindow()
        {
            _optionsFoldoutToggle = EditorGUILayout.Foldout( _optionsFoldoutToggle, new GUIContent( SerializedPropertyExplorerMenuHighlightColorsText ), true );
            if ( !_optionsFoldoutToggle ) return;
            
            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.indentLevel++;
                {
                    titleHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutTitleText, titleHighlightColor );
                    pathHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutPathText, pathHighlightColor );
                    typeHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutTypeText, typeHighlightColor );
                    objectHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutObjectIDText, objectHighlightColor );
                    valueHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutValueText, valueHighlightColor );
                    searchHighlightColor = DrawColorField( SerializedPropertyExplorerReadoutSearchText, searchHighlightColor );
                }
                EditorGUI.indentLevel--;
            }
            if ( !EditorGUI.EndChangeCheck() ) return;
            
            UpdateHexColors();
            Repaint();
        }

        private void AnalyzeObject()
        {
            _extractedData = new List<SerializedPropertyData>();
        
            if ( _object == null ) 
                return;
            
            Search( new SerializedObject( _object ).GetIterator() );
        }

        // private void Search( SerializedProperty prop )
        // {
        //     // AnalyzeProperty( prop );
        //     prop.Next( true );
        //     // bool enterChildren = IsExplorableType(prop ) && ( !prop.isArray || ( prop.isArray && expandArrays ) );
        //     bool enterChildren = IsExplorableType(prop );
        //     do
        //     {
        //         AnalyzeProperty( prop );
        //     } while ( prop.Next( enterChildren ) );
        // }
        
        private void Search( SerializedProperty prop )
        {
            AnalyzeProperty( prop );

            prop.Next( true );
            AnalyzeProperty( prop );

            bool enterChildren = true;
            SerializedProperty endProp = prop;
            Debug.Log( $"{GetColoredStringBlue( "-->" )} End property set to {GetColoredStringGreen(endProp.name)} for array {GetColoredStringOrange(prop.name)}" );
            bool foundEndProperty = true;

            
            while ( prop.Next( IsExplorableType( prop ) && enterChildren ) )
            {
                if ( prop.isArray && ( prop.propertyType == SerializedPropertyType.Generic ) )
                {
                    // Debug.Log( $"{GetColoredStringOrange(prop.name)} is an array of type {prop.arrayElementType} and is {GetColoredStringYellow( IsExplorableType( prop ).ToString() )} explorable." );
                    
                    if ( SerializedProperty.EqualContents( prop, endProp ) && !foundEndProperty )
                    {
                        Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property {GetColoredStringGreen(endProp.name)}, which is an array." );
                        foundEndProperty = true;
                    }
                    
                    enterChildren = expandArrays;
                    
                    if ( foundEndProperty )
                    {
                        endProp = prop.GetEndProperty();
                        Debug.Log( $"{GetColoredStringBlue( "-->" )} End property set to {GetColoredStringGreen(endProp.name)} for array {GetColoredStringOrange(prop.name)}" );
                        foundEndProperty = false;
                    }
                }
                else
                {
                    enterChildren = true;
                    
                    if ( SerializedProperty.EqualContents( prop, endProp ) && !foundEndProperty )
                    {
                        Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property: {GetColoredStringGreen(endProp.name)}" );
                        foundEndProperty = true;
                    }
                }
                

                // bool IsEndProperty()
                // {
                //     if ( endProp.isArray || !SerializedProperty.EqualContents( prop, endProp ) ) return false;
                //     
                //     Debug.Log( $"Found end property: {endProp.name}" );
                //     return true;
                // }

                AnalyzeProperty( prop );
            }
        }

        private void AnalyzeProperty( SerializedProperty prop ) => _extractedData.Add( new SerializedPropertyData( prop ) );


        private void DrawDataToWindow()
        {
            // int cachedIndentLevel = EditorGUI.indentLevel;
            string outputTabString = $"{GetColoredString( SerializedPropertyExplorerMenuReadoutDividerText, Color.grey )}    ";
            
            _scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );
            {
                var controlRect = new Rect();
                for (int index = 0; index < _extractedData.Count; index++)
                {
                    if ( index == _extractedData.Count - 2 )
                        _outputOutlineRect.yMax = GUILayoutUtility.GetLastRect().yMax - _scrollPos.y;
                    
                    SerializedPropertyData line = _extractedData[index];
                    
                    string outputString = "";
                    for (int i = 0; i < line.Depth; i++) 
                        outputString += outputTabString;
                    
                    string propDesc = _searchStr.Equals( "" ) 
                        ? line.GetColorizedInfoLine( _pathHighlightHexColor, _titleHighlightHexColor, 
                            _typeHighlightHexColor, _objectHighlightHexColor, _valueHighlightHexColor, simplifyPaths )
                        : line.GetInfoLine( simplifyPaths );
                    
                    outputString = HighlightSearchedWord( $"{outputString}{propDesc}" );

                    controlRect = EditorGUILayout.GetControlRect();
                    controlRect.xMin += 4f;
                    controlRect.xMax -= 4f;

                    // If line is not an object, just print its data.
                    if ( line.ObjectID <= 0 || line.Type.Equals( "PPtr<Prefab>" ) )
                    {
                        EditorGUI.LabelField( controlRect, outputString, _richTextStyle );
                        continue;
                    }

                    // This is an object so include a button to focus it in the editor.
                    GUILayout.BeginHorizontal();
                    {
                        var labelRect = new Rect( controlRect );
                        labelRect.width -= 45;
                        EditorGUI.LabelField( labelRect, outputString, _richTextStyle );
                        var buttonRect = new Rect( controlRect );
                        buttonRect.xMin += labelRect.width + 2f;
                        Color cachedColor = GUI.color;
                        GUI.color = objectHighlightColor;
                        if ( GUI.Button( buttonRect, SerializedPropertyExplorerReadoutButtonText ) )
                        {
                            EditorGUIUtility.PingObject( line.ObjectID );
                            
                            Selection.activeInstanceID = line.ObjectID;
                        }
                        GUI.color = cachedColor;
                    }
                    GUILayout.EndHorizontal();
                }

                _width = controlRect.width;
                EditorGUILayout.Space( 10f );
            }
            EditorGUILayout.EndScrollView();
            // EditorGUI.indentLevel = cachedIndentLevel;
        }

        private Color DrawColorField( string dataTitle, Color targetColor ) => EditorGUILayout.ColorField( dataTitle, targetColor, GUILayout.MaxWidth( 350 ) );

        private bool IsExplorableType( SerializedProperty property )
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
        
        private string HighlightSearchedWord( string propDesc ) => 
            !string.IsNullOrEmpty( _searchStr ) ? propDesc.Replace( _searchStr, _searchStrRep ) : propDesc;


        private int GetLengthOfLongestStringInSet()
        {
            int longestString = 0;
            foreach ( SerializedPropertyData serializedPropertyData in _extractedData )
            {
                longestString = Mathf.Max( longestString, serializedPropertyData.Path.Length );
            }

            return longestString;
        }
    }
}