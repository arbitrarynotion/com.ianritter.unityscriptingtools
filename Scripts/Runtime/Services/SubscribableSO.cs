using UnityEngine;
using UnityEngine.Events;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    public abstract class SubscribableSO : ScriptableObject
    {
        public UnityAction onSettingsUpdated;
        public void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();
    }
}