using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    [CanEditMultipleObjects]
    public abstract class SubscribableSO : ScriptableObject
    {
        public UnityAction onSettingsUpdated;
        [SerializeField] protected FormattedLogger logger;
        private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();
        
#region LifeCycle

        private void OnValidate()
        {
            // Debug.Log( "ObjectStackerSettingsSO OnValidate called." );
            RaiseOnSettingsUpdated();
        }

#endregion

        public void PrintMyEventSubscribers()
        {
            if( logger == null )
            {
                Debug.LogWarning( "You need to assign a logger first!" );
                return;
            }
            onSettingsUpdated.PrintMySubscribers
            (
                name,
                logger,
                nameof( onSettingsUpdated ),
                "Readers"
            );
        }
    }
}