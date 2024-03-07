using UnityEngine;

using Random = System.Random;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    public static class PerlinNoise
    {
        public static float[,] GenerateNoiseMap
        (
            int mapWidth,
            int mapHeight,
            float scale,
            int octaves,
            float persistence,
            float lacunarity
        )
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            // Ensure we don't divide by 0.
            scale = Mathf.Max( 0.0001f, scale );

            // We'll draw the map one horizontal line at a time, starting from the bottom.
            //   ▲              
            // y │              
            //   │              
            //   └────────►  
            //          x    
            for ( int y = 0; y < mapHeight; y++ )
            {
                for ( int x = 0; x < mapWidth; x++ )
                {
                    noiseMap[x, y] = GetPerlinNoiseSampleAtCoords(
                        x,
                        y,
                        scale,
                        octaves,
                        persistence,
                        lacunarity
                    );
                }
            }

            return noiseMap;
        }


        private static float GetPerlinNoiseSampleAtCoords
        (
            int x,
            int y,
            float scale,
            int octaves,
            float persistence,
            float lacunarity
        )
        {
            /*
               Persistence:  
               - Effects how much the small features introduced by Lacunarity influence the overall shape of the noise.
               - Practically, this means it multiplies the height of a noise sample effectively increasing its amplitude.
               - Controls the amplitude of the octaves (squishing or stretching them vertically).  
                   - amplitude = persistence^{n}, where n is the layer number.
               Persistence is the intensity (amplitude) of the noise height and it is exponentially decreased across the octaves (noise layers).
            */
            float amplitude = 1;

            /*
                Lacunarity: 
                - Increases the amount of small features in the noise.
                - Practically, this means is multiplies the 
                - Controls the frequency of the octaves (squishing or stretching horizontally). 
                    - frequency = lacunarity^{n}, where n is the layer number. 
                Lacunarity is the density of detail (frequency) of the noise and it increases exponentially across the octaves (noise layers)
            */
            float frequency = 1;

            float noiseHeight = 0;

            for ( int i = 0; i < octaves; i++ )
            {
                // Determine the sample location.
                float sampleX = x / scale * frequency;
                float sampleY = y / scale * frequency;

                // Get the Perlin noise sample at point x, y. The value returns is between 0 and 1, multiply by 2 and subtract 1 to make it between -1 and 1.
                float perlinValue = Mathf.PerlinNoise( sampleX, sampleY ) * 2 - 1;

                // Apply the amplitude to the resulting noise height.
                noiseHeight += perlinValue * amplitude;

                // As the following are added every loop, they become exponential.
                // amplitude = persistence^{n} (persistence must be between 0 and 1)
                amplitude *= persistence;

                // frequency = lacunarity^{n} (lacunarity must always be greater than 1)
                frequency *= lacunarity;
            }


            return noiseHeight;
        }
    }


    /// <summary>
    ///     To have the option of making Perlin noise less smooth in its tone transitions, we use octaves.<br/>
    ///     Octaves are layers of noise combined with each octave representing another layer.<br/>
    ///     Octave layers are additive, but each subsequent layer has less effect on the overall noise than the previous layer.<br/>
    ///     To control how the octaves are applied we use two values:<br/>
    ///     - Lacunarity:<br/>
    ///     This controls the frequency of the octaves (squishing or stretching horizontally).<br/>
    ///     More specifically: frequency = lacunarity^{n}, where n is the layer number.<br/>
    ///     - Persistence:<br/>
    ///     This controls the amplitude of the octaves (squishing or stretching them vertically).<br/>
    ///     More specifically: amplitude = persistence^{n}, where n is the layer number.<br/>
    /// </summary>
    public static class NoiseGenerator
    {
        public static float[,] GenerateNoiseMap
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


            Vector2[] octaveOffsets = new Vector2[octaves];
            for ( int i = 0; i < octaves; i++ )
            {
                float offsetX = prng.Next( -100000, 100000 ) + offset.x;
                float offsetY = prng.Next( -100000, 100000 ) + offset.y;
                octaveOffsets[i] = new Vector2( offsetX, offsetY );
            }

            // if ( scale <= 0 ) scale = 0.0001f;
            scale = Mathf.Max( 0.0001f, scale );

            float halfWidth = mapWidth / 2.0f;
            float halfHeight = mapHeight / 2.0f;


            // The noise map needs to be normalized to between 0 and 1 so we need to keep track of the min and max
            float minNoiseHeight = float.MaxValue;
            float maxNoiseHeight = float.MinValue;

            for ( int y = 0; y < mapHeight; y++ )
            {
                for ( int x = 0; x < mapWidth; x++ )
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for ( int i = 0; i < octaves; i++ )
                    {
                        // float sampleX = ( x - halfWidth ) / scale * frequency + octaveOffsets[i].x; // The higher the frequency, the further apart the 
                        // float sampleY = ( y - halfHeight ) / scale * frequency + octaveOffsets[i].y; // sample points so height values will change more rapidly
                        float scaledFrequency = scale * frequency;

                        float sampleX = ( x - halfWidth ) / scaledFrequency;
                        float sampleY = ( y - halfHeight ) / scaledFrequency;

                        sampleX += octaveOffsets[i].x;
                        sampleY += octaveOffsets[i].y;

                        // sampleX += octaveOffsets[i].x / scaledFrequency;
                        // sampleY += octaveOffsets[i].y / scaledFrequency;

                        float perlinValue = Mathf.PerlinNoise( sampleX, sampleY ) * 2 - 1; // *2-1 takes the 0 to 1 range and makes it a -1 to 1 range

                        noiseHeight += perlinValue * amplitude; // Increase noise height by the perlin value of each octave

                        amplitude *= persistence; // Amplitude decreases with each octave
                        frequency *= lacunarity; // Frequency increase with each octave since lacunarity > 1
                    }

                    // Register min/max noise height to use for normalization.
                    minNoiseHeight = Mathf.Min( noiseHeight, minNoiseHeight );
                    maxNoiseHeight = Mathf.Max( noiseHeight, maxNoiseHeight );

                    // if ( noiseHeight > maxNoiseHeight )
                    //     maxNoiseHeight = noiseHeight;
                    // else if ( noiseHeight < minNoiseHeight ) minNoiseHeight = noiseHeight;
                    noiseMap[x, y] = noiseHeight;
                }
            }

            // Normalize noise map values using the min and max heights registered
            for ( int y = 0; y < mapHeight; y++ )
            {
                for ( int x = 0; x < mapWidth; x++ )
                {
                    noiseMap[x, y] = Mathf.InverseLerp( minNoiseHeight, maxNoiseHeight, noiseMap[x, y] ); // InverseLerp assigns a value from 0 to 1 relative to the min and max specified
                }
            }

            return noiseMap;
        }

        // private static float GetPerlinNoiseSampleAtCoords(
        //     int x,
        //     int y,
        //     float scale,
        //     int octaves,
        //     float persistence,
        //     float lacunarity
        // )
        // {
        //
        // }


        public static float[,] GetNoiseMapScaledToGrid( float[,] noiseMap, int gridWidth, int gridHeight, int gridSizeX, int gridSizeY )
        {
            float[,] scaledNoiseMap = new float[gridSizeX, gridSizeY];
            for ( int y = 0; y < gridHeight; y++ )
            {
                for ( int x = 0; x < gridWidth; x++ )
                {
                    scaledNoiseMap[x, y] = GetNoiseValueAtCoords( noiseMap, x, y, gridWidth, gridHeight, gridSizeX, gridSizeY );
                }
            }

            return scaledNoiseMap;
        }

        /// <summary>
        ///     Given a noise map and a grid, provides the noise sample at the requested coordinates. Scales the coords to fit the noise map size.
        /// </summary>
        public static float GetNoiseValueAtCoords( float[,] noiseMap, int x, int y, int gridWidth, int gridHeight, int gridSizeX, int gridSizeY )
        {
            int noiseMapWidth = noiseMap.GetLength( 0 );
            int noiseMapHeight = noiseMap.GetLength( 1 );

            float nodeWidth = gridWidth / (float) gridSizeX;
            float nodeHeight = gridHeight / (float) gridSizeY;

            // The sampling coords may not share the scale of the noise map so we have to normalize
            // the x and y values to be relative to the noise map.
            float halfCell = nodeWidth / 2f;
            float percentOfWidth = ( x * nodeWidth + halfCell ) / gridWidth;
            float percentOfHeight = ( y * nodeHeight + halfCell ) / gridHeight;

            int xSample = Mathf.FloorToInt( percentOfWidth * noiseMapWidth );
            int ySample = Mathf.FloorToInt( percentOfHeight * noiseMapHeight );

            return noiseMap[xSample, ySample];
        }


        // public static float[] GetPerlinNoiseArray(
        //     int arraySize,
        //     int seed,
        //     float noiseScale,
        //     int octaves,
        //     float persistence,
        //     float lacunarity,
        //     float noiseOffsetHorizontal,
        //     float noiseOffsetVertical
        // )
        // {
        //     float[] noiseMap = new float[arraySize];
        //
        //     // Generate the map based on a seed
        //     var prng = new Random( seed );
        //     // var prng = new Random();
        //     Vector2[] octaveOffsets = new Vector2[octaves];
        //     for (int i = 0; i < octaves; i++)
        //     {
        //         float offsetX = prng.Next( -100000, 100000 ) + noiseOffsetHorizontal;
        //         float offsetY = prng.Next( -100000, 100000 ) + noiseOffsetVertical;
        //         octaveOffsets[i] = new Vector2( offsetX, offsetY );
        //     }
        //
        //     if ( noiseScale <= 0 ) noiseScale = 0.0001f;
        //
        //     // The noise map needs to be normalized to between 0 and 1 so we need to keep track of the min and max
        //     float minNoiseHeight = float.MaxValue;
        //     float maxNoiseHeight = float.MinValue;
        //
        //     for (int y = 0; y < arraySize; y++){
        //
        //         float amplitude = 1;
        //         float frequency = 1;
        //         float noiseHeight = 0;
        //
        //         for (int i = 0; i < octaves; i++){
        //             float sampleX = (1 - 0) / noiseScale * frequency + octaveOffsets[i].x; // The higher the frequency, the further apart the 
        //             float sampleY = (y - 0) / noiseScale * frequency + octaveOffsets[i].y; // sample points so height values will change more rapidly
        //
        //             float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2; // *2-1 takes the 0 to 1 range and makes it a -1 to 1 range
        //
        //             noiseHeight += perlinValue * amplitude; // Increase noise height by the perlin value of each octave
        //
        //             amplitude *= persistence; // Amplitude decreases with each octave
        //             frequency *= lacunarity; // Frequency increase with each octave since lacunarity > 1
        //         }
        //
        //         // Register min/max noise height
        //         if (noiseHeight > maxNoiseHeight){
        //             maxNoiseHeight = noiseHeight;
        //         }else if (noiseHeight < minNoiseHeight){
        //             minNoiseHeight = noiseHeight;
        //         }
        //         noiseMap[y] = noiseHeight;
        //     }
        //
        //     return noiseMap;
        // }
    }
}