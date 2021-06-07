using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uvOffset1 : MonoBehaviour
{
    float x = 1f;

    float xOffset;

    void Update()
    {

        xOffset -= (Time.deltaTime * x);
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(xOffset, 0));

    }
}
