using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Editor.CustomEditors
{
    [CustomEditor( typeof( CustomLogger ))]
    public class CustomLoggerEditor : UnityEditor.Editor
    {
        private CustomLogger _targetScript;
        
        private const string ShowLogsVarName = "showLogs";
        private const string NicifiedNamesVarName = "nicifiedNames";
        private const string LogPrefixVarName = "logPrefix";
        private const string BlockDividerVarName = "blockDivider";
        private const string BlockDividersVarName = "blockDividers";
        private const string MethodDividersVarName = "methodDividers";
        private const string LogBlockStartVarName = "logBlockStart";
        private const string LogBlockEndVarName = "logBlockEnd";
        private const string LogEventPrefixVarName = "logEventPrefix";
        
        private SerializedProperty _showLogsProperty;
        private SerializedProperty _nicifiedNamesProperty;
        private SerializedProperty _logPrefixProperty;
        private SerializedProperty _blockDividerProperty;
        private SerializedProperty _blockDividersProperty;
        private SerializedProperty _methodDividersProperty;
        private SerializedProperty _logBlockStartProperty;
        private SerializedProperty _logBlockEndProperty;
        private SerializedProperty _logEventPrefixProperty;
        
        private ColorPickerHandler _colorPickerHandler;

        
        private void OnEnable()
        {
            _targetScript = (CustomLogger) target;
            
            _showLogsProperty = serializedObject.FindProperty( ShowLogsVarName );
            _nicifiedNamesProperty = serializedObject.FindProperty( NicifiedNamesVarName );
            _logPrefixProperty = serializedObject.FindProperty( LogPrefixVarName );
            _blockDividerProperty = serializedObject.FindProperty( BlockDividerVarName );
            _blockDividersProperty = serializedObject.FindProperty( BlockDividersVarName );
            _methodDividersProperty = serializedObject.FindProperty( MethodDividersVarName );
            _logBlockStartProperty = serializedObject.FindProperty( LogBlockStartVarName );
            _logBlockEndProperty = serializedObject.FindProperty( LogBlockEndVarName );
            _logEventPrefixProperty = serializedObject.FindProperty( LogEventPrefixVarName );
            
            _colorPickerHandler = new ColorPickerHandler( 
                new Vector2( 10f, 10f ), 
                new Vector2(350, 400), 
                5
            );
            
            _colorPickerHandler.OnColorSelected += OnColorSelection;
        }
        
        private void OnDisable()
        {
            _colorPickerHandler.OnColorSelected -= OnColorSelection;
        }
        
        private void OnColorSelection( CustomColor color )
        {
            Debug.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            _colorPickerHandler.Close();
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            DrawClassMembers();
            EditorGUILayout.Space();
            DrawResetButton();
        }
        
        // [SerializeField] private bool showLogs = true;
        // [SerializeField] private bool nicifiedNames = true;
        //
        // [Header("Log Symbols")]
        // [SerializeField] private CustomLoggerSymbol logPrefix;
        // [SerializeField] private CustomLoggerSymbol blockDivider;
        // [SerializeField] private CustomLoggerSymbol blockDividers;
        // [SerializeField] private CustomLoggerSymbol methodDividers;
        // [SerializeField] private CustomLoggerSymbol logBlockStart;
        // [SerializeField] private CustomLoggerSymbol logBlockEnd;
        // [SerializeField] private CustomLoggerSymbol logEventPrefix;

        private void DrawClassMembers()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField( "Toggles", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _showLogsProperty );
                EditorGUILayout.PropertyField( _nicifiedNamesProperty );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField( "Log Symbols", EditorStyles.boldLabel );
            DrawCustomColorProperty( _logPrefixProperty );
            EditorGUI.indentLevel++;
            {
                DrawCustomColorProperty( _blockDividerProperty );
                DrawCustomColorProperty( _blockDividersProperty );
                DrawCustomColorProperty( _methodDividersProperty );
                DrawCustomColorProperty( _logBlockStartProperty );
                DrawCustomColorProperty( _logBlockEndProperty );
                DrawCustomColorProperty( _logEventPrefixProperty );
                // DrawCustomColorProperty();
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCustomColorProperty( SerializedProperty property )
        {
            Rect lineRect = EditorGUILayout.GetControlRect();
            // DrawRectOutline( lineRect, Color.grey );
            
            var labelRect = new Rect( lineRect );
            labelRect.width = EditorGUIUtility.labelWidth;
            // DrawRectOutline( labelRect, Color.green );
            
            var loggerSymbolRect = new Rect( lineRect );
            loggerSymbolRect.xMin += labelRect.width + 2f;
            loggerSymbolRect.width *= 0.9f;
            // DrawRectOutline( loggerSymbolRect, Color.cyan );
            
            var propertyField = new Rect( lineRect );
            propertyField.xMax = loggerSymbolRect.xMax;
            // DrawRectOutline( propertyField, Color.magenta );

            
            EditorGUI.PropertyField( propertyField, property );
            
            var buttonRect = new Rect( lineRect );
            buttonRect.xMin += labelRect.width + 2f + loggerSymbolRect.width + 2f;
            // DrawRectOutline( buttonRect, Color.yellow );
            SerializedProperty customColor = property.FindPropertyRelative( "customColor" );
            _colorPickerHandler.DrawColorPickerPropertyButton( buttonRect, customColor );
            
        }
        
        // private void DrawCustomColorField( SerializedProperty property )
        // {
        //     // return EditorGUILayout.ColorField( dataTitle, targetColor, GUILayout.MaxWidth( 350 ) );
        //     Rect lineRect = EditorGUILayout.GetControlRect( true );
        //     float availableWidth = lineRect.width;
        //     const float buttonWidth = 40f;
        //
        //     // float colorFieldWidth = availableWidth * 0.9f;
        //     float propertyFieldWidth = availableWidth - buttonWidth;
        //     float startOfButton = propertyFieldWidth;
        //     
        //     var colorFieldRect = new Rect( lineRect )
        //     {
        //         width = startOfButton
        //     };
        //     // DrawRectOutline( colorFieldRect, Color.cyan );
        //     EditorGUI.PropertyField( colorFieldRect, property );
        //     
        //     var buttonRect = new Rect( lineRect ) { width = buttonWidth };
        //     buttonRect.x += startOfButton;
        //     buttonRect.xMin += 2f;
        //     // DrawRectOutline( buttonRect, Color.green );
        //     SerializedProperty customColor = property.FindPropertyRelative( "customColor" );
        //     _colorPickerHandler.DrawColorPickerButton( buttonRect, targetColor );
        // }
        
        private void DrawResetButton()
        {
            Rect buttonRect = EditorGUILayout.GetControlRect();
            buttonRect.xMin += EditorGUIUtility.labelWidth;
            if ( GUI.Button( buttonRect, "Defaults" ) )
            {
                _targetScript.SetValuesToDefault();
            }
        }
    }
}
