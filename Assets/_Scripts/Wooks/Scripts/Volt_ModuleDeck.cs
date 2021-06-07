using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ModuleDeck : MonoBehaviour
{
    public static Volt_ModuleDeck S;

    public List<ModuleDeckSettingData> moduleDeckSettingDatas;
    ModuleDeckSettingData curModuleDeckSetting;

    public List<Volt_ModuleCardBase> attackCardPrefabs;
    public List<Volt_ModuleCardBase> moveCardPrefabs;
    public List<Volt_ModuleCardBase> tacticCardPrefabs;

    public List<Volt_ModuleCardBase> attackCardDeck;
    public List<Volt_ModuleCardBase> moveCardDeck;
    public List<Volt_ModuleCardBase> tacticCardDeck;
    
    [SerializeField]
    List<ModuleType> moduleTypePercentage;
    List<Card> attackCardPercentage;
    List<Card> moveCardPercentage; // etc...
    List<Card> tacticCardPercentage; // etc...확률 반영
                                     // Start is called before the first frame update
    private void Awake()
    {
        S = this;
        moduleTypePercentage = new List<ModuleType>();
        attackCardPercentage = new List<Card>();
        moveCardPercentage = new List<Card>();
        tacticCardPercentage = new List<Card>();
    }
    
    public void SetModuleDeckSettingData(MapType mapType)
    {
        Volt_GamePlayData.S.InitModuleUsedAndFrequencyUsedByEachRobot();

        foreach (var item in moduleDeckSettingDatas)
        {
            if (item.mapType == mapType)
                curModuleDeckSetting = item;
        }
        for (int i = 0; i < curModuleDeckSetting.movementTypePercentage; i++)
            moduleTypePercentage.Add(ModuleType.Movement);
        for (int i = 0; i < curModuleDeckSetting.tacticTypePercentage; i++)
            moduleTypePercentage.Add(ModuleType.Tactic);
        for (int i = 0; i < curModuleDeckSetting.attackTypePercentage; i++)
            moduleTypePercentage.Add(ModuleType.Attack);

        for (int i = 0; i < curModuleDeckSetting.AMARGEDDON; i++)
            tacticCardPercentage.Add(Card.AMARGEDDON);
        for (int i = 0; i < curModuleDeckSetting.ANCHOR; i++)
            tacticCardPercentage.Add(Card.ANCHOR);
        for (int i = 0; i < curModuleDeckSetting.BOMB; i++)
            tacticCardPercentage.Add(Card.BOMB);
        for (int i = 0; i < curModuleDeckSetting.DUMMYGEAR; i++)
            tacticCardPercentage.Add(Card.DUMMYGEAR);
        for (int i = 0; i < curModuleDeckSetting.EMP; i++)
            tacticCardPercentage.Add(Card.EMP);
        for (int i = 0; i < curModuleDeckSetting.HACKING; i++)
            tacticCardPercentage.Add(Card.HACKING);
        for (int i = 0; i < curModuleDeckSetting.SHIELD; i++)
            tacticCardPercentage.Add(Card.SHIELD);


        for (int i = 0; i < curModuleDeckSetting.CROSSFIRE; i++)
            attackCardPercentage.Add(Card.CROSSFIRE);
        for (int i = 0; i < curModuleDeckSetting.DOUBLEATTACK; i++)
            attackCardPercentage.Add(Card.DOUBLEATTACK);
        for (int i = 0; i < curModuleDeckSetting.GRENADES; i++)
            attackCardPercentage.Add(Card.GRENADES);
        for (int i = 0; i < curModuleDeckSetting.POWERBEAM; i++)
            attackCardPercentage.Add(Card.POWERBEAM);
        for (int i = 0; i < curModuleDeckSetting.PERNERATE; i++)
            attackCardPercentage.Add(Card.PERNERATE);
        for (int i = 0; i < curModuleDeckSetting.SAWBLADE; i++)
            attackCardPercentage.Add(Card.SAWBLADE);
        for (int i = 0; i < curModuleDeckSetting.SHOCKWAVE; i++)
            attackCardPercentage.Add(Card.SHOCKWAVE);
        for (int i = 0; i < curModuleDeckSetting.TIMEBOMB; i++)
            attackCardPercentage.Add(Card.TIMEBOMB);


        for (int i = 0; i < curModuleDeckSetting.DODGE; i++)
            moveCardPercentage.Add(Card.DODGE);
        for (int i = 0; i < curModuleDeckSetting.REPULSIONBLAST; i++)
            moveCardPercentage.Add(Card.REPULSIONBLAST);
        for (int i = 0; i < curModuleDeckSetting.TELEPORT; i++)
            moveCardPercentage.Add(Card.TELEPORT);
        for (int i = 0; i < curModuleDeckSetting.STEERINGNOZZLE; i++)
            moveCardPercentage.Add(Card.STEERINGNOZZLE);

    }
    void Start()
    {
        foreach (var item in attackCardPrefabs)
        {
            attackCardDeck.Add(Instantiate(item,this.transform));
            attackCardDeck.Add(Instantiate(item,this.transform));
        }
        foreach (var item in moveCardPrefabs)
        {
            moveCardDeck.Add(Instantiate(item,this.transform));
            moveCardDeck.Add(Instantiate(item,this.transform));
        }
        foreach (var item in tacticCardPrefabs)
        {
            tacticCardDeck.Add(Instantiate(item,this.transform));
            if (item.card == Card.AMARGEDDON) continue;
            tacticCardDeck.Add(Instantiate(item,this.transform));
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D)) {
        //    foreach (var item in moduleTypePercentage)
        //    {
        //        Debug.Log(item.ToString());
        //    }
        //}
    }
    public ModuleType GetRandomModuleType()//랜덤박스 모듈타입 정함.
    {
        int random = Random.Range(0, moduleTypePercentage.Count);

        return moduleTypePercentage[random];
    }
    public Volt_ModuleCardBase GetModuleCard(Card requestModule)
    {
        if (requestModule == Card.NONE) return null;
        foreach (var item in attackCardDeck)
        {
            if (item.card == requestModule)
            {
                return item;
            }
        }
        foreach (var item in moveCardDeck)
        {
            if (item.card == requestModule)
            {
                return item;
            }
        }
        foreach (var item in tacticCardDeck)
        {
            if (item.card == requestModule)
            {
                return item;
            }
        }
        //print("DrawSpecificModuleCard err" + requestModule.ToString());
        return null;
    }
    public bool IsHaveModuleCard(Card card)
    {
        foreach (var item in attackCardDeck)
        {
            if (item.card == card)
            {
                return true;
            }
        }
        foreach (var item in moveCardDeck)
        {
            if (item.card == card)
            {
                return true;
            }

        }
        foreach (var item in tacticCardDeck)
        {
            if (item.card == card)
            {
                return true;
            }
        }
        return false;
    }
    public Volt_ModuleCardBase DrawRandomModuleCard(Volt_RandomBox box)
    {
        
        switch (box.ModuleType)
        {
            case ModuleType.Attack:
                return GetModuleCard(attackCardPercentage[Random.Range(0, attackCardPercentage.Count)]);
            case ModuleType.Movement:
                return GetModuleCard(moveCardPercentage[Random.Range(0, moveCardPercentage.Count)]);
            case ModuleType.Tactic:
                return GetModuleCard(tacticCardPercentage[Random.Range(0, tacticCardPercentage.Count)]);
            default:
                //print("DrawRandomModuleCard Err");
                break;
        }
        //print("DrawRandomModuleCard Err");
        return null;

    }
    //void RemoveModuleFromDeck(Volt_ModuleCardBase cardBase)
    //{
    //    switch (cardBase.moduleType)
    //    {
    //        case ModuleType.Attack:
    //            attackCardDeck.Remove(cardBase);
    //            Debug.LogError($"Remove {cardBase.card.ToString()} From Deck");
    //            break;
    //        case ModuleType.Movement:
    //            moveCardDeck.Remove(cardBase);
    //            Debug.LogError($"Remove {cardBase.card.ToString()} From Deck");
    //            break;
    //        case ModuleType.Tactic:
    //            tacticCardDeck.Remove(cardBase);
    //            Debug.LogError($"Remove {cardBase.card.ToString()} From Deck");
    //            break;
    //        default:
    //            break;
    //    }
    //}
    public Volt_ModuleCardBase GetExistRandomModuleCard(ModuleType moduleType)
    {
        Volt_ModuleCardBase existConfirmedCardBase;
        switch (moduleType)
        {
            case ModuleType.Attack:
                do
                {
                    existConfirmedCardBase = GetModuleCard(attackCardPercentage[Random.Range(0, attackCardPercentage.Count)]);
                }
                while (existConfirmedCardBase == null || !IsExistSpecificModuleInDeck(existConfirmedCardBase));
                
                return existConfirmedCardBase;
            case ModuleType.Movement:
                do
                {
                    existConfirmedCardBase = GetModuleCard(moveCardPercentage[Random.Range(0, moveCardPercentage.Count)]);
                }
                while (existConfirmedCardBase == null || !IsExistSpecificModuleInDeck(existConfirmedCardBase));
                return existConfirmedCardBase;
            case ModuleType.Tactic:
                do
                {
                    existConfirmedCardBase = GetModuleCard(tacticCardPercentage[Random.Range(0, tacticCardPercentage.Count)]);
                }
                while (existConfirmedCardBase == null || !IsExistSpecificModuleInDeck(existConfirmedCardBase));
                return existConfirmedCardBase;
            default:
                //print("DrawRandomModuleCard Err2");
                break;
        }
        //print("DrawRandomModuleCard Err2");
        return null;
    }
    bool IsExistSpecificModuleInDeck(Volt_ModuleCardBase moduleCard)
    {
        switch (moduleCard.moduleType)
        {
            case ModuleType.Attack:
                foreach (var item in attackCardDeck)
                {
                    if (item == moduleCard)
                    {
                        //Debug.LogWarning(moduleCard.card.ToString() + " is Exist in Deck");
                        return true;
                    }
                }
                break;
            case ModuleType.Movement:
                foreach (var item in moveCardDeck)
                {
                    if (item == moduleCard)
                    {
                        //Debug.LogWarning(moduleCard.card.ToString() + " is Exist in Deck");
                        return true;
                    }
                }
                break;
            case ModuleType.Tactic:
                foreach (var item in tacticCardDeck)
                {
                    if (item == moduleCard)
                    {
                        //Debug.LogWarning(moduleCard.card.ToString() + " is Exist in Deck");
                        return true;
                    }
                }
                break;
            default:
                break;
        }
        //Debug.LogError(moduleCard.card.ToString() + " is Not Exist in Deck");
        return false;
    }
    public void ReturnModuleCard(Volt_ModuleCardBase moduleCard) //모듈카드를 사용 한 후, 여기에 타입에 따라 해당덱에 카드를 반환한다.
    {
        //Debug.Log("Return : " + moduleCard.card.ToString());
        switch (moduleCard.moduleType)
        {
            case ModuleType.Attack:
                //공격카드라면
                attackCardDeck.Add(moduleCard); //덱에 반납한다...
                break;
            case ModuleType.Movement:
                moveCardDeck.Add(moduleCard);
                break;
            case ModuleType.Tactic:
                tacticCardDeck.Add(moduleCard);
                break;
            default:
                break;
        }
    }
    
    public Volt_ModuleCardBase DrawRandomCard(ModuleType moduleType)
    {
        Volt_ModuleCardBase existConfirmedCardBase = GetExistRandomModuleCard(moduleType);

        //Debug.LogError(existConfirmedCardBase.card.ToString() + " Drawed!");
        return existConfirmedCardBase;
    }
    public Volt_ModuleCardBase GetModuleFromDeck(Card card)
    {
        Volt_ModuleCardBase cardBase = SearchModuleFromDeck(card);
        if (!cardBase)
        {
            //Debug.LogError("GetModuleFromDeck Error CardBase is null");
            return null;
        }
        switch (cardBase.moduleType)
        {
            case ModuleType.Attack:
                foreach (var item in attackCardDeck)
                {
                    if(item == cardBase)
                    {
                        attackCardDeck.Remove(item);
                        //Debug.LogError($"Remove {item.card.ToString()} From Deck");
                        return item;
                    }
                }
                break;
            case ModuleType.Movement:
                foreach (var item in moveCardDeck)
                {
                    if (item == cardBase)
                    {
                        moveCardDeck.Remove(item);
                        //Debug.LogError($"Remove {item.card.ToString()} From Deck");
                        return item;
                    }
                }
                break;
            case ModuleType.Tactic:
                foreach (var item in tacticCardDeck)
                {
                    if (item == cardBase)
                    {
                        tacticCardDeck.Remove(item);
                        //Debug.LogError($"Remove {item.card.ToString()} From Deck");
                        return item;
                    }
                }
                break;
            default:
                break;
        }
        
        return null;
    }
    public Volt_ModuleCardBase SearchModuleFromDeck(Card card)
    {
        foreach (var item in attackCardDeck)
        {
            if (item.card == card)
                return item;
        }
        foreach (var item in moveCardDeck)
        {
            if (item.card == card)
                return item;
        }
        foreach (var item in tacticCardDeck)
        {
            if (item.card == card)
                return item;
        }
        //Debug.LogError("SearchModuleFromDeck Error Card is : " + card.ToString());
        return null;
    }
    
}
