using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Editor.Services
{
    public static class SerializedObjectEditorExtensionMethods
    {
        public static void DrawScriptField( this SerializedObject so )
        {
            EditorGUI.BeginDisabledGroup( true );
            EditorGUILayout.PropertyField( so.FindProperty( "m_Script" ) );
            EditorGUI.EndDisabledGroup();
        }
    }
}