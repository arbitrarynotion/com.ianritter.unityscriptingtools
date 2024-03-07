using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.EditorWindows.SerializedPropertyExplorer;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.ExtensionMethods;
using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.Enums;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Editor;
using Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime;
using Packages.com.ianritter.unityscriptingtools.Tools.PrefabSpawner.Scripts.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Editor
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
#region DataMembers

        // Target
        private ObjectStack _objectStack;

        // Serialized Properties
        private SerializedProperty _totalObjectsProp;
        private SerializedProperty _settingsSOProp;
        private SerializedProperty _noiseSettingsSOProp;
        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;

        // This is a setting from within the settingsProp.
        // private SerializedProperty _showNoiseMeterProp;

        // Foldout bools
        private bool _showObjectStackerSettings = true;
        private bool _showNoiseSettings = true;

        // UI Formatting
        // private const float VerticalSeparator = 8f;
        private const float ParentFrameWidth = 2f;
        private const float ChildFrameWidth = 2f;
        // private const float FoldoutFramePadding = 2f;
        private const float EditorFrameBottomPadding = 10f;
        private const ElementFrameType FoldoutFrameType = ElementFrameType.LeftOnly;
        private const ElementFrameType EditorFrameType = ElementFrameType.PartialLeftFullBottom;

        // Debug
        private FormattedLogger _localLogger;


        // Scene GUI Formatting
        private const float NoiseMapTopMargin = 125f;
        private const float NoiseMapRightMargin = 43f;
        private const float NoiseMapWidth = 25f;
        private const float NoiseMapLabelWidth = 80f;

        private const float NoiseMapLabelRightMargin = 0f;

        // These are used to draw a frame around the editor area.
        private float _editorStartPosition;
        private float _editorEndPosition;

        // Embedded Editor
        private UnityEditor.Editor _settingsSOEditor;
        private Object _settingsSOEditorTarget;

        private UnityEditor.Editor _noiseSettingsSOEditor;
        private Object _noiseSettingsSOEditorTarget;

        private EmbedSOEditor _settingsEmbeddedSOEditor;
        private EmbedSOEditor _noiseSettingsEmbeddedSOEditor;

#endregion


#region UnityActions

        private static UnityAction _onRecompile;
        private static void RaiseOnRecompile() => _onRecompile?.Invoke();

#endregion


#region LifeCycle

        protected override void OnEnableFirst()
        {
            if( _localLogger == null )
                _localLogger = FormattedLoggerLoader.GetLogger( ObjectStackerELoggerName, DefaultLoggerPath );

            _localLogger.LogStart();

            _objectStack = target as ObjectStack;

            // Get the so prop.
            _totalObjectsProp = serializedObject.FindProperty( "totalObjects" );

            _settingsSOProp = serializedObject.FindProperty( "settingsSo" );
            _settingsEmbeddedSOEditor = new EmbedSOEditor( _settingsSOProp );
            _noiseSettingsSOProp = serializedObject.FindProperty( "noiseSettingsSO" );
            _noiseSettingsEmbeddedSOEditor = new EmbedSOEditor( _noiseSettingsSOProp );

            // _settingsSOProp.PrintSerializedPropertyInfo();

            // _settingsSerializedObject = new SerializedObject( _settingsSOProp.objectReferenceValue );
            // _showNoiseMeterProp = _settingsSerializedObject.FindProperty( nameof( ObjectStackerSettingsSO.showNoiseMeter ) );
            // _showNoiseMeterProp.PrintSerializedPropertyInfo();
            // _localLogger.LogObjectAssignmentResult( nameof(_showNoiseMeterProp ), _showNoiseMeterProp == null, FormattedLogType.Standard );

            _sceneViewVisualsModeProp = serializedObject.FindProperty( "sceneViewVisualsMode" );
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );

            // SettingsSOEditorIsValid();
            _settingsEmbeddedSOEditor.OnEnable();
            _noiseSettingsEmbeddedSOEditor.OnEnable();

            _onRecompile += OnScriptsLoaded;

            _localLogger.LogEnd();
        }

        protected override void OnDisableLast()
        {
            _localLogger.LogStart();

            // DoEditorCleanup();
            _settingsEmbeddedSOEditor.OnDisable();

            _onRecompile -= OnScriptsLoaded;

            _localLogger.LogEnd();
        }

        protected override void OnInspectorGUIFirst()
        {
            serializedObject.Update();

            serializedObject.DrawScriptField();

            LabelField( "Stacker Settings", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                PropertyField( _totalObjectsProp );

                PropertyField( _noiseSettingsSOProp );
                DrawSettingsSoInspector( ref _showNoiseSettings, "Noise Settings", _noiseSettingsEmbeddedSOEditor );
                if ( _noiseSettingsSOProp.objectReferenceValue != null && _showNoiseSettings )
                    Space( VerticalSeparator );

                PropertyField( _settingsSOProp );
                DrawSettingsSoInspector( ref _showObjectStackerSettings, "Object Stacker Settings", _settingsEmbeddedSOEditor );
                if( _settingsSOProp.objectReferenceValue != null && _showObjectStackerSettings )
                    Space( VerticalSeparator );
            }
            EditorGUI.indentLevel--;

            LabelField( "Visualizations", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                PropertyField( _sceneViewVisualsModeProp, new GUIContent( "Visualization Mode" ) );
                PropertyField( _prefabProp, new GUIContent( "Model Mode Prefab" ) );
            }
            EditorGUI.indentLevel--;

            LabelField( "Debug", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                PropertyField( _loggerProp );
            }
            EditorGUI.indentLevel--;

            if( !serializedObject.ApplyModifiedProperties() ) return;

            _localLogger.Log( "Change in Settings SO detected. Repainting Scene Views." );
            SceneView.RepaintAll();
        }

        private static void DrawSettingsSoInspector( ref bool toggle, string title, EmbedSOEditor editor )
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !editor.SettingsSOEditorIsValid() )
            {
                HelpBox( "Populate the object field to display the object's settings.", MessageType.Info );
                return;
            }

            // Draw Foldout with frame.
            // EditorGUI.indentLevel++;
            // Rect labelRect = GetFramedControlRect( Color.gray, FoldoutFrameType, 0f, true );
            // toggle = EditorGUI.Foldout( labelRect, toggle, title, true );
            // if ( !toggle ) return;
            toggle = DrawFoldoutSection( title, FoldoutFrameType, toggle );
            if( !toggle ) return;

            // EditorGUI.indentLevel--;


            // Space( VerticalSeparator );

            Rect foldoutFrameRect = editor.DrawSettingsSoInspector();
            foldoutFrameRect.xMin += ParentFrameWidth;
            foldoutFrameRect.yMax += EditorFrameBottomPadding;
            DrawRect( foldoutFrameRect, EditorFrameType, Color.gray, Color.gray, ChildFrameWidth, false );

            Space( VerticalSeparator );
        }
        
        protected override void DuringSceneGUILast()
        {
            // This solved the problem of the scene view not repainting until the next frame.
            // _localLogger.Log( "DuringSceneGUILast was called. Repainting scene views." );
            SceneView.RepaintAll();
        }

        protected void OnSceneGUI()
        {
            if( _objectStack == null ) return;

            if( _objectStack.GetSettingsSO() == null ) return;

            // Todo: Directly referencing the value is ugly. Should find a clean way with serialized properties. This works for now.
            if( !_objectStack.GetSettingsSO().showNoiseMeter ) return;

            DrawSceneViewNoiseMapPreview();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // RaiseOnRecompile();
        }

        private void OnScriptsLoaded()
        {
            // _objectStack.UpdateSettingsSoSubscriptions();
            _objectStack.UpdateSubscriptions();
        }

#endregion


#region EmbeddedEditor

        // /// <summary>
        // ///     Returns true if the editor variable holds a usable editor.
        // /// </summary>
        // private bool SettingsSOEditorIsValid()
        // {
        //     // Case 1: settingsProp is null
        //     //     - return false.
        //     // Ensure the settings property was found.
        //     if ( _settingsSOProp == null ) return false;
        //
        //     // Case 2: settingsProp.objectValue is null
        //     //     - return false.
        //     // Ensure that the object reference is not null.
        //     if ( _settingsSOProp.objectReferenceValue == null ) return false;
        //
        //     // Case 3: editor and target exist.
        //     //     When the target holds an object reference and the editor exists, nothing needs to be done.
        //     //     - return true.
        //     // Skip if we've already made an editor for this object.
        //     if ( _settingsSOEditor != null && _settingsSOEditorTarget == _settingsSOProp.objectReferenceValue ) return true;
        //
        //     // Case 4:  either the editor or the target are null.
        //     //     This means the a target change has occurred.
        //     //     - Save the object reference in the target.
        //     //     - Dispose of the old editor, if one exists.
        //     //     - Create a new editor for this object.
        //     //     - return true.
        //     _settingsSOEditorTarget = _settingsSOProp.objectReferenceValue;
        //
        //     _localLogger.Log( $"Creating a new editor for {GetColoredStringGreenYellow( _settingsSOProp.objectReferenceValue.name )}." );
        //     DoEditorCleanup();
        //
        //     // Create an editor for the so.
        //     _settingsSOEditor = CreateEditor( _settingsSOProp.objectReferenceValue );
        //     return true;
        // }
        //
        // private void DoEditorCleanup()
        // {
        //     if ( _settingsSOEditor == null ) return;
        //
        //     _localLogger.Log( $"Destroying old editor for {GetColoredStringMaroon( _settingsSOProp.objectReferenceValue.name )}." );
        //     DestroyImmediate( _settingsSOEditor );
        // }
        //
        // private void DrawSettingsSoInspector()
        // {
        //     // Optimization to avoid creating a new editor unless it's actually needed.
        //     if ( !SettingsSOEditorIsValid() ) return;
        //
        //     Space( VerticalSeparator );
        //
        //     // Draw Foldout with frame.
        //     Rect labelRect = GetFramedControlRect( Color.gray, FoldoutFrameType, 0f, true );
        //     _showObjectStackerSettings = EditorGUI.Foldout( labelRect, _showObjectStackerSettings, "Object Stacker Settings", true );
        //     if ( !_showObjectStackerSettings ) return;
        //
        //     Rect startRect = GUILayoutUtility.GetLastRect();
        //     _settingsSOEditor.OnInspectorGUI();
        //     Rect endRect = GUILayoutUtility.GetLastRect();
        //
        //     DrawSOFrame( startRect, endRect );
        //
        //     Space( VerticalSeparator );
        // }
        //
        // private static void DrawSOFrame( Rect startRect, Rect endRect )
        // {
        //     var finalRect = new Rect( startRect )
        //     {
        //         yMin = startRect.yMax,
        //         yMax = endRect.yMax + EditorFrameBottomPadding
        //     };
        //     finalRect.xMin += ParentFrameWidth;
        //     DrawRect( finalRect, EditorFrameType, Color.gray, Color.gray, ChildFrameWidth, false );
        // }

#endregion


#region SceneViewNoiseMapPreview

        /// <summary>
        ///     Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
        ///     left side that indicate where the objects are sampling the noise.
        /// </summary>
        private void DrawSceneViewNoiseMapPreview()
        {
            if( _noiseSettingsSOProp.objectReferenceValue == null ) return;

            Rect currentViewPortRect = Camera.current.pixelRect;
            float previewImageHeight = currentViewPortRect.height / 2f;
            float value = 0.25f;
            var outlineColor = new Color( value, value, value );

            value = 0.15f;
            var backgroundColor = new Color( value, value, value );

            var positionVector = new Vector2(
                currentViewPortRect.width - NoiseMapRightMargin - NoiseMapWidth,

                // NoiseMapTopMargin + NoiseMapRightMargin + 35f
                NoiseMapTopMargin
            );
            var sizeVector = new Vector2(
                NoiseMapWidth,
                previewImageHeight
            );
            var positionRect = new Rect( positionVector, sizeVector );

            DrawRectOutline( positionRect, Color.black );

            NoiseEditorUtilities.DrawSceneViewNoiseMapPreview(
                positionRect,
                _objectStack.GetNoiseMap2D(),
                Color.black,
                Color.white,
                _totalObjectsProp.intValue,
                outlineColor,
                2f,
                ScaleMode.StretchToFill
            );

            var labelPositionVector = new Vector2(
                currentViewPortRect.width - NoiseMapLabelWidth + NoiseMapLabelRightMargin,

                // NoiseMapTopMargin + singleLineHeight + standardVerticalSpacing
                NoiseMapTopMargin + previewImageHeight / 2f
            );
            var labelSizeRect = new Vector2(
                NoiseMapLabelWidth,
                EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
            );
            var labelRect = new Rect( labelPositionVector, labelSizeRect );

            Handles.BeginGUI();
            {
                GUIUtility.RotateAroundPivot( 90, labelRect.center );
                DrawSolidRectWithOutline( labelRect, outlineColor, backgroundColor, 2f );
                GUI.Label( labelRect, " Noise Meter" );
                GUIUtility.RotateAroundPivot( -90, labelRect.center );
            }
            Handles.EndGUI();
        }

#endregion
    }
}