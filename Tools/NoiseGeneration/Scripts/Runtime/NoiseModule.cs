using System;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    public interface INoiseSource
    {
        void Initialize( int noiseMapWidth, int noiseMapHeight );
        float GetNoiseAtIndex( int x, int y );
        void GenerateNoiseMap();
        NoiseSettingsSO GetNoiseSettingsSO();
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

        private bool _initialized = false;
        
        // Holds a 2D noise map.
        private float[,] NoiseMap2D
        {
            get
            {
                if ( !_initialized )
                    Debug.LogWarning( "Error! You need to initialized the noise module before getting its noise map!" );
                
                if( _noiseMap2D == null )
                    GenerateNoiseMap();
                return _noiseMap2D;
            }
            set => _noiseMap2D = value;
        }
        private float[,] _noiseMap2D;

        
        // Has an event that is raised when a setting to the noise map has changed.
        public UnityAction onSettingsUpdated;
        private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();
        

        private void OnDisable()
        {
            _initialized = false;
        }

        // Noise map size can be set and changed.
        public void Initialize( int noiseMapWidth, int noiseMapHeight )
        {
            _noiseMapWidth = noiseMapWidth;
            _noiseMapHeight = noiseMapHeight;
            _initialized = true;
            GenerateNoiseMap();
        }

        public float GetNoiseAtIndex( int x, int y ) => NoiseMap2D[x, y];
        
        public void GenerateNoiseMap()
        {
            if ( !_initialized ) return;
            
            NoiseMap2D = NoiseGenerator.GetNoiseMap(
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

        public NoiseSettingsSO GetNoiseSettingsSO() => settingsSo;
    }
}
