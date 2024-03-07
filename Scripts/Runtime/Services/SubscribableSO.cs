using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    public abstract class SubscribableSO : ScriptableObject
    {
        public UnityAction onSettingsUpdated;
        protected void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();
        
#region LifeCycle

        private void OnValidate()
        {
            // Debug.Log( "ObjectStackerSettingsSO OnValidate called." );
            RaiseOnSettingsUpdated();
        }

#endregion
    }
}