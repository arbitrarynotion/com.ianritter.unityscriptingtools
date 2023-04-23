using System;
using Packages.com.ianritter.unityscriptingtools.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows
{
    [Serializable]
    public class SerializedPropertyData
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
            string outputString = $"{TextFormat.GetColoredString( outputPath, pathHighlightHexColor )} = ";
            
            // Add objectID to output string.
            if ( ObjectID > 0 )
                outputString += $"{TextFormat.GetColoredString( "objectID", titleHighlightHexColor )}: {TextFormat.GetColoredString( ObjectID.ToString(), nameHighlightHexColor )}, ";
            
            // Add type to output string.
            outputString += $"{TextFormat.GetColoredString( "type", titleHighlightHexColor )}: {TextFormat.GetColoredString( Type, typeHighlightHexColor )}";
            
            // Add value to output string.
            if ( !_value.Equals( "" ))
                outputString += $"{TextFormat.GetColoredString( ", value", titleHighlightHexColor )}: {TextFormat.GetColoredString( _value, valueHighlightHexColor )}";
            
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
}