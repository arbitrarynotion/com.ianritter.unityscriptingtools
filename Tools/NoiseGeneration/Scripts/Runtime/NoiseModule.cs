using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using UnityEngine.Events;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    /// <summary>
    ///     A component that can provide noise value at an x and y coordinate.
    /// </summary>
    public class NoiseModule : MonoBehaviour, INoiseSource
    {
        // Holds a NoiseSettingsSO
        [SerializeField] private NoiseSettingsSO noiseSettingsSo;
        [SerializeField] private FormattedLogger logger;
        private NoiseSettingsSO _previousNoiseSettingsSo;

        private int _noiseMapWidth;
        private int _noiseMapHeight;

        [SerializeField] private UnityAction settingsSOChangedCallback;
        [SerializeField] private UnityAction noiseSettingsChangedCallback;

        private SubscriptionsHandler _subscriptionsHandler;

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

#region LifeCycle

        public void OnValidate()
        {
            string current = noiseSettingsSo != null ? noiseSettingsSo.name : "null";
            string previous = _previousNoiseSettingsSo != null ? _previousNoiseSettingsSo.name : "null";
            logger.Log( $"SettingsSOs: current {GetColoredStringYellowGreen( current )}, previous: {GetColoredStringGoldenrod( previous )}" );

            // Only call for an update if the settings so was actually changed.

            UpdateSubscriptions();

            if( noiseSettingsSo == _previousNoiseSettingsSo ) return;

            settingsSOChangedCallback?.Invoke();
        }

#endregion


#region Interface

        // Noise map size can be set and changed.
        public void Initialize( int noiseMapWidth, int noiseMapHeight, UnityAction settingsSOChangedCallBack, UnityAction noiseSettingsChangedCallBack )
        {
            logger.LogStart( true, true );

            _noiseMapWidth = noiseMapWidth;
            _noiseMapHeight = noiseMapHeight;
            settingsSOChangedCallback = settingsSOChangedCallBack;
            noiseSettingsChangedCallback = noiseSettingsChangedCallBack;

            _subscriptionsHandler.Initialize( logger );

            GenerateNoiseMap();

            logger.LogEnd();
        }

        public void UpdateNoiseMapSize( int noiseMapWidth, int noiseMapHeight )
        {
            _noiseMapWidth = noiseMapWidth;
            _noiseMapHeight = noiseMapHeight;

            GenerateNoiseMap();
        }

        public float GetNoiseAtIndex( int x, int y ) => NoiseMap2D[x, y];

        public float[,] Get2DNoiseMap() => NoiseMap2D;

        public NoiseSettingsSO GetNoiseSettingsSO() => noiseSettingsSo;

#endregion

        private void OnNoiseSettingsSOUpdated() => noiseSettingsChangedCallback?.Invoke();

        private void UpdateSubscriptions()
        {
            // string current = noiseSettingsSo != null ? noiseSettingsSo.name : "null";
            // string previous = _previousNoiseSettingsSo != null ? _previousNoiseSettingsSo.name : "null";
            // logger.LogStart( $"current {GetColoredStringYellowGreen( current )}, previous: {GetColoredStringGoldenrod( previous )}" );

            _previousNoiseSettingsSo = (NoiseSettingsSO) _subscriptionsHandler.UpdateSubscriptions( _previousNoiseSettingsSo, noiseSettingsSo, OnNoiseSettingsSOUpdated );

            // current = noiseSettingsSo != null ? noiseSettingsSo.name : "null";
            // previous = _previousNoiseSettingsSo != null ? _previousNoiseSettingsSo.name : "null";
            // logger.LogEnd($"current {GetColoredStringYellowGreen( current )}, previous: {GetColoredStringGoldenrod( previous )}");
        }

        private void GenerateNoiseMap()
        {
            if( noiseSettingsSo == null ) return;

            NoiseMap2D = NoiseGenerator.GetNoiseMap(
                _noiseMapWidth,
                _noiseMapHeight,
                noiseSettingsSo.seed,
                noiseSettingsSo.noiseScale,
                noiseSettingsSo.octaves,
                noiseSettingsSo.persistence,
                noiseSettingsSo.lacunarity,
                new Vector2( noiseSettingsSo.noiseOffsetHorizontal, noiseSettingsSo.noiseOffsetVertical )
            );
        }
    }
}