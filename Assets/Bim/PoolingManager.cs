using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bim
{
    /// <summary>
    /// Responsible for pooling the obstacles in the scene.
    /// Intended to be set in the editor and only accessed by some manager class that request an object when needed
    /// </summary>
    public class PoolingManager : MonoBehaviour
    {
        [Header("Required Parameters")]
        public int _PoolSizes;
        public List<GameObject> _ObstaclePrefabs;

        //private readonly Dictionary<ChoiceType, List<Obstacle>> _pools = new Dictionary<ChoiceType, List<Obstacle>>();
        private List<ObstacleType> _obstacleIndexList = new List<ObstacleType>();
        private Dictionary<ObstacleType, List<Obstacle>> _pools = new Dictionary<ObstacleType, List<Obstacle>>();

        private void Awake()
        {
            if (_ObstaclePrefabs.Count == 0) Debug.LogError("Please provide obstacle prefabs for " + gameObject.name + ": pooling manager.");
            
            for (int i = 0; i < _ObstaclePrefabs.Count; i++)
            {
                for (int j = 0; j < _PoolSizes; j++)
                {
                    MakeObstacle(i);
                }
            }
        }
        
        private void ReturnObject(Obstacle obj)
        {
            obj.transform.SetParent(transform);
            
            _pools[obj._Type].Add(obj);
            
            obj.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Makes a random obstacle, if assigned obstacles.
        /// </summary>
        public void MakeObstacle()
        {
            int index = Random.Range(0, _ObstaclePrefabs.Count);
            MakeObstacle(index);
        }

        /// <summary>
        /// Make an obstacle of a specific type
        /// </summary>
        /// <param name="type"></param>
        public void MakeObstacle(ObstacleType type)
        {
            int index = _obstacleIndexList.IndexOf(type);
            MakeObstacle(index);
        }

        /// <summary>
        /// Makes a specific obstacle, check the pools list for reference
        /// </summary>
        /// <param name="index"></param>
        public void MakeObstacle(int index)
        {
            if ( _ObstaclePrefabs[index] == null) Debug.LogError("Make object failed, please provide a valid obstacle index.");
            
            GameObject obstacle = _ObstaclePrefabs[index];
            
            Obstacle obj = Instantiate(obstacle, transform).GetComponent<Obstacle>();
            
            obj.Recycle.AddListener(ReturnObject);

            CheckForNewType(obj);
            
            _pools[obj._Type].Add(obj);

            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Return a obstacle of a specific prefab type.
        /// Creates a new obstacle if none are available
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Obstacle GetObstacle(ObstacleType type)
        {
            if(!_pools.ContainsKey(type)) Debug.LogError("Pooling manager does not contain the obstacle type " + type + "!");
            
            if (_pools[type].Count == 0)
            {
                MakeObstacle(_obstacleIndexList.IndexOf(type));
            }
            
            Obstacle selection = _pools[type][0];

            _pools[type].Remove(selection);
            
            selection.gameObject.SetActive(true);
            
            return selection;
        }

        /// <summary>
        /// Returns an obstacle that is assigned a given index in the Pooling Managers prefab list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Obstacle GetObstacle(int index)
        {
            ObstacleType type = _obstacleIndexList[index];
            return GetObstacle(type);
        }

        /// <summary>
        /// Returns a random obstacle
        /// </summary>
        /// <returns></returns>
        public Obstacle GetObstacle()
        {
            ObstacleType type = _obstacleIndexList[Random.Range(0, _ObstaclePrefabs.Count)];
            return GetObstacle(type);
        }
        
        // Utility
        private void CheckForNewType(Obstacle obj)
        {
            if (_pools.ContainsKey(obj._Type)) return;
            
            Debug.Log("Adding new object type to dict keys: " + obj._Type);
            _pools.Add(obj._Type, new List<Obstacle>());
            _obstacleIndexList.Add(obj._Type);
        }
    }
}
