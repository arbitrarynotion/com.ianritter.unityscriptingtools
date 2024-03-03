using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime._Testing.UnityOfficialExamples;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor._Testing.UnityOfficialExamples
{
    [CustomEditor( typeof( DrawLine ) )]
    public class DrawLineEditor : UnityEditor.Editor
    {
        // draw lines between a chosen game object
        // and a selection of added game objects

        private void OnSceneGUI()
        {
            // get the chosen game object
            var t = target as DrawLine;

            if ( t == null || t.GameObjects == null )
                return;

            // grab the center of the parent
            Vector3 center = t.transform.position;

            // iterate over game objects added to the array...
            for (int i = 0; i < t.GameObjects.Length; i++)
            {
                // ... and draw a line between them
                if ( t.GameObjects[i] != null )
                    Handles.DrawLine( center, t.GameObjects[i].transform.position );
            }
        }
    }
}