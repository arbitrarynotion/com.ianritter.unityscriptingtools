using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Runtime._Testing.CardStacks
{
    [CreateAssetMenu(menuName = "Utilities/Object Stacker Settings")]
    public class ObjectStackerSettingsSO : ScriptableObject
    {
        // private const int CardCount = 52;
        
        // [Header("Noise Generation")]
        public int seed = 31;
        [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
        [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
        [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;

        [Range( 1.0f, 8.0f )] public int octaves = 7;
        [Range( 0.5f, 1.0f )] public float persistence = 1f;
        [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
        
        public AnimationCurve noiseDampeningCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
        
        // [Header("Offsets")]
        [Range( 0.0001f, 0.001f )] public float verticalOffset = 0.0003f;
        
        // [Header("Rotation Effects")]
        [Range( -10f, 10f )] public float yRotOffset = 0.05f;
        [Range( -0.5f, 0.5f )] public float deckYRotSkew = -0.244f;
        [Range( 1, 52 )] public int topSkewCardCount = 7;
        [Range( -100f, 100f )] public float topCardsYRotSkew = 39f;
        public AnimationCurve rotationDampeningCurve = new AnimationCurve( new []
            {
                new Keyframe( time: 
                    -0.0035514832f,
                    0.0022468567f,
                    0.02133862f,
                    0.02133862f,
                    inWeight: 0,
                    outWeight: 0.69152474f 
                ),
                new Keyframe(
                0.1272265f,
                0.32834244f,
                8.195747f,
                8.195747f,
                0.33333334f,
                0.33333334f
                ),
                new Keyframe(
                0.16060005f,
                0.7679692f,
                5.6655655f,
                5.6655655f,
                0.33333334f,
                0.33333334f
                ),
                new Keyframe(
                0.3245624f,
                0.46598676f,
                -3.7884986f,
                -3.7884986f,
                0.22430015f,
                0.09893375f
                ),
                new Keyframe(
                0.61950034f,
                0.06995833f,
                -0.27939424f,
                -0.27939424f,
                0.21457842f,
                0.16766956f
                ),
                new Keyframe(
                1.00354f,
                0.0022468567f,
                -0.111717f,
                -0.111717f,
                0.20951769f,
                0f
                )
            } 
        );
        
        // [Header("Position Effects")]
        [Range( -10f, 10f )] 
        public float positionAffected = 0f;

        // [Header("Orientation")]
        public bool faceUp = false;

        [SerializeField]
        public UnityAction onSettingsUpdated;
        private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();

        private void OnValidate()
        {
            // Debug.Log( "ObjectStackerSettingsSO OnValidate called." );

            RaiseOnSettingsUpdated();
        }
    }
}