using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    [ExecuteInEditMode]
    [RequireComponent( typeof( NoiseModule ) )]
    public class NoiseMapSceneViewPreviewer : MonoBehaviour
    {
        private const float TextColorDefault = 0.80f;
        private const float FrameColorDefault = 0.15f;
        private const float BackgroundColorDefault = 0.25f;

        //  Scene GUI Formatting
        // Note that these will appear to be unused because they are used by this class's editor via their serialized properties.
        [SerializeField] private bool showNoiseMeter = true;

        [SerializeField] [Range( 0.025f, 1f )] private float mapPreviewWidth = 0.15f;
        [SerializeField] [Range( 0.5f, 1f )] private float mapPreviewHeight = 0.5f;

        [SerializeField] private Color highColor = new Color( 1f, 1f, 1f, 1f );
        [SerializeField] private Color lowColor = new Color( 0f, 0f, 0f, 1f );
        [SerializeField] private Color textColor = new Color( TextColorDefault, TextColorDefault, TextColorDefault, 1f );
        [SerializeField] private Color frameColor = new Color( FrameColorDefault, FrameColorDefault, FrameColorDefault, 1f );
        [SerializeField] private Color backgroundColor = new Color( BackgroundColorDefault, BackgroundColorDefault, BackgroundColorDefault, 1f );

        [SerializeField] [Range( 1f, 10f )] private float frameWidth = 2f;
        [SerializeField] [Range( 0f, 250f )] private float mapPreviewTopMargin = 125f;
        [SerializeField] [Range( 0f, 250f )] private float mapPreviewBottomMargin = 125f;
        [SerializeField] [Range( 40f, 250f )] private float mapPreviewRightMargin = 43f;

        [SerializeField] private FormattedLogger logger;

        [SerializeField] [HideInInspector] private ScaleMode mapScaleMode = ScaleMode.StretchToFill;
        [SerializeField] [HideInInspector] private INoiseSource noiseModule;

        private void OnEnable()
        {
            logger.LogStart();

            // Load the noise module attached to this object.
            noiseModule = GetComponent<INoiseSource>();
            logger.LogObjectAssignmentResult( nameof( noiseModule ), noiseModule == null, FormattedLogType.Standard );

            noiseModule.SubscribeToNoiseMapChanges( SetMapScale );

            logger.LogEnd();
        }

        public float[,] Get2DNoiseMap() => noiseModule?.Get2DNoiseMap();

        private void SetMapScale( int noiseMapWidth, int noiseMapHeight )
        {
            // If one of the dimensions is 1, the map is one dimensional.
            mapScaleMode = ( noiseMapWidth == 1 ) || ( noiseMapHeight == 1 ) ? ScaleMode.StretchToFill : ScaleMode.ScaleAndCrop;
        }
    }
}