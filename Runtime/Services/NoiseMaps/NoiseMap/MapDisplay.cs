using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.NoiseMaps.NoiseMap
{
    public static class MapDisplay
    {
        public static void DrawNoiseMap( Renderer textureRenderer, float[,] noiseMap, Color lowColor, Color highColor )
        {
            textureRenderer.sharedMaterial.mainTexture = GetNoiseMapTexture( noiseMap, lowColor, highColor );
        }

        public static Texture2D GetNoiseMapTexture( float[,] noiseMap, Color lowColor, Color highColor )
        {
            int width = noiseMap.GetLength( 0 );
            int height = noiseMap.GetLength( 1 );

            var texture = new Texture2D( width, height );

            Color[] colorMap = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp( lowColor, highColor, noiseMap[x, y] );
                }
            }

            texture.SetPixels( colorMap );
            texture.Apply();

            return texture;
        }
        
        public static Texture2D GetNoiseMapTexture( float[] noiseMap, Color lowColor, Color highColor )
        {
            int width = noiseMap.GetLength( 0 );
            int height = 5;

            var texture = new Texture2D( width, height );

            Color[] colorMap = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp( lowColor, highColor, noiseMap[x] );
                }
            }

            texture.SetPixels( colorMap );
            texture.Apply();

            return texture;
        }
    }
}