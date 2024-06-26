using Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.CustomColorPicker.ColorPickerPopupWindow;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUIFormatting;
using static UnityEditor.EditorGUILayout;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.FormattedDebugLogger
{
    [CustomEditor( typeof( FormattedLogger ) )]
    [CanEditMultipleObjects]
    public class FormattedLoggerEditor : UnityEditor.Editor
    {
        private const float SpaceBeforeButton = 10f;
        
        private const string ShowLogsVarName = "showLogs";
        private const string PadBlocksName = "padBlocks";
        private const string UseClassPrefixVarName = "useClassPrefix";
        private const string BoldMethodsVarName = "boldMethods";
        private const string BoldBlockMethodsVarName = "boldBlockMethods";
        private const string NicifiedNamesVarName = "nicifiedNames";

        private const string LogPrefixVarName = "logPrefix";
        private const string BlockDividerVarName = "blockDivider";
        private const string IndentMarkerVarName = "indentMarker";
        private const string MethodDividersVarName = "methodDividers";
        private const string LogBlockStartVarName = "logBlockStart";
        private const string LogBlockEndVarName = "logBlockEnd";
        private const string LogEventPrefixVarName = "logEventPrefix";
        private const string FocusAccentVarName = "focusAccent";

        private const string BlockMethodColorVarName = "blockMethodColor";
        private const string MethodColorVarName = "methodColor";

        private const string IncludeStackTraceVarName = "includeStackTrace";
        private const string FullPathNameVarName = "fullPathName";
        private const string TargetClassVarName = "targetClass";
        private const string TargetMethodVarName = "targetMethod";

        private SerializedProperty _showLogsProperty;
        private SerializedProperty _padBlocksProperty;
        private SerializedProperty _useClassPrefixProperty;
        private SerializedProperty _boldBlockMethodsProperty;
        private SerializedProperty _boldMethodsProperty;
        private SerializedProperty _nicifiedNamesProperty;

        private SerializedProperty _logPrefixProperty;
        private SerializedProperty _blockDividerProperty;
        private SerializedProperty _indentMarkerProperty;
        private SerializedProperty _methodDividersProperty;
        private SerializedProperty _logBlockStartProperty;
        private SerializedProperty _logBlockEndProperty;
        private SerializedProperty _logEventPrefixProperty;
        private SerializedProperty _focusedPrefixProperty;
        private SerializedProperty _focusedPostfixProperty;

        private SerializedProperty _blockMethodColorProperty;
        private SerializedProperty _methodColorProperty;
        private SerializedProperty _focusAccentProperty;

        private SerializedProperty _fullPathNameProperty;
        private SerializedProperty _includeStackTraceProperty;
        private SerializedProperty _targetClassProperty;
        private SerializedProperty _targetMethodProperty;

        private FormattedLogger _targetScript;

        private bool _debugFoldoutToggle;

#region LifeCycle

        private void OnEnable()
        {
            _targetScript = (FormattedLogger) target;

            LoadProperties();

            // Subscribe to the color picker handler to be notified when the color button returns a color.
            ColorPickerHandler.OnColorSelected += OnColorSelection;
        }

        private void OnDisable()
        {
            ColorPickerHandler.OnColorSelected -= OnColorSelection;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawClassMembers();
            Space( SpaceBeforeButton );
            DrawDefaultsButton();
            Space();
            DrawDebugSection();

            serializedObject.ApplyModifiedProperties();
        }

#endregion


#region Initialization

        private void LoadProperties()
        {
            _showLogsProperty = serializedObject.FindProperty( ShowLogsVarName );
            _padBlocksProperty = serializedObject.FindProperty( PadBlocksName );
            _useClassPrefixProperty = serializedObject.FindProperty( UseClassPrefixVarName );
            _boldMethodsProperty = serializedObject.FindProperty( BoldMethodsVarName );
            _boldBlockMethodsProperty = serializedObject.FindProperty( BoldBlockMethodsVarName );
            _nicifiedNamesProperty = serializedObject.FindProperty( NicifiedNamesVarName );

            _blockMethodColorProperty = serializedObject.FindProperty( BlockMethodColorVarName );
            _methodColorProperty = serializedObject.FindProperty( MethodColorVarName );

            _logPrefixProperty = serializedObject.FindProperty( LogPrefixVarName );
            _blockDividerProperty = serializedObject.FindProperty( BlockDividerVarName );
            _indentMarkerProperty = serializedObject.FindProperty( IndentMarkerVarName );
            _methodDividersProperty = serializedObject.FindProperty( MethodDividersVarName );
            _logBlockStartProperty = serializedObject.FindProperty( LogBlockStartVarName );
            _logBlockEndProperty = serializedObject.FindProperty( LogBlockEndVarName );
            _logEventPrefixProperty = serializedObject.FindProperty( LogEventPrefixVarName );
            _focusAccentProperty = serializedObject.FindProperty( FocusAccentVarName );

            _includeStackTraceProperty = serializedObject.FindProperty( IncludeStackTraceVarName );
            _fullPathNameProperty = serializedObject.FindProperty( FullPathNameVarName );
            _targetClassProperty = serializedObject.FindProperty( TargetClassVarName );
            _targetMethodProperty = serializedObject.FindProperty( TargetMethodVarName );
        }

#endregion


#region EventMethods

        /// <summary>
        ///     This method will be called when the color picker handler has received a color from the popup window, which it passes as a parameter in case it's needed elsewhere.
        ///     The picker will assign the color to the property on its end. On this end, you just need to applyModifiedProperties to save the change.
        /// </summary>
        /// <param name="color"></param>
        private void OnColorSelection( CustomColor color )
        {
            // Debug.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            // ColorPickerHandler.Close();
            // serializedObject.ApplyModifiedProperties();
            Repaint();
        }

#endregion


#region InspectorDrawing

        private void DrawClassMembers()
        {
            PropertyField( _logPrefixProperty );

            DrawTogglesSection();
            Space();
            DrawLogSymbolsSection();
            Space();
            DrawMethodNameColorsSection();
        }

        private void DrawLogSymbolsSection()
        {
            DrawLabelSection( "Log Symbols", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _blockDividerProperty );
                PropertyField( _indentMarkerProperty );
                PropertyField( _methodDividersProperty );
                PropertyField( _logBlockStartProperty );
                PropertyField( _logBlockEndProperty );
                PropertyField( _logEventPrefixProperty );
                PropertyField( _focusAccentProperty );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawMethodNameColorsSection()
        {
            DrawLabelSection( "Method Name Colors", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                ColorPickerHandler.DrawPropertyWithColorPicker( _blockMethodColorProperty, new GUIContent( "Block Methods" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _methodColorProperty, new GUIContent( "Basic Methods" ) );
            }
            EditorGUI.indentLevel--;
        }

        private void DrawTogglesSection()
        {
            DrawLabelSection( "Toggles", LabelHeadingFrameType );
            EditorGUI.indentLevel++;
            {
                PropertyField( _showLogsProperty );
                PropertyField( _padBlocksProperty );
                PropertyField( _useClassPrefixProperty );
                PropertyField( _boldMethodsProperty );
                PropertyField( _boldBlockMethodsProperty );
                PropertyField( _nicifiedNamesProperty );
            }
            EditorGUI.indentLevel--;
            
        }

        private void DrawDebugSection()
        {
            _debugFoldoutToggle = DrawFoldoutSection( "Debug", FoldoutFrameType, _debugFoldoutToggle );
            if( _debugFoldoutToggle )
            {
                EditorGUI.indentLevel++;
                {
                    PropertyField( _includeStackTraceProperty );
                    if( _includeStackTraceProperty.boolValue )
                    {
                        EditorGUI.indentLevel++;
                        {
                            PropertyField( _fullPathNameProperty );
                            PropertyField( _targetClassProperty );
                            PropertyField( _targetMethodProperty );
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }

            EditorGUI.indentLevel--;
        }

        private void DrawDefaultsButton()
        {
            Rect buttonRect = GetControlRect();
            // buttonRect.xMin += EditorGUIUtility.labelWidth;
            if( GUI.Button( buttonRect, new GUIContent( "Set to All Defaults", "Set all fields to their default values." ) ) ) _targetScript.SetValuesToDefault();
        }

#endregion
    }
}