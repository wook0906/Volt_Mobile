using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/FallDecision")]
public class FallDecision : DecisionBase
{
    public override bool Decision(StateMachine fsm)
    {
        SphereCollider collider = fsm.GetComponent<SphereCollider>();

        Collider[] colliders = Physics.OverlapSphere(collider.bounds.center,
            0.4f, fsm.holeMask);

        if(colliders.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
