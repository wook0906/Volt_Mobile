using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_SawBlade : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        //Debug.Log(owner.playerInfo.playerNumber + " Pick up SawBlade module");
        StartCoroutine(DelayedPickupModule());
    }
    IEnumerator DelayedPickupModule()
    {
        yield return new WaitUntil(() => Volt_GameManager.S.pCurPhase != Phase.simulation);
        owner.AddOnsMgr.IsHaveSawBlade = true;
    }

    public void SetOff()
    {
        //Debug.Log(owner.playerInfo.playerNumber + "Set Off Sawblade");
        owner.AddOnsMgr.IsHaveSawBlade = false;
    }
}
