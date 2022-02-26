using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Environment
{
    public class BackgroundSet : MonoBehaviour
    {
        [Header("Set Settings")]
        public EnvironmentPool _SetPieces;

        public int _Density;
        
        [Header("Placement settings")]
        public Transform _Origin;
        public Vector3 _OriginOffset;
        public Vector3 _SpawnRange;
        public float _RefreshDistance;

        [Header("Clean up settings")] 
        [Tooltip("How far does a set piece need to be before we hide it from the player.")]
        public float _CleanUpDistance;
        
        [Tooltip("How often do we check for a clean up, in seconds.")]
        public float _CleanUpRate;
        
        private List<PoolObject> _PiecesInUse = new List<PoolObject>();
        private Vector3 _lastSpawnPos;

        
        private float _timeOfLastCleanUp;
        
        private void Start()
        {
            _lastSpawnPos = transform.position;
            PlaceNumberOfPieces(_Density);
            _OriginOffset += new Vector3(0,0, _RefreshDistance);
            PlaceNumberOfPieces(_Density);
        }

        private void Update()
        {
            if (Vector3.Distance(_lastSpawnPos, _Origin.position) >= _RefreshDistance)
            {
                PlaceNumberOfPieces(_Density);
            }

            if (Time.time - _timeOfLastCleanUp >= _CleanUpRate)
            {
                
                foreach (PoolObject obj in _PiecesInUse)
                {
                    if (Mathf.Abs(obj.transform.position.z - _Origin.position.z) > _CleanUpDistance)
                    {
                        obj.ReturnObject();
                    }
                }
                
                _timeOfLastCleanUp = Time.time;
            }
            
        }

        private void PlaceNumberOfPieces(int num)
        {
            for (int i = 0; i < num; i++)
            {
                PlacePiece();
            }
            
            _lastSpawnPos = _Origin.position;
        }

        private void PlacePiece()
        {
            PoolObject piece = _SetPieces.GetObject();
            
            // find a random location
            Vector3 relativeLocation = new Vector3(Random.Range(_OriginOffset.x, _OriginOffset.x + _SpawnRange.x),
                0,
                Random.Range(_OriginOffset.z, _OriginOffset.z + _SpawnRange.z));
            
            // position the object relative to the player and offset in the found random location
            piece.PositionObject( relativeLocation + _Origin.position, Quaternion.identity, transform);
            
            _PiecesInUse.Add(piece);
        }
    }
}