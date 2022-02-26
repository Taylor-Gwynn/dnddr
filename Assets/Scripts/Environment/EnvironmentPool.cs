using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class EnvironmentPool : MonoBehaviour
    {
        [Header("Required Parameters")] 
        public int _PoolSizes;
        public List<GameObject> _EnvironmentPrefabs;

        private List<PoolObject> _pool = new List<PoolObject>();

        private void Awake()
        {
            if (_PoolSizes == 0 || _EnvironmentPrefabs.Count == 0)
            {
                Debug.Log("Environment pool, " + gameObject.name + ", has either not been assigned any prefabs or has a pool size of 0.");
                return;
            }

            foreach (GameObject obj in _EnvironmentPrefabs)
            {
                for (int i = 0; i < _PoolSizes; i++)
                {
                    MakeObject();
                }
            }
        }

        /// <summary>
        /// Make a specific prefab listed at a given index
        /// </summary>
        /// <param name="index"></param>
        private void MakeObject(int index)
        {
            if ( _EnvironmentPrefabs[index] == null) Debug.LogError("Make object failed, please provide a valid obstacle index.");
            
            GameObject prefab = _EnvironmentPrefabs[index];
            
            PoolObject obj = Instantiate(prefab, transform).GetComponent<PoolObject>();

            if (obj == null) Debug.LogError("PoolObject.cs component is missing from " + prefab + ", if it is to be used in the environment pool! " + "@" + gameObject.name);
            
            obj.Recycle.AddListener(ReturnObject);

            _pool.Add(obj);

            obj.gameObject.SetActive(false);
        }

        private void MakeObject()
        {
            int index = Random.Range(0, _EnvironmentPrefabs.Count);
            MakeObject(index);
        }

        /// <summary>
        /// Subscribed to each objects recycle event
        /// </summary>
        /// <param name="obj"></param>
        private void ReturnObject(PoolObject obj)
        {
            obj.transform.SetParent(transform);
            
            _pool.Add(obj);
            
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Returns a pool object to be used by some external manager
        /// </summary>
        /// <returns></returns>
        public PoolObject GetObject()
        {
            if (_pool.Count == 0)
            {
                MakeObject();
            }

            int choice = Random.Range(0, _pool.Count);
            
            PoolObject selection = _pool[choice];
            
            _pool.Remove(selection);
            
            selection.gameObject.SetActive(true);
            
            return selection;
        }
    }
}
