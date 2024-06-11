using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime._Testing.UnityOfficialExamples;

using UnityEditor;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor._Testing.UnityOfficialExamples
{
    [CustomEditor( typeof( WaypointPath ) )]
    public class WaypointPathEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor currentTransformEditor;
        private int index;
        private WaypointPath myWayPath;
        private string[] optionsList;
        private Transform selectedTransform;

        private void GetWaypoints()
        {
            myWayPath = target as WaypointPath;

            if( myWayPath.wayPointArray != null )
            {
                optionsList = new string[myWayPath.wayPointArray.Length];

                for ( int i = 0; i < optionsList.Length; i++ )
                {
                    Transform wayPoint = myWayPath.wayPointArray[i];

                    if( wayPoint != null )
                        optionsList[i] = wayPoint.name;
                    else
                        optionsList[i] = $"Empty waypoint {i + 1}";
                }
            }
        }

        public override void OnInspectorGUI()
        {
            GetWaypoints();
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();

            if( optionsList != null )
                index = EditorGUILayout.Popup( "Select Waypoint", index, optionsList );

            if( EditorGUI.EndChangeCheck() )
            {
                UnityEditor.Editor tmpEditor = null;

                if( index < myWayPath.wayPointArray.Length )
                {
                    selectedTransform = myWayPath.wayPointArray[index];

                    //Creates an Editor for selected Component from a Popup
                    tmpEditor = CreateEditor( selectedTransform );
                }
                else
                    selectedTransform = null;

                // If there isn't a Transform currently selected then destroy the existing editor
                if( currentTransformEditor != null ) DestroyImmediate( currentTransformEditor );

                currentTransformEditor = tmpEditor;
            }

            // Shows the created Editor beneath CustomEditor
            if( currentTransformEditor != null &&
                selectedTransform != null ) currentTransformEditor.OnInspectorGUI();
        }
    }
}