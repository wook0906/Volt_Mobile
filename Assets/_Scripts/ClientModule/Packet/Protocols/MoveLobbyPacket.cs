using UnityEditor;
using UnityEngine;
public class MoveLobbyPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("MoveLobbyPacket Unpack");

        if(Volt_GameManager.S)
            Volt_GameManager.S.pCurPhase = Phase.gameOver;
        
        Volt_PlayerData.instance.NeedReConnection = false;
        CommunicationWaitQueue.Instance.ResetOrder();
        CommunicationInfo.IsBoardGamePlaying = false;
        CommunicationInfo.IsMobileActive = true;

        Managers.Scene.LoadSceneAsync(Define.Scene.Lobby);

    }
}
