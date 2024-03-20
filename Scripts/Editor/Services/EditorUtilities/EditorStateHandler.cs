using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.EditorUtilities
{
    public class EditorStateEntry
    {
        // This must be a unique string as it is used to save and look up the value.
        public string Key;

        // This is the value that will be saved or returned as the result of a successful lookup.
        public bool DefaultValue;
    }

    public class EditorStateHandler
    {
        /* The purpose of this class is to simplify the process of utilizing EditorPrefs to save
            and load bool editor state data.
            
            I want to be able to just hand a list of unique names to this class, together with their
            default values, and be able to both save and load the stored value using the name as the
            reference.
         */

        private EditorStateEntry[] _editorStateEntries;
        private string _instanceID;

        public void SetBools( string instanceID, params (string varName, bool defaultValue)[] editorStateEntries )
        {
            _instanceID = instanceID;

            foreach ( (string key, bool value) editorStateEntry in editorStateEntries )
            {
                string name = $"{instanceID}_{editorStateEntry.key}";
                if( EditorPrefs.HasKey( name ) ) continue;

                EditorPrefs.SetBool( name, editorStateEntry.value );
            }
        }

        public bool GetBool( string varName )
        {
            string name = $"{_instanceID}_{varName}";
            if( EditorPrefs.HasKey( name ) ) return EditorPrefs.GetBool( name );

            Debug.LogWarning( $"Warning! Couldn't find editor bool {GetColoredStringYellow( name )}" );
            return false;
        }
    }
}