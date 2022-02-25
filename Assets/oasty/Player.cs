using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class that implements player score, health and animation controllers,
//      initiates inputs and health/scoring calculations.
public class Player : BeatMover
{
    public AnimationClip[] succAnimClips;
    public AnimationClip[] failAnimClips;
    private ObstaclePath obstaclePath;
    public ChoiceType currAction;
    public int health;
    public int score;
    private Animator animator;
    public AnimatorOverrideController TestAnimOverride;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        obstaclePath = FindObjectOfType<ObstaclePath>();
        animator = GetComponent<Animator>();
        // animator.runtimeAnimatorController = TestAnimOverride;
    }

    // Update is called once per frame
    void Update()
    {
        //for lazy debug:
        if(Input.anyKeyDown){
            StartChoice(ChoiceType.Dex);
            animator.SetBool("successParam", true);
            animator.SetTrigger("EnteringInteraction");
            // animator.runtimeAnimatorController = TestAnimOverride2;
            // AnimationClip currWindupAnim = succAnimClips[0]; //assumes "windup" animation is first in the obstacle's list of player animations
            // animOverride["emptyWindupSucc"] = currWindupAnim;
            // animOverride["emptyWindupFail"] = currWindupAnim;
            // Debug.Log("currWindupAnim: "+currWindupAnim.name);
        }
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    //    assumes it is being called only when an input is possible (around beat 4)
    public bool StartChoice(ChoiceType choice){
        bool isSuccess = false;
        int points = 0;
        
        Bim.ObstacleType obstacle = obstaclePath.GetCurrObstacleType();
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
        throw new NotImplementedException();
    }

    // draws "good", "great", "perfect" or "miss" icon near player
    private void DisplayScoreQuality(int points)
    {
        throw new NotImplementedException();
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
//  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20