using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.ExtensionMethods
{
    public static class SerializedObjectExtensionMethods
    {
        public static void DrawScriptField( this SerializedObject so )
        {
            EditorGUI.BeginDisabledGroup( true );
            EditorGUILayout.PropertyField( so.FindProperty( "m_Script" ) );
            EditorGUI.EndDisabledGroup();
        }
    }
}