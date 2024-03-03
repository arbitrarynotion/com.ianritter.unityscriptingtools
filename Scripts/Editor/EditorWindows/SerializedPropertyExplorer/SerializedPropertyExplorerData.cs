using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomColors;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.ToolingConstants;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.EditorWindows.SerializedPropertyExplorer
{
    [CreateAssetMenu(menuName = SerializedPropertyExplorerMenuTitle)]
    public class SerializedPropertyExplorerData : ScriptableObject
    {
        public bool expandArrays = true;
        public bool simplifyPaths = true;
        public CustomColor titleHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTitleText, Color.grey );
        public CustomColor pathHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutPathText, Color.blue );
        public CustomColor typeHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTypeText, Color.yellow );
        public CustomColor objectHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutObjectIDText, Color.yellow );
        public CustomColor valueHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutValueText, Color.yellow );
        public CustomColor searchHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutSearchText, Color.green );
    }
}
