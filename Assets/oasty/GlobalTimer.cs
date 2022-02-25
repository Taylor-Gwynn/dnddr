using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public List<BeatMover> listeners;
    public int BPM;                     // Beats per minute
    public int TIME_SIGNATURE;          // Beats per bar
    
    private float beatTimerDefault;     // Length of time of a beat
    private int beat;                   // Value from 1 to TIME_SIGNATURE indicating how far along the bar we are
    private int bar;                    // The bar we're current in (starting at 1)
    private float beatTimer;            // Time until the next beat

    // Start is called before the first frame update
    void Start()
    {
        beatTimerDefault = 60f / BPM;
        beatTimer = beatTimerDefault;
        beat = 1;
        bar = 1;
    }

    // Update is called once per frame
    void Update()
    {
        BeatTick();
    }

    // increments timers/beats based on Time.deltaTime
    void BeatTick(){
        beatTimer -= Time.deltaTime;
        // We have reached a new beat
        if (beatTimer < 0){
            SendBeat();
            beat++;
            beatTimer = beatTimerDefault;
            // We have reached a new bar
            if (beat > TIME_SIGNATURE){
                bar++;
                beat = 1;
                SendBar();
            }
        }
    }

    void SendBeat(){
        // Debug.Log("Beat: " + beat);
        foreach (BeatMover x in listeners){
            x.OnBeat();
        }
    }
    void SendBar(){
        // Debug.Log("Bar:  " + bar);
        foreach (BeatMover x in listeners){
            x.OnBar();
        }
    }

    public int GetBeat()
    {
        return beat;
    }

    public int GetBar()
    {
        return bar;
    }
}
