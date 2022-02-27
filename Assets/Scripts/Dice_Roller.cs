using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice_Roller : MonoBehaviour
{
    public TextMeshPro _diceText;
    
    public float _rollTime;
    public float _rollSpeed;
    private float lastRollTime = 0f;
    
    private int result = -1;
    private int displayNum;
    private float rollStartTime;
    private bool rollStarted = false;
    private SkinnedMeshRenderer mesh;
    private Animator animator;
    
    private void Start() {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (rollStarted)
        {
            Roll();
        }
    }

    void Roll()
    {
        if (Time.time >= rollStartTime + _rollTime)
        {
            result = Random.Range(1, 21);
            displayNum = result;
            SetNumOnMaterial(result);
            animator.SetTrigger("roll");
            rollStarted = false;
        }
        else if (Time.time - lastRollTime >= _rollSpeed)
        {
            displayNum = Random.Range(1, 21);
            lastRollTime = Time.time;
        }
    }
    
    public int GetResult()
    {
        return result;
    }

    public int GetDisplayNum()
    {
        return displayNum;
    }

    public float NumToUVOffset(int dieNumber){
        //UV offset y = 0       : 20
        //UV offset y = -0.0259  : 1
        //UV offset y = -0.496  : 20
        float offset = (-(0.496f-0.0259f)/19)*((dieNumber));
        // Debug.Log("offset: "+offset);
        return offset;
    }

    public void SetNumOnMaterial(int dieNumber){
        float offset = NumToUVOffset(dieNumber);
        mesh.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    public void RollDie()
    {
        rollStarted = true;
    }
}
