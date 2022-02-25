using System;
using System.Collections;
using System.Collections.Generic;
using Bim;
using UnityEngine;

// The class that stores obstacles, moves the player along them, 
//   and performs the appropriate animations
public class ObstaclePath : MonoBehaviour
{
    // upcomingObstacles stores all the obstacles in front of the player
    // Once they've been interacted with, they're moved to completedObstacles
    // completedObstacles that are off-screen are despawned
    public Queue<Obstacle> upcomingObstacles = new Queue<Obstacle>();
    public List<Obstacle> completedObstacles = new List<Obstacle>();
                                
    public Player player;
    // public GameObject GlobalTimerObject;   //reference to the object tracking the time, in "beats"
    public GlobalTimer timer;
    public PoolingManager Pooler;
    
    public int SpawnBeat;           // Beat to spawn an obstacle on
    public int SpawnBar;            // Bar to spawn an obstacle on (1 = every bar, 2 = every other bar..)
    private int nextSpawnBar;       // When the next obstacle is going to be spawned

    public int ObstacleSpawnDistance;       // Distance from player (forwards) where obstacles are spawned
    public int ObstacleDespawnDistance;     // Distance from player (backwards) where obstacles are added back to the pool

    //returns the upcoming Obstacle
    public Obstacle GetCurrObstacle(){
        Obstacle curr;
        if (upcomingObstacles.Count > 0){
            curr = upcomingObstacles.Peek();
        }else{
            Debug.Log("ObstaclePath's upcomingObstacles is empty, cannot peek.");
            return null;
        }
        return curr;
    }
    

    // returns the point value of a correct input at the current timing (ie right now)
    public int GetScore()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnBar = SpawnBar;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic for determining whether or not to spawn an obstacle each frame
        if (timer.GetBar() == nextSpawnBar & timer.GetBeat() == SpawnBeat)
        {
            DespawnObstacles();
            SpawnObstacle();
            nextSpawnBar += SpawnBar;
        }
        
        // Testing Method
        TempInteractWithObstacles();
    }

    void SpawnObstacle()
    {
        Obstacle obs = Pooler.GetObstacle();
        obs.transform.position = player.transform.position + new Vector3(0, 0, ObstacleSpawnDistance);
        upcomingObstacles.Enqueue(obs);
    }

    void DespawnObstacles()
    {
        for (int i = 0; i < completedObstacles.Count; i++)
        {
            if (player.transform.position.z - completedObstacles[i].transform.position.z >= ObstacleDespawnDistance)
            {
                completedObstacles[i].ReturnObstacle();
                completedObstacles.RemoveAt(i);
                i--;
            }
        }
    }

    // Testing method for moving obstacles from upcoming to completed
    // Now that I think about it we can probably keep this since the player will always pass objects they've interacted with
    void TempInteractWithObstacles()
    {
        if (upcomingObstacles.Count == 0) return;
        
        if (upcomingObstacles.Peek().transform.position.z < player.transform.position.z)
        {
            completedObstacles.Add(upcomingObstacles.Dequeue());
        }
    }
}
