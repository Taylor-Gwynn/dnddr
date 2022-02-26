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
    public GameObject PlayerMovement;
    public AnimatorOverrideController STR_animOverride;
    public AnimatorOverrideController CHA_animOverride;
    public AnimatorOverrideController DEX_animOverride;
    public AnimatorOverrideController INT_animOverride;
    public AnimatorOverrideController CON_animOverride;
    public PlayerInput Input;
    // public GlobalTimer timer;
    private ObstaclePath obstaclePath;
    public ChoiceType currAction;           // set as soon as input is recieved, from windup to the end of anim. Is None otherwise.
    public int health;
    public int score;
    private bool isDoingAction = false;             // set when the action takes place, after windup ends.
    // private bool isAtObstacle;              // set when player is occupying space (within bar) of object action
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
        if (!isDoingAction){
            Approach(PlayerMovement, 10f*Time.deltaTime);
        }

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
        
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    //    NO LONGER assumes it is being called only when an input is possible (around beat 4)
    public bool StartChoice(ChoiceType choice){
        Debug.Log("Player Input: " + choice.ToString()+", Valid Timing: " + ValidActionTime());
        
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
        
        Bim.ObstacleType obstacleType = obstaclePath.GetCurrObstacle().GetObstacleType();
        Bim.Obstacle obstacle = obstaclePath.GetCurrObstacle();
        if (obstacleType == null){Debug.Log("Did not pull an obstacle from ObstaclePath!");}
        isSuccess = obstacleType._ChoiceType == choice;
        obstacle.Interact(isSuccess);
        animator.SetBool("successParam", isSuccess);
        animator.SetTrigger("WindupInteraction");
        if (isSuccess){
            Debug.Log("correct choice!!!!!!");
            animator.runtimeAnimatorController = obstacleType._PlayerAnimOverride; //apply appropriate obstacle animations to the player
        }else{
            Debug.Log("wrong choice..");
            animator.runtimeAnimatorController = ChooseFailClip(currAction); //apply choice animations
        }
        
        if (obstacleType._PlayerAnimOverride == null){Debug.Log(obstacleType.name+" likely has no PlayerAnimOverride!");}
        
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

    public AnimatorOverrideController ChooseFailClip(ChoiceType choice){
        switch (choice)
        {
            case ChoiceType.Dex:
                return DEX_animOverride;
            case ChoiceType.Con:
                return CON_animOverride;
            case ChoiceType.Cha:
                return CHA_animOverride;
            case ChoiceType.Int:
                return INT_animOverride;
                case ChoiceType.Str:
                return STR_animOverride;
            default:
                Debug.Log("Error: player probably does not have fail clips assigned script");
                return null;
        }
    }

    // moves the player towards the given object with speed given
    public void Approach(GameObject obj, float speed){
        float MIN_SNAP_DIST = 0.1f;
        if ((obj.transform.position -this.transform.position).magnitude < MIN_SNAP_DIST){
            this.transform.position = obj.transform.position;
        }else{
            Vector3 towards = (obj.transform.position-this.transform.position).normalized * speed;
            this.transform.position += towards;
        }
    }

    public override void OnBeat(){
        // Debug.Log("onBeat "+timer.GetBeat()+", currAction: "+currAction+", isDoingAction: "+isDoingAction);
        animator.ResetTrigger("WindupInteraction");
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