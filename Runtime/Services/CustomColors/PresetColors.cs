using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors
{
    public static class PresetColors
    {
        // public static CustomColor Blonde = new CustomColor( nameof( Blonde ), new Color( 0.94f, 0.89f, 0.71f ) );
        // public static CustomColor BabyYellow = new CustomColor( nameof( BabyYellow ), new Color( 1.00f, 0.99f, 0.79f ) );
        // public static CustomColor BananaYellow = new CustomColor( nameof( BananaYellow ), new Color( 1.00f, 0.87f, 0.00f ) );
        // public static CustomColor Butter = new CustomColor( nameof( Butter ), new Color( 0.96f, 0.92f, 0.38f ) );
        // public static CustomColor LemonYellow = new CustomColor( nameof( LemonYellow ), new Color( 1.00f, 0.95f, 0.43f ) );
        // public static CustomColor NeonYellow = new CustomColor( nameof( NeonYellow ), new Color( 0.88f, 0.91f, 0.13f ) );
        // public static CustomColor Brass = new CustomColor( nameof( Brass ), new Color( 0.67f, 0.62f, 0.24f ) );
        
        // Grey Tones.
        // public static CustomColor Black = new CustomColor( nameof( Black ), new Color(0, 0, 0 ) );
        public static CustomColor Grey = new CustomColor( nameof( Grey ), new Color( 0.5f, 0.5f, 0.5f ) );

        public static CustomColor LightGrey = new CustomColor( nameof( LightGrey ), new Color( 0.75f, 0.75f, 0.75f ) );
        // public static CustomColor White = new CustomColor( nameof( White ), new Color( 1, 1, 1 ) );

        // Grays
        public static CustomColor Gainsboro = new CustomColor( nameof( Gainsboro ), new Color( 0.86f, 0.86f, 0.86f ) );
        public static CustomColor LightGray = new CustomColor( nameof( LightGray ), new Color( 0.83f, 0.83f, 0.83f ) );
        public static CustomColor Silver = new CustomColor( nameof( Silver ), new Color( 0.75f, 0.75f, 0.75f ) );
        public static CustomColor DarkGray = new CustomColor( nameof( DarkGray ), new Color( 0.66f, 0.66f, 0.66f ) );
        public static CustomColor Gray = new CustomColor( nameof( Gray ), new Color( 0.50f, 0.50f, 0.50f ) );
        public static CustomColor DimGray = new CustomColor( nameof( DimGray ), new Color( 0.41f, 0.41f, 0.41f ) );
        public static CustomColor LightSlateGray = new CustomColor( nameof( LightSlateGray ), new Color( 0.47f, 0.53f, 0.60f ) );
        public static CustomColor SlateGray = new CustomColor( nameof( SlateGray ), new Color( 0.44f, 0.50f, 0.56f ) );
        public static CustomColor DarkSlateGray = new CustomColor( nameof( DarkSlateGray ), new Color( 1.00f, 1.00f, 1.00f ) );
        public static CustomColor Black = new CustomColor( nameof( Black ), new Color( 0.00f, 0.00f, 0.00f ) );

        // Whites
        public static CustomColor White = new CustomColor( nameof( White ), new Color( 1.00f, 1.00f, 1.00f ) );
        public static CustomColor Snow = new CustomColor( nameof( Snow ), new Color( 1.00f, 0.98f, 0.98f ) );
        public static CustomColor HoneyDew = new CustomColor( nameof( HoneyDew ), new Color( 0.94f, 1.00f, 0.94f ) );
        public static CustomColor MintCream = new CustomColor( nameof( MintCream ), new Color( 0.96f, 1.00f, 0.98f ) );
        public static CustomColor Azure = new CustomColor( nameof( Azure ), new Color( 0.94f, 1.00f, 1.00f ) );
        public static CustomColor AliceBlue = new CustomColor( nameof( AliceBlue ), new Color( 0.94f, 0.97f, 1.00f ) );
        public static CustomColor GhostWhite = new CustomColor( nameof( GhostWhite ), new Color( 0.97f, 0.97f, 1.00f ) );
        public static CustomColor WhiteSmoke = new CustomColor( nameof( WhiteSmoke ), new Color( 0.96f, 0.96f, 0.96f ) );
        public static CustomColor SeaShell = new CustomColor( nameof( SeaShell ), new Color( 1.00f, 0.96f, 0.93f ) );
        public static CustomColor Beige = new CustomColor( nameof( Beige ), new Color( 0.96f, 0.96f, 0.86f ) );
        public static CustomColor OldLace = new CustomColor( nameof( OldLace ), new Color( 0.99f, 0.96f, 0.90f ) );
        public static CustomColor FloralWhite = new CustomColor( nameof( FloralWhite ), new Color( 1.00f, 0.98f, 0.94f ) );
        public static CustomColor Ivory = new CustomColor( nameof( Ivory ), new Color( 1.00f, 1.00f, 0.94f ) );
        public static CustomColor AntiqueWhite = new CustomColor( nameof( AntiqueWhite ), new Color( 0.98f, 0.92f, 0.84f ) );
        public static CustomColor Linen = new CustomColor( nameof( Linen ), new Color( 0.98f, 0.94f, 0.90f ) );
        public static CustomColor LavenderBlush = new CustomColor( nameof( LavenderBlush ), new Color( 1.00f, 0.94f, 0.96f ) );
        public static CustomColor MistyRose = new CustomColor( nameof( MistyRose ), new Color( 1.00f, 0.89f, 0.88f ) );


        // Reds
        public static CustomColor IndianRed = new CustomColor( nameof( IndianRed ), new Color( 0.80f, 1.00f, 1.00f ) );
        public static CustomColor LightCoral = new CustomColor( nameof( LightCoral ), new Color( 0.94f, 0.50f, 0.50f ) );
        public static CustomColor Salmon = new CustomColor( nameof( Salmon ), new Color( 0.98f, 0.50f, 0.45f ) );
        public static CustomColor DarkSalmon = new CustomColor( nameof( DarkSalmon ), new Color( 0.91f, 0.59f, 0.48f ) );
        public static CustomColor LightSalmon = new CustomColor( nameof( LightSalmon ), new Color( 1.00f, 0.63f, 0.48f ) );
        public static CustomColor Crimson = new CustomColor( nameof( Crimson ), new Color( 0.86f, 0.78f, 1.00f ) );
        public static CustomColor Red = new CustomColor( nameof( Red ), new Color( 1.00f, 0.00f, 0.00f ) );
        public static CustomColor FireBrick = new CustomColor( nameof( FireBrick ), new Color( 0.70f, 1.00f, 1.00f ) );
        public static CustomColor DarkRed = new CustomColor( nameof( DarkRed ), new Color( 0.55f, 0.00f, 0.00f ) );

        // Pinks
        public static CustomColor Pink = new CustomColor( nameof( Pink ), new Color( 1.00f, 0.75f, 0.80f ) );
        public static CustomColor LightPink = new CustomColor( nameof( LightPink ), new Color( 1.00f, 0.71f, 0.76f ) );
        public static CustomColor HotPink = new CustomColor( nameof( HotPink ), new Color( 1.00f, 0.41f, 0.71f ) );
        public static CustomColor DeepPink = new CustomColor( nameof( DeepPink ), new Color( 1.00f, 0.78f, 0.58f ) );
        public static CustomColor MediumVioletRed = new CustomColor( nameof( MediumVioletRed ), new Color( 0.78f, 0.82f, 0.52f ) );
        public static CustomColor PaleVioletRed = new CustomColor( nameof( PaleVioletRed ), new Color( 0.86f, 0.44f, 0.58f ) );

        // Oranges
        public static CustomColor Coral = new CustomColor( nameof( Coral ), new Color( 1.00f, 0.50f, 1.00f ) );
        public static CustomColor Tomato = new CustomColor( nameof( Tomato ), new Color( 1.00f, 1.00f, 1.00f ) );
        public static CustomColor OrangeRed = new CustomColor( nameof( OrangeRed ), new Color( 1.00f, 1.00f, 0.00f ) );
        public static CustomColor DarkOrange = new CustomColor( nameof( DarkOrange ), new Color( 1.00f, 0.55f, 0.00f ) );
        public static CustomColor Orange = new CustomColor( nameof( Orange ), new Color( 1.00f, 0.65f, 0.00f ) );


        // Yellows
        public static CustomColor Gold = new CustomColor( nameof( Gold ), new Color( 1.00f, 0.84f, 0.00f ) );
        public static CustomColor Yellow = new CustomColor( nameof( Yellow ), new Color( 1.00f, 1.00f, 0.00f ) );
        public static CustomColor LightYellow = new CustomColor( nameof( LightYellow ), new Color( 1.00f, 1.00f, 0.88f ) );
        public static CustomColor LemonChiffon = new CustomColor( nameof( LemonChiffon ), new Color( 1.00f, 0.98f, 0.80f ) );
        public static CustomColor LightGoldenrodYellow = new CustomColor( nameof( LightGoldenrodYellow ), new Color( 0.98f, 0.98f, 0.82f ) );
        public static CustomColor PapayaWhip = new CustomColor( nameof( PapayaWhip ), new Color( 1.00f, 0.94f, 0.84f ) );
        public static CustomColor Moccasin = new CustomColor( nameof( Moccasin ), new Color( 1.00f, 0.89f, 0.71f ) );
        public static CustomColor PeachPuff = new CustomColor( nameof( PeachPuff ), new Color( 1.00f, 0.85f, 0.73f ) );
        public static CustomColor PaleGoldenrod = new CustomColor( nameof( PaleGoldenrod ), new Color( 0.93f, 0.91f, 0.67f ) );
        public static CustomColor Khaki = new CustomColor( nameof( Khaki ), new Color( 0.94f, 0.90f, 0.55f ) );
        public static CustomColor DarkKhaki = new CustomColor( nameof( DarkKhaki ), new Color( 0.74f, 0.72f, 0.42f ) );

        // Purples
        public static CustomColor Lavender = new CustomColor( nameof( Lavender ), new Color( 0.90f, 0.90f, 0.98f ) );
        public static CustomColor Thistle = new CustomColor( nameof( Thistle ), new Color( 0.85f, 0.75f, 0.85f ) );
        public static CustomColor Plum = new CustomColor( nameof( Plum ), new Color( 0.87f, 0.63f, 0.87f ) );
        public static CustomColor Violet = new CustomColor( nameof( Violet ), new Color( 0.93f, 0.51f, 0.93f ) );
        public static CustomColor Orchid = new CustomColor( nameof( Orchid ), new Color( 0.85f, 0.44f, 0.84f ) );
        public static CustomColor Fuchsia = new CustomColor( nameof( Fuchsia ), new Color( 1.00f, 0.00f, 1.00f ) );
        public static CustomColor Magenta = new CustomColor( nameof( Magenta ), new Color( 1.00f, 0.00f, 1.00f ) );
        public static CustomColor MediumOrchid = new CustomColor( nameof( MediumOrchid ), new Color( 0.73f, 1.00f, 0.83f ) );
        public static CustomColor MediumPurple = new CustomColor( nameof( MediumPurple ), new Color( 0.58f, 0.44f, 0.86f ) );
        public static CustomColor RebeccaPurple = new CustomColor( nameof( RebeccaPurple ), new Color( 0.40f, 1.00f, 0.60f ) );
        public static CustomColor BlueViolet = new CustomColor( nameof( BlueViolet ), new Color( 0.54f, 1.00f, 0.89f ) );
        public static CustomColor DarkViolet = new CustomColor( nameof( DarkViolet ), new Color( 0.58f, 0.00f, 0.83f ) );
        public static CustomColor DarkOrchid = new CustomColor( nameof( DarkOrchid ), new Color( 0.60f, 1.00f, 0.80f ) );
        public static CustomColor DarkMagenta = new CustomColor( nameof( DarkMagenta ), new Color( 0.55f, 0.00f, 0.55f ) );
        public static CustomColor Purple = new CustomColor( nameof( Purple ), new Color( 0.50f, 0.00f, 0.50f ) );
        public static CustomColor Indigo = new CustomColor( nameof( Indigo ), new Color( 1.00f, 0.00f, 0.51f ) );
        public static CustomColor SlateBlue = new CustomColor( nameof( SlateBlue ), new Color( 0.42f, 1.00f, 0.80f ) );
        public static CustomColor DarkSlateBlue = new CustomColor( nameof( DarkSlateBlue ), new Color( 1.00f, 1.00f, 0.55f ) );

        // Greens
        public static CustomColor GreenYellow = new CustomColor( nameof( GreenYellow ), new Color( 0.68f, 1.00f, 1.00f ) );
        public static CustomColor Chartreuse = new CustomColor( nameof( Chartreuse ), new Color( 0.50f, 1.00f, 0.00f ) );
        public static CustomColor LawnGreen = new CustomColor( nameof( LawnGreen ), new Color( 0.49f, 0.99f, 0.00f ) );
        public static CustomColor Lime = new CustomColor( nameof( Lime ), new Color( 0.00f, 1.00f, 0.00f ) );
        public static CustomColor LimeGreen = new CustomColor( nameof( LimeGreen ), new Color( 1.00f, 0.80f, 1.00f ) );
        public static CustomColor PaleGreen = new CustomColor( nameof( PaleGreen ), new Color( 0.60f, 0.98f, 0.60f ) );
        public static CustomColor LightGreen = new CustomColor( nameof( LightGreen ), new Color( 0.56f, 0.93f, 0.56f ) );
        public static CustomColor MediumSpringGreen = new CustomColor( nameof( MediumSpringGreen ), new Color( 0.00f, 0.98f, 0.60f ) );
        public static CustomColor SpringGreen = new CustomColor( nameof( SpringGreen ), new Color( 0.00f, 1.00f, 0.50f ) );
        public static CustomColor MediumSeaGreen = new CustomColor( nameof( MediumSeaGreen ), new Color( 1.00f, 0.70f, 0.44f ) );
        public static CustomColor SeaGreen = new CustomColor( nameof( SeaGreen ), new Color( 1.00f, 0.55f, 1.00f ) );
        public static CustomColor ForestGreen = new CustomColor( nameof( ForestGreen ), new Color( 1.00f, 0.55f, 1.00f ) );
        public static CustomColor Green = new CustomColor( nameof( Green ), new Color( 0.00f, 0.50f, 0.00f ) );
        public static CustomColor DarkGreen = new CustomColor( nameof( DarkGreen ), new Color( 0.00f, 0.39f, 0.00f ) );
        public static CustomColor YellowGreen = new CustomColor( nameof( YellowGreen ), new Color( 0.60f, 0.80f, 1.00f ) );
        public static CustomColor OliveDrab = new CustomColor( nameof( OliveDrab ), new Color( 0.42f, 0.56f, 1.00f ) );
        public static CustomColor Olive = new CustomColor( nameof( Olive ), new Color( 0.50f, 0.50f, 0.00f ) );
        public static CustomColor DarkOliveGreen = new CustomColor( nameof( DarkOliveGreen ), new Color( 1.00f, 0.42f, 1.00f ) );
        public static CustomColor MediumAquamarine = new CustomColor( nameof( MediumAquamarine ), new Color( 0.40f, 0.80f, 0.67f ) );
        public static CustomColor DarkSeaGreen = new CustomColor( nameof( DarkSeaGreen ), new Color( 0.56f, 0.74f, 0.55f ) );
        public static CustomColor LightSeaGreen = new CustomColor( nameof( LightSeaGreen ), new Color( 1.00f, 0.70f, 0.67f ) );
        public static CustomColor DarkCyan = new CustomColor( nameof( DarkCyan ), new Color( 0.00f, 0.55f, 0.55f ) );
        public static CustomColor Teal = new CustomColor( nameof( Teal ), new Color( 0.00f, 0.50f, 0.50f ) );


        // Blues
        public static CustomColor Aqua = new CustomColor( nameof( Aqua ), new Color( 0.00f, 1.00f, 1.00f ) );
        public static CustomColor Cyan = new CustomColor( nameof( Cyan ), new Color( 0.00f, 1.00f, 1.00f ) );
        public static CustomColor LightCyan = new CustomColor( nameof( LightCyan ), new Color( 0.88f, 1.00f, 1.00f ) );
        public static CustomColor PaleTurquoise = new CustomColor( nameof( PaleTurquoise ), new Color( 0.69f, 0.93f, 0.93f ) );
        public static CustomColor Aquamarine = new CustomColor( nameof( Aquamarine ), new Color( 0.50f, 1.00f, 0.83f ) );
        public static CustomColor Turquoise = new CustomColor( nameof( Turquoise ), new Color( 1.00f, 0.88f, 0.82f ) );
        public static CustomColor MediumTurquoise = new CustomColor( nameof( MediumTurquoise ), new Color( 1.00f, 0.82f, 0.80f ) );
        public static CustomColor DarkTurquoise = new CustomColor( nameof( DarkTurquoise ), new Color( 0.00f, 0.81f, 0.82f ) );
        public static CustomColor CadetBlue = new CustomColor( nameof( CadetBlue ), new Color( 1.00f, 0.62f, 0.63f ) );
        public static CustomColor SteelBlue = new CustomColor( nameof( SteelBlue ), new Color( 1.00f, 0.51f, 0.71f ) );
        public static CustomColor LightSteelBlue = new CustomColor( nameof( LightSteelBlue ), new Color( 0.69f, 0.77f, 0.87f ) );
        public static CustomColor PowderBlue = new CustomColor( nameof( PowderBlue ), new Color( 0.69f, 0.88f, 0.90f ) );
        public static CustomColor LightBlue = new CustomColor( nameof( LightBlue ), new Color( 0.68f, 0.85f, 0.90f ) );
        public static CustomColor SkyBlue = new CustomColor( nameof( SkyBlue ), new Color( 0.53f, 0.81f, 0.92f ) );
        public static CustomColor LightSkyBlue = new CustomColor( nameof( LightSkyBlue ), new Color( 0.53f, 0.81f, 0.98f ) );
        public static CustomColor DeepSkyBlue = new CustomColor( nameof( DeepSkyBlue ), new Color( 0.00f, 0.75f, 1.00f ) );
        public static CustomColor DodgerBlue = new CustomColor( nameof( DodgerBlue ), new Color( 1.00f, 0.56f, 1.00f ) );
        public static CustomColor CornflowerBlue = new CustomColor( nameof( CornflowerBlue ), new Color( 0.39f, 0.58f, 0.93f ) );
        public static CustomColor MediumSlateBlue = new CustomColor( nameof( MediumSlateBlue ), new Color( 0.48f, 0.41f, 0.93f ) );
        public static CustomColor RoyalBlue = new CustomColor( nameof( RoyalBlue ), new Color( 1.00f, 0.41f, 0.88f ) );
        public static CustomColor Blue = new CustomColor( nameof( Blue ), new Color( 0.00f, 0.00f, 1.00f ) );
        public static CustomColor MediumBlue = new CustomColor( nameof( MediumBlue ), new Color( 0.00f, 0.00f, 0.80f ) );
        public static CustomColor DarkBlue = new CustomColor( nameof( DarkBlue ), new Color( 0.00f, 0.00f, 0.55f ) );
        public static CustomColor Navy = new CustomColor( nameof( Navy ), new Color( 0.00f, 0.00f, 0.50f ) );
        public static CustomColor MidnightBlue = new CustomColor( nameof( MidnightBlue ), new Color( 0.98f, 0.98f, 0.44f ) );

        // Browns
        public static CustomColor Cornsilk = new CustomColor( nameof( Cornsilk ), new Color( 1.00f, 0.97f, 0.86f ) );
        public static CustomColor BlanchedAlmond = new CustomColor( nameof( BlanchedAlmond ), new Color( 1.00f, 0.92f, 0.80f ) );
        public static CustomColor Bisque = new CustomColor( nameof( Bisque ), new Color( 1.00f, 0.89f, 0.77f ) );
        public static CustomColor NavajoWhite = new CustomColor( nameof( NavajoWhite ), new Color( 1.00f, 0.87f, 0.68f ) );
        public static CustomColor Wheat = new CustomColor( nameof( Wheat ), new Color( 0.96f, 0.87f, 0.70f ) );
        public static CustomColor BurlyWood = new CustomColor( nameof( BurlyWood ), new Color( 0.87f, 0.72f, 0.53f ) );
        public static CustomColor Tan = new CustomColor( nameof( Tan ), new Color( 0.82f, 0.71f, 0.55f ) );
        public static CustomColor RosyBrown = new CustomColor( nameof( RosyBrown ), new Color( 0.74f, 0.56f, 0.56f ) );
        public static CustomColor SandyBrown = new CustomColor( nameof( SandyBrown ), new Color( 0.96f, 0.64f, 1.00f ) );
        public static CustomColor Goldenrod = new CustomColor( nameof( Goldenrod ), new Color( 0.85f, 0.65f, 1.00f ) );
        public static CustomColor DarkGoldenrod = new CustomColor( nameof( DarkGoldenrod ), new Color( 0.72f, 0.53f, 0.43f ) );
        public static CustomColor Peru = new CustomColor( nameof( Peru ), new Color( 0.80f, 0.52f, 1.00f ) );
        public static CustomColor Chocolate = new CustomColor( nameof( Chocolate ), new Color( 0.82f, 0.41f, 1.00f ) );
        public static CustomColor SaddleBrown = new CustomColor( nameof( SaddleBrown ), new Color( 0.55f, 1.00f, 0.75f ) );
        public static CustomColor Sienna = new CustomColor( nameof( Sienna ), new Color( 0.63f, 1.00f, 1.00f ) );
        public static CustomColor Brown = new CustomColor( nameof( Brown ), new Color( 0.65f, 1.00f, 1.00f ) );
        public static CustomColor Maroon = new CustomColor( nameof( Maroon ), new Color( 0.50f, 0.00f, 0.00f ) );
        
        
    
        
        
        // public static CustomColor Red = new CustomColor( nameof( Red ), new Color(.9f, .3f, .3f ) );
        // public static CustomColor Green = new CustomColor( nameof( Green ), new Color(.3f, .8f, .3f ) );
        // public static CustomColor Lime = new CustomColor( nameof( Lime ), new Color(.63f, .77f, .2f ) );
        // public static CustomColor Blue = new CustomColor( nameof( Blue ), new Color(.3f, .6f, .9f ) );
        // public static CustomColor Teal = new CustomColor( nameof( Teal ), new Color(.21f, .85f, .76f ) );
        // public static CustomColor Yellow = new CustomColor( nameof( Yellow ), new Color(.9f, .8f, .4f ) );
        // public static CustomColor Orange = new CustomColor( nameof( Orange ), new Color(.9f, .5f, .2f ) );
        // public static CustomColor Violet = new CustomColor( nameof( Violet ), new Color(.6f, .15f, .7f ) );
        // public static CustomColor Purple = new CustomColor( nameof( Purple ), new Color(.5f, .2f, .65f ) );
        
        

        public static List<CustomColor> GetAllColors()
        {
            IEnumerable<FieldInfo> fieldInfoArray = ( GetAllFields( typeof( PresetColors ) ) );
            return fieldInfoArray.Select( fieldInfo => ( (CustomColor) fieldInfo.GetValue( null ) ) ).ToList();
        }
        
        public static IEnumerable<FieldInfo> GetAllFields(Type type) {
            return type.GetNestedTypes().SelectMany(GetAllFields)
                .Concat(type.GetFields());
        }

        private static Color GetConvertedColor( int r, int g, int b ) => new Color(
            GetPercentageOfColor( r ),
            GetPercentageOfColor( g ),
            GetPercentageOfColor( b )
        );

        private static float GetPercentageOfColor( int colorChannelValue ) => Mathf.InverseLerp( 0, 255, colorChannelValue );
    }
}
