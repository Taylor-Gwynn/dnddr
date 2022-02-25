using System;
using System.Collections;
using System.Collections.Generic;
using Bim;
using UnityEngine;

// The class that stores obstacles, moves the player along them, 
//   and performs the appropriate animations
public class ObstaclePath : MonoBehaviour
{
    public Queue<Obstacle> obstacles; //stores upcoming obstacles.
                                // index 0 is "current", takes 4 metronome "beats" to complete,
                                // and index 1 is 4 "beats" away (must walk there)
                                //once the current obstacle is completed (pass or fail), it is popped.
    
    public Player player;
    // public GameObject GlobalTimerObject;   //reference to the object tracking the time, in "beats"
    public GlobalTimer timer;
    public PoolingManager Pooler;
    
    public int SpawnBeat;           // Beat to spawn an obstacle on
    public int SpawnBar;            // Bar to spawn an obstacle on (1 = every bar, 2 = every other bar..)
    private int nextSpawnBar;       // When the next obstacle is going to be spawned

    public int ObstacleSpawnDistance;       // Distance from player where obstacles are spawned
    
    //returns the ChoiceType of the upcoming obstacle
    public ChoiceType GetCurrChoice()
    {
        throw new NotImplementedException();
    }

    // returns the point value of a correct input at the current timing
    public int GetScore()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        // timer = GlobalTimerObject.GetComponent<GlobalTimer>();
        nextSpawnBar = SpawnBar;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic for determining whether or not to spawn an obstacle
        if (timer.GetBar() == nextSpawnBar & timer.GetBeat() == SpawnBeat)
        {
            SpawnObstacle();
            nextSpawnBar += SpawnBar;
        }
    }

    void SpawnObstacle()
    {
        Obstacle obs = Pooler.GetObstacle();
        obs.transform.position = player.transform.position + new Vector3(0, 0, ObstacleSpawnDistance);
    }
}
