using System.Collections;
using UnityEngine;

public class Volt_Module_TimeBomb : Volt_ModuleCardBase, IActiveModuleCard
{
    public AttackType AttackType { get; private set; }

    [Header("Set in inspector")]
    public GameObject timeBombPrefab;
    
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
