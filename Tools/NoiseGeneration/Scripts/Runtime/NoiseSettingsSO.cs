using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    [CreateAssetMenu(menuName = "Utilities/Noise Settings"), CanEditMultipleObjects]
    public class NoiseSettingsSO : SubscribableSO
    {
        public int seed = 31;
        [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
        [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
        [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;

        [Range( 1.0f, 8.0f )] public int octaves = 7;
        [Range( 0.5f, 1.0f )] public float persistence = 1f;
        [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
    }
}
