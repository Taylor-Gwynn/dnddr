using System.Collections.Generic;
using UnityEngine;

namespace Bim
{
    /// <summary>
    /// Responsible for pooling the obstacles in the scene.
    /// Intended to be set in the editor and only accessed by some manager class that request an object when needed
    /// </summary>
    public class PoolingManager : MonoBehaviour
    {
        public int _PoolSize;
        public GameObject _ObjectPrefab;

        private readonly List<Obstacle> _pool = new List<Obstacle>();

        private void MakeObject()
        {
            Obstacle obj = Instantiate(_ObjectPrefab, transform).GetComponent<Obstacle>();
            
            obj.Recycle.AddListener(ReturnObject);
            
            _pool.Add(obj);
            
            obj.gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            for (int i = 0; i < _PoolSize; i++)
            {
                MakeObject();
            }
        }

        private void ReturnObject(Obstacle obj)
        {
            obj.transform.SetParent(transform);
            
            _pool.Add(obj);
            
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Return a obstacle object on request.
        /// Creates a new obstacle if none are available.
        /// </summary>
        /// <returns></returns>
        public Obstacle GetObject()
        {
            if (_pool.Count == 0)
            {
                MakeObject();
            }
            
            Obstacle selection = _pool[0];
            
            _pool.Remove(selection);

            selection.gameObject.SetActive(true);

            return selection;
        }

    }
}
