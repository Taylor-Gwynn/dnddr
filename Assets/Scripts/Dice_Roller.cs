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
    
    void Update()
    {
        if (Input.GetKeyDown("r") & !rollStarted)
        {
            rollStartTime = Time.time;
            rollStarted = true;
            lastRollTime = Time.time;
        }

        if (rollStarted)
        {
            Roll();
            _diceText.text = displayNum.ToString();
        }
    }

    void Roll()
    {
        if (Time.time >= rollStartTime + _rollTime)
        {
            result = Random.Range(1, 20);
            displayNum = result;
            rollStarted = false;
        }
        else if (Time.time - lastRollTime >= _rollSpeed)
        {
            displayNum = Random.Range(1, 20);
            lastRollTime = Time.time;
        }

        // Debug.Log("Time: " + Time.time);
        // Debug.Log("start + roll: " + (rollStartTime + _rollTime));
        // Debug.Log("Time.time - lastRollTime" + (Time.time - lastRollTime));
    }
    
    public int GetResult()
    {
        return result;
    }

    public int GetDisplayNum()
    {
        return displayNum;
    }
}
