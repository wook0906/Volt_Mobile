using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBombAttack : BaseAttack
{
    public override void Init(StateMachine fsm, Volt_ModuleCardBase moduleCard)
    {
        base.Init(fsm, moduleCard);

    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        //Debug.Log("Throw TimeBomb!");
        GameObject timeBombGO = Volt_PrefabFactory.S.PopObject(Define.Objects.TIMEBOMB);
        timeBombGO.GetComponent<Volt_TimeBomb>().StartWaitMoveCoroutine();
        if (timeBombGO == null)
            return;

        Vector3 pos = fsm.transform.position;
        pos.y += fsm.transform.localScale.y * .5f;
        timeBombGO.transform.position = pos;
        timeBombGO.GetComponent<Volt_TimeBomb>().Initialize(fsm.Owner,
            fsm.transform);

        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
            fsm.behavior.Direction, fsm.behavior.BehaviorPoints);
        timeBombGO.GetComponent<Volt_TimeBomb>().DoMove(targetTile);

        BlinkTiles(new Volt_Tile[1] { targetTile });
    }
}
