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
    // public GlobalTimer timer;
    private ObstaclePath obstaclePath;
    public ChoiceType currAction;           // set as soon as input is recieved, from windup to the end of anim. Is None otherwise.
    public int health;
    public int score;
    private bool isDoingAction;             // set when the action takes place, after windup ends.
    private bool isAtObstacle;              // set when player is occupying space (within bar) of object action
    public AnimatorOverrideController noneAnimOverride;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        obstaclePath = FindObjectOfType<ObstaclePath>();
        
        animator.runtimeAnimatorController = noneAnimOverride;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.actions["Dex"].triggered)
        {
            currAction = ChoiceType.Dex;
            StartChoice(currAction);
        }
        else if (Input.actions["Str"].triggered)
        {
            currAction = ChoiceType.Str;
            StartChoice(currAction);
        }
        else if (Input.actions["Con"].triggered)
        {
            currAction = ChoiceType.Con;
            StartChoice(currAction);
        }
        else if (Input.actions["Int"].triggered)
        {
            currAction = ChoiceType.Int;
            StartChoice(currAction);
        }
        else if (Input.actions["Cha"].triggered)
        {
            currAction = ChoiceType.Cha;
            StartChoice(currAction);
        }
        

        // animator.ResetTrigger("beat");//deprecated trigger
        

        //for lazy debug:
        // if(Input.anyKeyDown){
        //     StartChoice(ChoiceType.Dex);
        //     animator.SetBool("successParam", true);
        //     animator.SetTrigger("WindupInteraction");
        //     obstaclePath.GetCurrObstacle().Interact(true);
        //     // animator.runtimeAnimatorController = TestAnimOverride2;
        //     // AnimationClip currWindupAnim = succAnimClips[0]; //assumes "windup" animation is first in the obstacle's list of player animations
        //     // animOverride["emptyWindupSucc"] = currWindupAnim;
        //     // animOverride["emptyWindupFail"] = currWindupAnim;
        //     // Debug.Log("currWindupAnim: "+currWindupAnim.name);
        // }
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    //    NO LONGER assumes it is being called only when an input is possible (around beat 4)
    public bool StartChoice(ChoiceType choice){
        Debug.Log("Player Input: " + choice.ToString());
        Debug.Log("Valid Timing: " + ValidActionTime());
        
        // Check the Timer to see if input is allowed at this beat
        if (!ValidActionTime())
        {
            currAction = ChoiceType.None;
            // Subtract score
            // Play fail sound/animation

            return false;
        }

        bool isSuccess = false;
        int points = 0;
        
        Bim.ObstacleType obstacle = obstaclePath.GetCurrObstacle().GetObstacleType();
        if (obstacle == null){Debug.Log("Did not pull an obstacle from ObstaclePath!");}
        
        isSuccess = obstacle._ChoiceType == choice;
        animator.SetBool("successParam", isSuccess);
        animator.SetTrigger("WindupInteraction");
        // animator.ResetTrigger("WindupInteraction");

        animator.runtimeAnimatorController = obstacle._PlayerAnimOverride; //apply appropriate obstacle animations to the player
        
        if (obstacle._PlayerAnimOverride == null){Debug.Log(obstacle.name+" likely has no PlayerAnimOverride!");}
        
        score += points;
        DisplayScoreQuality(points);
        UpdateHealth(points);

        return isSuccess;
    }

    // Checks if the the current time is a valid for an action
    // Valid times are between beats 3.5 and 4.5
    private bool ValidActionTime()
    {
        float validStart = 3.5f;
        float validEnd = 4.5f;
        float currentBeat = timer.GetPreciseBeat();     // Metronome sound seems to have a ~500ms delay

        // Second part of the | is there since we coded the beat to start at 0, so it accounts for beat 4.5 actually being represented as 0.5
        return (currentBeat >= validStart & currentBeat <= validEnd) 
                | (validEnd > timer.TIME_SIGNATURE & currentBeat < validEnd - timer.TIME_SIGNATURE)
                && obstaclePath.GetInRange(this.transform);
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
        Debug.Log("onBeat "+timer.GetBeat()+", currAction: "+currAction+", isDoingAction: "+isDoingAction);
        animator.ResetTrigger("WindupInteraction");
        // if (currAction != ChoiceType.None){ //if an input was successfully recieved, begin interaction
        //     isDoingAction = true;
        // }else{
        //     isDoingAction = false;
        // }
        
        // Debug.Log("Player's OnBeat()");
        // animator.SetTrigger("beat"); // <- for segmented animations... eh... probably won't.
        // animator.ResetTrigger("EndingInteraction");
        // animator.ResetTrigger("BeginningAction");
    }

    public override void OnBar(){
        //resume walking after action?
        if (isDoingAction){
            currAction = ChoiceType.None;
            isDoingAction = false;
            Debug.Log("resume walking after action...");
            animator.SetTrigger("EndingInteraction");
            animator.ResetTrigger("BeginningAction");

        }

        //start action if winding up?
        if (currAction != ChoiceType.None){//should be set by StartChoice()
            isDoingAction = true;
            Debug.Log("OnBar: beginningAction!");
            animator.SetTrigger("BeginningAction");
            animator.ResetTrigger("EndingInteraction");
        }
        
    }

}