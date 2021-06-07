using UnityEngine;
using System;

public class ApplicationQuitPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("ApplicationQuitPacket Unpack");
        //Volt_DontDestroyPanel.S.OnDisconnected();
        Application.Quit();
    }
} 