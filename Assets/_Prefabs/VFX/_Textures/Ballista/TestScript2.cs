using System;

using System.IO;

using System.Net.Sockets;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.Globalization;

public class TestScript2 : MonoBehaviour
{
    public Transform dest;
    public float speed = 1f;
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position = Vector3.MoveTowards(transform.position, dest.position, Time.deltaTime * speed);
        }
    }
}
