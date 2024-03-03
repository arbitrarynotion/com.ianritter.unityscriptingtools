using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor._Testing.SOPropertyDrawer
{
    // [CustomEditor( typeof( TestScriptableObject ) )]
    public class TestSoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Rect positionRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight );
            // EditorGUI.LabelField( positionRect, "Test SO Custom Editor is working." );
            EditorGUILayout.LabelField( "Test SO Custom Editor is working." );
        }
        
        
    }
}
