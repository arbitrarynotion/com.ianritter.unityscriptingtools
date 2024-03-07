using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime
{
    // public interface ISettingsSO
    // {
    //     Action onSettingsUpdated;
    //     void RaiseOnSettingsUpdated();
    // }
    
    
    [CreateAssetMenu(menuName = "Utilities/Object Stacker Settings")]
    public class ObjectStackerSettingsSO : SubscribableSO
    {

#region DataMembers
        
#region NoiseSettings

        public bool showNoiseMeter = true;
        // public int seed = 31;
        // [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
        // [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
        // [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;
        //
        // [Range( 1.0f, 8.0f )] public int octaves = 7;
        // [Range( 0.5f, 1.0f )] public float persistence = 1f;
        // [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
        
#endregion

#region NoiseDrivenEffects
        public bool lockBottomObject = false;

#region RotationSkew

        public AnimationCurve noiseDampeningCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
        [Range(0f, 50f)]
        public float noiseMultiplier = 1;
        

#endregion

#region PositionShift

        [Range( 0f, 1f )] 
        public float posXNoiseShift = 0f;
        public AnimationCurve posXNoiseCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

        [Range( 0f, 1f )] 
        public float posZNoiseShift = 0f;
        public AnimationCurve posZNoiseCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

#endregion

#endregion


#region ManualAdjustments

#region Rotation

        [Range( -10f, 10f )] public float yRotOffset = 0.05f;
        [Range( -0.5f, 0.5f )] public float deckYRotSkew = -0.244f;
        [Range( 0f, 1f )] public float topDownSkewPercent = 0f;
        [Range( -50f, 50f )] public float topObjectsYRotSkew = 39f;
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
        public bool faceUp = false;

#endregion

#region Position

        public float modelHeight = 1f;
        [Range( 0, 0.001f )] public float verticalOffset = 0.0003f;

#endregion
        
#endregion
        
#endregion

        
// #region Events
//
//         [SerializeField]
//         public UnityAction onSettingsUpdated;
//         private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();
//
// #endregion


// #region LifeCycle
//
//         private void OnValidate()
//         {
//             // Debug.Log( "ObjectStackerSettingsSO OnValidate called." );
//             RaiseOnSettingsUpdated();
//         }
//
// #endregion
    }
}