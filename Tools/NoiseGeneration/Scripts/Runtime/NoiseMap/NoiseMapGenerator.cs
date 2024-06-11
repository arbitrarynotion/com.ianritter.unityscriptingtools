using UnityEngine;
using Random = System.Random;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime.NoiseMap
{
    public static class NoiseMapGenerator
    {
        public static float[,] GenerateNoiseMap(
            int mapWidth,
            int mapHeight,
            int seed,
            float scale,
            int octaves,
            float persistence,
            float lacunarity,
            Vector2 offset )
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            // Generate the map based on a seed
            var prng = new Random( seed );
            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next( -100000, 100000 ) + offset.x;
                float offsetY = prng.Next( -100000, 100000 ) + offset.y;
                octaveOffsets[i] = new Vector2( offsetX, offsetY );
            }

            if ( scale <= 0 ) scale = 0.0001f;

            float halfWidth = mapWidth / 2.0f;
            float halfHeight = mapHeight / 2.0f;

            // The noise map needs to be normalized to between 0 and 1 so we need to keep track of the min and max
            float minNoiseHeight = float.MaxValue;
            float maxNoiseHeight = float.MinValue;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = ( x - halfWidth ) / scale * frequency + octaveOffsets[i].x; // The higher the frequency, the further apart the 
                        float sampleY = ( y - halfHeight ) / scale * frequency + octaveOffsets[i].y; // sample points so height values will change more rapidly

                        float perlinValue = Mathf.PerlinNoise( sampleX, sampleY ) * 2 - 1; // *2-1 takes the 0 to 1 range and makes it a -1 to 1 range

                        noiseHeight += perlinValue * amplitude; // Increase noise height by the perlin value of each octave

                        amplitude *= persistence; // Amplitude decreases with each octave
                        frequency *= lacunarity; // Frequency increase with each octave since lacunarity > 1
                    }

                    // Register min/max noise height
                    if ( noiseHeight > maxNoiseHeight )
                        maxNoiseHeight = noiseHeight;
                    else if ( noiseHeight < minNoiseHeight ) minNoiseHeight = noiseHeight;
                    noiseMap[x, y] = noiseHeight;
                }
            }

            // Normalize noise map values using the min and max heights registered
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp( minNoiseHeight, maxNoiseHeight, noiseMap[x, y] ); // InverseLerp assigns a value from 0 to 1 relative to the min and max specified
                }
            }

            return noiseMap;
        }
    }
}