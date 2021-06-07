using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_UIRotate : MonoBehaviour
{
    public float speed = 60f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, Time.deltaTime * speed)); 
    }
}
