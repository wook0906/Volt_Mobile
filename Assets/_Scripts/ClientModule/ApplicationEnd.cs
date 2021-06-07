using UnityEngine;

public class ApplicationEnd : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        //Debug.Log("OnApplicationQuit()");

        PacketTransmission.SendDisConnectedPacket();

        ClientSocketModule.Close();
    }
}