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
        public LayerMask _AvoidanceMask;
        
        [Header("Placement settings")]
        public Transform _Origin;
        public Vector3 _OriginOffset;
        public Vector3 _SpawnRange;
        public float _RefreshDistance;
        public float _SafetyRange;

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
            
            Vector3 potentialPosition = new Vector3();
            bool acceptablePlacement = false;
            int num = 100;
            
            var position = _Origin.transform.position;
            
            while(!acceptablePlacement)
            {
                num--;

                float x = position.x + _OriginOffset.x + Random.Range(-_SpawnRange.x, _SpawnRange.x);
                float y = position.y + _OriginOffset.y + Random.Range(-_SpawnRange.y, _SpawnRange.y);
                float z = position.z + _OriginOffset.z + Random.Range(-_SpawnRange.z, _SpawnRange.z);
                
                potentialPosition = new Vector3(x,y,z);

                var results = Physics.OverlapSphere(potentialPosition, _SafetyRange, _AvoidanceMask);

                acceptablePlacement = results.Length == 0;

                if (num == 0) break;
            }

            if (!acceptablePlacement)
            {
                piece.ReturnObject();
                return;
            }
            
            // position the object relative to the player and offset in the found random location
            piece.PositionObject( potentialPosition, Quaternion.Euler(0, 90, 0), transform);
            
            _PiecesInUse.Add(piece);
        }

        // private Vector3 FindLocation(int attempts)
        // {
        //     
        //
        //     return potentialPosition;
        // }
    }
}