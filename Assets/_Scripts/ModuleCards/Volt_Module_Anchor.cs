using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Anchor : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        Debug.Log("Pick up anchor module");
        owner.AddOnsMgr.IsHaveAnchor = true;
    }

    

    public void SetOff()
    {
        owner.AddOnsMgr.IsHaveAnchor = false;
    }
}
