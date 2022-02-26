using System;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    /// <summary>
    /// Attach this component to any object to allow it to be pooled.
    /// </summary>
    public class PoolObject : MonoBehaviour
    {
        // the environment pool subscribes and listens for this event
        protected internal UnityEvent<PoolObject> Recycle = new UnityEvent<PoolObject>();

        public void ReturnObject()
        {
            Recycle.Invoke(this);
        }

        public void PositionObject(Vector3 position, Quaternion angles, Transform parent)
        {
            Transform trans = transform;
            
            trans.position = position;
            trans.rotation = angles;
            trans.SetParent(parent);
        }
    }
}
