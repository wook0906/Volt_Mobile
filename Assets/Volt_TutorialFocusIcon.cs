using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_TutorialFocusIcon : MonoBehaviour
{
    Vector3 originScale;
    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Mathf.PingPong(Time.time, 0.3f) + 1f * originScale.x;
        float y = Mathf.PingPong(Time.time, 0.3f) + 1f * originScale.y;
        float z = Mathf.PingPong(Time.time, 0.3f) + 1f * originScale.z;
        transform.localScale = new Vector3(x, y, z);
        transform.GetComponent<UISprite>().alpha = Mathf.PingPong(Time.time * 1.665f, 0.5f)+0.5f;
    }
}
