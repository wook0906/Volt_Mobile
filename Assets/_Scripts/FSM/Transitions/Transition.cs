using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{
    public DecisionBase decision;
    public StateBase trueState;
    public StateBase falseState;
}
