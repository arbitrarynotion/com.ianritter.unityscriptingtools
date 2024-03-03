using System;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Runtime
{
    [Serializable]
    public class GridNode : IComparable<GridNode>
    {
        [SerializeField] private string displayName;
        [SerializeField] private string hierarchyName;
        [SerializeField] private float noiseValue;
        
        [Header( "Position")]
        [SerializeField] private Vector2Int gridPosition;
        [SerializeField] private Vector3 worldPosition;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 worldPositionWithOffset;

        [SerializeField] private float priority; // Set when adding to priority queue: based on 

        // Debug
        public bool debugFocused = false;
        public bool debugFocusedNeighbor = false;

        public string GetDisplayName() => displayName;
        public string GetHierarchyName() => hierarchyName;
        
        /// <summary> This should be used in all cases where the location of the newWare is needed by agents in-game. </summary>
        public Vector3 GetOffsetWorldPosition() => worldPositionWithOffset;
        public Vector2Int GetGridPosition() => gridPosition;
        

        public GridNode( Vector3 worldPosition, float noiseValue, Vector2Int gridPosition )
        {
            this.worldPosition = worldPosition;
            // Default to worldPosition for nodes that never get set.
            worldPositionWithOffset = worldPosition;

            this.noiseValue = noiseValue;
            this.gridPosition = gridPosition;
        }

        
        // node.CompareTo(otherNode); returns -1 if less, 0 if equal, 1 if greater
        public int CompareTo( GridNode other )
        {
            int result;
            if ( priority < other.priority )
                result = -1;
            else if ( priority > other.priority )
                result = 1;
            else
                result = 0;

            return result;
        }
        
#region Universal
        
        public float GetNoiseValue() => noiseValue;
        public void SetNoiseValue( float value ) => noiseValue = value;

        /// <summary>
        /// This should only be used in search algorithms where the distance between nodes is important as the offsets
        /// shouldn't be included in the newWare positions in that context.
        /// </summary>
        public Vector3 GetGridWorldPosition() => worldPosition;
        public Vector3 GetPositionOffset() => positionOffset;

#endregion
        
    }
}