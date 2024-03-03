using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.EditorWindows.ColorEntryHelperWindow
{
    public class CustomColorLibrary : ScriptableObject
    {
        public string libraryName;
        public CustomColor[] customColors;
    }
}
