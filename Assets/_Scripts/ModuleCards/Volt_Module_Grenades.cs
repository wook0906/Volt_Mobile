using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Grenades : Volt_ModuleCardBase, IActiveModuleCard
{
    public AttackType AttackType { get; private set; }

    [Header("Set In Inspector")]
    public GameObject grenadesPrefab;

    public void SetOn()
    {
        ActiveType = 1;
    }

    public void SetOff()
    {
        ActiveType = 2;
    }

    public override void Initialize(Volt_Robot owner)
    {
        base.Initialize(owner);
        AttackType = AttackType.Throw;
    }
}
