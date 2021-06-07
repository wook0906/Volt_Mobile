using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesAttack : BaseAttack
{
    public override void Init(StateMachine fsm, Volt_ModuleCardBase moduleCard)
    {
        base.Init(fsm, moduleCard);
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        GameObject grenadesGo = Volt_PrefabFactory.S.PopObject(Define.Objects.GRENADE);
        if (grenadesGo == null)
            return;

        Vector3 pos = fsm.transform.position;
        pos.y += fsm.transform.localScale.y * .5f;
        grenadesGo.transform.position = pos;
        grenadesGo.GetComponent<Volt_Grenades>().Initialize(fsm.Owner,
            fsm.transform);

        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
            fsm.behavior.Direction, fsm.behavior.BehaviorPoints);
        grenadesGo.GetComponent<Volt_Grenades>().DoMove(targetTile);

        List<Volt_Tile> targetTiles = new List<Volt_Tile>(targetTile.GetAdjecentTiles());
        targetTiles.Add(targetTile);
        BlinkTiles(targetTiles.ToArray());
    }
}
