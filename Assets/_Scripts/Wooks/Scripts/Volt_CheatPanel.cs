using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Volt_CheatPanel : MonoBehaviour
{
    public static Volt_CheatPanel S;
    [SerializeField]
    Volt_PlayerInfo curSelectedPlayer;
    public List<GameObject> playerBtns;
    public List<List<Card>> playerHasModuleList;
    public GameObject suddenDeathToggle;
    // Start is called before the first frame update
    private void Awake()
    {
        S = this;
    }

    private IEnumerator Start()
    {
        Init();
        yield return new WaitUntil(() => { return Volt_GameManager.S; });

        if (!Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
            StartCoroutine(CoInit());
    }
    public void Init()
    {
        playerHasModuleList = new List<List<Card>>();
        playerHasModuleList.Add(new List<Card>());
        playerHasModuleList.Add(new List<Card>());
        playerHasModuleList.Add(new List<Card>());
        playerHasModuleList.Add(new List<Card>());
    }
    IEnumerator CoInit()
    {
        yield return new WaitUntil(() => Volt_PlayerManager.S.I != null);
        curSelectedPlayer = Volt_PlayerManager.S.GetPlayerByPlayerNumber(1);
        if (Volt_GameManager.S.mapType == MapType.Ruhrgebiet)
        {
            suddenDeathToggle.SetActive(false);
        }
    }
    /*
    public void OnClickPlayerSelectBtn(int playerNumber)
    {
        curSelectedPlayer = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        for (int i = 0; i < playerBtns.Count; i++)
        {
            if (i == playerNumber - 1)
                playerBtns[i].GetComponent<Image>().color = Color.green;
            else
                playerBtns[i].GetComponent<Image>().color = Color.white;
        }
    }*/
    public void OnClickPlayerSelectBtn(GameObject btn)
    {
        int playerNumber = int.Parse(btn.name);
        curSelectedPlayer = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        for (int i = 0; i < playerBtns.Count; i++)
        {
            if (i == playerNumber - 1)
                playerBtns[i].GetComponent<UISprite>().color = Color.green;
            //playerBtns[i].GetComponent<Image>().color = Color.green;
            else
                playerBtns[i].GetComponent<UISprite>().color = Color.green;
            //playerBtns[i].GetComponent<Image>().color = Color.white;
        }
    }
    
    public void NoticeGetModuleCard(int playerNumber, Card card)
    {
        if(!Volt_GameManager.S.IsTutorialMode)
            playerHasModuleList[playerNumber - 1].Add(card);
    }
    public void NoticeLostModuleCard(int playerNumber, Card card)
    {
        if (Volt_GameManager.S.IsTutorialMode) return;
        if (IsHaveSameCard(playerNumber, card))
        {
            playerHasModuleList[playerNumber - 1].Remove(card);
        }
    }

    public void OnClickModuleBtn(GameObject moduleBtn)
    {
        Volt_ModuleCardBase clickedModule;
        if (curSelectedPlayer == null)
            return;
        if (curSelectedPlayer.playerRobot == null) return;
        Volt_Robot robot = curSelectedPlayer.playerRobot.GetComponent<Volt_Robot>();
        switch (moduleBtn.name)
        {
            case "Amargeddon":
                //print("Amargeddon Btn Clicked");
                if (IsHaveSameCard(Card.AMARGEDDON))
                {
                    Volt_GameManager.S.AmargeddonCount = 0;
                    robot.moduleCardExcutor.DestroyCard(Card.AMARGEDDON);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.AMARGEDDON);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.AMARGEDDON))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.AMARGEDDON);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.AMARGEDDON);
                        }
                    }
                }
                break;
            case "Anchor":
                //print("Anchor Btn Clicked");
                if (IsHaveSameCard(Card.ANCHOR))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.ANCHOR);
                    robot.AddOnsMgr.IsHaveAnchor = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.ANCHOR);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.ANCHOR))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.ANCHOR);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.ANCHOR);
                        }
                    }
                }
                break;
            case "Bomb":
                //print("Bomb Btn Clicked");
                if (IsHaveSameCard(Card.BOMB))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.BOMB);
                    robot.AddOnsMgr.IsHaveBomb = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.BOMB);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.BOMB))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.BOMB);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.BOMB);
                        }
                    }
                }
                break;
            case "Crossfire":
                //print("Crossfire Btn Clicked");
                if (IsHaveSameCard(Card.CROSSFIRE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.CROSSFIRE);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.CROSSFIRE);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.CROSSFIRE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.CROSSFIRE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.CROSSFIRE);
                        }
                    }
                }
                break;
            case "Dodge":
                //print("Dodge Btn Clicked");
                if (IsHaveSameCard(Card.DODGE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.DODGE);
                    robot.AddOnsMgr.IsDodgeOn = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.DODGE);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.DODGE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.DODGE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.DODGE);
                        }
                    }
                }
                break;
            case "DoubleAttack":
                //print("DoubleAttack Btn Clicked");
                if (IsHaveSameCard(Card.DOUBLEATTACK))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.DOUBLEATTACK);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.DOUBLEATTACK);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.DOUBLEATTACK))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.DOUBLEATTACK);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.DOUBLEATTACK);
                        }
                    }
                }
                break;
            case "DummyGear":
                //print("DummyGear Btn Clicked");
                if (IsHaveSameCard(Card.DUMMYGEAR))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.DUMMYGEAR);
                    robot.AddOnsMgr.IsDummyGearOn = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.DUMMYGEAR);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.DUMMYGEAR))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.DUMMYGEAR);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.DUMMYGEAR);
                        }
                    }
                }
                break;
            case "EMP":
                //print("EMP Btn Clicked");
                if (IsHaveSameCard(Card.EMP))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.EMP);

                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.EMP);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.EMP))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.EMP);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.EMP);
                        }
                    }
                }
                break;
            case "Grenade":
                //print("Grenade Btn Clicked");
                if (IsHaveSameCard(Card.GRENADES))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.GRENADES);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.GRENADES);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.GRENADES))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.GRENADES);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.GRENADES);
                        }
                    }
                }
                break;
            case "Hacking":
                //print("Hacking Btn Clicked");
                if (IsHaveSameCard(Card.HACKING))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.HACKING);
                    robot.AddOnsMgr.IsHackingOn = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.HACKING);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.HACKING))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.HACKING);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.HACKING);
                        }
                    }
                }
                break;
            case "Pernerate":
                //print("Pernerate Btn Clicked");
                if (IsHaveSameCard(Card.PERNERATE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.PERNERATE);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.PERNERATE);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.PERNERATE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.PERNERATE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.PERNERATE);
                        }
                    }
                }
                break;
            case "PowerBeam":
                //print("PowerBeam Btn Clicked");
                if (IsHaveSameCard(Card.POWERBEAM))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.POWERBEAM);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.POWERBEAM);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.POWERBEAM))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.POWERBEAM);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.POWERBEAM);
                        }
                    }
                }
                break;
            case "RepulsionBlast":
                //print("RepulsionBlast Btn Clicked");
                if (IsHaveSameCard(Card.REPULSIONBLAST))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.REPULSIONBLAST);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.REPULSIONBLAST);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.REPULSIONBLAST))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.REPULSIONBLAST);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.REPULSIONBLAST);
                        }
                    }
                }
                break;
            case "SawBlade":
                //print("SawBlade Btn Clicked");
                if (IsHaveSameCard(Card.SAWBLADE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.SAWBLADE);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.SAWBLADE);
                    robot.AddOnsMgr.IsHaveSawBlade = false;
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.SAWBLADE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.SAWBLADE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.SAWBLADE);
                        }
                    }
                }
                break;
            case "Shield":
                //print("Shield Btn Clicked");
                if (IsHaveSameCard(Card.SHIELD))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.SHIELD);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.SHIELD);
                    robot.AddOnsMgr.ShieldPoints = 0;
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.SHIELD))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.SHIELD);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.SHIELD);
                        }
                    }
                }
                break;
            case "ShockWave":
                //print("ShockWave Btn Clicked");
                if (IsHaveSameCard(Card.SHOCKWAVE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.SHOCKWAVE);
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.SHOCKWAVE);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.SHOCKWAVE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.SHOCKWAVE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.SHOCKWAVE);
                        }

                    }
                }
                break;
            case "SteeringNozzle":
                //print("SteeringNozzle Btn Clicked");
                if (IsHaveSameCard(Card.STEERINGNOZZLE))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.STEERINGNOZZLE);
                    robot.AddOnsMgr.IsSteeringNozzleOn = false;
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.STEERINGNOZZLE);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.STEERINGNOZZLE))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.STEERINGNOZZLE);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.STEERINGNOZZLE);
                        }
                    }
                }
                break;
            case "Teleport":
                //print("Teleport Btn Clicked");
                if (IsHaveSameCard(Card.TELEPORT))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.TELEPORT);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.TELEPORT);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.TELEPORT))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.TELEPORT);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.TELEPORT);
                        }
                    }
                }
                break;
            case "TimeBomb":
                //print("TimeBomb Btn Clicked");
                if (IsHaveSameCard(Card.TIMEBOMB))
                {
                    robot.moduleCardExcutor.DestroyCard(Card.TIMEBOMB);
                    
                    playerHasModuleList[curSelectedPlayer.playerNumber - 1].Remove(Card.TIMEBOMB);
                }
                else
                {
                    if (Volt_ModuleDeck.S.IsHaveModuleCard(Card.TIMEBOMB))
                    {
                        clickedModule = Volt_ModuleDeck.S.GetModuleFromDeck(Card.TIMEBOMB);
                        if (robot.moduleCardExcutor.IsCanEquip(clickedModule))
                        {
                            robot.OnPickupNewModuleCard(clickedModule);
                            playerHasModuleList[curSelectedPlayer.playerNumber - 1].Add(Card.TIMEBOMB);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
    public void RobotDead(Volt_Robot robot)
    {
        playerHasModuleList[robot.playerInfo.playerNumber - 1].Clear();
    }
    public Button GetBtnByName(string name)
    {
        return transform.Find(name).GetComponent<Button>();
    }

    bool IsHaveSameCard(Card card)
    {
        foreach (var item in playerHasModuleList[curSelectedPlayer.playerNumber-1])
        {
            if(item == card)
                return true;
        }
        return false;
    }

    bool IsHaveSameCard(int playerNumber, Card card)
    {
        foreach (var item in playerHasModuleList[playerNumber - 1])
        {
            if (item == card)
                return true;
        }
        return false;
    }

    public void OnPressdownHpBtn(GameObject btn)
    {
        Volt_Robot robot = curSelectedPlayer.playerRobot.GetComponent<Volt_Robot>();
        if (btn.name == "HP+")
        {
            if (!(robot.HitCount <= 0))
                robot.HitCount -= 1;
        }
        else
        {
            if (!(robot.HitCount >= 2))
                robot.HitCount += 1;
        }
    }
    public void OnPressdownVpBtn(GameObject btn)
    {
        //TODO : 테스트를 위해서 서버에도 승점 패킷을 전송할지 선택해야함.
        if (btn.name == "VP+")
        {
            curSelectedPlayer.VictoryPoint++;
        }
        else
            curSelectedPlayer.VictoryPoint--;
    }

    public void OnToggleEndless(UIToggle toggle)
    {
        if (Volt_GameManager.S == null)
            return;
        if (!toggle)
            return;

        if (!toggle.value)
        {
            Volt_GameManager.S.isEndlessGame = false;
        }
        else
        {
            Volt_GameManager.S.isEndlessGame = true;
        }
    }
    public void OnToggleSuddenDeath(UIToggle toggle)
    {
        if (Volt_GameManager.S == null)
            return;
        if (!toggle)
            return;

        if (!toggle.value)
        {
            Volt_GameManager.S.isOnSuddenDeath = false;
        }
        else
        {
            Volt_GameManager.S.isOnSuddenDeath = true;
        }
    }
}
