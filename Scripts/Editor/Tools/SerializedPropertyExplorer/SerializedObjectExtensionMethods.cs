﻿using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer
{
    public static class SerializedObjectExtensionMethods
    {
        public static void PrintSerializedPropertyInfo( this SerializedObject serializedObject, string searchString = "", bool expandArrays = true, bool simplifyPaths = true )
        {
            SerializedPropertyInfoExtractor.PrintSerializedPropertyInfo( serializedObject.GetIterator().Copy(), searchString, expandArrays, simplifyPaths );
        }
    }
}