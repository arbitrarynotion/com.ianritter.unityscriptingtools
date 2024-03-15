using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    /// <summary>
    ///     A component that can provide noise value at an x and y coordinate.
    /// </summary>
    // public class NoiseModule : MonoBehaviour, INoiseSource, ILoggable
    public class NoiseModule : MonoBehaviour, INoiseSource
    {
        private const float TextColorDefault = 0.80f;
        private const float FrameColorDefault = 0.25f;
        private const float BackgroundColorDefault = 0.15f;
        
        // Holds a NoiseSettingsSO
        // [SerializeField] private PerlinNoiseSettingsSO noiseSettingsSo;
        [SerializeField] private SubscribableNoiseSettingsSO noiseSettingsSo;
        [SerializeField] private FormattedLogger logger;

        // Scene GUI Formatting
        // [SerializeField] private bool showNoiseMeter = true;
        // [SerializeField] private Color highColor = new Color( 1f, 1f, 1f, 1f );
        // [SerializeField] private Color lowColor = new Color( 0f, 0f, 0f, 1f );
        // [SerializeField] private Color textColor = new Color( TextColorDefault, TextColorDefault, TextColorDefault, 1f );
        // [SerializeField] private Color frameColor = new Color( FrameColorDefault, FrameColorDefault, FrameColorDefault, 1f );
        // [SerializeField] private Color backgroundColor = new Color( FrameColorDefault, FrameColorDefault, FrameColorDefault, 1f );
        // [SerializeField] [Range( 1f, 10f )] private float frameWidth = 2f;
        // [SerializeField] [Range( 0f, 250f )] private float mapPreviewTopMargin = 125f;
        // [SerializeField] [Range( 0f, 250f )] private float mapPreviewBottomMargin = 125f;
        // [SerializeField] [Range( 40f, 250f )] private float mapPreviewRightMargin = 43f;
        // [SerializeField] [Range( 0.025f, 1f )] private float mapPreviewWidth = 0.15f;
        // [SerializeField] [Range( 0.5f, 1f )] private float mapPreviewHeight = 0.5f;
        // [SerializeField] [Range( 0f, 250f )] private float mapPreviewLabelWidth = 80f;
        // [SerializeField] [Range( 0f, 250f )] private float mapPreviewLabelRightMargin;
        // [SerializeField] [HideInInspector] private int noiseMapHeight;
        // [SerializeField] [HideInInspector] private ScaleMode mapScaleMode = ScaleMode.StretchToFill;

        private SubscribableNoiseSettingsSO _previousNoiseSettingsSo;

        
        [SerializeField] private UnityAction settingsSOChangedCallback;
        [SerializeField] private UnityAction noiseSettingsChangedCallback;
        
        private int NoiseMapWidth { get; set; }
        private int NoiseMapHeight { get; set; }

        public UnityAction<int, int> onNoiseMapSizeUpdated;
        private void RaiseOnNoiseMapSizeUpdated() => onNoiseMapSizeUpdated?.Invoke( NoiseMapWidth, NoiseMapHeight );

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
        // Note: As this class works as a service, it initializes on the call to Initialize instead of during OnEnable.

        private void OnDisable()
        {
            logger.LogStart( true );

            UpdateSubscriptions();

            logger.LogEnd();
        }
        
        public void OnValidate()
        {
            // Only call for an update if the settings so was actually changed.
            if( noiseSettingsSo == _previousNoiseSettingsSo ) return;

            UpdateSubscriptions();

            settingsSOChangedCallback?.Invoke();
        }

#endregion


#region Interface

        public void Initialize( int newNoiseMapWidth, int newNoiseMapHeight, UnityAction settingsSOChangedCallBack, UnityAction noiseSettingsChangedCallBack )
        {
            logger.LogStart( true, true );

            // Update subscriptions first so that they will respond to the change in the noise map size.
            UpdateSubscriptions( settingsSOChangedCallBack, noiseSettingsChangedCallBack );
            UpdateNoiseMapSize( newNoiseMapWidth, newNoiseMapHeight );

            logger.LogEnd();
        }

        public void UpdateNoiseMapSize( int newNoiseMapWidth, int newNoiseMapHeight )
        {
            NoiseMapWidth = newNoiseMapWidth;
            NoiseMapHeight = newNoiseMapHeight;
            
            // SetMapScale();
            RaiseOnNoiseMapSizeUpdated();

            GenerateNoiseMap();
        }

        public float GetNoiseAtIndex( int x, int y ) => NoiseMap2D[x, y];

        // [FormattedAutoLogger]
        public float[,] Get2DNoiseMap() => NoiseMap2D;
        
        public void SubscribeToNoiseMapChanges( UnityAction<int, int> callback ) => onNoiseMapSizeUpdated += callback;

#endregion


#region EventMethods

        private void OnNoiseSettingsSOUpdated() => noiseSettingsChangedCallback?.Invoke();
        
        private void UpdateSubscriptions()
        {
            _previousNoiseSettingsSo = (SubscribableNoiseSettingsSO) SubscriptionsHandler.UpdateSubscriptions
            (
                _previousNoiseSettingsSo,
                noiseSettingsSo,
                OnNoiseSettingsSOUpdated,
                logger
            );
        }

#endregion


#region Helpers
        
        private void UpdateSubscriptions( UnityAction settingsSOChangedCallBack, UnityAction noiseSettingsChangedCallBack )
        {
            settingsSOChangedCallback = settingsSOChangedCallBack;
            noiseSettingsChangedCallback = noiseSettingsChangedCallBack;

            UpdateSubscriptions();
        }

        // private void SetMapScale()
        // {
        //     // If one of the dimensions is 1, the map is one dimensional.
        //     mapScaleMode = ( NoiseMapWidth == 1 ) || ( noiseMapHeight == 1 ) ? ScaleMode.StretchToFill : ScaleMode.ScaleAndCrop;
        // }

        private void GenerateNoiseMap() => NoiseMap2D = noiseSettingsSo.GenerateNoiseMap( NoiseMapWidth, NoiseMapHeight );

#endregion

        // These are for the ILoggable interface - part of the auto logger I was working on.
        // public FormattedLogger GetFormattedLogger() => logger;
        // public string GetName() => name;
    }
}