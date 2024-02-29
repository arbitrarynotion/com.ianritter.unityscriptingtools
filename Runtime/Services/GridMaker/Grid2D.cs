using System;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.GridMaker
{
    [Serializable]
    public class Grid2D
    {
        public int gridSizeX;
        public int gridSizeY;
        public float gridWidth;
        public float gridHeight;

        public float nodeWidth;
        public float nodeHeight;

        public Vector3 worldTopLeft;
        
        public Grid2D( int gridSizeX, int gridSizeY, float gridWidth, float gridHeight, Transform transform )
        {
            this.gridSizeX = gridSizeX;
            this.gridSizeY = gridSizeY;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            
            nodeWidth = gridWidth / this.gridSizeX;
            nodeHeight = gridHeight / this.gridSizeY;

            float gridXHalfWay = ( -gridWidth ) / 2f;
            float gridYHalfWay = ( gridHeight ) / 2f;
                
            worldTopLeft = new Vector3( gridXHalfWay, 0f, gridYHalfWay );
            
            // The position acts as the world center.
            worldTopLeft = ( worldTopLeft + transform.position );
        }
    }
}