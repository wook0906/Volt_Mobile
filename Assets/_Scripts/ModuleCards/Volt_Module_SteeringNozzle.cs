using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_SteeringNozzle : Volt_ModuleCardBase, IAddOnsModule
{
    public void OnPickupModule()
    {
        owner.AddOnsMgr.IsSteeringNozzleOn = true;
    }

    public void SetOff()
    {
        owner.AddOnsMgr.IsSteeringNozzleOn = false;
    }
}
