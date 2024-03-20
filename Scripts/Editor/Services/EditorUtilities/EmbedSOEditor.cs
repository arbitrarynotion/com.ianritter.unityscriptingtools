using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Editor;
using static UnityEngine.Object;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static UnityEditor.EditorGUILayout;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.Graphics.UI.EditorDividerGraphics;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using Object = UnityEngine.Object;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    [Serializable]
    public class EmbedSOEditor
    {
        private const float DividerColorValue = 0.15f;
        private static readonly Color DividerColor = new Color( DividerColorValue, DividerColorValue, DividerColorValue );
        private const float DividerThickness = 2f;
        private const float DividerTopMargin = 12f;
        private const float DividerBottomMargin = 8f;
        private const float DividerLeftMargin = 19f;
        private const float DividerRightMargin = 3f;

        private readonly SerializedProperty _settingsSOProp;

        private string _cachedEditorName;

        [SerializeField] private FormattedLogger logger;
        [SerializeField] private UnityEditor.Editor settingsSOEditor;
        [SerializeField] private Object settingsSOEditorTarget;
        [SerializeField] private bool foldoutToggle = true;

        public EmbedSOEditor( SerializedProperty settingsSOProp )
        {
            _settingsSOProp = settingsSOProp;
        }

        public void DrawSettingsSoInspector( string title, bool drawFrame = true )
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !SettingsSOEditorIsValid() )
            {
                HelpBox( "Populate the object field to display the object's settings.", MessageType.Info );
                return;
            }

            DrawDivider( DividerColor, DividerThickness, DividerLeftMargin, DividerRightMargin, DividerTopMargin, DividerBottomMargin );

            foldoutToggle = DrawFoldoutSection( title, FoldoutFrameType, foldoutToggle );
            if( !foldoutToggle ) return;


            Rect foldoutFrameRect = DrawSettingsSoInspector();

            if( !drawFrame ) return;

            foldoutFrameRect.xMin += ParentFrameWidth;
            foldoutFrameRect.yMax += EditorFrameBottomPadding;
            DrawRect( foldoutFrameRect, EditorFrameType, Color.gray, Color.gray, ChildFrameWidth, false );

            Space( VerticalSeparator );
        }

        public Rect DrawSettingsSoInspector()
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !SettingsSOEditorIsValid() ) return new Rect();

            Rect startRect = GUILayoutUtility.GetLastRect();
            settingsSOEditor.OnInspectorGUI();
            Rect endRect = GUILayoutUtility.GetLastRect();

            return new Rect( startRect )
            {
                yMin = startRect.yMax,
                yMax = endRect.yMax
            };
        }

        public void OnEnable()
        {
            if( logger == null )
                logger = FormattedLoggerLoader.GetLogger( EmbeddedSOEditorLoggerName, DefaultLoggerPath );

            // logger.LogObjectAssignmentResult( nameof( logger ), logger );

            SettingsSOEditorIsValid();
        }

        public void OnDisable() => DoEditorCleanup();

        /// <summary>
        ///     Returns true if the editor variable holds a usable editor.
        /// </summary>
        public bool SettingsSOEditorIsValid()
        {
            // Case 1: settingsProp is null
            //     - return false.
            // Ensure the settings property was found.
            if( _settingsSOProp == null ) return false;

            // Case 2: settingsProp.objectValue is null
            //     - return false.
            // Ensure that the object reference is not null.
            if( _settingsSOProp.objectReferenceValue == null )
            {
                if( settingsSOEditor != null )
                    DoEditorCleanup();
                return false;
            }

            // Case 3: editor and target exist.
            //     When the target holds an object reference and the editor exists, nothing needs to be done.
            //     - return true.
            // Skip if we've already made an editor for this object.
            if( ( settingsSOEditor != null ) &&
                ( settingsSOEditorTarget == _settingsSOProp.objectReferenceValue ) ) return true;

            // Case 4:  either the editor or the target are null.
            //     This means the a target change has occurred.
            //     - Save the object reference in the target.
            //     - Dispose of the old editor, if one exists.
            //     - Create a new editor for this object.
            //     - return true.
            settingsSOEditorTarget = _settingsSOProp.objectReferenceValue;

            DoEditorCleanup();

            // Create an editor for the so.
            logger.Log( $"Creating a new editor for {GetColoredStringGreenYellow( _settingsSOProp.objectReferenceValue.name )}." );
            settingsSOEditor = CreateEditor( _settingsSOProp.objectReferenceValue );
            _cachedEditorName = _settingsSOProp.objectReferenceValue.name;
            return true;
        }

        private void DoEditorCleanup()
        {
            if( settingsSOEditor == null ) return;

            logger.Log( $"Destroying old editor for {GetColoredStringLightCoral( _cachedEditorName )}." );
            DestroyImmediate( settingsSOEditor, false );
        }
    }
}