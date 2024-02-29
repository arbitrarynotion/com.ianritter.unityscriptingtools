using UnityEngine;
using Random = System.Random;

namespace Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks
{
    public static class NoiseGenerator
    {
        public static float[] GetPerlinNoiseArray(
            int arraySize,
            int seed,
            float noiseScale,
            int octaves,
            float persistence,
            float lacunarity,
            float noiseOffsetHorizontal,
            float noiseOffsetVertical
        )
        {
            float[] noiseMap = new float[arraySize];
        
            // Generate the map based on a seed
            var prng = new Random( seed );
            // var prng = new Random();
            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next( -100000, 100000 ) + noiseOffsetHorizontal;
                float offsetY = prng.Next( -100000, 100000 ) + noiseOffsetVertical;
                octaveOffsets[i] = new Vector2( offsetX, offsetY );
            }

            if ( noiseScale <= 0 ) noiseScale = 0.0001f;
        
            // The noise map needs to be normalized to between 0 and 1 so we need to keep track of the min and max
            float minNoiseHeight = float.MaxValue;
            float maxNoiseHeight = float.MinValue;
        
            for (int y = 0; y < arraySize; y++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++){
                    float sampleX = (1 - 0) / noiseScale * frequency + octaveOffsets[i].x; // The higher the frequency, the further apart the 
                    float sampleY = (y - 0) / noiseScale * frequency + octaveOffsets[i].y; // sample points so height values will change more rapidly

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2; // *2-1 takes the 0 to 1 range and makes it a -1 to 1 range

                    noiseHeight += perlinValue * amplitude; // Increase noise height by the perlin value of each octave

                    amplitude *= persistence; // Amplitude decreases with each octave
                    frequency *= lacunarity; // Frequency increase with each octave since lacunarity > 1
                }

                // Register min/max noise height
                if (noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[y] = noiseHeight;
            }

            return noiseMap;
        }
    }
}
