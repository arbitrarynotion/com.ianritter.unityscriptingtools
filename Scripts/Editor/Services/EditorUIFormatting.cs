using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.UI.UIEditorOnlyRectGraphics;
using static UnityEditor.EditorGUI;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public static class EditorUIFormatting
    {
        public const ElementFrameType Level1FrameType = ElementFrameType.BottomOnly;
        
        public const ElementFrameType FoldoutFrameType = ElementFrameType.LeftAndTopOnly;
        public const ElementFrameType SubFoldoutFrameType = ElementFrameType.LeftAndTopOnly;
        public const ElementFrameType LabelHeadingFrameType = ElementFrameType.LeftOnly;
        public const ElementFrameType SubLabelHeadingFrameType = ElementFrameType.None;
        public const ElementFrameType EditorFrameType = ElementFrameType.PartialLeftFullBottom;
        
        public const float ParentFrameWidth = 2f;
        public const float ChildFrameWidth = 2f;
        
        public const float TitleFrameBottomPadding = 2f;
        public const float TitleLeftEdgePadding = 4f;
        public const float BetweenSectionPadding = 8f;
        public const float VerticalSeparator = 8f;
        
        public const float EditorFrameBottomPadding = 10f;


        public static bool DrawFoldoutSection( string titleText, ElementFrameType frameType, bool toggle )
        {
            Space( TitleFrameBottomPadding );

            const float value = 0.19f;
            bool result = Foldout
            (
                GetFramedControlRect
                (
                    Color.gray,
                    new Color( value, value, value ),
                    frameType,
                    0f,
                    true
                ),
                toggle,
                titleText,
                true
            );
            
            Space( TitleFrameBottomPadding );

            return result;
        }

        public static void DrawLabelSection( string titleText, ElementFrameType frameType )
        {
            // Space( VerticalSeparator );
            Space( TitleFrameBottomPadding );

            const float value = 0.19f;
            LabelField(
                GetFramedControlRect
                (
                    Color.gray,
                    new Color( value, value, value ),
                    frameType,
                    TitleLeftEdgePadding
                ),
                titleText,
                EditorStyles.boldLabel
            );
            
            Space( TitleFrameBottomPadding );
        }

        public static bool DrawFoldoutSection( string titleText, bool toggle, ElementFrameType frameType, float titleLeftEdgePadding, float titleUnderFramePadding )
        {
            const float value = 0.19f;
            bool result = Foldout(
                GetFramedControlRect
                (
                    Color.gray,
                    new Color( value, value, value ),
                    frameType,
                    titleLeftEdgePadding
                ),
                toggle,
                titleText,
                EditorStyles.boldLabel
            );
            Space( titleUnderFramePadding );
            return result;
        }
    }
}