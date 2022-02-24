using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public Transform playerTransform;

    private float spawnX;
    private float spawnY;
    private float spawnZ = 0.0f;
    private float tileLength = 2f;
    private int numTilesOnScreen = 20;
    private float safeZone = 15f;

    private List<GameObject> activeTiles;
    
    void Start()
    {
        activeTiles = new List<GameObject>();
        
        // Starting position of the tiles
        spawnX = playerTransform.position.x;
        spawnY = playerTransform.position.y - 0.2f;
        spawnZ = playerTransform.position.z - tileLength * 7;
        
        // Spawn Starting tiles
        for (int i = 0; i < numTilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Spawn New tile & Remove Old one
        if (playerTransform.position.z - safeZone > (spawnZ - numTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefabs[0], new Vector3(spawnX, spawnY, spawnZ), Quaternion.identity, this.transform);
        
        spawnZ += tileLength;
        
        activeTiles.Add(tile);
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
