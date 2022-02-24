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
    public GlobalTimer timer; //reference to the object tracking the time, in "beats"

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
