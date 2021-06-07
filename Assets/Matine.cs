using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matine : MonoBehaviour
{
    public float radius = 3f;
    public float speed = 2f;
    float runningTime;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        runningTime += Time.deltaTime * speed;
        float x = radius * Mathf.Cos(runningTime);
        float z = radius * Mathf.Sin(runningTime);
        this.transform.position = new Vector3(x,this.transform.position.y,z);
        transform.LookAt(target);
        //Debug.Log(x + "," + z + ", " + runningTime);
    }
}
