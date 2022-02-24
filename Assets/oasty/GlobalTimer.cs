using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public int BPM = 120;
    public int TIME_SIGNATURE = 4; //beats per measure
    private int beatTimerDefault;
    public int beat; //value from 0 to 3 indicating how far along the "bar" we are
    public int bar; // increasing value indication how many measures from the start we are
    public float beatTimer; //decreasing value to the next "beat"
    public List<BeatMover> listeners;

    // Start is called before the first frame update
    void Start()
    {
        beatTimer = beatTimerDefault;
        beatTimerDefault = 60/BPM;
    }

    // Update is called once per frame
    void Update()
    {
        BeatTick();
    }

    // increments timers/beats based on Time.deltaTime
    void BeatTick(){
        beatTimer -= Time.deltaTime;
        if (beatTimer < 0){
            beat++;
            beatTimer = beatTimerDefault;
            SendBeat();
            if (beat <= TIME_SIGNATURE){
                bar++;
                beat = 0;
                SendBar();
                
            }
        }
    }

    void SendBeat(){
        foreach (BeatMover x in listeners){
            x.OnBeat();
        }
    }
    void SendBar(){
        foreach (BeatMover x in listeners){
            x.OnBar();
        }
    }
}
