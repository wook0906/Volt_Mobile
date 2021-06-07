using UnityEngine;
using UnityEditor;
using Facebook.Unity;

public class DeleteUserPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("DeleteUserPacket Unpack");
    }

}