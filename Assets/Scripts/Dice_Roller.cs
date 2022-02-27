using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice_Roller : MonoBehaviour
{
    public TextMeshPro _diceText;
    public SkinnedMeshRenderer _Mesh;
    public Animator _Animator;
    
    public float _rollTime;
    public float _rollSpeed;
    private float lastRollTime = 0f;
    
    private int result = -1;
    private int displayNum;
    private float rollStartTime;
    private bool rollStarted = false;
    private SkinnedMeshRenderer mesh;
    
    
    private void Start() {
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
        _Mesh.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    public void RollDie()
    {
        result = Random.Range(1, 21);
        displayNum = result;
        SetNumOnMaterial(result);
        _Animator.SetTrigger("roll");
        rollStarted = false;
    }
}
