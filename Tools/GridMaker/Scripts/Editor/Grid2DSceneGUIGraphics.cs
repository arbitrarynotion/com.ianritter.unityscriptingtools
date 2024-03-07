using Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Runtime;

using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering;

using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.Scene.SceneGUIHandles;

namespace Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Editor
{
    public static class Grid2DSceneGUIGraphics
    {
#region 2DGrid

        public static void Draw2DGrid( Grid2D grid, Transform transform, bool showPlusAndMinusSigns, float lineThickness = 2f )
        {
            Draw2DGrid( transform, new Vector2( grid.gridWidth, grid.gridHeight ), grid.gridSizeX, grid.gridSizeY, showPlusAndMinusSigns, lineThickness );
        }

        /// <summary>
        ///     Draws a 2d grid along the x and z axis. Note that this method does not currently support other axis or orientations.
        /// </summary>
        /// <param name="transform">Transform of the grid's game object, which acts as the center and the context to align rotations.</param>
        /// <param name="gridSize">Size of the grid in world space.</param>
        /// <param name="gridSizeX">Grid will be divided in to this many horizontal sections.</param>
        /// <param name="gridSizeY">Grid will be divided in to this many vertical sections.</param>
        /// <param name="showPlusAndMinusSigns">Show plus and minus signs at edges of grid to help verify orientation.</param>
        public static void Draw2DGrid( Transform transform, Vector2 gridSize, int gridSizeX, int gridSizeY, bool showPlusAndMinusSigns, float lineThickness = 2f )
        {
            // Note: grid is worked out in local space then converted to world space at the end (using transform.TransformPoint())..

            var grid = new Grid2D( gridSizeX, gridSizeY, gridSize.x, gridSize.y, transform );

            Vector3 horizontal = Vector3.right;
            var vertical = new Vector3( 0, 0, -1 );

            // Draw horizontal lines.
            for ( int x = 0; x < gridSizeX + 1; x++ )
            {
                DrawGridLine( x, grid.nodeWidth, horizontal, vertical, grid.gridHeight );
            }

            // Draw vertical lines.
            for ( int y = 0; y < gridSizeY + 1; y++ )
            {
                DrawGridLine( y, grid.nodeHeight, vertical, horizontal, grid.gridWidth );
            }

            if( showPlusAndMinusSigns )
                DrawPlusAndMinusSigns( grid.worldTopLeft, grid.gridWidth, grid.gridHeight );

            void DrawGridLine
            (
                int currentLine,
                float nodeWidth,
                Vector3 parallel,
                Vector3 perpendicular,
                float perpendicularScale
            )
            {
                float parallelScale = currentLine * nodeWidth;
                Vector3 lineStart = grid.worldTopLeft + parallel * parallelScale;
                Vector3 lineEnd = lineStart + perpendicular * perpendicularScale;

                // With all points established, convert them from local to world space. This ensures they keep their
                // position relative the position and rotation of their parent transform.
                lineStart = transform.TransformPoint( lineStart );
                lineEnd = transform.TransformPoint( lineEnd );

                Handles.DrawAAPolyLine( lineThickness, lineStart, lineEnd );
            }
        }


        /// <summary>
        ///     Draws a sphere at the center of each cell in a 2D grid.
        /// </summary>
        public static void Draw2DGridNodes( Grid2D grid, Transform transform, Color color, float radius = 0.1f )
        {
            // Offset to place the nodes in the center of the grid cells.
            var shiftToCenter = new Vector3( grid.nodeWidth / 2f, 0f, -grid.nodeHeight / 2f );
            Vector3 topLeft = grid.worldTopLeft + shiftToCenter;

            topLeft = transform.TransformPoint( topLeft );

            Handles.zTest = CompareFunction.LessEqual;

            for ( int x = 0; x < grid.gridSizeX; x++ )
            {
                for ( int y = 0; y < grid.gridSizeY; y++ )
                {
                    var nodePos = new Vector3( grid.nodeWidth * x, 0f, -grid.nodeHeight * y );
                    nodePos = transform.TransformPoint( nodePos );
                    DrawSphere( topLeft + nodePos, color, radius );
                }
            }
        }


        /// <summary>
        ///     Draws a sphere at the center of each cell in a 2D grid.
        /// </summary>
        /// <param name="transform">Transform of the grid's game object, which acts as the center and the context to align rotations.</param>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        /// <param name="gridSizeX">Grid will be divided in to this many horizontal sections.</param>
        /// <param name="gridSizeY">Grid will be divided in to this many vertical sections.</param>
        public static void Draw2DGridNodes
        (
            Transform transform,
            float gridWidth,
            float gridHeight,
            int gridSizeX,
            int gridSizeY,
            Color color,
            float radius = 0.1f
        )
        {
            float nodeWidthX = gridWidth / gridSizeX;
            float nodeWidthY = gridHeight / gridSizeY;

            // Get top left corner of grid space.
            var worldTopLeft = new Vector3( -nodeWidthX * gridSizeX / 2f, 0f, nodeWidthY * gridSizeY / 2f );

            // Offset center to be the center of the grid space.
            var shiftToCenter = new Vector3( nodeWidthX / 2f, 0f, -nodeWidthY / 2f );
            worldTopLeft += shiftToCenter;
            worldTopLeft = transform.TransformPoint( worldTopLeft );

            for ( int x = 0; x < gridSizeX; x++ )
            {
                for ( int y = 0; y < gridSizeY; y++ )
                {
                    var nodePos = new Vector3( nodeWidthX * x, 0f, -nodeWidthY * y );
                    nodePos = transform.TransformPoint( nodePos );
                    DrawSphere( worldTopLeft + nodePos, color, radius );
                }
            }
        }

#endregion


#region Helpers

        /// <summary>
        ///     Draws plus and minus sign symbols at the appropriate edges of the grid. Defaults colors are blue for z and red for x.
        /// </summary>
        /// <param name="worldTopLeft">Top left edge of the grid.</param>
        /// <param name="lengthX">Length of the grid along the x axis.</param>
        /// <param name="lengthY">Length of the grid along the y axis.</param>
        /// <param name="edgePadding">World space distance between the edge of the grid and the center of the symbols.</param>
        /// <param name="scale">Size of the symbols.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame rectangles in the symbols.</param>
        public static void DrawPlusAndMinusSigns
        ( Vector3 worldTopLeft, float lengthX, float lengthY, float edgePadding = 0.2f,
            float scale = 0.5f, float lineThickness = 3f
        )
        {
            DrawPlusAndMinusSigns( worldTopLeft, lengthX, lengthY, Color.red, Color.blue, edgePadding, scale, lineThickness );
        }

        /// <summary>
        ///     Draws plus and minus sign symbols at the appropriate edges of the grid. Defaults colors are blue for z and red for x.
        /// </summary>
        /// <param name="worldTopLeft">Top left edge of the grid.</param>
        /// <param name="lengthX">Length of the grid along the x axis.</param>
        /// <param name="lengthY">Length of the grid along the y axis.</param>
        /// <param name="xAxisColor">Color used for the x axis symbols.</param>
        /// <param name="yAxisColor">Color used for the y axis symbols.</param>
        /// <param name="edgePadding">World space distance between the edge of the grid and the center of the symbols.</param>
        /// <param name="scale">Size of the symbols.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame rectangles in the symbols.</param>
        public static void DrawPlusAndMinusSigns
        ( Vector3 worldTopLeft, float lengthX, float lengthY,
            Color xAxisColor, Color yAxisColor, float edgePadding = 0.2f, float scale = 0.5f, float lineThickness = 3f
        )
        {
            Vector3 plusSignY = worldTopLeft + new Vector3( lengthX / 2, 0, edgePadding );
            DrawPlusSign( plusSignY, scale, lineThickness, yAxisColor );

            Vector3 minusSignY = worldTopLeft + new Vector3( lengthX / 2, 0, -lengthY - edgePadding );
            DrawMinusSign( minusSignY, scale, lineThickness, yAxisColor );

            Vector3 plusSignX = worldTopLeft + new Vector3( lengthX + edgePadding, 0, -lengthY / 2 );
            DrawPlusSign( plusSignX, scale, lineThickness, xAxisColor );

            Vector3 minusSignX = worldTopLeft + new Vector3( -edgePadding, 0, -lengthY / 2 );
            DrawMinusSign( minusSignX, scale, lineThickness, xAxisColor );
        }

        /// <summary>
        ///     Draws a plus sign from two 2D wire frame rectangles.
        /// </summary>
        /// <param name="center">Plus sign will be centered at this position.</param>
        /// <param name="scale">Scale of the symbol.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame.</param>
        /// <param name="color">Color of the symbol</param>
        public static void DrawPlusSign( Vector3 center, float scale, float lineThickness, Color color )
        {
            float width = 0.3f * scale;
            float height = 0.05f * scale;
            Draw2dWireRectangle( center, width, height, lineThickness, color );
            Draw2dWireRectangle( center, height, width, lineThickness, color );
        }

        /// <summary>
        ///     Draws a minus sign from a 2D wire frame rectangle.
        /// </summary>
        /// <param name="center">Minus sign will be centered at this position.</param>
        /// <param name="scale">Scale of the symbol.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame.</param>
        /// <param name="color">Color of the symbol</param>
        public static void DrawMinusSign( Vector3 center, float scale, float lineThickness, Color color )
        {
            float width = 0.3f * scale;
            float height = 0.05f * scale;
            Draw2dWireRectangle( center, width, height, lineThickness, color );
        }

        /// <summary>
        ///     Draw a 2D wire frame rectangle. This a essentially equivalent to a Gizmos.DrawWireSquare() but with Handles.
        /// </summary>
        /// <param name="center">Rectangle will be centered at this position.</param>
        /// <param name="width">Length of rectangle along the x axis.</param>
        /// <param name="height">Length of rectangle along the y axis.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame.</param>
        /// <param name="color">Color of the wire frame.</param>
        public static void Draw2dWireRectangle( Vector3 center, float width, float height, float lineThickness, Color color )
        {
            Color storedColor = Handles.color;
            Handles.color = color;

            float halfWidth = width / 2;
            float halfHeight = height / 2;

            Vector3 topLeft = center + new Vector3( -halfWidth, 0, halfHeight );
            Vector3 bottomLeft = center + new Vector3( -halfWidth, 0, -halfHeight );
            Vector3 topRight = center + new Vector3( halfWidth, 0, halfHeight );
            Vector3 bottomRight = center + new Vector3( halfWidth, 0, -halfHeight );

            Handles.DrawAAPolyLine( lineThickness, topLeft, topRight );
            Handles.DrawAAPolyLine( lineThickness, topRight, bottomRight );
            Handles.DrawAAPolyLine( lineThickness, bottomRight, bottomLeft );
            Handles.DrawAAPolyLine( lineThickness, bottomLeft, topLeft );

            Handles.color = storedColor;
        }

#endregion
    }
}