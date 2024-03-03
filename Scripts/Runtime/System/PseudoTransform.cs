using System;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System
{
    [Serializable]
    public class PseudoTransform
    {
        [SerializeField] public Vector3 position;
        [SerializeField] public Quaternion rotation;
        [SerializeField] public Vector3 scale;

        public PseudoTransform( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public PseudoTransform( Transform transform )
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        public PseudoTransform()
        {
            
        }

        public void ApplyPositionAndRotation( Transform transform )
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }
}