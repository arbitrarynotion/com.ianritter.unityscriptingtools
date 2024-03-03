using UnityEditor;
using UnityEngine;


// Found this basis for this online, credit: BrodyB at https://gist.github.com/BrodyB/bb8894a88a0ae2b385bddf5bc9232f30.
namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    /// <summary>
    ///     This window shows how you can listen for and consume user input events
    ///     from the Scene View. Super useful for making editor tools!
    /// </summary>
    public class SceneViewControlWindow : EditorWindow
    {
        /// <summary>
        ///     Open the window
        /// </summary>
        [MenuItem( "Window/Scene View Control" )]
        private static void Open()
        {
            var win = GetWindow<SceneViewControlWindow>();
            win.titleContent = new GUIContent( "Scene View Control" );
            win.Show();
        }

        /// <summary>
        ///     When we open the window, subscribe to the SceneView's input event
        /// </summary>
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        /// <summary>
        ///     When we close, unsubscribe from the SceneView
        /// </summary>
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox( "While this window is open, only scene objects with Colliders can be selected in the Scene View!", MessageType.Info );
        }


        /// <summary>
        ///     Draw or do input/handle overriding in the scene view
        /// </summary>
        private void OnSceneGUI( SceneView sceneView )
        {
            // Event.current houses information on scene view input this cycle
            Event current = Event.current;

            switch (current.type)
            {
                // If user has pressed the Left Mouse Button, do something and
                // swallow it so nothing else hears the event
                case EventType.MouseDown when current.button == 0:
                {
                    // While this tool is open, only allow the user to select scene
                    // objects with a Collider component on them
                    if ( !Select<Collider>( current ) )
                    {
                        // If nothing with Collider found, unselect everything
                        Selection.activeGameObject = null;
                    }

                    break;
                }
                
                // After you've done all your custom event interpreting and swallowing,
                // you have to call this code to make sure swallowed events don't bleed out.
                // Not sure why, but that's the rules.
                case EventType.Layout:
                    HandleUtility.AddDefaultControl( GUIUtility.GetControlID( GetHashCode(), FocusType.Passive ) );
                    break;
            }
        }


        /// <summary>
        ///     When user attempts to select an object, this sees if they selected an
        ///     object with the given component. This will swallow the event and select
        ///     the object if successful.
        /// </summary>
        /// <param name="e">Event from OnSceneGUI</param>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Returns the object</returns>
        public static GameObject Select<T>( Event e ) where T : Component
        {
            // Abort is there is not camera.
            if ( Camera.current == null ) return null;
            
            Ray ray = HandleUtility.GUIPointToWorldRay( e.mousePosition );

            // Abort if the raycast fails to hit anything.
            if ( !Physics.Raycast( ray, out RaycastHit hit ) ) return null;

            // Abort if what the raycast hits has no collider (technically not possible).
            if ( hit.collider == null ) return null;
            
            GameObject gameObj = hit.collider.gameObject;
            
            // Finally, bail if the object the raycast hit doesn't have the component we're looking for.
            if ( gameObj.GetComponent<T>() == null ) return null;
            
            // Consume the event.
            e.Use();
            
            // Select the object.
            Selection.activeGameObject = gameObj;
            
            return gameObj;
        }
    }
}