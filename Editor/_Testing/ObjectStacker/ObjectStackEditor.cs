using Packages.com.ianritter.unityscriptingtools.Editor._Testing.PrefabSpawner;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.EditorDividerGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.ObjectStacker
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
        private SerializedProperty _totalObjectsProp;
        private SerializedProperty _settingsProp;
        private SerializedProperty _showWireFrame;
        private SerializedProperty _showPreviewProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;
        private UnityEditor.Editor _editor;
        private bool _showSoSettings = true;

        private const float VerticalSeparator = 4f;
        private const int OutlineLineWidth = 2;
        private const float FoldoutFramePadding = 3f;

        private CustomLogger _localLogger;
        
        protected override void OnEnableFirst()
        {
            if ( _localLogger == null )
                _localLogger = CustomLoggerLoader.GetLogger( "ObjectStackerEditorLogger", "Assets/_Testing/ScriptableObjects/Loggers/" );

            _localLogger.LogStart();
            
            // Get the so prop.
            _totalObjectsProp = serializedObject.FindProperty( "totalObjects" );
            _settingsProp = serializedObject.FindProperty( "settingsSo" );
            _showWireFrame = serializedObject.FindProperty( "showWireFrames" );
            _showPreviewProp = serializedObject.FindProperty( "showPreview" );
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );

            _localLogger.LogEnd();
        }

        protected override void OnDisableLast()
        {
            if ( _editor != null )
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

            // EditorGUILayout.Space( VerticalSeparator );
            
            EditorGUILayout.LabelField( "Debug", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _showWireFrame );
                EditorGUILayout.PropertyField( _showPreviewProp );
                EditorGUILayout.PropertyField( _prefabProp );
                EditorGUILayout.PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSettingsSoInspector()
        {
            if ( _settingsProp == null ) return;

            // Make sure that the object reference is not null.
            if ( _settingsProp.objectReferenceValue == null ) return;
            
            // Create an editor for the so.
            _editor = CreateEditor( _settingsProp.objectReferenceValue );

            if (_editor == null ) return;

            EditorGUILayout.Space( VerticalSeparator );

            Rect labelRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight + ( 2 * FoldoutFramePadding ) );
            DrawRectOutline( labelRect, Color.gray, OutlineLineWidth );
            _showSoSettings = EditorGUI.Foldout( labelRect, _showSoSettings, "Object Stacker Settings", true );
            if ( _showSoSettings )
            {
                // labelRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + FoldoutFramePadding;
                // DrawSOFrame( labelRect );
                
                _editor.OnInspectorGUI();
                DrawDivider( Color.gray, OutlineLineWidth, 5, 5, 4, 0 );
            }
        }

        private void DrawSOFrame( Rect startingRect )
        {
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
