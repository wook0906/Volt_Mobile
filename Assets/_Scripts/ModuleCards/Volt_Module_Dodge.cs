using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Dodge : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        //Debug.Log("Pick up dodge module");
        owner.AddOnsMgr.IsDodgeOn = true;
    }
    public void SetOff()
    {
        owner.AddOnsMgr.IsDodgeOn = false;
    }
}
