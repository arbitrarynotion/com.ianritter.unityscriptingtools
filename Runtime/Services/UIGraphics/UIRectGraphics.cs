using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics
{
    public static class UIRectGraphics
    {
        /// <summary>
        ///     Draws the provided rect's outline with the specified color and line thickness.
        /// </summary>
        public static void DrawRectOutline( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            // Variable width lines.
            DrawRectLeftEdge( rect, outlineColor, lineThickness );
            DrawRectRightEdge( rect, outlineColor, lineThickness );
            
            // Constant width lines.
            DrawRectTopEdge( rect, outlineColor, lineThickness );
            DrawRectBottomEdge( rect, outlineColor, lineThickness );
        }

        /// <summary>
        ///     Draws the provided rect's left edge with the specified color and line thickness.
        /// </summary>
        public static void DrawRectLeftEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect leftRect = rect;
            leftRect.width = lineThickness;
            DrawRect( leftRect, outlineColor );
        }

        /// <summary>
        ///     Draws the provided rect's right edge with the specified color and line thickness.
        /// </summary>
        public static void DrawRectRightEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect rightRect = rect;
            rightRect.xMin += rightRect.width - lineThickness;
            DrawRect( rightRect, outlineColor );
        }
        
        /// <summary>
        ///     Draws the provided rect's top edge with the specified color and line thickness.
        /// </summary>
        public static void DrawRectTopEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect topRect = rect;
            topRect.height = lineThickness;
            DrawRect( topRect, outlineColor );
        }
        
        /// <summary>
        ///     Draws the provided rect's bottom edge with the specified color and line thickness.
        /// </summary>
        public static void DrawRectBottomEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect bottomRect = rect;
            bottomRect.yMin += bottomRect.height - lineThickness;
            DrawRect( bottomRect, outlineColor );
        }

        /// <summary>
        ///     Draws the provided rect as a solid rectangle with the specified color.
        /// </summary>
        public static void DrawSolidRect( Rect rect, Color color )
        {
            DrawRect( rect, color );
        }

        // Hijacked this from EditorGUI to detach this class from Editor dependency.
        private static void DrawRect( Rect rect, Color color )
        {
            if ( Event.current.type != EventType.Repaint )
                return;
            Color color1 = GUI.color;
            GUI.color *= color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = color1;
        }

        /// <summary>
        ///     Draws the provided rect as a solid rectangle with the specified color including outlines with the specified line thickness.
        /// </summary>
        public static void DrawSolidRectWithOutline( Rect rect, Color outlineColor, Color backGroundColor, float lineThickness = 1f )
        {
            DrawSolidRect( rect, backGroundColor );
            DrawRectOutline( rect, outlineColor, lineThickness );
        }
        
        public static void DrawRect( Rect frameRect, ElementFrameType frameType, Color frameOutlineColor, Color backgroundColor, 
            float outlineThickness, bool includeBackground )
        {
            if ( includeBackground )
                DrawSolidRect( frameRect, backgroundColor );
            
            switch (frameType)
            {
                case ElementFrameType.None:
                    break;
                case ElementFrameType.FullOutline:
                    DrawRectOutline( frameRect, frameOutlineColor, outlineThickness );
                    return;
                case ElementFrameType.Corners:
                    DrawRectFromPartialCorners( frameRect, frameOutlineColor, outlineThickness );
                    return;
                case ElementFrameType.CornersSkipTopLines:
                    DrawRectFromPartialCorners( frameRect, frameOutlineColor, outlineThickness, true );
                    return;
                case ElementFrameType.CornersBottomOnly:
                    DrawRectFromPartialCornersBottomOnly( frameRect, frameOutlineColor, outlineThickness );
                    return;
                case ElementFrameType.CornersLeftBottomOnly:
                    DrawLeftBottomCornerPartialFrame( frameRect, frameOutlineColor, outlineThickness );
                    return;
                case ElementFrameType.SkipBottom:
                    DrawRectTopEdge( frameRect, frameOutlineColor, outlineThickness );
                    break;
                case ElementFrameType.SkipTop:
                    break;
                case ElementFrameType.LeftAndBottomOnly:
                    break;
                case ElementFrameType.BottomOnly:
                    break;
                case ElementFrameType.LeftOnly:
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( frameType ), frameType,
                        "Type not handled. Was the enum updated with new values?" );
            }

            // Left
            if (frameType == ElementFrameType.LeftAndBottomOnly
                || frameType == ElementFrameType.LeftOnly
                || frameType == ElementFrameType.SkipBottom
                || frameType == ElementFrameType.SkipTop)
            {
                DrawRectLeftEdge( frameRect, frameOutlineColor, outlineThickness );
            }
            
            // Right
            if (frameType == ElementFrameType.SkipBottom
                || frameType == ElementFrameType.SkipTop)
            {
                DrawRectRightEdge( frameRect, frameOutlineColor, outlineThickness );
            }
            
            // Bottom
            if (frameType == ElementFrameType.SkipTop 
                || frameType == ElementFrameType.LeftAndBottomOnly 
                || frameType == ElementFrameType.BottomOnly)
            {
                DrawRectBottomEdge( frameRect, frameOutlineColor, outlineThickness );
            }
        }
        
        
        
        public static void DrawLeftTopCornerPartialFrame( Rect frameRect, Color backgroundColor, bool skipTopLines = false, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect ) {height = partialCornerLineLength};
            DrawRectLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
            
            if (skipTopLines)
                return;
            
            var horizontalLineRect = new Rect( frameRect ) {width = partialCornerLineLength};
            DrawRectTopEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }
        
        public static void DrawRightTopCornerPartialFrame( Rect frameRect, Color backgroundColor, bool skipTopLines = false, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect ) {height = partialCornerLineLength};
            verticalLineRect.xMin += ( frameRect.width - outlineThickness );
            DrawRectLeftEdge( verticalLineRect, backgroundColor, outlineThickness );

            if (skipTopLines)
                return;
            
            var horizontalLineRect = new Rect( frameRect );
            horizontalLineRect.xMin += ( frameRect.width - partialCornerLineLength );
            DrawRectTopEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }
        
        public static void DrawLeftBottomCornerPartialFrame( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect );
            verticalLineRect.yMin += ( frameRect.height - partialCornerLineLength );
            DrawRectLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
            
            var horizontalLineRect = new Rect( frameRect ) {width = partialCornerLineLength};
            DrawRectBottomEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }

        public static void DrawRightBottomCornerPartialFrame( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect );
            verticalLineRect.yMin += ( frameRect.height - partialCornerLineLength );
            verticalLineRect.xMin += ( frameRect.width - outlineThickness );
            DrawRectLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
            
            var horizontalLineRect = new Rect( frameRect );
            horizontalLineRect.xMin += ( frameRect.width - partialCornerLineLength );
            DrawRectBottomEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }
        
        
        public static void DrawRectFromPartialCornersBottomOnly( Rect frameRect, Color frameOutlineColor, float outlineThickness )
        {
            DrawLeftBottomCornerPartialFrame( frameRect, frameOutlineColor, outlineThickness );
            DrawRightBottomCornerPartialFrame( frameRect, frameOutlineColor, outlineThickness );
        }

        public static void DrawRectFromPartialCorners( Rect frameRect, Color frameOutlineColor, float outlineThickness, bool skipTopLine = false )
        {
            DrawLeftTopCornerPartialFrame( frameRect, frameOutlineColor, skipTopLine, outlineThickness );
            DrawRightTopCornerPartialFrame( frameRect, frameOutlineColor, skipTopLine, outlineThickness );
            DrawLeftBottomCornerPartialFrame( frameRect, frameOutlineColor, outlineThickness );
            DrawRightBottomCornerPartialFrame( frameRect, frameOutlineColor, outlineThickness );
        }
        
        public static bool FrameHasTop( ElementFrameType frameType )
        {
            return frameType != ElementFrameType.None &&
                   ( frameType == ElementFrameType.FullOutline
                     || frameType == ElementFrameType.Corners
                     || frameType == ElementFrameType.SkipBottom );
        }

        public static bool FrameHasLeft( ElementFrameType frameType )
        {
            return frameType != ElementFrameType.None &&
                   frameType != ElementFrameType.BottomOnly;
        }

        public static bool FrameHasRight( ElementFrameType frameType )
        {
            return frameType != ElementFrameType.None &&
                   !( frameType == ElementFrameType.BottomOnly
                      || frameType == ElementFrameType.LeftOnly
                      || frameType == ElementFrameType.CornersLeftBottomOnly
                      || frameType == ElementFrameType.LeftAndBottomOnly);
        }

        public static bool FrameHasBottom( ElementFrameType frameType )
        {
            return frameType != ElementFrameType.None &&
                   !( frameType == ElementFrameType.SkipBottom
                      || frameType == ElementFrameType.LeftOnly );
        }
        
        
        public static Texture2D GenerateTexture( int width, int height, Color color )
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            var backgroundTexture = new Texture2D( width, height );

            backgroundTexture.SetPixels( pixels );
            backgroundTexture.Apply();

            return backgroundTexture;
        }
    }
}
