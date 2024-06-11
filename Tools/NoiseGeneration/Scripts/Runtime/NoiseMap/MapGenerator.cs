using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime.NoiseMap
{
    public class MapGenerator : MonoBehaviour
    {
        public int mapWidth;
        public int mapHeight;
        public float noiseScale;

        public void GenerateMap()
        {
            // float[,] noiseMap = PerlinNoise.GetNoiseMap( mapWidth, mapHeight, noiseScale );
        }
    }
}
