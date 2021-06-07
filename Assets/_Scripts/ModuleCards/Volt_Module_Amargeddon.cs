using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Amargeddon : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        //Debug.Log("Pick up amargeddon module");
        OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseAmargeddon, owner.playerInfo.playerNumber);
        if (Volt_GameManager.S.AmargeddonCount == 0)
        {
            Volt_GameManager.S.AmargeddonCount = 8;
            Volt_GameManager.S.AmargeddonPlayer = owner.playerInfo.playerNumber;
        }
    }

    public void SetOff()
    {
        Volt_GameManager.S.AmargeddonCount = 0;
        Volt_GameManager.S.AmargeddonPlayer = 0;
    }
}
