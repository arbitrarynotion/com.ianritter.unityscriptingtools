using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.GUIGraphics
{
    /// <summary>
    ///     This class is for drawing graphics directly into the scene view via SpawnPrefabsInSceneView.
    ///     Note that these use Handles, not Gizmos so don't try to use them in OnDrawGizmos.
    /// </summary>
    public static class SceneGUIHandles
    {
        /// <summary>
        ///     Draw Handles sphere at pos with color.
        /// </summary>
        /// <param name="pos">Location of the sphere.</param>
        /// <param name="color">Color of the sphere. Previous handles color will be preserved.</param>
        /// <param name="size">Size of the sphere. Default is 0.1f.</param>
        [UsedImplicitly]
        public static void DrawSphere( Vector3 pos, Color color, float size = 0.1f )
        {
            // Preserve current handles color.
            Color savedColor = Handles.color;
            Handles.color = color;

            DrawSphere( pos, size );

            // Restore previous handles color.
            Handles.color = savedColor;
        }
        
        /// <summary>
        ///     Draw Handles sphere at pos.
        /// </summary>
        /// <param name="pos">Location of the sphere.</param>
        /// <param name="size">Size of the sphere. Default is 0.1f.</param>
        [UsedImplicitly]
        public static void DrawSphere( Vector3 pos, float size = 0.1f )
        {
            Handles.SphereHandleCap( 0, pos, Quaternion.identity, size, EventType.Repaint );
        }
    }
}