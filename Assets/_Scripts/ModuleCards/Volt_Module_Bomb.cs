using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Bomb : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        Debug.Log("Pick up bomb module");
        owner.AddOnsMgr.IsHaveBomb = true;
    }


    public void SetOff()
    {
        owner.AddOnsMgr.IsHaveBomb = false;
    }
}
