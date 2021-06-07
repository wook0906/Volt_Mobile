using UnityEngine;
using UnityEditor;

public class CancelSearchingEnemyPlayerPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("CancelSearchingEnemyPlayerPacket Unpack");

        CommunicationWaitQueue.Instance.ResetOrder();
        CommunicationInfo.IsBoardGamePlaying = false;
        Managers.Scene.LoadSceneAsync(Define.Scene.Lobby);
    }

}