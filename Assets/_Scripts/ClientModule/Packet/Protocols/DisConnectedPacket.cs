using UnityEditor;
using UnityEngine;
public class DisConnectedPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("DisConnect Unpack");
        
        ClientSocketModule.Close();
    }
}
