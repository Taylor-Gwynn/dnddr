using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The class that implements player score, health and animation controllers,
//      initiates inputs and health/scoring calculations.
public class Player : BeatMover
{
    // public AnimationClip[] succAnimClips;
    // public AnimationClip[] failAnimClips;
    public PlayerInput Input;
    private ObstaclePath obstaclePath;
    public ChoiceType currAction;
    public int health;
    public int score;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        obstaclePath = FindObjectOfType<ObstaclePath>();
        
        // animator.runtimeAnimatorController = TestAnimOverride;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.actions["Dex"].triggered)
        {
            Debug.Log("Dex Action");
        }
        else if (Input.actions["Str"].triggered)
        {
            Debug.Log("Str Action");
        }
        else if (Input.actions["Con"].triggered)
        {
            Debug.Log("Con Action");
        }
        else if (Input.actions["Int"].triggered)
        {
            Debug.Log("Int Action");
        }
        else if (Input.actions["Cha"].triggered)
        {
            Debug.Log("Cha Action");
        }
        
        // //for lazy debug:
        // if(Input.anyKeyDown){
        //     StartChoice(ChoiceType.Dex);
        //     animator.SetBool("successParam", true);
        //     animator.SetTrigger("EnteringInteraction");
        //     obstaclePath.GetCurrObstacle().Interact(true);
        //     // animator.runtimeAnimatorController = TestAnimOverride2;
        //     // AnimationClip currWindupAnim = succAnimClips[0]; //assumes "windup" animation is first in the obstacle's list of player animations
        //     // animOverride["emptyWindupSucc"] = currWindupAnim;
        //     // animOverride["emptyWindupFail"] = currWindupAnim;
        //     // Debug.Log("currWindupAnim: "+currWindupAnim.name);
        // }
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    //    assumes it is being called only when an input is possible (around beat 4)
    public bool StartChoice(ChoiceType choice){
        bool isSuccess = false;
        int points = 0;
        
        Bim.ObstacleType obstacle = obstaclePath.GetCurrObstacle().GetObstacleType();
    if (obstacle == null){Debug.Log("Did not pull an obstacle from ObstaclePath!");}
        currAction = choice;
        isSuccess = obstacle._ChoiceType == choice;
        animator.SetBool("successParam", isSuccess);
        animator.runtimeAnimatorController = obstacle._PlayerAnimOverride; //apply appropriate obstacle animations to the player
    if (obstacle._PlayerAnimOverride == null){Debug.Log(obstacle.name+" likely has no PlayerAnimOverride!");}
        score += points;
        DisplayScoreQuality(points);
        UpdateHealth(points);

        return isSuccess;
    }

    //damages or increases health based on score and combo(?)
    private void UpdateHealth(int score){
        // throw new NotImplementedException();
    }

    // draws "good", "great", "perfect" or "miss" icon near player
    private void DisplayScoreQuality(int points)
    {
        // throw new NotImplementedException();
    }

    public override void OnBeat(){
        //cycles through next animation to put in override...
        // animator.SetTrigger("EnteringInteraction");
        // Debug.Log("Player's OnBeat()");
        animator.SetTrigger("beat");
    }

    public override void OnBar(){
    }

}