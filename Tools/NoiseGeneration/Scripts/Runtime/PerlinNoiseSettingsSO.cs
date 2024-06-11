using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    public interface INoiseSettings
    {
        float[,] GenerateNoiseMap( int noiseMapWidth, int noiseMapHeight );
    }

    public abstract class SubscribableNoiseSettingsSO : SubscribableSO, INoiseSettings
    {
        public float[,] GenerateNoiseMap( int noiseMapWidth, int noiseMapHeight ) => GetNoiseMap( noiseMapWidth, noiseMapHeight );

        protected abstract float[,] GetNoiseMap( int noiseMapWidth, int noiseMapHeight );
    }
    
    // Note: I decided not to go with an INoiseSettingsSO because treating this SO like an interface
    // would result in the SubscribableSO no longer working correctly.
    [CreateAssetMenu(menuName = "Utilities/Noise Settings/Perlin Noise Settings"), CanEditMultipleObjects]
    public class PerlinNoiseSettingsSO : SubscribableNoiseSettingsSO
    {
        public int seed = 31;
        [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
        [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
        [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;

        [Range( 1.0f, 8.0f )] public int octaves = 7;
        [Range( 0.5f, 1.0f )] public float persistence = 1f;
        [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
        
        protected override float[,] GetNoiseMap( int noiseMapWidth, int noiseMapHeight ) =>
            PerlinNoiseGenerator.GetNoiseMap(
                noiseMapWidth,
                noiseMapHeight,
                seed,
                noiseScale,
                octaves,
                persistence,
                lacunarity,
                new Vector2( noiseOffsetHorizontal, noiseOffsetVertical )
            );
    }
}
