
using UnityEngine;

public class SimulationCompletionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("SimulationCompletionPacket Unpack");

        Volt_GameManager.S.ResolutionStart();

    }
}