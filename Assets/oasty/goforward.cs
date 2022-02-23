using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goforward : MonoBehaviour
{
    Transform transf;
    public Vector3 forward;
    // Start is called before the first frame update
    void Start()
    {
        transf = GetComponent<Transform>();
        // forward = new Vector3(0, 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transf.Translate(forward*Time.deltaTime);
    }
}
