using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleType
{
    Attack,Movement,Tactic,Max
}
public enum BehaviourType
{
    None, Move, Attack
}

public abstract class Volt_ModuleCardBase : MonoBehaviour
{
    [Header("SetInspector")]
    public Card             card;
    public SkillType        skillType;
    public BehaviourType    behaviourType;
    public ActiveTime       activeTime;
    public ModuleType       moduleType;
    
    [Header("SetInScript")]
    [SerializeField]
    protected Volt_Robot    owner;
    int activeType;
    public int ActiveType
    {
        get { return activeType; }
        set
        {
            activeType = value;
            //Debug.Log($"#############[AtiveType Setter] Card:{card.ToString()} chagne value:{activeType}");
        }
    }
    
    public virtual void Initialize(Volt_Robot owner) //0 == passive 1 == Active On 2 == Active Off
    {
        this.owner = owner;
        if (skillType == SkillType.Passive)
            ActiveType = 0;
        else
            ActiveType = 2;
    }

    // 여기에 사용 후 공통으로 처리해줘야할 것이 있다면 넣으면됨
    public virtual void OnUseCard()
    {
        Volt_GamePlayData.S.RenewModuleUsedAndFrequencyUsedByEachRobot(owner.playerInfo.playerNumber,
            this);

        owner.moduleCardExcutor.DestroyCard(this.card);
    }
}
