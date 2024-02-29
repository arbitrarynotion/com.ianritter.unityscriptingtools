using System;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services
{
    /// <summary>
    ///     Extend this class to utilize Scene View mesh drawing.
    /// </summary>
    [ExecuteInEditMode]
    public abstract class PrefabSpawnerRoot : MonoBehaviour
    {
        [SerializeField] protected bool showPreview;
        
        /// <summary>
        ///     Provide the var name for the prefab to be spawned using the `nameof(varNameHere)` method.
        /// </summary>
        public abstract string GetPrefabVarName();

        public abstract string GetSpawnPointsVarName();
    }
}