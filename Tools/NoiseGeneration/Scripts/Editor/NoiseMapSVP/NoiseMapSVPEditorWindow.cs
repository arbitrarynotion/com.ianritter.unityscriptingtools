using System;
using System.IO;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor.NoiseMapSVP {
    public class NoiseMapSVPEditorWindow : EditorWindow
    {
        /* Todo: Convert the Noise Map Scene View Preview component to an editor window.
            This will involve first combining the MonoBehaviour and its editor, then reworking the code to 
            locate all NoiseModule components and draw the noise meter only for the one attached to the selected object.
         
            - Create editor window class that can be opened from the Tools menu.
            - Convert the NMSVP class into a scriptable object.
            - Use an embedded SO editor to draw the SO's settings in the editor window.
            - Gather a list of all NoiseModule components in the scene during on enable.
                - Subscribe to all NoiseModule components with a method that sets the map scale but only if the
                    noise module that was updated is a component on the currently selected object.
            - Provide a 2D noise map on demand but only for the noise module on the currently selected object.
         */
        
        [MenuItem( NoiseMapSVPMenuTitle )]
        private static void Init()
        {
            var window = (NoiseMapSVPEditorWindow) GetWindow( typeof( NoiseMapSVPEditorWindow ) );
            window.titleContent = new GUIContent( NoiseMapSVPWindowTitle );
            window.Show();
        }

#region DataMembers
        
        /* Creat asset example:
         
        [UnityEditor.MenuItem("Assets/Create/UI Toolkit/Text Settings", false)]
        public static void CreateUITKTextSettingsAsset()
        {
          string path;
          if (Selection.assetGUIDs.Length == 0)
          {
            path = "Assets";
          }
          else
          {
            path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            if (Path.GetExtension(path) != "")
              path = Path.GetDirectoryName(path);
          }
          string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/UITK Text Settings.asset");
          PanelTextSettings instance = ScriptableObject.CreateInstance<PanelTextSettings>();
          AssetDatabase.CreateAsset((Object) instance, uniqueAssetPath);
          EditorUtility.SetDirty((Object) instance);
          AssetDatabase.SaveAssets();
          EditorUtility.FocusProjectWindow();
          EditorGUIUtility.PingObject((Object) instance);
        }
         
         */

        private NoiseMapSVPData NoiseMapSvpDataSO
        {
            get
            {
                // Only one data SO should exist. Return it if it's already been instantiated, otherwise create it.
                if( _noiseMapSvpDataSO == null )
                {
                    // Instantiate the scriptable object.
                    // NoiseMapSVPData instance = CreateInstance<NoiseMapSVPData>();
                    
                    // Todo: the instance doesn't persist unless it is manually saved into an asset on disk. Below is ChatGPT's rough instructions on how to do that.
                    var instance = CreateInstance<NoiseMapSVPData>();
                    
                    // Choose a path and filename for the asset
                    string path = NoiseMapSVPDataPath;
                    
                    // Make sure the path is unique so it doesn't overwrite any existing asset
                    int uniqueID = 0;
                    while (File.Exists(path))
                    {
                        uniqueID++;
                        path = NoiseMapSVPDataPath + uniqueID + ".asset";
                    }
                    
                    // Save the ScriptableObject asset to the specified path
                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return _noiseMapSvpDataSO;
            }
        }
        private NoiseMapSVPData _noiseMapSvpDataSO;

        private EmbedSOEditor _noiseMapSVPDataEmbeddedEditor;

        private SerializedObject _serializedObject;
        private SerializedProperty _noiseMapSVPDataSOProp;

#endregion

        
#region FoldoutToggles

        private bool NoiseMapSVPSOToggle
        {
            get => EditorPrefs.GetBool( $"{GetInstanceID()}_{nameof( NoiseMapSVPSOToggle )}", true );
            set => EditorPrefs.SetBool( $"{GetInstanceID()}_{nameof( NoiseMapSVPSOToggle )}", value );
        }


#endregion
        

#region LifeCycle

        private void OnEnable()
        {
            // Initialize the embedded SO editor.
            LoadObjectStackProperties();
            InitializeEmbeddedEditors();    
            
            // - Gather a list of all NoiseModule components in the scene during on enable.
            //     - Subscribe to all NoiseModule components with a method that sets the map scale but only if the
            //        noise module that was updated is a component on the currently selected object.
            
            
        }

        private void OnDisable()
        {
            _noiseMapSVPDataEmbeddedEditor.OnDisable();
        }

        private void OnGUI()
        {
            DrawSettingsSOSection();
        }

#endregion
        
        
#region Initialization

        private void LoadDataSO()
        {
            if( _noiseMapSVPDataSOProp == null ) return;

            if( _noiseMapSVPDataSOProp.objectReferenceValue != null ) return;
            
            // Get data so, save it into data prop's object reference, apply properties.
        }

        private void InitializeEmbeddedEditors()
        {
            _noiseMapSVPDataEmbeddedEditor.OnEnable();
        }

        private void LoadObjectStackProperties()
        {
            _serializedObject = new SerializedObject( this );
            _noiseMapSVPDataSOProp = _serializedObject.FindProperty( nameof(_noiseMapSvpDataSO) );
            _noiseMapSVPDataEmbeddedEditor = new EmbedSOEditor( _noiseMapSVPDataSOProp );
        }

#endregion


#region DrawEditorWindow

        private void DrawSettingsSOSection()
        {
            PropertyField( _noiseMapSVPDataSOProp );
            // Draw the embedded editor.
            _noiseMapSVPDataEmbeddedEditor.DrawSettingsSoInspector( "Noise Map SVP Settings" );
            if( NoiseMapSVPSOToggle )
                Space( VerticalSeparator );
        }

#endregion
    }
}
