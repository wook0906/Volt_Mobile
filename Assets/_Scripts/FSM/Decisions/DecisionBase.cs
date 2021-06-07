using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionBase : ScriptableObject
{
    public abstract bool Decision(StateMachine fsm);
}
