using Services.CustomColors;
using UnityEngine;
using static ToolingConstants;

namespace Editor.EditorWindows
{
    [CreateAssetMenu(menuName = SerializedPropertyExplorerDataMenuName)]
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
