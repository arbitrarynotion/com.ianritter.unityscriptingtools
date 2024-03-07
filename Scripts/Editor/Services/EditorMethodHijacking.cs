using System;
using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEngine;

// Type
// Path
// Assembly, MethodInfo, BindingFlags
// EditorApplication
// Application

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public class EditorMethodHijacking : UnityEditor.Editor
    {
        // The purpose of this script is to document methods used to access and hijack
        // Unity's internal methods.

        // This is an example used in Unity's official tutorial projects to hijack the LoadWindowLayout method in UnityEditor.WindowLayout.
        private static void LoadLayout()
        {
            MethodInfo newMethodInfo = GetEditorMethodInfo(
                "UnityEditor.WindowLayout",
                "LoadWindowLayout",
                BindingFlags.Public | BindingFlags.Static
            );
            newMethodInfo.Invoke( null, new object[] { Path.Combine( Application.dataPath, "TutorialInfo/Layout.wlt" ), false } );
        }

        /// <summary>
        ///     Use reflection to the the method info for an otherwise inaccessible method.
        /// </summary>
        /// <param name="className">
        ///     The name of the class that contains the method.<br/>
        ///     Example: "UnityEditor.WindowLayout"
        /// </param>
        /// <param name="methodName">
        ///     The name of the method.<br/>
        ///     Example: "LoadWindowLayout"
        /// </param>
        /// <param name="bindingFlags">
        ///     The binding flags that specific the type of access the method has.<br/>
        ///     Example: BindingFlags.Public | BindingFlags.Static
        /// </param>
        /// <returns>The System.Reflection.MethodInfo for methodName in className.</returns>
        private static MethodInfo GetEditorMethodInfo( string className, string methodName, BindingFlags bindingFlags )
        {
            Assembly assembly = typeof( EditorApplication ).Assembly;
            Type windowLayoutType = assembly.GetType( className, true );
            return windowLayoutType.GetMethod( methodName, bindingFlags );
        }
    }
}