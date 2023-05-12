using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors
{
    public static class PresetColors
    {

        
        // Grey Tones.
        // public static CustomColor Black = new CustomColor( nameof( Black ), new Color(0, 0, 0 ) );
        public static readonly CustomColor Grey = new CustomColor( nameof( Grey ), new Color( 0.5f, 0.5f, 0.5f ) );

        public static readonly CustomColor LightGrey = new CustomColor( nameof( LightGrey ), new Color( 0.75f, 0.75f, 0.75f ) );
        // public static CustomColor White = new CustomColor( nameof( White ), new Color( 1, 1, 1 ) );

        // Grays
        public static readonly CustomColor Gainsboro = new CustomColor( nameof( Gainsboro ), new Color( 0.86f, 0.86f, 0.86f ) );
        public static readonly CustomColor LightGray = new CustomColor( nameof( LightGray ), new Color( 0.83f, 0.83f, 0.83f) );
        public static readonly CustomColor Silver = new CustomColor( nameof( Silver ), new Color( 0.75f, 0.75f, 0.75f) );
        public static readonly CustomColor DarkGray = new CustomColor( nameof( DarkGray ), new Color( 0.66f, 0.66f, 0.66f) );
        public static readonly CustomColor Gray = new CustomColor( nameof( Gray ), new Color( 0.50f, 0.50f, 0.50f) );
        public static readonly CustomColor DimGray = new CustomColor( nameof( DimGray ), new Color( 0.41f, 0.41f, 0.41f) );
        public static readonly CustomColor LightSlateGray = new CustomColor( nameof( LightSlateGray ), new Color( 0.47f, 0.53f, 0.60f) );
        public static readonly CustomColor SlateGray = new CustomColor( nameof( SlateGray ), new Color( 0.44f, 0.50f, 0.56f) );
        public static readonly CustomColor DarkSlateGray = new CustomColor( nameof( DarkSlateGray ), new Color( 0.18f, 0.31f, 0.31f) );
        public static readonly CustomColor Black = new CustomColor( nameof( Black ), new Color( 0.00f, 0.00f, 0.00f) );

        
        // Whites
        public static readonly CustomColor White = new CustomColor( nameof( White ), new Color( 1.00f, 1.00f, 1.00f) );
        public static readonly CustomColor Snow = new CustomColor( nameof( Snow ), new Color( 1.00f, 0.98f, 0.98f) );
        public static readonly CustomColor HoneyDew = new CustomColor( nameof( HoneyDew ), new Color( 0.94f, 1.00f, 0.94f) );
        public static readonly CustomColor MintCream = new CustomColor( nameof( MintCream ), new Color( 0.96f, 1.00f, 0.98f) );
        public static readonly CustomColor Azure = new CustomColor( nameof( Azure ), new Color( 0.94f, 1.00f, 1.00f) );
        public static readonly CustomColor AliceBlue = new CustomColor( nameof( AliceBlue ), new Color( 0.94f, 0.97f, 1.00f) );
        public static readonly CustomColor GhostWhite = new CustomColor( nameof( GhostWhite ), new Color( 0.97f, 0.97f, 1.00f) );
        public static readonly CustomColor WhiteSmoke = new CustomColor( nameof( WhiteSmoke ), new Color( 0.96f, 0.96f, 0.96f) );
        public static readonly CustomColor SeaShell = new CustomColor( nameof( SeaShell ), new Color( 1.00f, 0.96f, 0.93f) );
        public static readonly CustomColor Beige = new CustomColor( nameof( Beige ), new Color( 0.96f, 0.96f, 0.86f) );
        public static readonly CustomColor OldLace = new CustomColor( nameof( OldLace ), new Color( 0.99f, 0.96f, 0.90f) );
        public static readonly CustomColor FloralWhite = new CustomColor( nameof( FloralWhite ), new Color( 1.00f, 0.98f, 0.94f) );
        public static readonly CustomColor Ivory = new CustomColor( nameof( Ivory ), new Color( 1.00f, 1.00f, 0.94f) );
        public static readonly CustomColor AntiqueWhite = new CustomColor( nameof( AntiqueWhite ), new Color( 0.98f, 0.92f, 0.84f) );
        public static readonly CustomColor Linen = new CustomColor( nameof( Linen ), new Color( 0.98f, 0.94f, 0.90f) );
        public static readonly CustomColor LavenderBlush = new CustomColor( nameof( LavenderBlush ), new Color( 1.00f, 0.94f, 0.96f) );
        public static readonly CustomColor MistyRose = new CustomColor( nameof( MistyRose ), new Color( 1.00f, 0.89f, 0.88f) );


        // Reds
        public static readonly CustomColor IndianRed = new CustomColor( nameof( IndianRed ), new Color(0.80f, 0.36f, 0.36f) );
        public static readonly CustomColor LightCoral = new CustomColor( nameof( LightCoral ), new Color(0.94f, 0.50f, 0.50f) );
        public static readonly CustomColor Salmon = new CustomColor( nameof( Salmon ), new Color(0.98f, 0.50f, 0.45f) );
        public static readonly CustomColor DarkSalmon = new CustomColor( nameof( DarkSalmon ), new Color(0.91f, 0.59f, 0.48f) );
        public static readonly CustomColor LightSalmon = new CustomColor( nameof( LightSalmon ), new Color(1.00f, 0.63f, 0.48f) );
        public static readonly CustomColor Crimson = new CustomColor( nameof( Crimson ), new Color(0.86f, 0.08f, 0.24f) );
        public static readonly CustomColor Red = new CustomColor( nameof( Red ), new Color(1.00f, 0.00f, 0.00f) );
        public static readonly CustomColor FireBrick = new CustomColor( nameof( FireBrick ), new Color(0.70f, 0.13f, 0.13f) );
        public static readonly CustomColor DarkRed = new CustomColor( nameof( DarkRed ), new Color(0.55f, 0.00f, 0.00f) );

        // Pinks
        public static readonly CustomColor Pink = new CustomColor( nameof( Pink ), new Color(1.00f, 0.75f, 0.80f) );
        public static readonly CustomColor LightPink = new CustomColor( nameof( LightPink ), new Color(1.00f, 0.71f, 0.76f) );
        public static readonly CustomColor HotPink = new CustomColor( nameof( HotPink ), new Color(1.00f, 0.41f, 0.71f) );
        public static readonly CustomColor DeepPink = new CustomColor( nameof( DeepPink ), new Color(1.00f, 0.08f, 0.58f) );
        public static readonly CustomColor MediumVioletRed = new CustomColor( nameof( MediumVioletRed ), new Color(0.78f, 0.08f, 0.52f) );
        public static readonly CustomColor PaleVioletRed = new CustomColor( nameof( PaleVioletRed ), new Color(0.86f, 0.44f, 0.58f) );

        // Oranges
        public static readonly CustomColor Coral = new CustomColor( nameof( Coral ), new Color(1.00f, 0.50f, 0.31f) );
        public static readonly CustomColor Tomato = new CustomColor( nameof( Tomato ), new Color(1.00f, 0.39f, 0.28f) );
        public static readonly CustomColor OrangeRed = new CustomColor( nameof( OrangeRed ), new Color(1.00f, 0.27f, 0.00f) );
        public static readonly CustomColor DarkOrange = new CustomColor( nameof( DarkOrange ), new Color(1.00f, 0.55f, 0.00f) );
        public static readonly CustomColor Orange = new CustomColor( nameof( Orange ), new Color(1.00f, 0.65f, 0.00f) );


        // Yellows
        public static readonly CustomColor Gold = new CustomColor( nameof( Gold ), new Color( 1.00f, 0.84f, 0.00f) );
        public static readonly CustomColor Yellow = new CustomColor( nameof( Yellow ), new Color( 1.00f, 1.00f, 0.00f) );
        public static readonly CustomColor LightYellow = new CustomColor( nameof( LightYellow ), new Color( 1.00f, 1.00f, 0.88f) );
        public static readonly CustomColor LemonChiffon = new CustomColor( nameof( LemonChiffon ), new Color( 1.00f, 0.98f, 0.80f) );
        public static readonly CustomColor LightGoldenrodYellow = new CustomColor( nameof( LightGoldenrodYellow ), new Color( 0.98f, 0.98f, 0.82f) );
        public static readonly CustomColor PapayaWhip = new CustomColor( nameof( PapayaWhip ), new Color( 1.00f, 0.94f, 0.84f) );
        public static readonly CustomColor Moccasin = new CustomColor( nameof( Moccasin ), new Color( 1.00f, 0.89f, 0.71f) );
        public static readonly CustomColor PeachPuff = new CustomColor( nameof( PeachPuff ), new Color( 1.00f, 0.85f, 0.73f) );
        public static readonly CustomColor PaleGoldenrod = new CustomColor( nameof( PaleGoldenrod ), new Color( 0.93f, 0.91f, 0.67f) );
        public static readonly CustomColor Khaki = new CustomColor( nameof( Khaki ), new Color( 0.94f, 0.90f, 0.55f) );
        public static readonly CustomColor DarkKhaki = new CustomColor( nameof( DarkKhaki ), new Color( 0.74f, 0.72f, 0.42f) );
        public static readonly CustomColor Blonde = new CustomColor( nameof( Blonde ), new Color( 0.94f, 0.89f, 0.71f ) );
        public static readonly CustomColor BabyYellow = new CustomColor( nameof( BabyYellow ), new Color( 1.00f, 0.99f, 0.79f ) );
        public static readonly CustomColor BananaYellow = new CustomColor( nameof( BananaYellow ), new Color( 1.00f, 0.87f, 0.00f ) );
        public static readonly CustomColor Butter = new CustomColor( nameof( Butter ), new Color( 0.96f, 0.92f, 0.38f ) );
        public static readonly CustomColor LemonYellow = new CustomColor( nameof( LemonYellow ), new Color( 1.00f, 0.95f, 0.43f ) );
        public static readonly CustomColor NeonYellow = new CustomColor( nameof( NeonYellow ), new Color( 0.88f, 0.91f, 0.13f ) );
        public static readonly CustomColor Brass = new CustomColor( nameof( Brass ), new Color( 0.67f, 0.62f, 0.24f ) );

        // Purples
        public static readonly CustomColor Lavender = new CustomColor( nameof( Lavender ), new Color( 0.90f, 0.90f, 0.98f) );
        public static readonly CustomColor Thistle = new CustomColor( nameof( Thistle ), new Color( 0.85f, 0.75f, 0.85f) );
        public static readonly CustomColor Plum = new CustomColor( nameof( Plum ), new Color( 0.87f, 0.63f, 0.87f) );
        public static readonly CustomColor Violet = new CustomColor( nameof( Violet ), new Color( 0.93f, 0.51f, 0.93f) );
        public static readonly CustomColor Orchid = new CustomColor( nameof( Orchid ), new Color( 0.85f, 0.44f, 0.84f) );
        public static readonly CustomColor Fuchsia = new CustomColor( nameof( Fuchsia ), new Color( 1.00f, 0.00f, 1.00f) );
        public static readonly CustomColor Magenta = new CustomColor( nameof( Magenta ), new Color( 1.00f, 0.00f, 1.00f) );
        public static readonly CustomColor MediumOrchid = new CustomColor( nameof( MediumOrchid ), new Color( 0.73f, 0.33f, 0.83f) );
        public static readonly CustomColor MediumPurple = new CustomColor( nameof( MediumPurple ), new Color( 0.58f, 0.44f, 0.86f) );
        public static readonly CustomColor RebeccaPurple = new CustomColor( nameof( RebeccaPurple ), new Color( 0.40f, 0.20f, 0.60f) );
        public static readonly CustomColor BlueViolet = new CustomColor( nameof( BlueViolet ), new Color( 0.54f, 0.17f, 0.89f) );
        public static readonly CustomColor DarkViolet = new CustomColor( nameof( DarkViolet ), new Color( 0.58f, 0.00f, 0.83f) );
        public static readonly CustomColor DarkOrchid = new CustomColor( nameof( DarkOrchid ), new Color( 0.60f, 0.20f, 0.80f) );
        public static readonly CustomColor DarkMagenta = new CustomColor( nameof( DarkMagenta ), new Color( 0.55f, 0.00f, 0.55f) );
        public static readonly CustomColor Purple = new CustomColor( nameof( Purple ), new Color( 0.50f, 0.00f, 0.50f) );
        public static readonly CustomColor Indigo = new CustomColor( nameof( Indigo ), new Color( 0.29f, 0.00f, 0.51f) );
        public static readonly CustomColor SlateBlue = new CustomColor( nameof( SlateBlue ), new Color( 0.42f, 0.35f, 0.80f) );
        public static readonly CustomColor DarkSlateBlue = new CustomColor( nameof( DarkSlateBlue ), new Color( 0.28f, 0.24f, 0.55f) );

        // Greens
        public static readonly CustomColor GreenYellow = new CustomColor( nameof( GreenYellow ), new Color( 0.68f, 1.00f, 0.18f) );
        public static readonly CustomColor Chartreuse = new CustomColor( nameof( Chartreuse ), new Color( 0.50f, 1.00f, 0.00f) );
        public static readonly CustomColor LawnGreen = new CustomColor( nameof( LawnGreen ), new Color( 0.49f, 0.99f, 0.00f) );
        public static readonly CustomColor Lime = new CustomColor( nameof( Lime ), new Color( 0.00f, 1.00f, 0.00f) );
        public static readonly CustomColor LimeGreen = new CustomColor( nameof( LimeGreen ), new Color( 0.20f, 0.80f, 0.20f) );
        public static readonly CustomColor PaleGreen = new CustomColor( nameof( PaleGreen ), new Color( 0.60f, 0.98f, 0.60f) );
        public static readonly CustomColor LightGreen = new CustomColor( nameof( LightGreen ), new Color( 0.56f, 0.93f, 0.56f) );
        public static readonly CustomColor MediumSpringGreen = new CustomColor( nameof( MediumSpringGreen ), new Color( 0.00f, 0.98f, 0.60f) );
        public static readonly CustomColor SpringGreen = new CustomColor( nameof( SpringGreen ), new Color( 0.00f, 1.00f, 0.50f) );
        public static readonly CustomColor MediumSeaGreen = new CustomColor( nameof( MediumSeaGreen ), new Color( 0.24f, 0.70f, 0.44f) );
        public static readonly CustomColor SeaGreen = new CustomColor( nameof( SeaGreen ), new Color( 0.18f, 0.55f, 0.34f) );
        public static readonly CustomColor ForestGreen = new CustomColor( nameof( ForestGreen ), new Color( 0.13f, 0.55f, 0.13f) );
        public static readonly CustomColor Green = new CustomColor( nameof( Green ), new Color( 0.00f, 0.50f, 0.00f) );
        public static readonly CustomColor DarkGreen = new CustomColor( nameof( DarkGreen ), new Color( 0.00f, 0.39f, 0.00f) );
        public static readonly CustomColor YellowGreen = new CustomColor( nameof( YellowGreen ), new Color( 0.60f, 0.80f, 0.20f) );
        public static readonly CustomColor OliveDrab = new CustomColor( nameof( OliveDrab ), new Color( 0.42f, 0.56f, 0.14f) );
        public static readonly CustomColor Olive = new CustomColor( nameof( Olive ), new Color( 0.50f, 0.50f, 0.00f) );
        public static readonly CustomColor DarkOliveGreen = new CustomColor( nameof( DarkOliveGreen ), new Color( 0.33f, 0.42f, 0.18f) );
        public static readonly CustomColor MediumAquamarine = new CustomColor( nameof( MediumAquamarine ), new Color( 0.40f, 0.80f, 0.67f) );
        public static readonly CustomColor DarkSeaGreen = new CustomColor( nameof( DarkSeaGreen ), new Color( 0.56f, 0.74f, 0.55f) );
        public static readonly CustomColor LightSeaGreen = new CustomColor( nameof( LightSeaGreen ), new Color( 0.13f, 0.70f, 0.67f) );
        public static readonly CustomColor DarkCyan = new CustomColor( nameof( DarkCyan ), new Color( 0.00f, 0.55f, 0.55f) );
        public static readonly CustomColor Teal = new CustomColor( nameof( Teal ), new Color( 0.00f, 0.50f, 0.50f) );

        // Blues
        public static readonly CustomColor Aqua = new CustomColor( nameof( Aqua ), new Color( 0.00f, 1.00f, 1.00f ) );
        public static readonly CustomColor Cyan = new CustomColor( nameof( Cyan ), new Color( 0.00f, 1.00f, 1.00f ) );
        public static readonly CustomColor LightCyan = new CustomColor( nameof( LightCyan ), new Color( 0.88f, 1.00f, 1.00f ) );
        public static readonly CustomColor PaleTurquoise = new CustomColor( nameof( PaleTurquoise ), new Color( 0.69f, 0.93f, 0.93f ) );
        public static readonly CustomColor Aquamarine = new CustomColor( nameof( Aquamarine ), new Color( 0.50f, 1.00f, 0.83f ) );
        public static readonly CustomColor Turquoise = new CustomColor( nameof( Turquoise ), new Color( 0.25f, 0.88f, 0.82f ) );
        public static readonly CustomColor MediumTurquoise = new CustomColor( nameof( MediumTurquoise ), new Color( 0.28f, 0.82f, 0.80f ) );
        public static readonly CustomColor DarkTurquoise = new CustomColor( nameof( DarkTurquoise ), new Color( 0.00f, 0.81f, 0.82f ) );
        public static readonly CustomColor CadetBlue = new CustomColor( nameof( CadetBlue ), new Color( 0.37f, 0.62f, 0.63f ) );
        public static readonly CustomColor SteelBlue = new CustomColor( nameof( SteelBlue ), new Color( 0.27f, 0.51f, 0.71f ) );
        public static readonly CustomColor LightSteelBlue = new CustomColor( nameof( LightSteelBlue ), new Color( 0.69f, 0.77f, 0.87f ) );
        public static readonly CustomColor PowderBlue = new CustomColor( nameof( PowderBlue ), new Color( 0.69f, 0.88f, 0.90f ) );
        public static readonly CustomColor LightBlue = new CustomColor( nameof( LightBlue ), new Color( 0.68f, 0.85f, 0.90f ) );
        public static readonly CustomColor SkyBlue = new CustomColor( nameof( SkyBlue ), new Color( 0.53f, 0.81f, 0.92f ) );
        public static readonly CustomColor LightSkyBlue = new CustomColor( nameof( LightSkyBlue ), new Color( 0.53f, 0.81f, 0.98f ) );
        public static readonly CustomColor DeepSkyBlue = new CustomColor( nameof( DeepSkyBlue ), new Color( 0.00f, 0.75f, 1.00f ) );
        public static readonly CustomColor DodgerBlue = new CustomColor( nameof( DodgerBlue ), new Color( 0.12f, 0.56f, 1.00f ) );
        public static readonly CustomColor CornflowerBlue = new CustomColor( nameof( CornflowerBlue ), new Color( 0.39f, 0.58f, 0.93f ) );
        public static readonly CustomColor MediumSlateBlue = new CustomColor( nameof( MediumSlateBlue ), new Color( 0.48f, 0.41f, 0.93f ) );
        public static readonly CustomColor RoyalBlue = new CustomColor( nameof( RoyalBlue ), new Color( 0.25f, 0.41f, 0.88f ) );
        public static readonly CustomColor Blue = new CustomColor( nameof( Blue ), new Color( 0.00f, 0.00f, 1.00f ) );
        public static readonly CustomColor MediumBlue = new CustomColor( nameof( MediumBlue ), new Color( 0.00f, 0.00f, 0.80f ) );
        public static readonly CustomColor DarkBlue = new CustomColor( nameof( DarkBlue ), new Color( 0.00f, 0.00f, 0.55f ) );
        public static readonly CustomColor Navy = new CustomColor( nameof( Navy ), new Color( 0.00f, 0.00f, 0.50f ) );
        public static readonly CustomColor MidnightBlue = new CustomColor( nameof( MidnightBlue ), new Color( 0.10f, 0.10f, 0.44f ) );
        
        // Browns
        public static readonly CustomColor Cornsilk = new CustomColor( nameof( Cornsilk ), new Color( 1.00f, 0.97f, 0.86f) );
        public static readonly CustomColor BlanchedAlmond = new CustomColor( nameof( BlanchedAlmond ), new Color( 1.00f, 0.92f, 0.80f) );
        public static readonly CustomColor Bisque = new CustomColor( nameof( Bisque ), new Color( 1.00f, 0.89f, 0.77f) );
        public static readonly CustomColor NavajoWhite = new CustomColor( nameof( NavajoWhite ), new Color( 1.00f, 0.87f, 0.68f) );
        public static readonly CustomColor Wheat = new CustomColor( nameof( Wheat ), new Color( 0.96f, 0.87f, 0.70f) );
        public static readonly CustomColor BurlyWood = new CustomColor( nameof( BurlyWood ), new Color( 0.87f, 0.72f, 0.53f) );
        public static readonly CustomColor Tan = new CustomColor( nameof( Tan ), new Color( 0.82f, 0.71f, 0.55f) );
        public static readonly CustomColor RosyBrown = new CustomColor( nameof( RosyBrown ), new Color( 0.74f, 0.56f, 0.56f) );
        public static readonly CustomColor SandyBrown = new CustomColor( nameof( SandyBrown ), new Color( 0.96f, 0.64f, 0.38f) );
        public static readonly CustomColor Goldenrod = new CustomColor( nameof( Goldenrod ), new Color( 0.85f, 0.65f, 0.13f) );
        public static readonly CustomColor DarkGoldenrod = new CustomColor( nameof( DarkGoldenrod ), new Color( 0.72f, 0.53f, 0.04f) );
        public static readonly CustomColor Peru = new CustomColor( nameof( Peru ), new Color( 0.80f, 0.52f, 0.25f) );
        public static readonly CustomColor Chocolate = new CustomColor( nameof( Chocolate ), new Color( 0.82f, 0.41f, 0.12f) );
        public static readonly CustomColor SaddleBrown = new CustomColor( nameof( SaddleBrown ), new Color( 0.55f, 0.27f, 0.07f) );
        public static readonly CustomColor Sienna = new CustomColor( nameof( Sienna ), new Color( 0.63f, 0.32f, 0.18f) );
        public static readonly CustomColor Brown = new CustomColor( nameof( Brown ), new Color( 0.65f, 0.16f, 0.16f) );
        public static readonly CustomColor Maroon = new CustomColor( nameof( Maroon ), new Color( 0.50f, 0.00f, 0.00f) );
        
        /// <summary>
        /// Get all of the Custom Colors from the PresetColors class.
        /// </summary>
        public static List<CustomColor> GetAllColors()
        {
            IEnumerable<FieldInfo> fieldInfoArray = ( GetAllFields( typeof( PresetColors ) ) );
            return fieldInfoArray.Select( fieldInfo => ( (CustomColor) fieldInfo.GetValue( null ) ) ).ToList();
        }

        /// <summary>
        /// Collect all of the fields from the specified class. This approach dynamically loads all fields so adding more fields to the class won't require any change to this code.
        /// </summary>
        private static IEnumerable<FieldInfo> GetAllFields(Type type) {
            return type.GetNestedTypes().SelectMany(GetAllFields).Concat(type.GetFields());
        }

        private static Color GetConvertedColor( int r, int g, int b ) => new Color(
            GetPercentageOfColor( r ),
            GetPercentageOfColor( g ),
            GetPercentageOfColor( b )
        );

        // private static float GetPercentageOfColor( int colorChannelValue ) => Mathf.InverseLerp( 0, 255, colorChannelValue );
        private static float GetPercentageOfColor( int colorChannelValue ) => ( (float)colorChannelValue / 255 );
    }
}
