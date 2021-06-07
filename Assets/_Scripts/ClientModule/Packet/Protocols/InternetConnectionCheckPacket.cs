using UnityEditor;
using UnityEngine;
public class InternetConnectionCheckPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("InternetConnectionCheckPacket Unpack");
        
    }
}
