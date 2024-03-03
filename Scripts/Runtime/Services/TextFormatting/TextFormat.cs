using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors.PresetColors;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.TextFormatting
{
    public static class TextFormat
    {
        public static string GetColoredString( string data, Color color ) => $"<color=#{ColorUtility.ToHtmlStringRGBA( color )}>{data}</color>";
        public static string GetColoredString( string data, string hexColor ) => $"<color={hexColor}>{data}</color>";

        public static string GetColoredStringGrey( string data ) => $"<color={Grey.GetHex()}>{data}</color>";
        
        public static string GetColoredStringLightGrey( string data ) => $"<color={LightGrey.GetHex()}>{data}</color>";
        
        // Grays
        public static string GetColoredStringGainsboro( string data ) => $"<color={Gainsboro.GetHex()}>{data}</color>";
        public static string GetColoredStringLightGray( string data ) => $"<color={LightGray.GetHex()}>{data}</color>";
        public static string GetColoredStringSilver( string data ) => $"<color={Silver.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkGray( string data ) => $"<color={DarkGray.GetHex()}>{data}</color>";
        public static string GetColoredStringGray( string data ) => $"<color={Gray.GetHex()}>{data}</color>";
        public static string GetColoredStringDimGray( string data ) => $"<color={DimGray.GetHex()}>{data}</color>";
        public static string GetColoredStringLightSlateGray( string data ) => $"<color={LightSlateGray.GetHex()}>{data}</color>";
        public static string GetColoredStringSlateGray( string data ) => $"<color={SlateGray.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkSlateGray( string data ) => $"<color={DarkSlateGray.GetHex()}>{data}</color>";
        public static string GetColoredStringBlack( string data ) => $"<color={Black.GetHex()}>{data}</color>";
        
        // Whites
        public static string GetColoredStringWhite( string data ) => $"<color={White.GetHex()}>{data}</color>";
        public static string GetColoredStringSnow( string data ) => $"<color={Snow.GetHex()}>{data}</color>";
        public static string GetColoredStringHoneyDew( string data ) => $"<color={HoneyDew.GetHex()}>{data}</color>";
        public static string GetColoredStringMintCream( string data ) => $"<color={MintCream.GetHex()}>{data}</color>";
        public static string GetColoredStringAzure( string data ) => $"<color={Azure.GetHex()}>{data}</color>";
        public static string GetColoredStringAliceBlue( string data ) => $"<color={AliceBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringGhostWhite( string data ) => $"<color={GhostWhite.GetHex()}>{data}</color>";
        public static string GetColoredStringWhiteSmoke( string data ) => $"<color={WhiteSmoke.GetHex()}>{data}</color>";
        public static string GetColoredStringSeaShell( string data ) => $"<color={SeaShell.GetHex()}>{data}</color>";
        public static string GetColoredStringBeige( string data ) => $"<color={Beige.GetHex()}>{data}</color>";
        public static string GetColoredStringOldLace( string data ) => $"<color={OldLace.GetHex()}>{data}</color>";
        public static string GetColoredStringFloralWhite( string data ) => $"<color={FloralWhite.GetHex()}>{data}</color>";
        public static string GetColoredStringIvory( string data ) => $"<color={Ivory.GetHex()}>{data}</color>";
        public static string GetColoredStringAntiqueWhite( string data ) => $"<color={AntiqueWhite.GetHex()}>{data}</color>";
        public static string GetColoredStringLinen( string data ) => $"<color={Linen.GetHex()}>{data}</color>";
        public static string GetColoredStringLavenderBlush( string data ) => $"<color={LavenderBlush.GetHex()}>{data}</color>";
        public static string GetColoredStringMistyRose( string data ) => $"<color={MistyRose.GetHex()}>{data}</color>";
        
        // Reds
        public static string GetColoredStringIndianRed( string data ) => $"<color={IndianRed.GetHex()}>{data}</color>";
        public static string GetColoredStringLightCoral( string data ) => $"<color={LightCoral.GetHex()}>{data}</color>";
        public static string GetColoredStringSalmon( string data ) => $"<color={Salmon.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkSalmon( string data ) => $"<color={DarkSalmon.GetHex()}>{data}</color>";
        public static string GetColoredStringLightSalmon( string data ) => $"<color={LightSalmon.GetHex()}>{data}</color>";
        public static string GetColoredStringCrimson( string data ) => $"<color={Crimson.GetHex()}>{data}</color>";
        public static string GetColoredStringRed( string data ) => $"<color={Red.GetHex()}>{data}</color>";
        public static string GetColoredStringFireBrick( string data ) => $"<color={FireBrick.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkRed( string data ) => $"<color={DarkRed.GetHex()}>{data}</color>";
        
        // Pinks
        public static string GetColoredStringPink( string data ) => $"<color={Pink.GetHex()}>{data}</color>";
        public static string GetColoredStringLightPink( string data ) => $"<color={LightPink.GetHex()}>{data}</color>";
        public static string GetColoredStringHotPink( string data ) => $"<color={HotPink.GetHex()}>{data}</color>";
        public static string GetColoredStringDeepPink( string data ) => $"<color={DeepPink.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumVioletRed( string data ) => $"<color={MediumVioletRed.GetHex()}>{data}</color>";
        public static string GetColoredStringPaleVioletRed( string data ) => $"<color={PaleVioletRed.GetHex()}>{data}</color>";
        
        // Oranges
        public static string GetColoredStringCoral( string data ) => $"<color={Coral.GetHex()}>{data}</color>";
        public static string GetColoredStringTomato( string data ) => $"<color={Tomato.GetHex()}>{data}</color>";
        public static string GetColoredStringOrangeRed( string data ) => $"<color={OrangeRed.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkOrange( string data ) => $"<color={DarkOrange.GetHex()}>{data}</color>";
        public static string GetColoredStringOrange( string data ) => $"<color={Orange.GetHex()}>{data}</color>";
        
        // Yellows
        public static string GetColoredStringGold( string data ) => $"<color={Gold.GetHex()}>{data}</color>";
        public static string GetColoredStringYellow( string data ) => $"<color={Yellow.GetHex()}>{data}</color>";
        public static string GetColoredStringLightYellow( string data ) => $"<color={LightYellow.GetHex()}>{data}</color>";
        public static string GetColoredStringLemonChiffon( string data ) => $"<color={LemonChiffon.GetHex()}>{data}</color>";
        public static string GetColoredStringLightGoldenrodYellow( string data ) => $"<color={LightGoldenrodYellow.GetHex()}>{data}</color>";
        public static string GetColoredStringPapayaWhip( string data ) => $"<color={PapayaWhip.GetHex()}>{data}</color>";
        public static string GetColoredStringMoccasin( string data ) => $"<color={Moccasin.GetHex()}>{data}</color>";
        public static string GetColoredStringPeachPuff( string data ) => $"<color={PeachPuff.GetHex()}>{data}</color>";
        public static string GetColoredStringPaleGoldenrod( string data ) => $"<color={PaleGoldenrod.GetHex()}>{data}</color>";
        public static string GetColoredStringKhaki( string data ) => $"<color={Khaki.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkKhaki( string data ) => $"<color={DarkKhaki.GetHex()}>{data}</color>";
        
        // Purples
        public static string GetColoredStringLavender( string data ) => $"<color={Lavender.GetHex()}>{data}</color>";
        public static string GetColoredStringThistle( string data ) => $"<color={Thistle.GetHex()}>{data}</color>";
        public static string GetColoredStringPlum( string data ) => $"<color={Plum.GetHex()}>{data}</color>";
        public static string GetColoredStringViolet( string data ) => $"<color={Violet.GetHex()}>{data}</color>";
        public static string GetColoredStringOrchid( string data ) => $"<color={Orchid.GetHex()}>{data}</color>";
        public static string GetColoredStringFuchsia( string data ) => $"<color={Fuchsia.GetHex()}>{data}</color>";
        public static string GetColoredStringMagenta( string data ) => $"<color={Magenta.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumOrchid( string data ) => $"<color={MediumOrchid.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumPurple( string data ) => $"<color={MediumPurple.GetHex()}>{data}</color>";
        public static string GetColoredStringRebeccaPurple( string data ) => $"<color={RebeccaPurple.GetHex()}>{data}</color>";
        public static string GetColoredStringBlueViolet( string data ) => $"<color={BlueViolet.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkViolet( string data ) => $"<color={DarkViolet.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkOrchid( string data ) => $"<color={DarkOrchid.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkMagenta( string data ) => $"<color={DarkMagenta.GetHex()}>{data}</color>";
        public static string GetColoredStringPurple( string data ) => $"<color={Purple.GetHex()}>{data}</color>";
        public static string GetColoredStringIndigo( string data ) => $"<color={Indigo.GetHex()}>{data}</color>";
        public static string GetColoredStringSlateBlue( string data ) => $"<color={SlateBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkSlateBlue( string data ) => $"<color={DarkSlateBlue.GetHex()}>{data}</color>";
        
        // Greens
        public static string GetColoredStringGreenYellow( string data ) => $"<color={GreenYellow.GetHex()}>{data}</color>";
        public static string GetColoredStringChartreuse( string data ) => $"<color={Chartreuse.GetHex()}>{data}</color>";
        public static string GetColoredStringLawnGreen( string data ) => $"<color={LawnGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringLime( string data ) => $"<color={Lime.GetHex()}>{data}</color>";
        public static string GetColoredStringLimeGreen( string data ) => $"<color={LimeGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringPaleGreen( string data ) => $"<color={PaleGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringLightGreen( string data ) => $"<color={LightGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumSpringGreen( string data ) => $"<color={MediumSpringGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringSpringGreen( string data ) => $"<color={SpringGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumSeaGreen( string data ) => $"<color={MediumSeaGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringSeaGreen( string data ) => $"<color={SeaGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringForestGreen( string data ) => $"<color={ForestGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringGreen( string data ) => $"<color={Green.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkGreen( string data ) => $"<color={DarkGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringYellowGreen( string data ) => $"<color={YellowGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringOliveDrab( string data ) => $"<color={OliveDrab.GetHex()}>{data}</color>";
        public static string GetColoredStringOlive( string data ) => $"<color={Olive.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkOliveGreen( string data ) => $"<color={DarkOliveGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumAquamarine( string data ) => $"<color={MediumAquamarine.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkSeaGreen( string data ) => $"<color={DarkSeaGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringLightSeaGreen( string data ) => $"<color={LightSeaGreen.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkCyan( string data ) => $"<color={DarkCyan.GetHex()}>{data}</color>";
        public static string GetColoredStringTeal( string data ) => $"<color={Teal.GetHex()}>{data}</color>";
        
        // Blues
        public static string GetColoredStringAqua( string data ) => $"<color={Aqua.GetHex()}>{data}</color>";
        public static string GetColoredStringCyan( string data ) => $"<color={Cyan.GetHex()}>{data}</color>";
        public static string GetColoredStringLightCyan( string data ) => $"<color={LightCyan.GetHex()}>{data}</color>";
        public static string GetColoredStringPaleTurquoise( string data ) => $"<color={PaleTurquoise.GetHex()}>{data}</color>";
        public static string GetColoredStringAquamarine( string data ) => $"<color={Aquamarine.GetHex()}>{data}</color>";
        public static string GetColoredStringTurquoise( string data ) => $"<color={Turquoise.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumTurquoise( string data ) => $"<color={MediumTurquoise.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkTurquoise( string data ) => $"<color={DarkTurquoise.GetHex()}>{data}</color>";
        public static string GetColoredStringCadetBlue( string data ) => $"<color={CadetBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringSteelBlue( string data ) => $"<color={SteelBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringLightSteelBlue( string data ) => $"<color={LightSteelBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringPowderBlue( string data ) => $"<color={PowderBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringLightBlue( string data ) => $"<color={LightBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringSkyBlue( string data ) => $"<color={SkyBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringLightSkyBlue( string data ) => $"<color={LightSkyBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringDeepSkyBlue( string data ) => $"<color={DeepSkyBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringDodgerBlue( string data ) => $"<color={DodgerBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringCornflowerBlue( string data ) => $"<color={CornflowerBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumSlateBlue( string data ) => $"<color={MediumSlateBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringRoyalBlue( string data ) => $"<color={RoyalBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringBlue( string data ) => $"<color={Blue.GetHex()}>{data}</color>";
        public static string GetColoredStringMediumBlue( string data ) => $"<color={MediumBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkBlue( string data ) => $"<color={DarkBlue.GetHex()}>{data}</color>";
        public static string GetColoredStringNavy( string data ) => $"<color={Navy.GetHex()}>{data}</color>";
        public static string GetColoredStringMidnightBlue( string data ) => $"<color={MidnightBlue.GetHex()}>{data}</color>";
        
        // Browns
        public static string GetColoredStringCornsilk( string data ) => $"<color={Cornsilk.GetHex()}>{data}</color>";
        public static string GetColoredStringBlanchedAlmond( string data ) => $"<color={BlanchedAlmond.GetHex()}>{data}</color>";
        public static string GetColoredStringBisque( string data ) => $"<color={Bisque.GetHex()}>{data}</color>";
        public static string GetColoredStringNavajoWhite( string data ) => $"<color={NavajoWhite.GetHex()}>{data}</color>";
        public static string GetColoredStringWheat( string data ) => $"<color={Wheat.GetHex()}>{data}</color>";
        public static string GetColoredStringBurlyWood( string data ) => $"<color={BurlyWood.GetHex()}>{data}</color>";
        public static string GetColoredStringTan( string data ) => $"<color={Tan.GetHex()}>{data}</color>";
        public static string GetColoredStringRosyBrown( string data ) => $"<color={RosyBrown.GetHex()}>{data}</color>";
        public static string GetColoredStringSandyBrown( string data ) => $"<color={SandyBrown.GetHex()}>{data}</color>";
        public static string GetColoredStringGoldenrod( string data ) => $"<color={Goldenrod.GetHex()}>{data}</color>";
        public static string GetColoredStringDarkGoldenrod( string data ) => $"<color={DarkGoldenrod.GetHex()}>{data}</color>";
        public static string GetColoredStringPeru( string data ) => $"<color={Peru.GetHex()}>{data}</color>";
        public static string GetColoredStringChocolate( string data ) => $"<color={Chocolate.GetHex()}>{data}</color>";
        public static string GetColoredStringSaddleBrown( string data ) => $"<color={SaddleBrown.GetHex()}>{data}</color>";
        public static string GetColoredStringSienna( string data ) => $"<color={Sienna.GetHex()}>{data}</color>";
        public static string GetColoredStringBrown( string data ) => $"<color={Brown.GetHex()}>{data}</color>";
        public static string GetColoredStringMaroon( string data ) => $"<color={Maroon.GetHex()}>{data}</color>";
        
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
