using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor.Graphics
{
    public class SceneGuiGraphics : MonoBehaviour
    {
    /// <summary>
    /// Draw the x, y, and z axis arrows at Vector3.zero of the provided local matrix localToWorldMtx.
    /// </summary>
    /// <param name="localToWorldMtx"></param>
    public static void DrawAxes( Matrix4x4 localToWorldMtx )
    {
        Vector3 origin = localToWorldMtx.MultiplyPoint3x4( Vector3.zero );
        Vector3 up = localToWorldMtx.MultiplyVector( Vector3.up );
        Vector3 right = localToWorldMtx.MultiplyVector( Vector3.right );
        Vector3 forward = localToWorldMtx.MultiplyVector( Vector3.forward );

        DrawAAPolyArrow( localToWorldMtx, origin, origin + up, Color.green, 3, true );
        DrawAAPolyArrow( localToWorldMtx, origin, origin + right, Color.red );
        DrawAAPolyArrow( localToWorldMtx, origin, origin + forward, Color.blue, 3, true );
    }

    /// <summary>
    /// Draw Anti-aliased arrow. LocalToWorldMatrix is used to ensure arrowhead lines up with custom axes.
    /// </summary>
    /// <param name="localToWorldMtx">Local space matrix.</param>
    /// <param name="arrowOrigin">Position of the arrow base.</param>
    /// <param name="arrowDestination">Direction the arrow is pointing in (not including local origin offset)</param>
    /// <param name="color"></param>
    /// <param name="lineThickness">Thickness of the lines used to draw the arrow.</param>
    /// <param name="flipArrowheadAxis">Flips the arrowhead's plane from the x axis to the z axis.</param>
    /// <param name="arrowheadScale">Size of the arrowhead will be this percent of the arrow's length.</param>
    /// <param name="scaleArrowheadWithLength">Arrowhead size will increase/decrease with the arrow length.</param>
    public static void DrawAAPolyArrow( Matrix4x4 localToWorldMtx, Vector3 arrowOrigin, Vector3 arrowDestination, 
        Color color, int lineThickness = 3, bool flipArrowheadAxis = false,
        float arrowheadScale = 0.1f, bool scaleArrowheadWithLength = true )
    {
        Vector3 upAxis = localToWorldMtx.MultiplyVector( Vector3.up );
        Vector3 rightAxis = localToWorldMtx.MultiplyVector( Vector3.right );
        
        DrawPolyArrow( arrowOrigin, arrowDestination, color, rightAxis, upAxis, 
            lineThickness, flipArrowheadAxis, arrowheadScale, scaleArrowheadWithLength );
    }

    /// <summary>
    /// Draw Anti-aliased arrow. Arrowhead will be lined up to the cross product of the world up and the arrow direction.
    /// </summary>
    /// <param name="arrowOrigin">Position of the arrow base.</param>
    /// <param name="arrowDestination">Direction the arrow is pointing in (not including local origin offset)</param>
    /// <param name="color"></param>
    /// <param name="lineThickness">Thickness of the lines used to draw the arrow.</param>
    /// <param name="flipArrowheadAxis">Flips the arrowhead's plane from the x axis to the z axis.</param>
    /// <param name="arrowheadScale">Size of the arrowhead will be this percent of the arrow's length.</param>
    /// <param name="scaleArrowheadWithLength">Arrowhead size will increase/decrease with the arrow length.</param>
    public static void DrawAAPolyArrow( Vector3 arrowOrigin, Vector3 arrowDestination, 
        Color color, int lineThickness = 3, bool flipArrowheadAxis = false,
        float arrowheadScale = 0.1f, bool scaleArrowheadWithLength = true )
    {
        Vector3 arrowUp = Vector3.up;
        Vector3 arrowDirection = (  arrowOrigin - arrowDestination ).normalized;

        Vector3 upAxis = Vector3.Cross( arrowDirection, arrowUp ).normalized;
        Vector3 rightAxis = Vector3.Cross( arrowDirection, upAxis ).normalized;
        
        DrawPolyArrow( arrowOrigin, arrowDestination, color, rightAxis, upAxis, 
            lineThickness, flipArrowheadAxis, arrowheadScale, scaleArrowheadWithLength);
    }

    private static void DrawPolyArrow( Vector3 arrowOrigin, Vector3 arrowDestination, Color color, 
        Vector3 rightAxis, Vector3 upAxis, float lineThickness, bool flipArrowheadAxis,
        float arrowheadScale = 0.1f, bool scaleArrowheadWithLength = true )
    {
        // Using the camera up is another approach but it doesn't seem to be a marked improvement
        // over the current method.
//        Camera cam = SceneView.currentDrawingSceneView.camera;
//        if ( cam == null )
//        {
//            return;
//        }
//
//        upAxis = cam.transform.up;
        Vector3 arrowDirection = (  arrowOrigin - arrowDestination );
        Vector3 arrowheadAxis = flipArrowheadAxis ? rightAxis : upAxis;

        float arrowheadWidth = scaleArrowheadWithLength? arrowheadScale * arrowDirection.magnitude : arrowheadScale;
        float arrowheadLength = arrowheadWidth;
        Vector3 arrowheadEdgeOffsetX = arrowheadAxis * ( arrowheadWidth / 2f );
        Vector3 arrowheadEdgeOffsetY = arrowDirection.normalized * arrowheadLength;
        
        Vector3 arrowheadLeft = arrowDestination - arrowheadEdgeOffsetX + arrowheadEdgeOffsetY;
        Vector3 arrowheadRight = arrowDestination + arrowheadEdgeOffsetX + arrowheadEdgeOffsetY;
        
        // Preserve current handles color.
        Color savedColor = Handles.color;
        Handles.color = color;
        
        // Shaft.
        Handles.DrawAAPolyLine( lineThickness, arrowOrigin, arrowDestination );
        
        // Sides of arrowhead.
        Handles.DrawAAPolyLine( lineThickness, arrowDestination, arrowheadLeft );
        Handles.DrawAAPolyLine( lineThickness, arrowDestination, arrowheadRight );
        
        // Restore previous handles color.
        Handles.color = savedColor;
    }

        /// <summary>
        /// Draw Handles sphere at pos with color.
        /// </summary>
        /// <param name="pos">Location of the sphere.</param>
        /// <param name="color">Color of the sphere. Previous handles color will be preserved.</param>
        /// <param name="size">Size of the sphere. Default is 0.1f.</param>
        public static void DrawSphere( Vector3 pos, Color color, float size = 0.1f)
        {
            // Preserve current handles color.
            Color savedColor = Handles.color;
            Handles.color = color;
            
            DrawSphere( pos, size );
            
            // Restore previous handles color.
            Handles.color = savedColor;
        }

        /// <summary>
        /// Draw Handles sphere at pos.
        /// </summary>
        /// <param name="pos">Location of the sphere.</param>
        /// <param name="size">Size of the sphere. Default is 0.1f.</param>
        public static void DrawSphere( Vector3 pos, float size = 0.1f )
        {
            Handles.SphereHandleCap( -1, pos, Quaternion.identity, size, EventType.Repaint );
        }

        /// <summary>
        /// Draws a 2d grid along the x and z axis. Note that this method does not current support other axis or orientations.
        /// </summary>
        /// <param name="center">Position of the grid.</param>
        /// <param name="worldSize">Size of the grid in world space.</param>
        /// <param name="gridSizeX">Grid will be divided in to this many horizontal sections.</param>
        /// <param name="gridSizeY">Grid will be divided in to this many vertical sections.</param>
        /// <param name="showPlusAndMinusSigns">Show plus and minus signs at edges of grid to help verify orientation.</param>
        public static void Draw2dGrid( Vector3 center, Vector2 worldSize, int gridSizeX, int gridSizeY, bool showPlusAndMinusSigns )
        {
            float nodeWidthX = worldSize.x / gridSizeX;
            float nodeWidthY = worldSize.y / gridSizeY;
            
            float lengthX = gridSizeX * nodeWidthX;
            float lengthY = gridSizeY * nodeWidthY;
            var worldTopLeft = new Vector3( ( -nodeWidthX * gridSizeX ) / 2, 0f, ( nodeWidthY * gridSizeY ) / 2 );
            worldTopLeft += center;
        
            Vector3 horizontal = Vector3.right;
            var vertical = new Vector3( 0, 0, -1 );
            
            // Draw horizontal lines.
            for (int x = 0; x < gridSizeX + 1; x++)
            {
                DrawGridLine( x, nodeWidthX, horizontal, vertical, lengthY );
            }
            
            // Draw vertical lines.
            for (int y = 0; y < gridSizeY + 1; y++)
            {
                DrawGridLine( y, nodeWidthY, vertical, horizontal, lengthX );
            }
            
            if (showPlusAndMinusSigns)
                DrawPlusAndMinusSigns( worldTopLeft, lengthX, lengthY );

            void DrawGridLine( int currentLine, float nodeWidth, 
                Vector3 parallel, Vector3 perpendicular, float perpendicularScale )
            {
                float parallelScale = currentLine * nodeWidth;
                Vector3 lineStart = worldTopLeft + ( parallel * parallelScale );
                Vector3 lineEnd = lineStart + ( perpendicular * perpendicularScale );
                Handles.DrawAAPolyLine( 2f, lineStart, lineEnd );
            }
        }
                
        /// <summary>
        /// Draws plus and minus sign symbols at the appropriate edges of the grid. Defaults colors are blue for z and red for x.
        /// </summary>
        /// <param name="worldTopLeft">Top left edge of the grid.</param>
        /// <param name="lengthX">Length of the grid along the x axis.</param>
        /// <param name="lengthY">Length of the grid along the y axis.</param>
        /// <param name="edgePadding">World space distance between the edge of the grid and the center of the symbols.</param>
        /// <param name="scale">Size of the symbols.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame rectangles in the symbols.</param>
        public static void DrawPlusAndMinusSigns( Vector3 worldTopLeft, float lengthX, float lengthY, float edgePadding = 0.2f, 
            float scale = 0.5f, float lineThickness = 3f )
        {
            DrawPlusAndMinusSigns( worldTopLeft, lengthX, lengthY, Color.red, Color.blue, edgePadding, scale, lineThickness );
        }

        /// <summary>
        /// Draws plus and minus sign symbols at the appropriate edges of the grid. Defaults colors are blue for z and red for x.
        /// </summary>
        /// <param name="worldTopLeft">Top left edge of the grid.</param>
        /// <param name="lengthX">Length of the grid along the x axis.</param>
        /// <param name="lengthY">Length of the grid along the y axis.</param>
        /// <param name="xAxisColor">Color used for the x axis symbols.</param>
        /// <param name="yAxisColor">Color used for the y axis symbols.</param>
        /// <param name="edgePadding">World space distance between the edge of the grid and the center of the symbols.</param>
        /// <param name="scale">Size of the symbols.</param>
        /// <param name="lineThickness">Thickness of the lines used to draw the wire frame rectangles in the symbols.</param>
        public static void DrawPlusAndMinusSigns( Vector3 worldTopLeft, float lengthX, float lengthY, 
            Color xAxisColor, Color yAxisColor, float edgePadding = 0.2f, float scale = 0.5f, float lineThickness = 3f )
        {
            Vector3 plusSignY = worldTopLeft + new Vector3( ( lengthX / 2 ), 0 , edgePadding );
            DrawPlusSign( plusSignY, scale, lineThickness, yAxisColor );
            
            Vector3 minusSignY = worldTopLeft + new Vector3( ( lengthX / 2 ), 0 , -lengthY - edgePadding );
            DrawMinusSign( minusSignY, scale, lineThickness, yAxisColor );

            Vector3 plusSignX = worldTopLeft + new Vector3( ( lengthX + edgePadding ), 0, ( -lengthY / 2 ) );
            DrawPlusSign( plusSignX, scale, lineThickness, xAxisColor );
            
            Vector3 minusSignX = worldTopLeft + new Vector3( -edgePadding, 0, ( -lengthY / 2 ) );
            DrawMinusSign( minusSignX, scale, lineThickness, xAxisColor );
        }

        /// <summary>
        /// Draws a plus sign from two 2D wire frame rectangles.
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
        /// Draws a minus sign from a 2D wire frame rectangle.
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
        /// Draw a 2D wire frame rectangle.
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

            float halfWidth = ( width / 2 );
            float halfHeight = ( height / 2 );
            
            Vector3 topLeft = center + new Vector3( -halfWidth, 0, halfHeight);
            Vector3 bottomLeft = center + new Vector3( -halfWidth, 0, -halfHeight);
            Vector3 topRight = center + new Vector3( halfWidth, 0, halfHeight);
            Vector3 bottomRight = center + new Vector3( halfWidth, 0, -halfHeight);

            Handles.DrawAAPolyLine( lineThickness, topLeft, topRight );
            Handles.DrawAAPolyLine( lineThickness, topRight, bottomRight );
            Handles.DrawAAPolyLine( lineThickness, bottomRight, bottomLeft );
            Handles.DrawAAPolyLine( lineThickness, bottomLeft, topLeft );

            Handles.color = storedColor;
        }

        /// <summary>
        /// Draws a sphere at the center of each cell in a 2D grid.
        /// </summary>
        /// <param name="worldSize">Size of the grid in world space.</param>
        /// <param name="gridSizeX">Grid will be divided in to this many horizontal sections.</param>
        /// <param name="gridSizeY">Grid will be divided in to this many vertical sections.</param>
        public static void Draw2dGridNodes( Vector2 worldSize, int gridSizeX, int gridSizeY )
        {
            float nodeWidthX = worldSize.x / gridSizeX;
            float nodeWidthY = worldSize.y / gridSizeY;
            
            var worldTopLeft = new Vector3( ( -nodeWidthX * gridSizeX ) / 2, 0f, ( nodeWidthY * gridSizeY ) / 2 );
            var shiftToCenter = new Vector3( ( nodeWidthX / 2), 0f, ( -nodeWidthY / 2 ) );
            worldTopLeft += shiftToCenter;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    var nodePos = new Vector3( ( nodeWidthX * x ), 0f, ( -nodeWidthY * y ) );
                    DrawSphere( worldTopLeft + nodePos, Color.yellow );
                }
            }
        }
    }
}
