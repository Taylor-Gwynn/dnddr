using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatMover : MonoBehaviour
{
    private GlobalTimer timer;
    // An abstract class that reacts to input from the GlobalTimer

    public void Start() {
        // Debug.Log("is this happening?: "+this.name);
        timer = FindObjectOfType<GlobalTimer>();
        timer.RegisterListener(this);
    }

    public abstract void OnBeat();

    public abstract void OnBar();

}
