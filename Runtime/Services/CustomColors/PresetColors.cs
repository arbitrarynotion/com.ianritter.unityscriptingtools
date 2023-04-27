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
        public static CustomColor Grey = new CustomColor( nameof( Grey ), new Color(0.5f, 0.5f, 0.5f ) );
        public static CustomColor LightGrey = new CustomColor( nameof( LightGrey ), new Color(0.75f, 0.75f, 0.75f ) );
        // public static CustomColor White = new CustomColor( nameof( White ), new Color(1, 1, 1 ) );
        
        // Grays
        public static CustomColor Gainsboro = new CustomColor( nameof( Gainsboro ), GetConvertedColor( 220, 220, 220) );
        public static CustomColor LightGray = new CustomColor( nameof( LightGray ), GetConvertedColor( 211, 211, 211) );
        public static CustomColor Silver = new CustomColor( nameof( Silver ), GetConvertedColor( 192, 192, 192) );
        public static CustomColor DarkGray = new CustomColor( nameof( DarkGray ), GetConvertedColor( 169, 169, 169) );
        public static CustomColor Gray = new CustomColor( nameof( Gray ), GetConvertedColor( 128, 128, 128) );
        public static CustomColor DimGray = new CustomColor( nameof( DimGray ), GetConvertedColor( 105, 105, 105) );
        public static CustomColor LightSlateGray = new CustomColor( nameof( LightSlateGray ), GetConvertedColor( 119, 136, 153) );
        public static CustomColor SlateGray = new CustomColor( nameof( SlateGray ), GetConvertedColor( 112, 128, 144) );
        public static CustomColor DarkSlateGray = new CustomColor( nameof( DarkSlateGray ), GetConvertedColor( 47, 79, 79) );
        public static CustomColor Black = new CustomColor( nameof( Black ), GetConvertedColor( 0, 0, 0) );
        
        // Whites
        public static CustomColor White = new CustomColor( nameof( White ), GetConvertedColor( 255, 255, 255) );
        public static CustomColor Snow = new CustomColor( nameof( Snow ), GetConvertedColor( 255, 250, 250) );
        public static CustomColor HoneyDew = new CustomColor( nameof( HoneyDew ), GetConvertedColor( 240, 255, 240) );
        public static CustomColor MintCream = new CustomColor( nameof( MintCream ), GetConvertedColor( 245, 255, 250) );
        public static CustomColor Azure = new CustomColor( nameof( Azure ), GetConvertedColor( 240, 255, 255) );
        public static CustomColor AliceBlue = new CustomColor( nameof( AliceBlue ), GetConvertedColor( 240, 248, 255) );
        public static CustomColor GhostWhite = new CustomColor( nameof( GhostWhite ), GetConvertedColor( 248, 248, 255) );
        public static CustomColor WhiteSmoke = new CustomColor( nameof( WhiteSmoke ), GetConvertedColor( 245, 245, 245) );
        public static CustomColor SeaShell = new CustomColor( nameof( SeaShell ), GetConvertedColor( 255, 245, 238) );
        public static CustomColor Beige = new CustomColor( nameof( Beige ), GetConvertedColor( 245, 245, 220) );
        public static CustomColor OldLace = new CustomColor( nameof( OldLace ), GetConvertedColor( 253, 245, 230) );
        public static CustomColor FloralWhite = new CustomColor( nameof( FloralWhite ), GetConvertedColor( 255, 250, 240) );
        public static CustomColor Ivory = new CustomColor( nameof( Ivory ), GetConvertedColor( 255, 255, 240) );
        public static CustomColor AntiqueWhite = new CustomColor( nameof( AntiqueWhite ), GetConvertedColor( 250, 235, 215) );
        public static CustomColor Linen = new CustomColor( nameof( Linen ), GetConvertedColor( 250, 240, 230) );
        public static CustomColor LavenderBlush = new CustomColor( nameof( LavenderBlush ), GetConvertedColor( 255, 240, 245) );
        public static CustomColor MistyRose = new CustomColor( nameof( MistyRose ), GetConvertedColor( 255, 228, 225) );
        
        
        
        
        // Reds
        public static CustomColor IndianRed = new CustomColor( nameof( IndianRed ), GetConvertedColor( 205, 92, 92 ) );
        public static CustomColor LightCoral = new CustomColor( nameof( LightCoral ), GetConvertedColor( 240, 128, 128 ) );
        public static CustomColor Salmon = new CustomColor( nameof( Salmon ), GetConvertedColor( 250, 128, 114 ) );
        public static CustomColor DarkSalmon = new CustomColor( nameof( DarkSalmon ), GetConvertedColor( 233, 150, 122 ) );
        public static CustomColor LightSalmon = new CustomColor( nameof( LightSalmon ), GetConvertedColor( 255, 160, 122 ) );
        public static CustomColor Crimson = new CustomColor( nameof( Crimson ), GetConvertedColor( 220, 20, 60 ) );
        public static CustomColor Red = new CustomColor( nameof( Red ), GetConvertedColor( 255, 0, 0 ) );
        public static CustomColor FireBrick = new CustomColor( nameof( FireBrick ), GetConvertedColor( 178, 34, 34 ) );
        public static CustomColor DarkRed = new CustomColor( nameof( DarkRed ), GetConvertedColor( 139, 0, 0 ) );

        // Pinks
        public static CustomColor Pink = new CustomColor( nameof( Pink ), GetConvertedColor( 255, 192, 203 ) );
        public static CustomColor LightPink = new CustomColor( nameof( LightPink ), GetConvertedColor( 255, 182, 193 ) );
        public static CustomColor HotPink = new CustomColor( nameof( HotPink ), GetConvertedColor( 255, 105, 180 ) );
        public static CustomColor DeepPink = new CustomColor( nameof( DeepPink ), GetConvertedColor( 255, 20, 147 ) );
        public static CustomColor MediumVioletRed = new CustomColor( nameof( MediumVioletRed ), GetConvertedColor( 199, 21, 133 ) );
        public static CustomColor PaleVioletRed = new CustomColor( nameof( PaleVioletRed ), GetConvertedColor( 219, 112, 147 ) );

        // Oranges
        // public static CustomColor LightSalmon = new CustomColor( nameof( LightSalmon ), GetConvertedColor( 255, 160, 122 ) );
        public static CustomColor Coral = new CustomColor( nameof( Coral ), GetConvertedColor( 255, 127, 80 ) );
        public static CustomColor Tomato = new CustomColor( nameof( Tomato ), GetConvertedColor( 255, 99, 71 ) );
        public static CustomColor OrangeRed = new CustomColor( nameof( OrangeRed ), GetConvertedColor( 255, 69, 0 ) );
        public static CustomColor DarkOrange = new CustomColor( nameof( DarkOrange ), GetConvertedColor( 255, 140, 0 ) );
        public static CustomColor Orange = new CustomColor( nameof( Orange ), GetConvertedColor( 255, 165, 0 ) );
        
        // public static CustomColor Blonde = new CustomColor( nameof( Blonde ), new Color( 0.94f, 0.89f, 0.71f ) );
        // public static CustomColor BabyYellow = new CustomColor( nameof( BabyYellow ), new Color( 1.00f, 0.99f, 0.79f ) );
        // public static CustomColor BananaYellow = new CustomColor( nameof( BananaYellow ), new Color( 1.00f, 0.87f, 0.00f ) );
        // public static CustomColor Butter = new CustomColor( nameof( Butter ), new Color( 0.96f, 0.92f, 0.38f ) );
        // public static CustomColor LemonYellow = new CustomColor( nameof( LemonYellow ), new Color( 1.00f, 0.95f, 0.43f ) );
        // public static CustomColor NeonYellow = new CustomColor( nameof( NeonYellow ), new Color( 0.88f, 0.91f, 0.13f ) );
        // public static CustomColor Brass = new CustomColor( nameof( Brass ), new Color( 0.67f, 0.62f, 0.24f ) );
        
        
        // Yellows
        public static CustomColor Gold = new CustomColor( nameof( Gold ), GetConvertedColor( 255, 215, 0 ) );
        public static CustomColor Yellow = new CustomColor( nameof( Yellow ), GetConvertedColor( 255, 255, 0 ) );
        public static CustomColor LightYellow = new CustomColor( nameof( LightYellow ), GetConvertedColor( 255, 255, 224 ) );
        public static CustomColor LemonChiffon = new CustomColor( nameof( LemonChiffon ), GetConvertedColor( 255, 250, 205 ) );
        public static CustomColor LightGoldenrodYellow = new CustomColor( nameof( LightGoldenrodYellow ), GetConvertedColor( 250, 250, 210 ) );
        public static CustomColor PapayaWhip = new CustomColor( nameof( PapayaWhip ), GetConvertedColor( 255, 239, 213 ) );
        public static CustomColor Moccasin = new CustomColor( nameof( Moccasin ), GetConvertedColor( 255, 228, 181 ) );
        public static CustomColor PeachPuff = new CustomColor( nameof( PeachPuff ), GetConvertedColor( 255, 218, 185 ) );
        public static CustomColor PaleGoldenrod = new CustomColor( nameof( PaleGoldenrod ), GetConvertedColor( 238, 232, 170 ) );
        public static CustomColor Khaki = new CustomColor( nameof( Khaki ), GetConvertedColor( 240, 230, 140 ) );
        public static CustomColor DarkKhaki = new CustomColor( nameof( DarkKhaki ), GetConvertedColor( 189, 183, 107 ) );
        
        // Purples
        public static CustomColor Lavender = new CustomColor( nameof( Lavender ), GetConvertedColor(230, 230, 250) );
        public static CustomColor Thistle = new CustomColor( nameof( Thistle ), GetConvertedColor(216, 191, 216) );
        public static CustomColor Plum = new CustomColor( nameof( Plum ), GetConvertedColor(221, 160, 221) );
        public static CustomColor Violet = new CustomColor( nameof( Violet ), GetConvertedColor(238, 130, 238) );
        public static CustomColor Orchid = new CustomColor( nameof( Orchid ), GetConvertedColor(218, 112, 214) );
        public static CustomColor Fuchsia = new CustomColor( nameof( Fuchsia ), GetConvertedColor(255, 0, 255) );
        public static CustomColor Magenta = new CustomColor( nameof( Magenta ), GetConvertedColor(255, 0, 255) );
        public static CustomColor MediumOrchid = new CustomColor( nameof( MediumOrchid ), GetConvertedColor(186, 85, 211) );
        public static CustomColor MediumPurple = new CustomColor( nameof( MediumPurple ), GetConvertedColor(147, 112, 219) );
        public static CustomColor RebeccaPurple = new CustomColor( nameof( RebeccaPurple ), GetConvertedColor(102, 51, 153) );
        public static CustomColor BlueViolet = new CustomColor( nameof( BlueViolet ), GetConvertedColor(138, 43, 226) );
        public static CustomColor DarkViolet = new CustomColor( nameof( DarkViolet ), GetConvertedColor(148, 0, 211) );
        public static CustomColor DarkOrchid = new CustomColor( nameof( DarkOrchid ), GetConvertedColor(153, 50, 204) );
        public static CustomColor DarkMagenta = new CustomColor( nameof( DarkMagenta ), GetConvertedColor(139, 0, 139) );
        public static CustomColor Purple = new CustomColor( nameof( Purple ), GetConvertedColor(128, 0, 128) );
        public static CustomColor Indigo = new CustomColor( nameof( Indigo ), GetConvertedColor(75, 0, 130) );
        public static CustomColor SlateBlue = new CustomColor( nameof( SlateBlue ), GetConvertedColor(106, 90, 205) );
        public static CustomColor DarkSlateBlue = new CustomColor( nameof( DarkSlateBlue ), GetConvertedColor(72, 61, 139) );
        // public static CustomColor MediumSlateBlue = new CustomColor( nameof( MediumSlateBlue ), GetConvertedColor(123, 104, 238) );
        
        // Greens
        public static CustomColor GreenYellow = new CustomColor( nameof( GreenYellow ), GetConvertedColor( 173, 255, 47 ) );
        public static CustomColor Chartreuse = new CustomColor( nameof( Chartreuse ), GetConvertedColor( 127, 255, 0 ) );
        public static CustomColor LawnGreen = new CustomColor( nameof( LawnGreen ), GetConvertedColor( 124, 252, 0 ) );
        public static CustomColor Lime = new CustomColor( nameof( Lime ), GetConvertedColor( 0, 255, 0 ) );
        public static CustomColor LimeGreen = new CustomColor( nameof( LimeGreen ), GetConvertedColor( 50, 205, 50 ) );
        public static CustomColor PaleGreen = new CustomColor( nameof( PaleGreen ), GetConvertedColor( 152, 251, 152 ) );
        public static CustomColor LightGreen = new CustomColor( nameof( LightGreen ), GetConvertedColor( 144, 238, 144 ) );
        public static CustomColor MediumSpringGreen = new CustomColor( nameof( MediumSpringGreen ), GetConvertedColor( 0, 250, 154 ) );
        public static CustomColor SpringGreen = new CustomColor( nameof( SpringGreen ), GetConvertedColor( 0, 255, 127 ) );
        public static CustomColor MediumSeaGreen = new CustomColor( nameof( MediumSeaGreen ), GetConvertedColor( 60, 179, 113 ) );
        public static CustomColor SeaGreen = new CustomColor( nameof( SeaGreen ), GetConvertedColor( 46, 139, 87 ) );
        public static CustomColor ForestGreen = new CustomColor( nameof( ForestGreen ), GetConvertedColor( 34, 139, 34 ) );
        public static CustomColor Green = new CustomColor( nameof( Green ), GetConvertedColor( 0, 128, 0 ) );
        public static CustomColor DarkGreen = new CustomColor( nameof( DarkGreen ), GetConvertedColor( 0, 100, 0 ) );
        public static CustomColor YellowGreen = new CustomColor( nameof( YellowGreen ), GetConvertedColor( 154, 205, 50 ) );
        public static CustomColor OliveDrab = new CustomColor( nameof( OliveDrab ), GetConvertedColor( 107, 142, 35 ) );
        public static CustomColor Olive = new CustomColor( nameof( Olive ), GetConvertedColor( 128, 128, 0 ) );
        public static CustomColor DarkOliveGreen = new CustomColor( nameof( DarkOliveGreen ), GetConvertedColor( 85, 107, 47 ) );
        public static CustomColor MediumAquamarine = new CustomColor( nameof( MediumAquamarine ), GetConvertedColor( 102, 205, 170 ) );
        public static CustomColor DarkSeaGreen = new CustomColor( nameof( DarkSeaGreen ), GetConvertedColor( 143, 188, 139 ) );
        public static CustomColor LightSeaGreen = new CustomColor( nameof( LightSeaGreen ), GetConvertedColor( 32, 178, 170 ) );
        public static CustomColor DarkCyan = new CustomColor( nameof( DarkCyan ), GetConvertedColor( 0, 139, 139 ) );
        public static CustomColor Teal = new CustomColor( nameof( Teal ), GetConvertedColor( 0, 128, 128 ) );
        
        
        // Blues
        public static CustomColor Aqua = new CustomColor( nameof( Aqua ), GetConvertedColor( 0, 255, 255) );
        public static CustomColor Cyan = new CustomColor( nameof( Cyan ), GetConvertedColor( 0, 255, 255) );
        public static CustomColor LightCyan = new CustomColor( nameof( LightCyan ), GetConvertedColor( 224, 255, 255) );
        public static CustomColor PaleTurquoise = new CustomColor( nameof( PaleTurquoise ), GetConvertedColor( 175, 238, 238) );
        public static CustomColor Aquamarine = new CustomColor( nameof( Aquamarine ), GetConvertedColor( 127, 255, 212) );
        public static CustomColor Turquoise = new CustomColor( nameof( Turquoise ), GetConvertedColor( 64, 224, 208) );
        public static CustomColor MediumTurquoise = new CustomColor( nameof( MediumTurquoise ), GetConvertedColor( 72, 209, 204) );
        public static CustomColor DarkTurquoise = new CustomColor( nameof( DarkTurquoise ), GetConvertedColor( 0, 206, 209) );
        public static CustomColor CadetBlue = new CustomColor( nameof( CadetBlue ), GetConvertedColor( 95, 158, 160) );
        public static CustomColor SteelBlue = new CustomColor( nameof( SteelBlue ), GetConvertedColor( 70, 130, 180) );
        public static CustomColor LightSteelBlue = new CustomColor( nameof( LightSteelBlue ), GetConvertedColor( 176, 196, 222) );
        public static CustomColor PowderBlue = new CustomColor( nameof( PowderBlue ), GetConvertedColor( 176, 224, 230) );
        public static CustomColor LightBlue = new CustomColor( nameof( LightBlue ), GetConvertedColor( 173, 216, 230) );
        public static CustomColor SkyBlue = new CustomColor( nameof( SkyBlue ), GetConvertedColor( 135, 206, 235) );
        public static CustomColor LightSkyBlue = new CustomColor( nameof( LightSkyBlue ), GetConvertedColor( 135, 206, 250) );
        public static CustomColor DeepSkyBlue = new CustomColor( nameof( DeepSkyBlue ), GetConvertedColor( 0, 191, 255) );
        public static CustomColor DodgerBlue = new CustomColor( nameof( DodgerBlue ), GetConvertedColor( 30, 144, 255) );
        public static CustomColor CornflowerBlue = new CustomColor( nameof( CornflowerBlue ), GetConvertedColor( 100, 149, 237) );
        public static CustomColor MediumSlateBlue = new CustomColor( nameof( MediumSlateBlue ), GetConvertedColor( 123, 104, 238) );
        public static CustomColor RoyalBlue = new CustomColor( nameof( RoyalBlue ), GetConvertedColor( 65, 105, 225) );
        public static CustomColor Blue = new CustomColor( nameof( Blue ), GetConvertedColor( 0, 0, 255) );
        public static CustomColor MediumBlue = new CustomColor( nameof( MediumBlue ), GetConvertedColor( 0, 0, 205) );
        public static CustomColor DarkBlue = new CustomColor( nameof( DarkBlue ), GetConvertedColor( 0, 0, 139) );
        public static CustomColor Navy = new CustomColor( nameof( Navy ), GetConvertedColor( 0, 0, 128) );
        public static CustomColor MidnightBlue = new CustomColor( nameof( MidnightBlue ), GetConvertedColor( 25, 25, 112) );
        
        // Browns
        public static CustomColor Cornsilk = new CustomColor( nameof( Cornsilk ), GetConvertedColor( 255, 248, 220) );
        public static CustomColor BlanchedAlmond = new CustomColor( nameof( BlanchedAlmond ), GetConvertedColor( 255, 235, 205) );
        public static CustomColor Bisque = new CustomColor( nameof( Bisque ), GetConvertedColor( 255, 228, 196) );
        public static CustomColor NavajoWhite = new CustomColor( nameof( NavajoWhite ), GetConvertedColor( 255, 222, 173) );
        public static CustomColor Wheat = new CustomColor( nameof( Wheat ), GetConvertedColor( 245, 222, 179) );
        public static CustomColor BurlyWood = new CustomColor( nameof( BurlyWood ), GetConvertedColor( 222, 184, 135) );
        public static CustomColor Tan = new CustomColor( nameof( Tan ), GetConvertedColor( 210, 180, 140) );
        public static CustomColor RosyBrown = new CustomColor( nameof( RosyBrown ), GetConvertedColor( 188, 143, 143) );
        public static CustomColor SandyBrown = new CustomColor( nameof( SandyBrown ), GetConvertedColor( 244, 164, 96) );
        public static CustomColor Goldenrod = new CustomColor( nameof( Goldenrod ), GetConvertedColor( 218, 165, 32) );
        public static CustomColor DarkGoldenrod = new CustomColor( nameof( DarkGoldenrod ), GetConvertedColor( 184, 134, 11) );
        public static CustomColor Peru = new CustomColor( nameof( Peru ), GetConvertedColor( 205, 133, 63) );
        public static CustomColor Chocolate = new CustomColor( nameof( Chocolate ), GetConvertedColor( 210, 105, 30) );
        public static CustomColor SaddleBrown = new CustomColor( nameof( SaddleBrown ), GetConvertedColor( 139, 69, 19) );
        public static CustomColor Sienna = new CustomColor( nameof( Sienna ), GetConvertedColor( 160, 82, 45) );
        public static CustomColor Brown = new CustomColor( nameof( Brown ), GetConvertedColor( 165, 42, 42) );
        public static CustomColor Maroon = new CustomColor( nameof( Maroon ), GetConvertedColor( 128, 0, 0) );
        
        
    
        
        
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
