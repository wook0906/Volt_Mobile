using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Hacking : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        //Debug.Log("Pick up hacking module");
        owner.AddOnsMgr.IsHackingOn = true;
    }

    public void SetOff()
    {
        owner.AddOnsMgr.IsHackingOn = false;
    }
}
