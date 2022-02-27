using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goforward : MonoBehaviour
{
    Transform transf;
    public Vector3 forward;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        transf = GetComponent<Transform>();
        player = FindObjectOfType<Player>();
        // forward = new Vector3(0, 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // if (! player.isDoingAction){
            transf.Translate(forward*Time.deltaTime);
        // }
    }
}
