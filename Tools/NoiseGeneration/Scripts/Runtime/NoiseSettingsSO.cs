using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    [CreateAssetMenu(menuName = "Utilities/Noise Settings")]
    public class NoiseSettingsSO : SubscribableSO
    {
        public int seed = 31;
        [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
        [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
        [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;

        [Range( 1.0f, 8.0f )] public int octaves = 7;
        [Range( 0.5f, 1.0f )] public float persistence = 1f;
        [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
        
        public bool showNoiseMeter = true;
        // Scene GUI Formatting
        public float noiseMapTopMargin = 125f;
        public float noiseMapRightMargin = 43f;
        public float noiseMapWidth = 25f;
        public float noiseMapLabelWidth = 80f;
        public float noiseMapLabelRightMargin = 0f;
        
        public float[,] GetNoiseMap2D( int mapWidth, int mapHeight ) =>
            NoiseGenerator.GetNoiseMap(
                mapWidth,
                mapHeight,
                seed,
                noiseScale,
                octaves,
                persistence,
                lacunarity,
                new Vector2
                ( 
                    noiseOffsetHorizontal, 
                    noiseOffsetVertical 
                )
            );
    }
}
