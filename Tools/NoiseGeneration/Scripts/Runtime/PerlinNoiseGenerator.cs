using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.ExtensionMethods;
using UnityEngine;
using Random = System.Random;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    /// <summary>
    ///     The core idea of Perlin noise is that we want a coherent noise texture, meaning it changes gradually from<br/>
    ///     pixel to pixel, and we want to have a way to increase the complexity of the noise without losing the overall<br/>
    ///     shape of it. To do this, we use multiple layers of noise (octaves). As we want each additional layer<br/>
    ///     to be more detailed, we increase the frequency of each layer (lacunarity) but as we also want each additional<br/>
    ///     layer to have less influence on the overall shape, we also decrease the amplitude of each layer (persistence).<br/>
    ///     A good analogy of this is that the first layer is the mountains, the second is the boulders, and the third the<br/>
    ///     small rocks, etc.
    /// </summary>
    public static class PerlinNoiseGenerator
    {
        public static float[,] GetNoiseMap
            (
                int mapWidth,
                int mapHeight,
                int seed,
                float scale,
                int octaves,
                float persistence,
                float lacunarity,
                Vector2 offset
            )
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            // Generate the map based on a seed
            var prng = new Random( seed );

            // We actually only work with a single Perlin noise map but we apply a random x and y offset to each
            // layer to target different areas of the map.
            Vector2[] octaveOffsets = new Vector2[octaves];
            for ( int i = 0; i < octaves; i++ )
            {
                float offsetX = prng.Next( -100000, 100000 ) + offset.x;
                float offsetY = prng.Next( -100000, 100000 ) + offset.y;
                octaveOffsets[i] = new Vector2( offsetX, offsetY );
            }

            // To obviate division by 0, we enforce a min value on the scale.
            scale = Mathf.Max( 0.0001f, scale );

            // Set the focal point of the noise to the center of the map so that scaling is relative to the center.
            float halfWidth = mapWidth / 2.0f;
            float halfHeight = mapHeight / 2.0f;

            // The noise map needs to be normalized to between 0 and 1 so we need to keep track of the min and max
            float minNoiseHeight = float.MaxValue;
            float maxNoiseHeight = float.MinValue;

            // We'll draw the map one horizontal line at a time, from left to right, starting from the bottom.
            //   ▲              
            // y │              
            //   │              
            //   └────────►  
            //          x   
            for ( int y = 0; y < mapHeight; y++ )
            {
                for ( int x = 0; x < mapWidth; x++ )
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for ( int i = 0; i < octaves; i++ )
                    {
                        // Get scaled x and y positions on the map.
                        float sampleX = ( x - halfWidth ) / scale;
                        float sampleY = ( y - halfHeight ) / scale;

                        // Multiply by the current lacunarity level, increasing the frequency.
                        sampleX *= frequency;
                        sampleY *= frequency;

                        // Apply offsets
                        sampleX += octaveOffsets[i].x;
                        sampleY += octaveOffsets[i].y;

                        // Get the Perlin noise value converted from [0,1] to [-1, 1].
                        float perlinValue = Mathf.PerlinNoise( sampleX, sampleY ).Convert0ToMaxToNegMaxToPosMax();

                        // Apply the current persistence level, increasing the amplitude.
                        noiseHeight += perlinValue * amplitude;

                        // Exponentially increment both amplitude and frequency to get the next persistence and lacunarity levels respectively.
                        // amplitude = persistence^{n} (persistence must be between 0 and 1)
                        amplitude *= persistence;

                        // frequency = lacunarity^{n} (lacunarity must always be greater than 1)
                        frequency *= lacunarity;
                    }

                    // Register min/max noise height to use for normalization.
                    minNoiseHeight = Mathf.Min( noiseHeight, minNoiseHeight );
                    maxNoiseHeight = Mathf.Max( noiseHeight, maxNoiseHeight );

                    // Write the finalized noise value to the 2D array.
                    noiseMap[x, y] = noiseHeight;
                }
            }

            return noiseMap.Normalize( minNoiseHeight, maxNoiseHeight );
        }
    }
}