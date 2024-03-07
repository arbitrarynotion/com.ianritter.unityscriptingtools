using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    public interface INoiseSource
    {
        void Initialize( int noiseMapWidth, int noiseMapHeight );
        float GetNoiseAtIndex( int x, int y );
        void GenerateNoiseMap();

    }

    /// <summary>
    ///     A component that can provide noise value at an x and y coordinate.
    /// </summary>
    public class NoiseModule : MonoBehaviour, INoiseSource
    {
        // Holds a NoiseSettingsSO
        [SerializeField] private NoiseSettingsSO settingsSo;

        private int _noiseMapWidth;
        private int _noiseMapHeight;
        
        // Holds a 2D noise map.
        private float[,] NoiseMap2D
        {
            get
            {
                if( _noiseMap2D == null )
                    GenerateNoiseMap();
                return _noiseMap2D;
            }
            set => _noiseMap2D = value;
        }
        private float[,] _noiseMap2D;


        // Noise map size can be set and changed.

        // Has an event that is raised when a setting to the noise map has changed.
        public UnityAction onSettingsUpdated;
        private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();


        public void Initialize( int noiseMapWidth, int noiseMapHeight )
        {
            _noiseMapWidth = noiseMapWidth;
            _noiseMapHeight = noiseMapHeight;
        }

        public float GetNoiseAtIndex( int x, int y ) => NoiseMap2D[x, y];
        
        public void GenerateNoiseMap() =>
            NoiseMap2D = NoiseGenerator.GenerateNoiseMap(
                _noiseMapWidth,
                _noiseMapHeight,
                settingsSo.seed,
                settingsSo.noiseScale,
                settingsSo.octaves,
                settingsSo.persistence,
                settingsSo.lacunarity,
                new Vector2( settingsSo.noiseOffsetHorizontal, settingsSo.noiseOffsetVertical )
            );
    }
}
