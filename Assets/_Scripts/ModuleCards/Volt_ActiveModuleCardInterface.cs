using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveModuleCard
{
    void SetOn();
    void SetOff();
    AttackType AttackType { get; }
}