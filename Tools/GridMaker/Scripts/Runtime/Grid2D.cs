using System;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.GridMaker.Scripts.Runtime
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
        
        public void UpdateNodeNoiseValues( float[,] noiseMap )
        {
            int noiseMapWidth = noiseMap.GetLength( 0 );
            int noiseMapHeight = noiseMap.GetLength( 1 );
            
            // Loop through each cell of the grid one line at a time starting at the bottom.
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    float halfCell = nodeWidth / 2f;
                    float percentOfWidth = ( ( x * nodeWidth ) + halfCell ) / gridWidth;
                    float percentOfHeight = ( ( y * nodeWidth ) + halfCell ) / gridHeight;
                    
                    int xSample = Mathf.FloorToInt( percentOfWidth * noiseMapWidth);
                    int ySample = Mathf.FloorToInt( percentOfHeight * noiseMapHeight);

                    float value = noiseMap[xSample, ySample];

                }
            }
        }
        
    }
}