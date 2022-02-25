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
    public ObstaclePath obstaclePath;
    public ChoiceType currAction;
    public int health;
    public int score;
    public Animator animator;
    public AnimatorOverrideController animOverride;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        obstaclePath = GetComponent<ObstaclePath>();
        animator = GetComponent<Animator>();
        animOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        // clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        // animatorOverrideController.GetOverrides(clipOverrides);
        animator.runtimeAnimatorController = animOverride;
    }

    // Update is called once per frame
    void Update()
    {
        //for lazy debug:
        if(Input.anyKeyDown){
            // StartChoice(ChoiceType.Dex);
            animator.SetBool("successParam", true);
            animator.SetTrigger("EnteringInteraction");
            AnimationClip currWindupAnim = succAnimClips[0]; //assumes "windup" animation is first in the obstacle's list of player animations
            animOverride["emptyWindupSucc"] = currWindupAnim;
            animOverride["emptyWindupFail"] = currWindupAnim;
            Debug.Log("currWindupAnim: "+currWindupAnim.name);
        }
    }

    // called immediately when player inputs a move (returns true iff it was correct choice)
    public bool StartChoice(ChoiceType choice){
        bool success = false;
        int points = 0;
        // string currAnim;
        Bim.ObstacleType obstacle = obstaclePath.GetCurrObstacleType();
        currAction = choice;
        // if (obstacle._ChoiceType == choice){
        //     success = true;
        //     currAnim = obstacle._PlayerSuccessSuccess.ToString();
        //     points = obstaclePath.GetScore();
        // }else{
        //     success = false;
        //     currAnim = obstacle._PlayerSuccessFail.ToString();
        //     points = 0;
        // }
        success = obstacle._ChoiceType == choice;
        animator.SetBool("successParam", success);
        AnimationClip currWindupAnim = obstacle._PlayerSuccessClips[0]; //assumes "windup" animation is first in the obstacle's list of player animations
        animOverride["emptyWindup"] = currWindupAnim;

        score += points;
        DisplayScoreQuality(points);
        UpdateHealth(points);
        // animator.SetTrigger(currAnim);
        // animator.SetInteger("obstacle", (int)obstaclePath.GetCurrObstacleID());

        return success;
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
    }

    public override void OnBar(){
    }

}
