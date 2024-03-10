using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Tools.NoiseGeneration.Scripts.Runtime
{
    public interface INoiseSource
    {
        void Initialize( int noiseMapWidth, int noiseMapHeight, UnityAction settingsSOChangedCallBack, UnityAction noiseSettingsChangedCallBack );
        void UpdateNoiseMapSize( int noiseMapWidth, int noiseMapHeight );
        float GetNoiseAtIndex( int x, int y );
        float[,] Get2DNoiseMap();
        NoiseSettingsSO GetNoiseSettingsSO();
    }
}