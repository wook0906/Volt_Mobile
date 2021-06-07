using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_CameraFollow : MonoBehaviour
{
    Transform targetCamera;
    public bool alwaysFollow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alwaysFollow && targetCamera != null)
        {
            LookAt();
        }
    }

    public void LookAt()
    {
        this.transform.LookAt(targetCamera);
    }

}
