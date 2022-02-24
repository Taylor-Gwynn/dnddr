using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class that implements player score, health and animation controllers,
//      initiates inputs and health/scoring calculations.
public class Player : MonoBehaviour
{
    // public List<AnimationClip> animList;
    public ObstaclePath obstaclePath;
    public ChoiceType currAction;
    public int health;
    public int score;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    public bool StartChoice(ChoiceType choice){
        bool success = false;
        int points = 0;
        currAction = choice;
        if (obstaclePath.GetCurrChoice() == choice){
            success = true;
            points += obstaclePath.GetScore();
        }
        score += points;
        DisplayScoreQuality(points);

        return success;
    }

    // draws "good", "great", "perfect" or "miss" icon near player
    private void DisplayScoreQuality(int points)
    {
        throw new NotImplementedException();
    }
}
