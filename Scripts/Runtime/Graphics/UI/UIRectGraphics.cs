using System;

using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI
{
    /// <summary>
    ///     This class provides various methods for drawing in either the Inspector Window or and Editor Window.
    /// </summary>
    public static class UIRectGraphics
    {
        /// <summary>
        ///     Draws the provided rect's outline with the specified color and line thickness. If you want this outline to have
        ///     a background, draw a solid rect first, then draw this outline.
        /// </summary>
        public static void DrawRectOutline( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            // Variable width lines.
            DrawFullLeftEdge( rect, outlineColor, lineThickness );
            DrawFullRightEdge( rect, outlineColor, lineThickness );

            // Constant width lines.
            DrawFullTopEdge( rect, outlineColor, lineThickness );
            DrawFullBottomEdge( rect, outlineColor, lineThickness );
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
            if( Event.current.type != EventType.Repaint )
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

        public static void DrawRect
        ( Rect frameRect, ElementFrameType frameType, Color frameOutlineColor, Color backgroundColor,
            float outlineThickness, bool includeBackground
        )
        {
            if( includeBackground )
                DrawSolidRect( frameRect, backgroundColor );

            // Partial edges.
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
                    DrawFullTopEdge( frameRect, frameOutlineColor, outlineThickness );
                    break;
                case ElementFrameType.SkipTop:
                    break;
                case ElementFrameType.LeftAndBottomOnly:
                    break;
                case ElementFrameType.BottomOnly:
                    break;
                case ElementFrameType.LeftOnly:
                    break;
                case ElementFrameType.FullLeftPartialBottom:
                    DrawFullLeftPartialBottom( frameRect, frameOutlineColor, outlineThickness );
                    break;
                case ElementFrameType.PartialLeftFullBottom:
                    DrawPartialLeftFullBottom( frameRect, frameOutlineColor, outlineThickness );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( frameType ), frameType,
                        "Type not handled. Was the enum updated with new values?" );
            }

            // Complete edges.
            // Left
            if( frameType == ElementFrameType.LeftAndBottomOnly ||
                frameType == ElementFrameType.LeftOnly ||
                frameType == ElementFrameType.SkipBottom ||
                frameType == ElementFrameType.SkipTop ||
                frameType == ElementFrameType.FullLeftPartialBottom )
                DrawFullLeftEdge( frameRect, frameOutlineColor, outlineThickness );

            // Right
            if( frameType == ElementFrameType.SkipBottom ||
                frameType == ElementFrameType.SkipTop )
                DrawFullRightEdge( frameRect, frameOutlineColor, outlineThickness );

            // Bottom
            if( frameType == ElementFrameType.SkipTop ||
                frameType == ElementFrameType.LeftAndBottomOnly ||
                frameType == ElementFrameType.BottomOnly ||
                frameType == ElementFrameType.PartialLeftFullBottom )
                DrawFullBottomEdge( frameRect, frameOutlineColor, outlineThickness );
        }

        /// <summary>
        ///     Returns a 2D texture with the given width, height, and color. Useful for button backgrounds to avoid
        ///     automatic button tinting.
        /// </summary>
        public static Texture2D GenerateTexture( int width, int height, Color color )
        {
            Color[] pixels = new Color[width * height];

            for ( int i = 0; i < pixels.Length; i++ )
            {
                pixels[i] = color;
            }

            var backgroundTexture = new Texture2D( width, height );

            backgroundTexture.SetPixels( pixels );
            backgroundTexture.Apply();

            return backgroundTexture;
        }


#region FullEdges

        /// <summary>
        ///     Draws the provided rect's left edge with the specified color and line thickness.
        /// </summary>
        public static void DrawFullLeftEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect leftRect = rect;
            leftRect.width = lineThickness;
            DrawRect( leftRect, outlineColor );
        }

        /// <summary>
        ///     Draws the provided rect's right edge with the specified color and line thickness.
        /// </summary>
        public static void DrawFullRightEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect rightRect = rect;
            rightRect.xMin += rightRect.width - lineThickness;
            DrawRect( rightRect, outlineColor );
        }

        /// <summary>
        ///     Draws the provided rect's top edge with the specified color and line thickness.
        /// </summary>
        public static void DrawFullTopEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect topRect = rect;
            topRect.height = lineThickness;
            DrawRect( topRect, outlineColor );
        }

        /// <summary>
        ///     Draws the provided rect's bottom edge with the specified color and line thickness.
        /// </summary>
        public static void DrawFullBottomEdge( Rect rect, Color outlineColor, float lineThickness = 1f )
        {
            Rect bottomRect = rect;
            bottomRect.yMin += bottomRect.height - lineThickness;
            DrawRect( bottomRect, outlineColor );
        }

#endregion


#region PartialEdges

        // Split rect edge parts into their own methods.
        // DrawPartialLeftUpperEdge
        public static void DrawPartialLeftUpperEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect ) { height = partialCornerLineLength };
            DrawFullLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialLeftLowerEdge
        public static void DrawPartialLeftLowerEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect );
            verticalLineRect.yMin += frameRect.height - partialCornerLineLength;
            DrawFullLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialRightUpperEdge
        public static void DrawPartialRightUpperEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect ) { height = partialCornerLineLength };
            verticalLineRect.xMin += frameRect.width - outlineThickness;
            DrawFullLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialRightLowerEdge
        public static void DrawPartialRightLowerEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var verticalLineRect = new Rect( frameRect );
            verticalLineRect.yMin += frameRect.height - partialCornerLineLength;
            verticalLineRect.xMin += frameRect.width - outlineThickness;
            DrawFullLeftEdge( verticalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialBottomLeftEdge
        public static void DrawPartialBottomLeftEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var horizontalLineRect = new Rect( frameRect ) { width = partialCornerLineLength };
            DrawFullBottomEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialBottomRightEdge
        public static void DrawPartialBottomRightEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var horizontalLineRect = new Rect( frameRect );
            horizontalLineRect.xMin += frameRect.width - partialCornerLineLength;
            DrawFullBottomEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialTopLeftEdge
        public static void DrawPartialTopLeftEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var horizontalLineRect = new Rect( frameRect ) { width = partialCornerLineLength };
            DrawFullTopEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }

        // DrawPartialTopRightEdge
        public static void DrawPartialTopRightEdge( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            var horizontalLineRect = new Rect( frameRect );
            horizontalLineRect.xMin += frameRect.width - partialCornerLineLength;
            DrawFullTopEdge( horizontalLineRect, backgroundColor, outlineThickness );
        }

#endregion

#region Combinations

        public static void DrawLeftTopCornerPartialFrame( Rect frameRect, Color backgroundColor, bool skipTopLines = false, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            // Draw left partial upper.
            DrawPartialLeftUpperEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );

            if( skipTopLines )
                return;

            // Draw top partial left.
            DrawPartialTopLeftEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );
        }

        public static void DrawRightTopCornerPartialFrame( Rect frameRect, Color backgroundColor, bool skipTopLines = false, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            // Right partial upper
            DrawPartialRightUpperEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );

            if( skipTopLines )
                return;

            // Top partial right
            DrawPartialTopRightEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );
        }

        public static void DrawLeftBottomCornerPartialFrame( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            // Left partial lower
            DrawPartialLeftLowerEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );

            // Bottom partial left
            DrawPartialBottomLeftEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );
        }

        public static void DrawRightBottomCornerPartialFrame( Rect frameRect, Color backgroundColor, float outlineThickness = 1f, float partialCornerLineLength = 10f )
        {
            // Right partial lower
            DrawPartialRightLowerEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );

            // Bottom partial right
            DrawPartialBottomRightEdge( frameRect, backgroundColor, outlineThickness, partialCornerLineLength );
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

        public static void DrawFullLeftPartialBottom( Rect frameRect, Color frameOutlineColor, float outlineThickness )
        {
            // Left full
            DrawFullLeftEdge( frameRect, frameOutlineColor, outlineThickness );

            // Bottom partial left
            DrawPartialBottomLeftEdge( frameRect, frameOutlineColor, outlineThickness );
        }

        public static void DrawPartialLeftFullBottom( Rect frameRect, Color frameOutlineColor, float outlineThickness )
        {
            // Left partial lower
            DrawPartialLeftLowerEdge( frameRect, frameOutlineColor, outlineThickness );

            // Bottom full
            DrawFullBottomEdge( frameRect, frameOutlineColor, outlineThickness );
        }

#endregion


#region EdgeChecks

        // Used for determining if padding should be applied to a given edge.

        public static bool FrameHasTop( ElementFrameType frameType ) =>
            frameType != ElementFrameType.None &&
            ( frameType == ElementFrameType.FullOutline
              || frameType == ElementFrameType.Corners
              || frameType == ElementFrameType.SkipBottom );

        public static bool FrameHasLeft( ElementFrameType frameType ) =>
            frameType != ElementFrameType.None &&
            frameType != ElementFrameType.BottomOnly;

        public static bool FrameHasRight( ElementFrameType frameType ) =>
            frameType != ElementFrameType.None &&
            !( frameType == ElementFrameType.BottomOnly
               || frameType == ElementFrameType.LeftOnly
               || frameType == ElementFrameType.CornersLeftBottomOnly
               || frameType == ElementFrameType.LeftAndBottomOnly );

        public static bool FrameHasBottom( ElementFrameType frameType ) =>
            frameType != ElementFrameType.None &&
            !( frameType == ElementFrameType.SkipBottom
               || frameType == ElementFrameType.LeftOnly );

#endregion
    }
}