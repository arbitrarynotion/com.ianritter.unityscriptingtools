
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.NoiseMaps.NoiseMap;
using UnityEngine;

namespace SpaceSim.Scripts.Runtime.Managers.Noise
{
    [ExecuteInEditMode]
    public class NoiseMapManager : MonoBehaviour
    // public class NoiseMapManager : MonoBehaviour
    {
        // [Header("Editor Toggles")]
        public bool showNoiseMap;
        public bool autoUpdate = true;
        // [SerializeField] private CustomLogger logger;
    
        // [Header("Noise Settings")]
        [SerializeField] private int seed = 31;
        [Range( 10, 100 )] [SerializeField] private int noiseResolution = 100;
        [Range( -5.0f, 5.0f )] [SerializeField] private float noiseOffsetHorizontal;
        [Range( -5.0f, 5.0f )] [SerializeField] private float noiseOffsetVertical;
        [Range( 1.1f, 50.0f )] [SerializeField] private float noiseScale = 25f;

        [Range( 1.0f, 8.0f )] [SerializeField] private int octaves = 3;
        [Range( 0.5f, 1.0f )] [SerializeField] private float persistence = 0.35f;
        [Range( 0.5f, 1.5f )] [SerializeField] private float lacunarity = 1.08f;
    
        // [Header("Noise Render Settings")]
        [SerializeField] private Color lowColor = Color.white;
        [SerializeField] private Color highColor = Color.black;
        [SerializeField] [Range( 0.0f, 1.0f )] private float colorAlpha = 0.5f;
        [SerializeField] private Renderer textureRenderer;

        public delegate void NoiseMapUpdated();
        public event NoiseMapUpdated OnNoiseMapUpdated;
        public void NoiseMapUpdatedNotify() => OnNoiseMapUpdated?.Invoke();

        // public override string GetLoggerName() => "NoiseMapManagerLogger";
        
        private float[,] _noiseMap;

        private void OnEnable()
        {
            if ( Application.isPlaying ) textureRenderer.enabled = false;
        }

        // public void Initialize( GameManager gameManager )
        // {
        //     _gameManager = gameManager;
        //     GenerateNoiseMap();
        // }

        public int GetSeed() => seed;
        
        // private void SetNoiseMapPlaneScale() => textureRenderer.transform.localScale = Vector3.one * ( _gameManager.GetWorldSize() / 10 );

        public void GenerateNoiseMap()
        {
            ApplyAlphaToColors();
            
            // SetNoiseMapPlaneScale();

            // if ( _mapDisplay == null ) _mapDisplay = new MapDisplay();

            _noiseMap = NoiseMapGenerator.GenerateNoiseMap(
                noiseResolution,
                noiseResolution,
                seed,
                noiseScale,
                octaves,
                persistence,
                lacunarity,
                new Vector2( noiseOffsetHorizontal, noiseOffsetVertical ) );

            MapDisplay.DrawNoiseMap( textureRenderer, _noiseMap, lowColor, highColor );
        }

        private void ApplyAlphaToColors()
        {
            lowColor.a = colorAlpha;
            highColor.a = colorAlpha;
        }

        public float[,] GetNoiseMap() => _noiseMap;

        public Renderer GetNoiseMapPlane() => textureRenderer;
        
        public Texture2D GetNoiseMapAs2DTexture() => (Texture2D) textureRenderer.sharedMaterial.mainTexture;
    }
}