using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Volt_ExtraModuleCard : Volt_ModuleCardBase
{
    public bool isCanUse = false;
    
    /// <summary>
    /// 새로운 행동 정보를 모듈카드에 맞게 생성후 리턴한다.
    /// </summary>
    /// <param name="curBehavior"></param>
    /// <returns></returns>
    public virtual Volt_RobotBehavior CreateBehavior(Volt_RobotBehavior curBehavior)
    {
        Volt_RobotBehavior behavior = new Volt_RobotBehavior();
        behavior.BehaviourType = curBehavior.BehaviourType;
        behavior.ActiveTime = activeTime;
        return behavior;
    }
    public void ExtraModuleCardInit()
    {
        isCanUse = false;
        StartCoroutine(DelayedEnterCanUseState());
    }

    IEnumerator DelayedEnterCanUseState()
    {
        yield return new WaitUntil(()=> Volt_GameManager.S.pCurPhase == Phase.behavoiurSelect);
        isCanUse = true;
    }
    public abstract void Activated();
}
