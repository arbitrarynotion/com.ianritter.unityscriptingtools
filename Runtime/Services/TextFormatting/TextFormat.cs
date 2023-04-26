using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors.PresetColors;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting
{
    public static class TextFormat
    {
        public static string GetColoredString( string data, Color color ) => $"<color=#{ColorUtility.ToHtmlStringRGBA( color )}>{data}</color>";
        public static string GetColoredString( string data, string hexColor ) => $"<color={hexColor}>{data}</color>";
        public static string GetColoredStringBlack( string data ) => $"<color={Black.GetHex()}>{data}</color>";
        public static string GetColoredStringWhite( string data ) => $"<color={White.GetHex()}>{data}</color>";
        public static string GetColoredStringRed( string data ) => $"<color={Red.GetHex()}>{data}</color>";
        public static string GetColoredStringGreen( string data ) => $"<color={Green.GetHex()}>{data}</color>";
        public static string GetColoredStringBlue( string data ) => $"<color={Blue.GetHex()}>{data}</color>";
        public static string GetColoredStringYellow( string data ) => $"<color={Yellow.GetHex()}>{data}</color>";
        public static string GetColoredStringOrange( string data ) => $"<color={Orange.GetHex()}>{data}</color>";

        /// <summary>
        ///     Adds spaces between camelcase naming and removes all special characters.
        /// </summary>
        public static string NicifyVariableName( string varName )
        {
            string returnName = "";
            for (int i = 0; i < varName.Length; i++)
            {
                char currentChar = varName[i];
                
                // Replace underscore with a space.
                if ( currentChar.Equals( '_' ) )
                {
                    returnName += " ";
                    continue;
                }
                
                // Remove special characters.
                if ( !char.IsLetterOrDigit( currentChar ) ) continue;


                // Insert space.
                if ( char.IsUpper( currentChar ) && ( i != 0 ) )
                {
                    // Check if next char is also upper case.
                    if ( i < ( varName.Length - 1 ) && !char.IsUpper( varName[i + 1] ))
                        returnName += " ";
                }

                returnName += currentChar.ToString();
            }

            return returnName;
        }
    }
}
