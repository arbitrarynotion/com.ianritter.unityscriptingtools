using Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.NoiseMaps.NoiseMap
{
    public class MapGenerator : MonoBehaviour
    {
        public int mapWidth;
        public int mapHeight;
        public float noiseScale;

        public void GenerateMap()
        {
            // float[,] noiseMap = PerlinNoise.GenerateNoiseMap( mapWidth, mapHeight, noiseScale );
        }
    }
}
