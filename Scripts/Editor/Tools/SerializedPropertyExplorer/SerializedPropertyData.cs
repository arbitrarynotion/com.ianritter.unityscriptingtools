using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer
{
    [Serializable]
    public class SerializedPropertyData
    {
        private readonly string _name;
        private readonly string _value;
        public readonly string Path;
        public readonly string Type;
        public SerializedProperty property;

        public SerializedPropertyData( SerializedProperty property )
        {
            this.property = property;
            Depth = Mathf.Max( property.depth, 0 );
            Path = property.propertyPath.Equals( "" ) ? "ROOT: " : property.propertyPath;
            _name = property.name;
            Type = property.type;
            _value = property.isArray && property.propertyType != SerializedPropertyType.String
                ? $"Array of {property.arraySize.ToString()} {property.arrayElementType}(s)."
                : property.GetValueAsString();
            ObjectId = property.GetUnityObjectId();
        }

        public int Depth { get; }
        public int ObjectId { get; }

        public string GetColorizedInfoLine
        (
            string pathHighlightHexColor,
            string titleHighlightHexColor,
            string typeHighlightHexColor,
            string nameHighlightHexColor,
            string valueHighlightHexColor,
            bool removeParentsFromPath = true
        )
        {
            string outputPath = Path;

            // Strip parents from path.
            if( Depth > 0 && removeParentsFromPath )
            {
                outputPath = "";
                string[] pathMembers = Path.Split( '.' );
                for ( int i = Depth; i < pathMembers.Length; i++ )
                {
                    outputPath += pathMembers[i];
                    if( i < pathMembers.Length - 1 )
                        outputPath += ".";
                }
            }

            // Add path to output string.
            string outputString = $"{GetColoredString( outputPath, pathHighlightHexColor )} = ";

            // Add objectID to output string.
            if( ObjectId > 0 )
                outputString += $"{GetColoredString( "objectID", titleHighlightHexColor )}: {GetColoredString( ObjectId.ToString(), nameHighlightHexColor )}, ";

            // Add type to output string.
            outputString += $"{GetColoredString( "type", titleHighlightHexColor )}: {GetColoredString( Type, typeHighlightHexColor )}";

            // Add value to output string.

            if( _name == "m_Script" )
                outputString += $"{GetColoredString( ", value", titleHighlightHexColor )}: {GetColoredString( "Script text", valueHighlightHexColor )}";
            else if( !_value.Equals( "" ) ) outputString += $"{GetColoredString( ", value", titleHighlightHexColor )}: {GetColoredString( _value, valueHighlightHexColor )}";

            // if ( !_value.Equals( "" ) )
            //     outputString += $"{GetColoredString( ", value", titleHighlightHexColor )}: {GetColoredString( _value, valueHighlightHexColor )}";

            return outputString;
        }

        public string GetInfoLine( bool removeParentsFromPath = true )
        {
            string outputPath = Path;
            if( Depth > 0 && removeParentsFromPath )
            {
                outputPath = "";
                string[] pathMembers = Path.Split( '.' );
                for ( int i = Depth; i < pathMembers.Length; i++ )
                {
                    outputPath += pathMembers[i];
                    if( i < pathMembers.Length - 1 )
                        outputPath += ".";
                }
            }

            string outputString = $"{outputPath} = ";

            if( ObjectId > 0 )
                outputString += $"objectID: {ObjectId.ToString()}, ";

            outputString += $"type: {Type}";

            if( _name == "m_Script" )
                outputString += ", value: script";
            else if( !_value.Equals( "" ) ) outputString += $", value: {_value}";

            return outputString;
        }
    }
}