using System.Collections.Generic;

using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;

using UnityEditor;

using UnityEngine;

using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.EditorWindows.SerializedPropertyExplorer
{
    public static class SerializedPropertyInfoExtractor
    {
        private static bool _expandArrays = true;
        private static bool _simplifyPaths = true;
        private static List<SerializedPropertyData> _extractedData;
        private static string _searchStr;
        private static string _searchStrRep;

        private static readonly CustomColor _titleHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTitleText, Color.grey );
        private static readonly CustomColor _pathHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutPathText, new Color( 0.13f, 0.7f, 0.67f ) );
        private static readonly CustomColor _typeHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTypeText, new Color( 1f, 0.65f, 0f ) );
        private static readonly CustomColor _objectHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutObjectIDText, new Color( 0.2f, 0.8f, 0.2f ) );
        private static readonly CustomColor _valueHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutValueText, new Color( 1f, 1f, 0f ) );
        private static readonly CustomColor _searchHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutSearchText, new Color( 0f, 1f, 0f ) );

        public static void PrintSerializedPropertyInfo( SerializedObject serializedObject, string searchString = "", bool expandArrays = true, bool simplifyPaths = true ) => PrintSerializedPropertyInfo( serializedObject.GetIterator().Copy(), searchString, expandArrays, simplifyPaths );

        public static void PrintSerializedPropertyInfo( SerializedProperty property, string searchString = "", bool expandArrays = true, bool simplifyPaths = true ) => Debug.Log( GetSerializedPropertyInfo( property, searchString, expandArrays, simplifyPaths ) );

        public static string GetSerializedPropertyInfo( SerializedObject serializedObject, string searchString = "", bool expandArrays = true, bool simplifyPaths = true ) => GetSerializedPropertyInfo( serializedObject.GetIterator().Copy(), searchString, expandArrays, simplifyPaths );

        public static string GetSerializedPropertyInfo( SerializedProperty property, string searchString = "", bool expandArrays = true, bool simplifyPaths = true )
        {
            _searchStr = searchString;
            _searchStrRep = $@"{GetColoredString( _searchStr, _searchHighlightColor.GetHex() )}";
            _expandArrays = expandArrays;
            _simplifyPaths = simplifyPaths;

            if( property == null )
            {
                Debug.LogWarning( "Can not extract: SerializedProperty is null!" );
                return "SerializedProperty is null!";
            }

            _extractedData = new List<SerializedPropertyData>();
            Search( property.Copy() );

            string outputString = "";

            // This is the tab added for each level of property data depth.
            string outputTabString = $"{GetColoredString( SerializedPropertyExplorerMenuReadoutDividerText, new Color( 0.1f, 0.1f, 0.1f ) )}    ";

            // Write each line of the serialized property.
            int previousDepth = 0;
            foreach ( SerializedPropertyData line in _extractedData )
            {
                // Start the current line's string.
                string currentLineString = "";

                if( line.Depth == 0 &&
                    previousDepth != 0 )
                    currentLineString += "\n";

                previousDepth = line.Depth;

                // Set the tab amount for current line.
                for ( int i = 0; i < line.Depth; i++ )
                {
                    currentLineString += outputTabString;
                }

                // Set coloration based on whether or not a search time has been specified.
                string propDesc = line.GetColorizedInfoLine( _pathHighlightColor.GetHex(), _titleHighlightColor.GetHex(),
                    _typeHighlightColor.GetHex(), _objectHighlightColor.GetHex(), _valueHighlightColor.GetHex(), _simplifyPaths );

                // if ( !propDesc.Substring( 0, 2 ).Equals( "m_" ) )
                //     propDesc += "\n";

                // if ( !propDesc.Substring( 0, 1 ).Equals( SerializedPropertyExplorerMenuReadoutDividerText ) )
                //     propDesc += "\n";

                // string propDesc = _searchStr.Equals( "" ) 
                //     ? line.GetColorizedInfoLine( _pathHighlightColor.GetHex(), _titleHighlightColor.GetHex(), 
                //         _typeHighlightColor.GetHex(), _objectHighlightColor.GetHex(), _valueHighlightColor.GetHex(), simplifyPaths )
                //     : line.GetInfoLine( simplifyPaths );

                // If a search word was specified, highlight it.
                currentLineString = HighlightSearchedWord( $"{currentLineString}{propDesc}" );

                outputString += currentLineString + "\n";
            }

            return outputString;
        }

        private static string HighlightSearchedWord( string propDesc ) => !string.IsNullOrEmpty( _searchStr ) ? propDesc.Replace( _searchStr, _searchStrRep ) : propDesc;


        private static void Search( SerializedProperty property )
        {
            AnalyzeProperty( property );

            property.Next( true );
            AnalyzeProperty( property );

            bool enterChildren = true;
            SerializedProperty endProp = property;

            // Debug.Log( $"{GetColoredStringBlue( "-->" )} End property set to {GetColoredStringGreen(endProp.name)} for array {GetColoredStringOrange(property.name)}" );
            bool foundEndProperty = true;


            while ( property.Next( IsExplorableType( property ) && enterChildren ) )
            {
                if( property.isArray &&
                    property.propertyType == SerializedPropertyType.Generic )
                {
                    // Debug.Log( $"{GetColoredStringOrange(property.name)} is an array of type {property.arrayElementType} and is {GetColoredStringYellow( IsExplorableType( property ).ToString() )} explorable." );

                    if( SerializedProperty.EqualContents( property, endProp ) &&
                        !foundEndProperty )
                    {
                        // Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property {GetColoredStringGreen(endProp.name)}, which is an array." );
                        foundEndProperty = true;
                    }

                    enterChildren = _expandArrays;

                    if( foundEndProperty )
                    {
                        endProp = property.GetEndProperty();

                        // Debug.Log( $"{GetColoredStringBlue( "-->" )} End property set to {GetColoredStringGreen(endProp.name)} for array {GetColoredStringOrange(property.name)}" );
                        foundEndProperty = false;
                    }
                }
                else
                {
                    enterChildren = true;

                    if( SerializedProperty.EqualContents( property, endProp ) &&
                        !foundEndProperty )
                    {
                        // Debug.Log( $"{GetColoredStringGreen( "<--" )} Found end property: {GetColoredStringGreen(endProp.name)}" );
                        foundEndProperty = true;
                    }
                }


                // bool IsEndProperty()
                // {
                //     if ( endProp.isArray || !SerializedProperty.EqualContents( property, endProp ) ) return false;
                //     
                //     Debug.Log( $"Found end property: {endProp.name}" );
                //     return true;
                // }

                AnalyzeProperty( property );
            }
        }

        private static void AnalyzeProperty( SerializedProperty property ) => _extractedData.Add( new SerializedPropertyData( property ) );


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
    }
}