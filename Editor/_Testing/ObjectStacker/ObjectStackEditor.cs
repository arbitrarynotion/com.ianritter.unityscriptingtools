using UnityEditor;
using UnityEngine;

using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.NoiseMaps.NoiseMap;

using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Editor._Testing.PrefabSpawner;

using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.EditorDividerGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.ObjectStacker
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
        private const float PreviewImageWidth = 9f;
        private const float PreviewImageHeight = 400f;
        private const float PreviewImageMargin = 5f;
        private const float TopMarginOffset = 110f;
        
        private ObjectStack _objectStack;
        
        private SerializedProperty _totalObjectsProp;
        private SerializedProperty _settingsProp;
        
        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _showWireFrameProp;
        private SerializedProperty _showPreviewProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;
        
        
        private SerializedProperty _noiseMapProp;
        
        
        private UnityEditor.Editor _editor;
        private bool _showSoSettings = true;

        private const float VerticalSeparator = 4f;
        private const int OutlineLineWidth = 2;
        private const float FoldoutFramePadding = 3f;

        private CustomLogger _localLogger;

        private Object _editorTarget;

        protected override void OnEnableFirst()
        {
            if ( _localLogger == null )
                _localLogger = CustomLoggerLoader.GetLogger( "ObjectStackerEditorLogger", "Assets/_Testing/ScriptableObjects/Loggers/" );

            _localLogger.LogStart();
            
            _objectStack = target as ObjectStack;

            // Get the so prop.
            _totalObjectsProp = serializedObject.FindProperty( "totalObjects" );
            _settingsProp = serializedObject.FindProperty( "settingsSo" );
            
            _sceneViewVisualsModeProp = serializedObject.FindProperty( "sceneViewVisualsMode" );
            _showWireFrameProp = serializedObject.FindProperty( "showWireFrames" );
            _showPreviewProp = serializedObject.FindProperty( "showPreview" );
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );
            
            _noiseMapProp = serializedObject.FindProperty( "noiseMap" );
            
            SettingsSOEditorIsValid();

            _localLogger.LogEnd();
        }

        /// <summary>
        /// Returns true if the editor variable holds a usable editor.
        /// </summary>
        private bool SettingsSOEditorIsValid()
        {
            // Ensure the settings property was found.
            if ( _settingsProp == null ) return false;
            
            // Ensure that the object reference is not null.
            if ( _settingsProp.objectReferenceValue == null ) return false;

            // Skip if we've already made an editor for this object.
            if ( _editorTarget == _settingsProp.objectReferenceValue ) return true;
            _editorTarget = _settingsProp.objectReferenceValue;
            
            _localLogger.Log( $"Creating a new editor for {GetColoredStringGreenYellow( _settingsProp.objectReferenceValue.name )}." );
            DoEditorCleanup();
            
            // Create an editor for the so.
            _editor = CreateEditor( _settingsProp.objectReferenceValue );
            return true;
        }

        protected override void OnDisableLast()
        {
            _localLogger.LogStart();
            
            DoEditorCleanup();
            
            _localLogger.LogEnd();
        }

        private void DoEditorCleanup()
        {
            if ( _editor == null ) return;
            
            _localLogger.Log( $"Destroying old editor for {GetColoredStringMaroon( _settingsProp.objectReferenceValue.name )}." );
            DestroyImmediate( _editor );
        }
        
        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();

            EditorGUILayout.LabelField( "Stacker Settings", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _totalObjectsProp );
                EditorGUILayout.PropertyField( _settingsProp );
                DrawSettingsSoInspector();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space( VerticalSeparator );
            
            EditorGUILayout.LabelField( "Visualizations", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _sceneViewVisualsModeProp, new GUIContent( "Visualization Mode") );
                EditorGUILayout.PropertyField( _prefabProp, new GUIContent( "Model Mode Prefab" ) );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.LabelField( "Debug", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;
            
            serializedObject.ApplyModifiedProperties();
        }

        
        private void DrawSettingsSoInspector()
        {
            // if ( _settingsProp == null ) return;
            //
            // // Make sure that the object reference is not null.
            // if ( _settingsProp.objectReferenceValue == null ) return;
            //
            // // Create an editor for the so.
            // _editor = CreateEditor( _settingsProp.objectReferenceValue );
            //
            // if (_editor == null ) return;

            // Optimization to avoid creating a new editor unless it's actually needed.
            if ( !SettingsSOEditorIsValid() ) return;

            EditorGUILayout.Space( VerticalSeparator );

            // Draw Foldout with frame.
            Rect labelRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight + ( 2 * FoldoutFramePadding ) );
            DrawRectOutline( labelRect, Color.gray, OutlineLineWidth );
            _showSoSettings = EditorGUI.Foldout( labelRect, _showSoSettings, "Object Stacker Settings", true );
            if ( !_showSoSettings ) return;
            
            // EditorGUI.indentLevel++;
            _editor.OnInspectorGUI();
            // EditorGUI.indentLevel--;

            DrawDivider( Color.gray, OutlineLineWidth, 5, 5, 4, 0 );
        }
        
        protected void OnSceneGUI()
        {
            Handles.BeginGUI();

            // Draw the 2D texture to the screen
            // GUILayout.Label( texture );
            // GUILayout.Box( texture, GUILayout.Width( 100f ), GUILayout.Height( 100f ) );

            
            Texture2D noiseMap2D = MapDisplay.GetNoiseMapTexture( _objectStack.GetNoiseMap2D(), Color.black, Color.white );
            // Texture2D noiseMap2DScaled = MapDisplay.GetNoiseMapTexture( _objectStack.GetNoiseMap2DScaled(), Color.black, Color.white );
            
        
            Rect windowWidth = Camera.current.pixelRect;
            var positionRect = new Vector2( 
                windowWidth.width - PreviewImageMargin - PreviewImageWidth, 
                PreviewImageMargin + TopMarginOffset
                );
            
            // var sizeRect = new Vector2( PreviewImageWidth, PreviewImageHeight );

            float variableHeight = windowWidth.height / 2f;
            var sizeRect = new Vector2( PreviewImageWidth, variableHeight );
            
            
            EditorGUI.DrawPreviewTexture( new Rect( positionRect, sizeRect ), noiseMap2D, null, ScaleMode.ScaleAndCrop, 0.0f, -1 );
            // positionRect.y += PreviewImageHeight + EditorGUIUtility.standardVerticalSpacing;
            // EditorGUI.DrawPreviewTexture( new Rect( positionRect, sizeRect ), noiseMap2DScaled, null, ScaleMode.ScaleToFit, 0.0f, -1 );
            DrawRectOutline( new Rect( positionRect, sizeRect ), Color.black, 2f );

            var nodeRect = new Rect( positionRect, new Vector2( 5, 2 ) );
            nodeRect.x -= 3f;
            // float spacing = PreviewImageHeight / noiseMap2D.height;
            float spacing = variableHeight / noiseMap2D.height;
            nodeRect.y += spacing / 2f;

            for (int x = 0; x < _totalObjectsProp.intValue; x++)
            {
                DrawRectOutline( nodeRect, Color.black );
                nodeRect.y += spacing;
            }
            
            Handles.EndGUI();
        }

        private void DrawSOFrame( Rect startingRect )
        {
            // labelRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + FoldoutFramePadding;
            // DrawSOFrame( labelRect );

            float totalHeight = 0f;
            
            Rect wholeFrameRect = new Rect( startingRect );
            
            Debug.Log( "Summing the property heights:");

            SerializedProperty property = _editor.serializedObject.GetIterator().Copy();
            property.Next( true );
            
            // totalHeight += EditorGUI.GetPropertyHeight( property );
            // Debug.Log( $"    adding property: {property.name}" );
            


            while ( property.Next( false ) )
            {
                if ( property.name.Substring( 0, 2 ).Equals( "m_" ) ) continue;

                var currentHeight = EditorGUI.GetPropertyHeight( property );
                totalHeight += currentHeight;
                
                DrawRectOutline( startingRect, Color.white, 1 );
                startingRect.y += currentHeight + EditorGUIUtility.standardVerticalSpacing;
                
                Debug.Log( $"    adding property: {property.name}" );
            }

            wholeFrameRect.yMax += totalHeight + 
                                   ( 5 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + ( 2 * VerticalSeparator ) );
            
            // var testRect = EditorGUILayout.GetControlRect( false, totalHeight );
            // DrawRectOutline( wholeFrameRect, Color.cyan, 1 );
        }
    }
}
