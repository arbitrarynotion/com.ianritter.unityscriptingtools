using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Editor.CustomEditors
{
    [CustomEditor( typeof( CustomLogger )), CanEditMultipleObjects]
    public class CustomLoggerEditor : UnityEditor.Editor
    {
        public Texture buttonTexture;
        
        private CustomLogger _targetScript;
        
        private const string ShowLogsVarName = "showLogs";
        private const string UseClassPrefixVarName = "useClassPrefix";
        private const string BoldMethodsVarName = "boldMethods";
        private const string BoldBlockMethodsVarName = "boldBlockMethods";
        private const string NicifiedNamesVarName = "nicifiedNames";
        private const string IncludeStackTraceVarName = "includeStackTrace";
        private const string FullPathNameVarName = "fullPathName";
        private const string TargetClassVarName = "targetClass";
        private const string TargetMethodVarName = "targetMethod";
        
        private const string LogPrefixVarName = "logPrefix";
        private const string BlockDividerVarName = "blockDivider";
        private const string IndentMarkerVarName = "indentMarker";
        private const string MethodDividersVarName = "methodDividers";
        private const string LogBlockStartVarName = "logBlockStart";
        private const string LogBlockEndVarName = "logBlockEnd";
        private const string LogEventPrefixVarName = "logEventPrefix";
        
        private const string BlockMethodColorVarName = "blockMethodColor";
        private const string MethodColorVarName = "methodColor";

        private SerializedProperty _showLogsProperty;
        private SerializedProperty _useClassPrefixProperty;
        private SerializedProperty _boldMethodsProperty;
        private SerializedProperty _boldBlockMethodsProperty;
        private SerializedProperty _nicifiedNamesProperty;
        private SerializedProperty _includeStackTraceProperty;
        private SerializedProperty _fullPathNameProperty;
        private SerializedProperty _targetClassProperty;
        private SerializedProperty _targetMethodProperty;
        
        private SerializedProperty _logPrefixProperty;
        private SerializedProperty _blockDividerProperty;
        private SerializedProperty _indentMarkerProperty;
        private SerializedProperty _methodDividersProperty;
        private SerializedProperty _logBlockStartProperty;
        private SerializedProperty _logBlockEndProperty;
        private SerializedProperty _logEventPrefixProperty;
        
        private SerializedProperty _blockMethodColorProperty;
        private SerializedProperty _methodColorProperty;
        
        private bool _debugFoldoutToggle = false;

        
        private void OnEnable()
        {
            _targetScript = (CustomLogger) target;
            
            _showLogsProperty = serializedObject.FindProperty( ShowLogsVarName );
            _useClassPrefixProperty = serializedObject.FindProperty( UseClassPrefixVarName );
            _boldMethodsProperty = serializedObject.FindProperty( BoldMethodsVarName );
            _boldBlockMethodsProperty = serializedObject.FindProperty( BoldBlockMethodsVarName );
            _nicifiedNamesProperty = serializedObject.FindProperty( NicifiedNamesVarName );
            _includeStackTraceProperty = serializedObject.FindProperty( IncludeStackTraceVarName );
            _fullPathNameProperty = serializedObject.FindProperty( FullPathNameVarName );
            _targetClassProperty = serializedObject.FindProperty( TargetClassVarName );
            _targetMethodProperty = serializedObject.FindProperty( TargetMethodVarName );
            
            _logPrefixProperty = serializedObject.FindProperty( LogPrefixVarName );
            _blockDividerProperty = serializedObject.FindProperty( BlockDividerVarName );
            _indentMarkerProperty = serializedObject.FindProperty( IndentMarkerVarName );
            _methodDividersProperty = serializedObject.FindProperty( MethodDividersVarName );
            _logBlockStartProperty = serializedObject.FindProperty( LogBlockStartVarName );
            _logBlockEndProperty = serializedObject.FindProperty( LogBlockEndVarName );
            _logEventPrefixProperty = serializedObject.FindProperty( LogEventPrefixVarName );
            
            _blockMethodColorProperty = serializedObject.FindProperty( BlockMethodColorVarName );
            _methodColorProperty = serializedObject.FindProperty( MethodColorVarName );
            
            // _buttonTextureProperty = serializedObject.FindProperty( ButtonTextureVarName );
            
            // _colorPickerHandler = new ColorPickerHandler( 
            //     new Vector2( 10f, 10f ), 
            //     350f, 400f, 
            //     5
            // );
            
            // Subscribe to the color picker handler to be notified when the color button returns a color.
            ColorPickerHandler.OnColorSelected += OnColorSelection;
        }
        
        private void OnDisable()
        {
            ColorPickerHandler.OnColorSelected -= OnColorSelection;
        }
        
        /// <summary>
        /// This method will be called when the color picker handler has received a color from the popup window, which it passes as a parameter in case it's needed elsewhere.
        /// The picker will assign the color to the property on its end. On this end, you just need to applyModifiedProperties to save the change.
        /// </summary>
        /// <param name="color"></param>
        private void OnColorSelection( CustomColor color )
        {
            // Debug.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            // ColorPickerHandler.Close();
            // serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawClassMembers();
            EditorGUILayout.Space();
            DrawResetButton();
            EditorGUILayout.Space();
            DrawDebugSection();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawClassMembers()
        {
            // EditorGUILayout.PropertyField( _buttonTextureProperty );
            
            EditorGUILayout.LabelField( "Toggles", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _showLogsProperty );
                EditorGUILayout.PropertyField( _useClassPrefixProperty );
                EditorGUILayout.PropertyField( _boldMethodsProperty );
                EditorGUILayout.PropertyField( _boldBlockMethodsProperty );
                EditorGUILayout.PropertyField( _nicifiedNamesProperty );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField( "Method Name Colors", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                ColorPickerHandler.DrawPropertyWithColorPicker( _blockMethodColorProperty, new GUIContent( "Block Methods" ) );
                ColorPickerHandler.DrawPropertyWithColorPicker( _methodColorProperty, new GUIContent( "Basic Methods" ) );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField( "Log Symbols", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField( _logPrefixProperty );
                EditorGUILayout.PropertyField( _blockDividerProperty );
                EditorGUILayout.PropertyField( _indentMarkerProperty );
                EditorGUILayout.PropertyField( _methodDividersProperty );
                EditorGUILayout.PropertyField( _logBlockStartProperty );
                EditorGUILayout.PropertyField( _logBlockEndProperty );
                EditorGUILayout.PropertyField( _logEventPrefixProperty );
            }
            EditorGUI.indentLevel--;
            
        }

        private void DrawDebugSection()
        {
            _debugFoldoutToggle = EditorGUILayout.Foldout( _debugFoldoutToggle, new GUIContent( "Debug" ), true );

            if ( _debugFoldoutToggle )
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.PropertyField( _includeStackTraceProperty );
                    if ( _includeStackTraceProperty.boolValue )
                    {
                        EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField( _fullPathNameProperty );
                            EditorGUILayout.PropertyField( _targetClassProperty );
                            EditorGUILayout.PropertyField( _targetMethodProperty );
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }
            EditorGUI.indentLevel--;
        }
        
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
