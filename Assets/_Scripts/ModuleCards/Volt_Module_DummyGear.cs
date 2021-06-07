using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_DummyGear : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        //Debug.Log("Pick up DummyGear module");
        owner.AddOnsMgr.IsDummyGearOn = true;
    }
    
    public void SetOff()
    {
        owner.AddOnsMgr.IsDummyGearOn = false;
    }
}
