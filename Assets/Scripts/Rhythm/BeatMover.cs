using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatMover : MonoBehaviour
{
    protected internal GlobalTimer timer;
    protected Animator animator;
    // An abstract class that reacts to input from the GlobalTimer

    public void Start() {
        // Debug.Log("is this happening?: "+this.name);
        timer = FindObjectOfType<GlobalTimer>();
        timer.RegisterListener(this);
        animator = GetComponent<Animator>();
    }

    public abstract void OnBeat();

    public abstract void OnBar();

}
