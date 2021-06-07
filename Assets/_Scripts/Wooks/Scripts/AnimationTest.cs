using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            anim.StopPlayback();
            anim.Play("Movement");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.StopPlayback();
            anim.Play("Idle");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.StopPlayback();
            anim.Play("Stop");
        }
    }
}
