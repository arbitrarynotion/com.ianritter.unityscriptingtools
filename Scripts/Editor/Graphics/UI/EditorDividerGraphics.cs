using UnityEditor;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.UI
{
    public static class EditorDividerGraphics
    {
        private const float StandardIndent = 15f;
        private const float MinIndentAdjustment = 2f;
        private const float StandardLeftMargin = 4f;

        /// <summary>
        ///     Builds a divider in between vertical elements in the inspector window. Presence of scrollbar is not considered
        ///     since there is no straightforward way I could find to detect it.
        /// </summary>
        /// <param name="dividerThickness">Vertical thickness of the line drawn.</param>
        /// <param name="leftMargin"></param>
        /// <param name="rightMargin">
        ///     Number of pixels of space between the end of the divider line and the right edge
        ///     of the of the inspector window.
        /// </param>
        /// <param name="dividerColor">Color of the line.</param>
        /// <param name="topSpacing"></param>
        /// <param name="bottomSpacing"></param>
        public static void DrawDivider
        (
            Color dividerColor,
            float dividerThickness = 1,
            float leftMargin = 5,
            float rightMargin = 5,
            float topSpacing = 12,
            float bottomSpacing = 12
        )
        {
            EditorGUILayout.Space( topSpacing );

            Rect divider = EditorGUILayout.GetControlRect( GUILayout.Height( dividerThickness ) );

            float indentAdjustment = Mathf.Max( MinIndentAdjustment, EditorGUI.indentLevel * StandardIndent );

            divider.x = indentAdjustment + leftMargin;
            divider.width += StandardIndent - indentAdjustment + StandardLeftMargin - rightMargin - leftMargin;
            divider.height = dividerThickness;
            EditorGUI.DrawRect( divider, dividerColor );

            EditorGUILayout.Space( bottomSpacing );
        }
    }
}