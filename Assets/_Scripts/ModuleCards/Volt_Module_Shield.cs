using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Shield : Volt_ModuleCardBase, IAddOnsModule
{
    [Header("Set In inspector")]
    public int shieldPoints = 2;

    public void OnPickupModule()
    {
        //Debug.Log("Pick up Shield module");
        OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseShield, owner.playerInfo.playerNumber);
        owner.AddOnsMgr.ShieldPoints = shieldPoints;
    }

    public void SetOff()
    {
        if(owner.AddOnsMgr.ShieldPoints != 0)
            owner.AddOnsMgr.ShieldPoints = 0;
    }
}
