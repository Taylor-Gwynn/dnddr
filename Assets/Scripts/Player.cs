using System;
using System.Collections;
using System.Collections.Generic;
using Player_Stuff;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// The class that implements player score, health and animation controllers,
//      initiates inputs and health/scoring calculations.
public class Player : BeatMover
{
    // public AnimationClip[] succAnimClips;
    // public AnimationClip[] failAnimClips;
    public GameObject PlayerMovement;
    private float movespeed;
    public TMPro.TMP_Text ScoreText;
    public AnimatorOverrideController STR_animOverride;
    public AnimatorOverrideController CHA_animOverride;
    public AnimatorOverrideController DEX_animOverride;
    public AnimatorOverrideController INT_animOverride;
    public AnimatorOverrideController CON_animOverride;
    public PlayerInput Input;
    // public GlobalTimer timer;
    private ObstaclePath obstaclePath;
    public ChoiceType currAction = ChoiceType.None;           // set as soon as input is recieved, from windup to the end of anim. Is None otherwise.
    public int score;
    public bool isDoingAction = false;             // set when the action takes place, after windup ends.
    // private bool isAtObstacle;              // set when player is occupying space (within bar) of object action
    private Vector3 anchorSpot;                 // the place to stop in front of an obstacle ( placed in OnBeat() )
    public AnimatorOverrideController noneAnimOverride;
    
    private float validActionStart = 3.3f;
    private float validActionEnd = 4.8f;

    public GameObject GameOverText;
    public HealthManager HPManager;

    public AudioSource soundulon;
    public AudioClip correctNoise;
    public AudioClip wrongNoise;
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        movespeed = PlayerMovement.GetComponent<goforward>().forward.magnitude;

        obstaclePath = FindObjectOfType<ObstaclePath>();

        animator.runtimeAnimatorController = noneAnimOverride;
        
        GameOverText.SetActive(false);
        soundulon = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDoingAction){
            Approach(PlayerMovement, 5f*movespeed*Time.deltaTime);
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
        
        // Game Over
        if (HPManager._CurrentHealth <= 0)
        {
            GameOverText.SetActive(true);
            StartCoroutine(Waiter());
        }
        
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    //    NO LONGER assumes it is being called only when an input is possible (around beat 4)
    public bool StartChoice(ChoiceType choice){
        Debug.Log("Player Input: " + choice.ToString()+", Valid Timing: " + ValidActionTime());
        
        // Check the Timer to see if input is allowed at this beat
        if (!ValidActionTime())
        {
            currAction = ChoiceType.None;
            HPManager.ReduceHealth(.25f);
            // Subtract score
            // Play fail sound/animation
            soundulon.clip = wrongNoise;
            soundulon.Play();

            return false;
        }

        //init
        bool isMatch = false;
        int points = 0;
        //getting info from obstacle
        Bim.Obstacle obstacle = obstaclePath.GetCurrObstacle();
        Bim.ObstacleType obstacleType = obstacle.GetObstacleType();
        
        if (obstacleType == null){Debug.Log("Did not pull an obstacle from ObstaclePath!");}
        
        //checking if it's right
        isMatch = obstacleType._ChoiceType == choice;
        obstacle.Interact(isMatch);
        
        //setting animators in motion
        animator.SetBool("successParam", isMatch);
        animator.SetTrigger("WindupInteraction");
        if (isMatch){
            Debug.Log("matching choice!");
            animator.runtimeAnimatorController = obstacleType._PlayerAnimOverride; //apply appropriate obstacle animations to the player
        }else{
            Debug.Log("different choice..");
            animator.runtimeAnimatorController = ChooseFailClip(currAction); //apply choice animations
        }
        if (obstacleType._PlayerAnimOverride == null){Debug.Log(obstacleType.name+" likely has no PlayerAnimOverride!");}
        
        //analyzing the rhythmic timing
        float timing = timer.GetPreciseBeat();
        Debug.Log("~~~~~~~~~~timing: "+timing);
        // if (timing > 0){ //if pressed late,
        //     this.transform.position = anchorSpot; //snap position back to where the beat was
        // }
        Judgement judgement;
        if (isMatch != obstacle._IsSupposedToPass){ //too far out (or wrong)
            judgement = Judgement.miss;
            soundulon.clip = wrongNoise;
        }else if(timing < 0){   //earlies
            soundulon.clip = correctNoise;
            if (Math.Abs(timing) > .35){
                judgement = Judgement.bad;
            }else if (Math.Abs(timing) > .26){
                judgement = Judgement.goodEarly;
            }else if (Math.Abs(timing) > .13){
                judgement = Judgement.greatEarly;
            }else{
                judgement = Judgement.perfectEarly;
            }
        }else{                  //lates
            soundulon.clip = correctNoise;
            if (Math.Abs(timing) > .35){
                judgement = Judgement.bad;
            }else if (Math.Abs(timing) > .26){
                judgement = Judgement.goodLate;
            }else if (Math.Abs(timing) > .13){
                judgement = Judgement.greatLate;
            }else{
                judgement = Judgement.perfectLate;
            }
        }

        
        points = HitScoreQuality(judgement);
        score += points;

        if (points < 0)
        {
            HPManager.ReduceHealth(0.1f);
        }
        else
        {
            HPManager.AddHealth(.25f);
        }

        UpdateScoreUI();

        return isMatch;
    }

    // Checks if the the current time is a valid for an action
    // Valid times are between beats 3.5 and 4.5
    private bool ValidActionTime()
    {
        float currentBeat = timer.GetPreciseBeat();     // Metronome sound seems to have a ~500ms delay

        // Second part of the | is there since we coded the beat to start at 0, so it accounts for beat 4.5 actually being represented as 0.5
        return (currentBeat >= validActionStart & currentBeat <= validActionEnd) 
                | (validActionEnd > timer.TIME_SIGNATURE & currentBeat < validActionEnd - timer.TIME_SIGNATURE)
                && obstaclePath.GetInRange(this.transform);
    }

    //draws current score on HUD
    private void UpdateScoreUI(){
        ScoreText.text = "Score: "+score;
    }

    // returns point value of judgement, draws "good", "great", "perfect" or "miss" icon near player
    private int HitScoreQuality(Judgement judgement)
    {
        soundulon.Play();
        switch (judgement){
            case Judgement.miss:
                return -3;
            case Judgement.bad:
                return -5;
            case Judgement.goodEarly:
            case Judgement.goodLate:
                return 3;
            case Judgement.greatEarly:
            case Judgement.greatLate:
                return 5;
            case Judgement.perfectEarly:
            case Judgement.perfectLate:
                return 6;
            default:
                return 0;   
        }
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
        // float MIN_SNAP_DIST = 100f; //uncomment to always snap to "sync"
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
        anchorSpot = this.transform.position;
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
        if (currAction != ChoiceType.None){
            //should be set by StartChoice()
            isDoingAction = true;
            Debug.Log("OnBar: beginningAction!");
            animator.SetTrigger("BeginningAction");

            animator.ResetTrigger("EndingInteraction");
        }
        
    }

}