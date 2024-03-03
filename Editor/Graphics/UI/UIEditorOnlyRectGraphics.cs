using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors.PresetColors;

namespace Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI
{
    public static class UIEditorOnlyRectGraphics
    {
        public static Rect GetFramedControlRect
        (
            Color frameColor,
            Color backgroundColor,
            ElementFrameType frameType = ElementFrameType.FullOutline,
            float leftPadding = 2f,
            bool isFoldout = false,
            float frameWidth = 2f,
            float padding = 2f
        )
        {
            return GetFramedControlRect
            (
                frameColor,
                backgroundColor,
                true,
                frameType,
                leftPadding,
                isFoldout,
                frameWidth,
                padding
            );
        }

        public static Rect GetFramedControlRect
            ( 
                Color frameColor, 
                ElementFrameType frameType = ElementFrameType.FullOutline, 
                float leftPadding = 2f, 
                bool isFoldout = false, 
                float frameWidth = 2f, 
                float padding = 2f 
            )
        {
            return GetFramedControlRect
            (
                frameColor,
                frameColor,
                false,
                frameType,
                leftPadding,
                isFoldout,
                frameWidth,
                padding
            );
        }

        private static Rect GetFramedControlRect
        (
            Color frameColor,
            Color backgroundColor,
            bool showBackground,
            ElementFrameType frameType = ElementFrameType.FullOutline,
            float leftPadding = 2f,
            bool isFoldout = false,
            float frameWidth = 2f,
            float padding = 2f
        )
        {
            if ( frameType == ElementFrameType.None ) 
                padding = 0f;
            
            // Set the height to standard line height plus ( 2 * padding ) + ( 2 * ( frameWidth / 2f ) ).
            Rect controlRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight + ( 2 * padding ) + frameWidth );
            
            // Set the frame rect to have an indent, with foldouts needing one less to fit their arrow.
            var frameRect = new Rect( controlRect);
            frameRect.xMin += ( isFoldout ? ( EditorGUI.indentLevel - 1 ) : EditorGUI.indentLevel ) * 15f;
            DrawRect( frameRect, frameType, frameColor, backgroundColor, frameWidth, showBackground );
            
            // Prep the return rect to have padding on the left side to move the text away from the frame.
            controlRect.xMin += padding + leftPadding;
            return controlRect;
        }
    }
}
