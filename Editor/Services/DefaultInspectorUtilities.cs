using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Editor.Services
{
    public static class DefaultInspectorUtilities
    {
        /// <summary>
        ///     Draw the script field for this inspector component. Looks and works just like the default one.
        /// </summary>
        public static void DrawScriptField( SerializedObject serializedObject )
        {
            EditorGUI.BeginDisabledGroup( true );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "m_Script" ) );
            EditorGUI.EndDisabledGroup();
        }
    }
}
