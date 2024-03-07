using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Editor;
using static UnityEngine.Object;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using Object = UnityEngine.Object;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    [Serializable]
    public class EmbedSOEditor
    {
        private readonly SerializedProperty _settingsSOProp;

        private string _cachedEditorName;

        [SerializeField] private FormattedLogger logger;
        [SerializeField] private UnityEditor.Editor settingsSOEditor;
        [SerializeField] private Object settingsSOEditorTarget;

        public EmbedSOEditor( SerializedProperty settingsSOProp )
        {
            _settingsSOProp = settingsSOProp;
        }

        public Rect DrawSettingsSoInspector()
        {
            // Optimization to avoid creating a new editor unless it's actually needed.
            if( !SettingsSOEditorIsValid() )
            {
                return new Rect();
            }

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
            if( settingsSOEditor != null &&
                settingsSOEditorTarget == _settingsSOProp.objectReferenceValue ) return true;

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