using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ModuleCardExcutor : MonoBehaviour
{
    [Header("Set in script")]
    [SerializeField]
    private const int maxCardEquipSlot = 2;
    [SerializeField]
    private Volt_ModuleCardBase[] curEquipedCards;

    private StateMachine fsm;

    private void Awake()
    {
        fsm = gameObject.GetOrAddComponent<StateMachine>();
        curEquipedCards = new Volt_ModuleCardBase[maxCardEquipSlot]{null,null};
    }


    public void PutNewCardInCards(Volt_ModuleCardBase newCard)
    {
        for (int i = 0; i < curEquipedCards.Length; i++)
        {
            if(curEquipedCards[i] == null)
            {
                if (newCard is IAddOnsModule)
                {
                    ((IAddOnsModule)newCard).OnPickupModule();
                    fsm.Owner.playerInfo.playerPanel.moduleActiveEffects[i].SetActive(true);
                }
                if (GetComponent<Volt_Robot>().playerInfo.PlayerType == PlayerType.AI)
                {
                    if (newCard is IActiveModuleCard)
                        SetOnActiveCard(newCard, i);
                }
                if(newCard is Volt_ExtraModuleCard)
                {
                    ((Volt_ExtraModuleCard)newCard).ExtraModuleCardInit();
                    fsm.Owner.playerInfo.playerPanel.moduleActiveEffects[i].SetActive(true);
                }
                GetComponent<Volt_Robot>().playerInfo.playerPanel.ModuleIconChange(i, newCard.card);

                curEquipedCards[i] = newCard;
                if(fsm.Owner == Volt_PlayerManager.S.I.GetRobot())
                    Volt_PlayerUI.S.GetNewModule(newCard.card);
                break;
            }
        }
    }

    public Volt_ModuleCardBase GetModuleCardByCardType(Card card)
    {
        foreach (var item in curEquipedCards)
        {
            if (item == null)
                continue;
            if(card == item.card)
            {
                return item;
            }
        }
        return null;
    }
    public bool IsHaveModuleCard(Card card)
    {
        foreach (var item in curEquipedCards)
        {
            if (item != null && item.card == card)
            {
                if (item is Volt_ExtraModuleCard)
                {
                    if (!((Volt_ExtraModuleCard)item).isCanUse)
                        return false;
                }
                return true;
            }
        }
        return false;
    }
    public Volt_ModuleCardBase[] GetCurEquipCards()
    {
        return curEquipedCards;
    }

    public List<Volt_ModuleCardBase> GetCurEquipNormalCards()
    {
        List<Volt_ModuleCardBase> normalModuleCards = new List<Volt_ModuleCardBase>();
        for(int i = 0; i < curEquipedCards.Length; ++i)
        {
            if (curEquipedCards[i] != null && curEquipedCards[i].skillType == SkillType.Active)
                normalModuleCards.Add(curEquipedCards[i]);
        }
        return normalModuleCards;
    }

    public void ForcedSetModule(int slotNumber, Volt_ModuleCardBase module, int moduleState)//for Sync
    {

        if (module.card == Card.NONE) return;
        Volt_Robot robot = GetComponent<Volt_Robot>();
        robot.OnPickupNewModuleCard(module);
        if (moduleState == 1)
            SetOnActiveCard(module, slotNumber);
        else if (moduleState == 2)
            SetOffActiveCard(slotNumber);
    }
    public void SetOnActiveCard(Volt_ModuleCardBase card, int slotNum)
    {
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(GetComponent<Volt_Robot>().playerInfo.playerNumber);
        curEquipedCards[slotNum] = card;

        if (slotNum == 0)
        {
            if (curEquipedCards[1] != null)
            {
                if (curEquipedCards[1] is IActiveModuleCard && curEquipedCards[1].activeTime == curEquipedCards[0].activeTime)
                {
                    SetOffActiveCard(1);
                }
            }
        }
        else if(slotNum == 1)
        {
            if (curEquipedCards[0] != null)
            {
                if (curEquipedCards[0] is IActiveModuleCard && curEquipedCards[1].activeTime == curEquipedCards[0].activeTime)
                {
                    SetOffActiveCard(0);
                }
            }
        }

        curEquipedCards[slotNum].ActiveType = 1;
        ((IActiveModuleCard)curEquipedCards[slotNum]).SetOn();
        
        if(player == Volt_PlayerManager.S.I)
            Volt_PlayerUI.S.moduleBtns[slotNum].isActive = true;

        player.playerPanel.moduleActiveEffects[slotNum].SetActive(true);
    }

    public void SetOffActiveCard(int slotNum)
    {
        if (curEquipedCards[slotNum])
        {
            curEquipedCards[slotNum].ActiveType = 2;
            if (curEquipedCards[slotNum] is IActiveModuleCard)
                ((IActiveModuleCard)curEquipedCards[slotNum]).SetOff();

            Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(GetComponent<Volt_Robot>().playerInfo.playerNumber);
            if (player == Volt_PlayerManager.S.I)
                Volt_PlayerUI.S.moduleBtns[slotNum].isActive = false;

            player.playerPanel.moduleActiveEffects[slotNum].SetActive(false);
        }
    }

    public bool IsCanEquip(Volt_ModuleCardBase card)
    {
        if (IsSlotFull()) return false;
        List<Card> cantUseCard = new List<Card> { Card.DUMMYGEAR, Card.HACKING, Card.TELEPORT, Card.STEERINGNOZZLE, Card.SAWBLADE };
        if (fsm.Owner.playerInfo.PlayerType == PlayerType.AI && cantUseCard.Contains(card.card))
        {
            return false;
        }
        foreach (var item in GetCurEquipCards())
        {
            if (item != null)
            {
                if (card.moduleType == ModuleType.Attack || card.moduleType == ModuleType.Movement)
                {
                    if (card.moduleType == item.moduleType)
                    {
                        return false;
                    }
                }
                if (card.card == item.card)
                    return false;
            }
        }
        return true;
    }
    public bool IsSlotFull()
    {
        foreach (var item in GetCurEquipCards())
        {
            if (item == null)
                return false;
        }
        return true;
    }

    
    public void DestroyCardAll()
    {
        for (int i = 0; i < curEquipedCards.Length; i++)
        {
            if (curEquipedCards[i] == null)
                continue;
            DestroyCard(curEquipedCards[i]);
        }
    }
    public void DestroyCard(Volt_ModuleCardBase card)
    {
        for (int i = 0; i < curEquipedCards.Length; i++)
        {
            if(curEquipedCards[i] == card)
            {
                //print(card.card.ToString()+" Destroy");
                if (curEquipedCards[i] is IActiveModuleCard)
                {
                    ((IActiveModuleCard)curEquipedCards[i]).SetOff();
                }
                if (curEquipedCards[i] is IAddOnsModule)
                {
                    ((IAddOnsModule)curEquipedCards[i]).SetOff();
                }
                Volt_ModuleDeck.S.ReturnModuleCard(curEquipedCards[i]);

                Volt_CheatPanel.S.NoticeLostModuleCard(GetComponent<Volt_Robot>().playerInfo.playerNumber, card.card);

                if (GetComponent<Volt_Robot>().playerInfo == Volt_PlayerManager.S.I)
                    Volt_PlayerUI.S.UnEquipModuleCard(i);

                GetComponent<Volt_Robot>().playerInfo.playerPanel.ModuleIconChange(i,Card.NONE);
                fsm.Owner.playerInfo.playerPanel.moduleActiveEffects[i].SetActive(false);
                curEquipedCards[i] = null;
                break;
            }
        }
    }

    public void DestroyCard(Card card)
    {
        for (int i = 0; i < curEquipedCards.Length; i++)
        {
            if (curEquipedCards[i] == null) continue;

            if (curEquipedCards[i].card == card)
            {
                DestroyCard(curEquipedCards[i]);
                break;
            }
        }
    }
    
    public bool ExcuteExtraCard()
    {
        //TODO: Check whether caller is player or killbot
        for (int slotNum = 0; slotNum < curEquipedCards.Length; slotNum++)
        {
            if (!IsEquipCardInSlot(slotNum))
                continue;

            if (curEquipedCards[slotNum] is Volt_ExtraModuleCard)
            {
                if (((Volt_ExtraModuleCard)curEquipedCards[slotNum]).isCanUse)
                {
                    ((Volt_ExtraModuleCard)curEquipedCards[slotNum]).Activated();
                    return true;
                }
                else
                    continue;
            }
        }
        return false;
    }

    private bool IsEquipCardInSlot(int slotNum)
    {
        if (curEquipedCards[slotNum] != null)
            return true;
        return false;
    }

    private bool IsEquipCardInSlot(Volt_ModuleCardBase card)
    {
        if (card != null)
            return true;
        return false;
    }

    private bool IsCanUseThisCard(Volt_ModuleCardBase card, BehaviourType behaviourType)
    {
        if (card.behaviourType == behaviourType)
            return true;
        return false;
    }
}
