using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer
{
    [CreateAssetMenu( menuName = SerializedPropertyExplorerMenuTitle )]
    public class SerializedPropertyExplorerData : ScriptableObject
    {
        public bool expandArrays = true;
        public CustomColor objectHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutObjectIDText, Color.yellow );
        public CustomColor pathHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutPathText, Color.blue );
        public CustomColor searchHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutSearchText, Color.green );
        public bool simplifyPaths = true;
        public CustomColor titleHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTitleText, Color.grey );
        public CustomColor typeHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutTypeText, Color.yellow );
        public CustomColor valueHighlightColor = new CustomColor( SerializedPropertyExplorerReadoutValueText, Color.yellow );
    }
}