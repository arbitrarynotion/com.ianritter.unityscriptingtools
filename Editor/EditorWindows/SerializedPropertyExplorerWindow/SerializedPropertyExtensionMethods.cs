﻿using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Editor.EditorWindows.SerializedPropertyExplorerWindow
{
    public static class SerializedPropertyExtensionMethods
    {
        public static void PrintSerializedPropertyInfo( this SerializedProperty property, string searchString = "", bool expandArrays = true, bool simplifyPaths = true )
        {
            SerializedPropertyInfoExtractor.PrintSerializedPropertyInfo( property, searchString, expandArrays, simplifyPaths );
        }
    }
}