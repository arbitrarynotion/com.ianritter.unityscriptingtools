using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime.NoiseMap;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.GUIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor
{
    public static class NoiseEditorUtilities
    {
        /// <summary>
        /// Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
        /// left side that indicate where the objects are sampling the noise.
        /// </summary>
        public static void DrawSceneViewNoiseMapPreview
            ( 
                Rect positionRect,
                float[,] noiseMap, 
                Color lowColor, 
                Color highColor, 
                int totalSamples,
                Color frameColor,
                float frameThickness = 2f,
                ScaleMode scaleMode = ScaleMode.ScaleAndCrop
            )
        {
            Handles.BeginGUI();
            {
                Texture2D noiseMap2D = MapDisplay.GetNoiseMapTexture( noiseMap, lowColor, highColor );
                
                EditorGUI.DrawPreviewTexture( positionRect, noiseMap2D, null, scaleMode, 0.0f, -1 );
                DrawRectOutline( positionRect, frameColor, frameThickness );

                var nodeRect = new Rect( positionRect )
                {
                    width = 5, 
                    height = 2
                };
                nodeRect.x -= 3f;
                
                // Make the height half of the current view port height.
                float variableHeight = Camera.current.pixelRect.height / 2f;
                // Evenly space the ticks out.
                float spacing = variableHeight / noiseMap2D.height;
                // Nudge the starting point down half the height of a separator to center the ticks.
                nodeRect.y += spacing / 2f;
                // Place ticks evenly spaced along the left edge of the preview border.
                for (int x = 0; x < totalSamples; x++)
                {
                    DrawRectOutline( nodeRect, frameColor );
                    nodeRect.y += spacing;
                }
            }
            Handles.EndGUI();
        }
    }
}
