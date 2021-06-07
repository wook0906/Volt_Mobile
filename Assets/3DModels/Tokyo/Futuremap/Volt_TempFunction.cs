using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_TempFunction : MonoBehaviour
{
    public float min;
    public float max;
    public float speed;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        mr.material.mainTextureScale = new Vector2(0, Mathf.PingPong(Time.time * speed, max - min) + min);
    }

}
