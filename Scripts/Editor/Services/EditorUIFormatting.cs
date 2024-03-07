using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.UI.UIEditorOnlyRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public static class EditorUIFormatting
    {
        public const ElementFrameType Level0FrameType = ElementFrameType.FullOutline;
        public const ElementFrameType Level1FrameType = ElementFrameType.BottomOnly;
        public const ElementFrameType Level2FrameType = ElementFrameType.None;
        public const float TitleFrameBottomPadding = 2f;
        public const float BetweenSectionPadding = 8f;
        public const float TitleLeftEdgePadding = 4f;
        public const float VerticalSeparator = 8f;


        public static bool DrawFoldoutSection( string titleText, ElementFrameType frameType, bool toggle )
        {
            const float value = 0.19f;
            return EditorGUI.Foldout
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
        }

        public static void DrawLabelSection( string titleText, ElementFrameType frameType, float titleLeftEdgePadding, float titleUnderFramePadding )
        {
            const float value = 0.19f;
            EditorGUI.LabelField(
                GetFramedControlRect
                (
                    Color.gray,
                    new Color( value, value, value ),
                    frameType,
                    titleLeftEdgePadding,
                    false
                ),
                titleText,
                EditorStyles.boldLabel
            );
            Space( titleUnderFramePadding );
        }

        public static bool DrawFoldoutSection( string titleText, bool toggle, ElementFrameType frameType, float titleLeftEdgePadding, float titleUnderFramePadding )
        {
            const float value = 0.19f;
            bool result = EditorGUI.Foldout(
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
