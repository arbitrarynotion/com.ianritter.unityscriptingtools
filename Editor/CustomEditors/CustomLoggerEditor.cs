using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.unityscriptingtools.Editor.CustomEditors
{
    [CustomEditor( typeof( CustomLogger ))]
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
        
        // private const string ButtonTextureVarName = "buttonTexture";
        
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
        
        // private SerializedProperty _buttonTextureProperty;
        
        // private ColorPickerHandler _colorPickerHandler;

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
            ColorPickerHandler.Close();
            serializedObject.ApplyModifiedProperties();
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
                DrawBasicColorProperty( _blockMethodColorProperty, new GUIContent( "Block Methods" ) );
                DrawBasicColorProperty( _methodColorProperty, new GUIContent( "Basic Methods" ) );
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField( "Log Symbols", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;
            {
                DrawCustomColorProperty( _logPrefixProperty );
                DrawCustomColorProperty( _blockDividerProperty );
                DrawCustomColorProperty( _indentMarkerProperty );
                DrawCustomColorProperty( _methodDividersProperty );
                DrawCustomColorProperty( _logBlockStartProperty );
                DrawCustomColorProperty( _logBlockEndProperty );
                DrawCustomColorProperty( _logEventPrefixProperty );
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


        private void DrawPropertyWithColorPicker( SerializedProperty fieldProperty, SerializedProperty targetProperty, GUIContent guiContent )
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            ColorPickerHandler.SetWindowPosition( controlRect.position );

            // Exclude color picker button width from available width.
            float availableWidth = controlRect.width - ColorPickerHandler.GetColorPickerButtonWidth();

            var lineRect = new Rect( controlRect )
            {
                width = availableWidth
            };
            // DrawRectOutline( lineRect, Color.grey );

            // Draw Property label and field.
            var propertyFieldRect = new Rect( lineRect );
            // DrawRectOutline( propertyField, Color.magenta );
            if ( guiContent != null )
            {
                EditorGUI.PropertyField( propertyFieldRect, fieldProperty, guiContent );
            }
            else
            {
                EditorGUI.PropertyField( propertyFieldRect, fieldProperty );
            }

            // Set the color picker button rect to start at the end of the available space plus a spacer.
            // Then get the width from the color picker handler.
            var buttonRect = new Rect( controlRect );
            buttonRect.xMin += availableWidth + 2f;
            buttonRect.width = ColorPickerHandler.GetColorPickerButtonWidth();
            // DrawRectOutline( buttonRect, Color.yellow );
            
            // Finally, pass the button rect and the color property to the color picker handler.
            // This can either be a direct color property via serializedObject.FindProperty or an indirect one via property.FindPropertyRelative.
            ColorPickerHandler.DrawColorPickerPropertyButton( buttonRect, targetProperty );
        }

        private void DrawBasicColorProperty( SerializedProperty property, GUIContent guiContent = null ) => 
            DrawPropertyWithColorPicker( property, property, guiContent );

        private void DrawCustomColorProperty( SerializedProperty property, GUIContent guiContent = null )
        {
            SerializedProperty colorProperty = property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "color" );
            if ( colorProperty == null )
            {
                Debug.LogError( "Failed to find 'customColor' property or its 'color' property." );
                return;
            }
            DrawPropertyWithColorPicker( property, colorProperty, guiContent );
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
        
        
        
        // private void DrawBasicColorProperty( SerializedProperty property )
        // {
        //     DrawPropertyWithColorPicker( property, property );
        //
        //     // Rect controlRect = EditorGUILayout.GetControlRect();
        //     //
        //     // float availableWidth = controlRect.width - _colorPickerHandler.GetColorPickerButtonWidth();
        //     //
        //     // var lineRect = new Rect( controlRect )
        //     // {
        //     //     width = availableWidth
        //     // };
        //     // DrawRectOutline( lineRect, Color.grey );
        //     //
        //     // var labelRect = new Rect( lineRect );
        //     // labelRect.width = EditorGUIUtility.labelWidth;
        //     // // DrawRectOutline( labelRect, Color.green );
        //     //
        //     // // EditorGUI.LabelField( labelRect, property.name );
        //     //
        //     // var loggerSymbolRect = new Rect( lineRect );
        //     // loggerSymbolRect.xMin += labelRect.width + 2f;
        //     // loggerSymbolRect.width *= 0.9f;
        //     // // DrawRectOutline( loggerSymbolRect, Color.cyan );
        //     //
        //     // var propertyField = new Rect( lineRect );
        //     // // propertyField.xMax = loggerSymbolRect.xMax;
        //     // DrawRectOutline( propertyField, Color.magenta );
        //     //
        //     // // int cachedIndentLevel = EditorGUI.indentLevel;
        //     // // EditorGUI.indentLevel = 0;
        //     // EditorGUI.PropertyField( propertyField, property );
        //     // // EditorGUI.indentLevel = cachedIndentLevel;
        //     //
        //     // var buttonRect = new Rect( controlRect );
        //     // // buttonRect.xMin += labelRect.width + 2f + loggerSymbolRect.width + 2f;
        //     // buttonRect.xMin += availableWidth + 2f;
        //     // buttonRect.width = _colorPickerHandler.GetColorPickerButtonWidth();
        //     // DrawRectOutline( buttonRect, Color.yellow );
        //     //
        //     // _colorPickerHandler.DrawColorPickerPropertyButton( buttonRect, property );
        // }
    }
}
