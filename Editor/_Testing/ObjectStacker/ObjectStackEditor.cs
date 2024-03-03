using UnityEditor;
using UnityEngine;

using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;

using Packages.com.ianritter.unityscriptingtools.Editor.Services;
using Packages.com.ianritter.unityscriptingtools.Editor._Testing.PrefabSpawner;
using Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows.SerializedPropertyExplorerWindow;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using UnityEngine.Events;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.UIEditorOnlyRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Editor.Graphics.UI.EditorDividerGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Editor._Testing.ObjectStacker
{
    [CustomEditor( typeof( ObjectStack ) )]
    public class ObjectStackEditor : PrefabSpawnerEditor
    {
#region DataMembers

        private const float PreviewImageWidth = 25f;
        private const float PreviewImageRightMargin = 43f;
        private const float TopMarginOffset = 125f;
        
        private const float PreviewLabelWidth = 80f;
        private const float PreviewLabelRightMargin = 0f;
        
        private ObjectStack _objectStack;
        
        private SerializedProperty _totalObjectsProp;
        private SerializedProperty _settingsProp;
        
        // This is a setting from within the settingsProp.
        // private SerializedProperty _showNoiseMeterProp;
        
        private SerializedProperty _sceneViewVisualsModeProp;
        private SerializedProperty _prefabProp;
        private SerializedProperty _loggerProp;

        private UnityEditor.Editor _editor;
        private bool _showSoSettings = true;

        private const float VerticalSeparator = 4f;
        private const float ParentFrameWidth = 2f;
        private const float childFrameWidth = 2f;
        private const float FoldoutFramePadding = 2f;
        private const float EditorFrameBottomPadding = 10f;
        private const ElementFrameType FoldoutFrameType = ElementFrameType.FullOutline;
        private const ElementFrameType EditorFrameType = ElementFrameType.BottomOnly;

        private CustomLogger _localLogger;

        private Object _editorTarget;

        private static UnityAction _onRecompile;
        private static void RaiseOnRecompile() => _onRecompile?.Invoke();

        private float _editorStartPosition;
        private float _editorEndPosition;

#endregion


#region LifeCycle

        protected override void OnEnableFirst()
        {
            if ( _localLogger == null )
                _localLogger = CustomLoggerLoader.GetLogger( "ObjectStackerEditorLogger", "Assets/_Testing/ScriptableObjects/Loggers/" );

            _localLogger.LogStart();
            
            _objectStack = target as ObjectStack;

            // Get the so prop.
            _totalObjectsProp = serializedObject.FindProperty( "totalObjects" );
            
            _settingsProp = serializedObject.FindProperty( "settingsSo" );
            _settingsProp.PrintSerializedPropertyInfo();
            // _settingsSerializedObject = new SerializedObject( _settingsProp.objectReferenceValue );
            // _showNoiseMeterProp = _settingsSerializedObject.FindProperty( nameof( ObjectStackerSettingsSO.showNoiseMeter ) );
            // _showNoiseMeterProp.PrintSerializedPropertyInfo();
            // _localLogger.LogObjectAssignmentResult( nameof(_showNoiseMeterProp ), _showNoiseMeterProp == null, CustomLogType.Standard );
            
            _sceneViewVisualsModeProp = serializedObject.FindProperty( "sceneViewVisualsMode" );
            _prefabProp = serializedObject.FindProperty( "prefab" );
            _loggerProp = serializedObject.FindProperty( "logger" );

            SettingsSOEditorIsValid();

            _onRecompile += OnScriptsLoaded;

            _localLogger.LogEnd();
        }

        protected override void OnDisableLast()
        {
            _localLogger.LogStart();
            
            DoEditorCleanup();
            
            _onRecompile -= OnScriptsLoaded;
            
            _localLogger.LogEnd();
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
            
            if ( serializedObject.ApplyModifiedProperties() )
            {
                _localLogger.Log( "Change in Settings SO detected. Repainting Scene Views." );
                SceneView.RepaintAll();
            }
        }

        protected override void DuringSceneGUILast()
        {
            // if ( Event.current.type == EventType.Repaint )
            // {
            //     _localLogger.Log( "DuringSceneGUI triggered on Repaint event." );
            //     return;
            // }
            // _localLogger.Log( "DuringSceneGUI triggered on non-Repaint event." );
            
            SceneView.RepaintAll();
        }

        protected void OnSceneGUI()
        {
            // Todo: Directly referencing the value is ugly. Should find a clean way with serialized properties. This works for now.
            if ( !_objectStack.GetSettingsSO().showNoiseMeter ) return;
            
            DrawSceneViewNoiseMapPreview();
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            // RaiseOnRecompile();
        }

        private void OnScriptsLoaded()
        {
            _objectStack.UpdateSettingsSoSubscriptions();
        }

#endregion


#region Helpers

        /// <summary>
        /// Returns true if the editor variable holds a usable editor.
        /// </summary>
        private bool SettingsSOEditorIsValid()
        {
            // Case 1: settingsProp is null
            //     - just return.
            
            // Case 2: settingsProp.objectValue is null
            //     - just return.
            
            // Case 3: editor for settingsProp have already been created.
            //     - just return.
            
            // Case 4: 
            
            
            // Ensure the settings property was found.
            if ( _settingsProp == null ) return false;
            
            // Ensure that the object reference is not null.
            if ( _settingsProp.objectReferenceValue == null ) return false;

            // Skip if we've already made an editor for this object.
            if ( ( _editor != null ) && ( _editorTarget == _settingsProp.objectReferenceValue ) ) return true;
            _editorTarget = _settingsProp.objectReferenceValue;
            
            _localLogger.Log( $"Creating a new editor for {GetColoredStringGreenYellow( _settingsProp.objectReferenceValue.name )}." );
            DoEditorCleanup();
            
            // Create an editor for the so.
            _editor = CreateEditor( _settingsProp.objectReferenceValue );
            return true;
        }

        private void DoEditorCleanup()
        {
            if ( _editor == null ) return;
            
            _localLogger.Log( $"Destroying old editor for {GetColoredStringMaroon( _settingsProp.objectReferenceValue.name )}." );
            DestroyImmediate( _editor );
        }

        private void DrawSettingsSoInspector()
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if ( !SettingsSOEditorIsValid() ) return;

            EditorGUILayout.Space( VerticalSeparator );

            // Draw Foldout with frame.
            Rect labelRect = GetFramedControlRect( Color.gray, FoldoutFrameType, 0f, true, ParentFrameWidth, FoldoutFramePadding );
            _showSoSettings = EditorGUI.Foldout( labelRect, _showSoSettings, "Object Stacker Settings", true );
            if ( !_showSoSettings ) return;

            Rect startRect = GUILayoutUtility.GetLastRect();
            _editor.OnInspectorGUI();
            Rect endRect = GUILayoutUtility.GetLastRect();

            DrawSOFrame( startRect, endRect );
            
            EditorGUILayout.Space( VerticalSeparator );
        }

        private static void DrawSOFrame( Rect startRect, Rect endRect )
        {
            var finalRect = new Rect( startRect )
            {
                yMin = startRect.yMax, 
                yMax = endRect.yMax + EditorFrameBottomPadding
            };
            finalRect.xMin += ParentFrameWidth;
            DrawRect( finalRect, EditorFrameType, Color.gray, Color.gray, childFrameWidth, false );
        }

#endregion


#region SceneViewNoiseMapPreview

        /// <summary>
        /// Draws a vertical bar that shows a preview of how the noise settings are being applied to the stack. Also includes ticks along the<br/>
        /// left side that indicate where the objects are sampling the noise.
        /// </summary>
        private void DrawSceneViewNoiseMapPreview()
        {
            Rect currentViewPortRect = Camera.current.pixelRect;
            float previewImageHeight = currentViewPortRect.height / 2f;
            float value = 0.25f;
            var outlineColor = new Color( value, value, value );
            value = 0.15f;
            var backgroundColor = new Color( value, value, value );

            var positionVector = new Vector2( 
                currentViewPortRect.width - PreviewImageRightMargin - PreviewImageWidth, 
                // TopMarginOffset + PreviewImageRightMargin + 35f
                TopMarginOffset
            );
            var sizeVector = new Vector2( 
                PreviewImageWidth, 
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
                currentViewPortRect.width - PreviewLabelWidth + PreviewLabelRightMargin,
                // TopMarginOffset + singleLineHeight + standardVerticalSpacing
                TopMarginOffset + ( previewImageHeight / 2f )
            );
            var labelSizeRect = new Vector2(
                PreviewLabelWidth,
                EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
            );
            var labelRect = new Rect( labelPositionVector, labelSizeRect );
            
            Handles.BeginGUI();
            {
                GUIUtility.RotateAroundPivot(90, labelRect.center );
                DrawSolidRectWithOutline( labelRect, outlineColor, backgroundColor, 2f );
                GUI.Label( labelRect, " Noise Meter" );
                GUIUtility.RotateAroundPivot(-90, labelRect.center );
            }
            Handles.EndGUI();
        }

#endregion
    }
}
