using UnityEditor;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Graphics.Scene
{
    public static class VectorArrowSceneGUIGraphics
    {
#region Helpers

        private static void DrawPolyArrow
        (
            Vector3 arrowOrigin,
            Vector3 arrowDestination,
            Color color,
            Vector3 rightAxis,
            Vector3 upAxis,
            float lineThickness,
            bool flipArrowheadAxis,
            float arrowheadScale = 0.1f,
            bool scaleArrowheadWithLength = true
        )
        {
            // Using the camera up is another approach but it doesn't seem to be a marked improvement
            // over the current method.
            // Camera cam = SceneView.currentDrawingSceneView.camera;
            // if ( cam == null )
            // {
            //     return;
            // }
            //
            // upAxis = cam.transform.up;
            Vector3 arrowDirection = arrowOrigin - arrowDestination;
            Vector3 arrowheadAxis = flipArrowheadAxis ? rightAxis : upAxis;

            float arrowheadWidth = scaleArrowheadWithLength ? arrowheadScale * arrowDirection.magnitude : arrowheadScale;
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

#endregion

#region Arrows

        public static void DrawAxes( Transform transform, float thickness = 3f, bool flip = false )
        {
            Vector3 position = transform.position;
            DrawAAPolyArrow( position, position + transform.up, transform.right, Color.green, thickness, flip );
            DrawAAPolyArrow( position, position + transform.right, transform.up, Color.red, thickness, flip );
            DrawAAPolyArrow( position, position + transform.forward, transform.right, Color.blue, thickness, !flip );
        }

        /// <summary>
        ///     Draw the x, y, and z axis arrows at Vector3.zero of the provided local matrix localToWorldMtx.
        /// </summary>
        /// <param name="localToWorldMtx"></param>
        public static void DrawAxes( Matrix4x4 localToWorldMtx )
        {
            Vector3 origin = localToWorldMtx.MultiplyPoint3x4( Vector3.zero );
            Vector3 up = localToWorldMtx.MultiplyVector( Vector3.up );
            Vector3 right = localToWorldMtx.MultiplyVector( Vector3.right );
            Vector3 forward = localToWorldMtx.MultiplyVector( Vector3.forward );

            DrawAAPolyArrow( localToWorldMtx, origin, origin + up, Color.green, 3f, true );
            DrawAAPolyArrow( localToWorldMtx, origin, origin + right, Color.red );
            DrawAAPolyArrow( localToWorldMtx, origin, origin + forward, Color.blue, 3f, true );
        }

        /// <summary>
        ///     Draw Anti-aliased arrow. LocalToWorldMatrix is used to ensure arrowhead lines up with custom axes.
        /// </summary>
        /// <param name="localToWorldMtx">Local space matrix.</param>
        /// <param name="arrowOrigin">Position of the arrow base.</param>
        /// <param name="arrowDestination">Direction the arrow is pointing in (not including local origin offset)</param>
        /// <param name="color"></param>
        /// <param name="lineThickness">Thickness of the lines used to draw the arrow.</param>
        /// <param name="flipArrowheadAxis">Flips the arrowhead's plane from the x axis to the z axis.</param>
        /// <param name="arrowheadScale">Size of the arrowhead will be this percent of the arrow's length.</param>
        /// <param name="scaleArrowheadWithLength">Arrowhead size will increase/decrease with the arrow length.</param>
        public static void DrawAAPolyArrow
        (
            Matrix4x4 localToWorldMtx,
            Vector3 arrowOrigin,
            Vector3 arrowDestination,
            Color color,
            float lineThickness = 3f,
            bool flipArrowheadAxis = false,
            float arrowheadScale = 0.1f,
            bool scaleArrowheadWithLength = true
        )
        {
            Vector3 upAxis = localToWorldMtx.MultiplyVector( Vector3.up );
            Vector3 rightAxis = localToWorldMtx.MultiplyVector( Vector3.right );

            DrawPolyArrow( arrowOrigin, arrowDestination, color, rightAxis, upAxis,
                lineThickness, flipArrowheadAxis, arrowheadScale, scaleArrowheadWithLength );
        }

        /// <summary>
        ///     Draw Anti-aliased arrow. Arrowhead will be lined up to the cross product of the world up and the arrow direction.
        /// </summary>
        /// <param name="arrowOrigin">Position of the arrow base.</param>
        /// <param name="arrowDestination">Direction the arrow is pointing in (not including local origin offset)</param>
        /// <param name="color"></param>
        /// <param name="up"></param>
        /// <param name="lineThickness">Thickness of the lines used to draw the arrow.</param>
        /// <param name="flipArrowheadAxis">Flips the arrowhead's plane from the x axis to the z axis.</param>
        /// <param name="arrowheadScale">Size of the arrowhead will be this percent of the arrow's length.</param>
        /// <param name="scaleArrowheadWithLength">Arrowhead size will increase/decrease with the arrow length.</param>
        public static void DrawAAPolyArrow
        (
            Vector3 arrowOrigin,
            Vector3 arrowDestination,
            Vector3 up,
            Color color,
            float lineThickness = 3f,
            bool flipArrowheadAxis = false,
            float arrowheadScale = 0.1f,
            bool scaleArrowheadWithLength = true
        )
        {
            Vector3 arrowUp = up;
            Vector3 arrowDirection = ( arrowOrigin - arrowDestination ).normalized;

            Vector3 upAxis = Vector3.Cross( arrowDirection, arrowUp ).normalized;
            Vector3 rightAxis = Vector3.Cross( arrowDirection, upAxis ).normalized;

            DrawPolyArrow(
                arrowOrigin,
                arrowDestination,
                color,
                rightAxis,
                upAxis,
                lineThickness,
                flipArrowheadAxis,
                arrowheadScale,
                scaleArrowheadWithLength
            );
        }

#endregion
    }
}