using UnityEditor;
using UnityEngine;
public class ReConnectBattlingRoomPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("ReConnectBattlingRoomPacket Unpack");
        
        

        Volt_PlayerData.instance.NeedReConnection = false;
        Volt_GameManager.S.pCurPhase = Phase.waitSync;
        Volt_GameManager.S.ForcedStopSimulate();
        Volt_GameManager.S.behaviourStack.Clear();
        Volt_GameManager.S.tmpBehaviours.Clear();

        Volt_GMUI.S.IsTickOn = false;
        Volt_GMUI.S.TickTimer = 99;

        //Debug.Log("ReConnectBattlingRoom To : " + Volt_GameManager.S.pCurPhase.ToString());

        switch (Volt_GameManager.S.pCurPhase)
        {
            case Phase.robotSetup:
                foreach (var item in Volt_PlayerManager.S.I.startingTiles)
                {
                    item.responseParticle.Stop();
                    item.responseParticle.gameObject.SetActive(false);
                }
                break;
            case Phase.behavoiurSelect:
                Volt_PlayerUI.S.BehaviourSelectOff();
                Volt_PlayerUI.S.ShowModuleButton(false);
                break;
            case Phase.rangeSelect:
                Volt_PlayerManager.S.I.playerCamRoot.CamInit();
                Volt_PlayerManager.S.I.playerCamRoot.SaveLastInfo();
                foreach (var item in Volt_ArenaSetter.S.GetTileArray())
                {
                    item.SetDefaultBlinkOption();
                    item.BlinkOn = false;
                }
                break;
        }
        Volt_GameManager.S.screenBlockPanel.SetActive(true);
        //²°À»‹š
    }
}
