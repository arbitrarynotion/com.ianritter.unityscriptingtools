using System;
using System.Collections.Generic;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    /// <summary>
    ///     A component that can provide noise value at an x and y coordinate.
    /// </summary>
    public class NoiseModule : MonoBehaviour, INoiseSource, ILoggable
    {
        private static Dictionary<NoiseModule, Action> reloadHandlers = new Dictionary<NoiseModule, Action>();
        private Action noiseSettingsReloaded;
        
        // Holds a NoiseSettingsSO
        [SerializeField] private NoiseSettingsSO noiseSettingsSo;
        [SerializeField] private FormattedLogger logger;

        // Scene GUI Formatting
        [SerializeField] private bool showNoiseMeter = true;
        [SerializeField] [Range(0f, 250f)] private float mapPreviewTopMargin = 125f;
        [SerializeField] [Range(0f, 250f)] private float mapPreviewBottomMargin = 125f;
        [SerializeField] [Range(40f, 250f)] private float mapPreviewRightMargin = 43f;
        [SerializeField] [Range(0.025f, 1f)] private float mapPreviewWidth = 0.15f;
        [SerializeField] [Range(0.5f, 1f)] private float mapPreviewHeight = 0.5f;
        [SerializeField] [Range(0f, 250f)] private float mapPreviewLabelWidth = 80f;
        [SerializeField] [Range(0f, 250f)] private float mapPreviewLabelRightMargin = 0f;

        private NoiseSettingsSO _previousNoiseSettingsSo;

        [SerializeField] [HideInInspector] private int noiseMapWidth;
        [SerializeField] [HideInInspector] private int noiseMapHeight;

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
            // Only call for an update if the settings so was actually changed.
            if( noiseSettingsSo == _previousNoiseSettingsSo ) return;

            if( _subscriptionsHandler == null ) return;

            UpdateSubscriptions();

            settingsSOChangedCallback?.Invoke();
        }

        // [UnityEditor.Callbacks.DidReloadScripts]
        // private static void OnScriptsReloaded() {
        //     foreach (var module in reloadHandlers.Keys)
        //     {
        //         // Resubscribe to the event
        //         module.noiseSettings.OnSettingsReloaded += reloadHandlers[module];
        //     }
        // }
#endregion


#region Interface

        public void Initialize( int newNoiseMapWidth, int newNoiseMapHeight, UnityAction settingsSOChangedCallBack, UnityAction noiseSettingsChangedCallBack )
        {
            logger.LogStart( true, true );

            noiseMapWidth = newNoiseMapWidth;
            noiseMapHeight = newNoiseMapHeight;
            settingsSOChangedCallback = settingsSOChangedCallBack;
            noiseSettingsChangedCallback = noiseSettingsChangedCallBack;

            _subscriptionsHandler.Initialize( logger );
            
            UpdateSubscriptions();

            GenerateNoiseMap();

            logger.LogEnd();
        }
        
        public void UpdateNoiseMapSize( int noiseMapWidth, int noiseMapHeight )
        {
            this.noiseMapWidth = noiseMapWidth;
            this.noiseMapHeight = noiseMapHeight;

            GenerateNoiseMap();
        }

        public float GetNoiseAtIndex( int x, int y ) => NoiseMap2D[x, y];

        [FormattedAutoLogger]
        public float[,] Get2DNoiseMap() => NoiseMap2D;

#endregion


#region EventMethods

        private void OnNoiseSettingsSOUpdated() => noiseSettingsChangedCallback?.Invoke();

        private void UpdateSubscriptions() => _previousNoiseSettingsSo = (NoiseSettingsSO) _subscriptionsHandler.UpdateSubscriptions( _previousNoiseSettingsSo, noiseSettingsSo, OnNoiseSettingsSOUpdated );

#endregion


#region Helpers

        private void GenerateNoiseMap()
        {
            if( noiseSettingsSo == null ) return;

            NoiseMap2D = NoiseGenerator.GetNoiseMap(
                noiseMapWidth,
                noiseMapHeight,
                noiseSettingsSo.seed,
                noiseSettingsSo.noiseScale,
                noiseSettingsSo.octaves,
                noiseSettingsSo.persistence,
                noiseSettingsSo.lacunarity,
                new Vector2( noiseSettingsSo.noiseOffsetHorizontal, noiseSettingsSo.noiseOffsetVertical )
            );
        }

#endregion

        public FormattedLogger GetFormattedLogger() => logger;
        public string GetName() => name;
    }
}