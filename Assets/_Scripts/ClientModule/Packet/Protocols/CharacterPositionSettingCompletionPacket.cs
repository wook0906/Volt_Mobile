using UnityEngine;

public class CharacterPositionSettingCompletionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("CharacterPositionSettingCompletionPacket Unpack");
        Volt_GameManager.S.SelectBehaviourTypeStart();
        Volt_PlayerUI.S.ShowModuleButton(true);
    }
}