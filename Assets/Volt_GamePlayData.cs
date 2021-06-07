using System.Collections.Generic;
using UnityEngine;



public class Volt_GamePlayData : MonoBehaviour
{
    public static Volt_GamePlayData S;
    public bool isPracticeMode;
    public Dictionary<int, List<int>> numberOfRobotsKilledByPlayerInThisGame;


    public void OnPlayGame(bool isPracticeMode)
    {
        this.isPracticeMode = isPracticeMode;
    }
    /// <summary>
    /// killPlayer가 얼마나 destroy를 행하였는지 저장 및 갱신한다. 이 함수는 RenewOtherRobotsAttackedByRobotsOnThatTurn 인해 자동으로 호출된다.
    /// </summary>
    /// <param name="killPlayer"></param>
    /// <param name="destroyedPlayer"></param>
    public void RenewNumberOfRobotsKilledByPlayerInThisGame(int killPlayer, int destroyedPlayer)
    {
        if (killPlayer == 0) return;
        if (isPracticeMode) return;
        //Debug.Log("RenewNumberOfRobotsKilledByPlayerInThisGame");

        numberOfRobotsKilledByPlayerInThisGame[killPlayer].Add(destroyedPlayer);

        //List<int> tmpList;
        //if (numberOfRobotsKilledByPlayerInThisGame.TryGetValue(killPlayer, out tmpList))
        //{
        //    tmpList.Add(destroyedPlayer);
        //}
        //else
        //{
        //    Debug.Log("RenewNumberOfRobotsKilledByPlayer Error");
        //    return;
        //}
        //numberOfRobotsKilledByPlayerInThisGame[killPlayer] = tmpList;

        if (IsKillAllTheOtherRobotsInThisGame(killPlayer))
        {
            //Debug.Log("");
            //DB 2000012 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000012, killPlayer,true);
        }
    }

    public Dictionary<int, List<int>> numberOfRobotsKilledByPlayerInThisTurn;
    /// <summary>
    /// 한턴에서 killPlayer가 얼마나 destroy를 행하였는지 저장 및 갱신한다. 이 함수는 RenewOtherRobotsAttackedByRobotsOnThatTurn 인해 자동으로 호출된다.
    /// </summary>
    /// <param name="killPlayer"></param>
    /// <param name="destroyedPlayer"></param>
    public void RenewNumberOfRobotsKilledByPlayerInThisTurn(int killPlayer, int destroyedPlayer)
    {
        if (killPlayer == 0) return;
        if (isPracticeMode)
        {
            //Debug.Log("연습모드 였다고 한다.");
            return;
        }
        //Debug.Log("RenewNumberOfRobotsKilledByPlayerInThisTurn");
        numberOfRobotsKilledByPlayerInThisTurn[killPlayer].Add(destroyedPlayer);

        if (IsKillAllTheOtherRobotsInThatTurn(killPlayer))
        {
            //DB 2000021 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000021, killPlayer,true);
        }

        RenewNumberOfRobotsKilledByPlayerInThisGame(killPlayer, destroyedPlayer);
    }
    public void ClearNumberOfRobotsKilledByPlayerInThisTurn()
    {
        if (isPracticeMode) return;
        //Debug.Log("ClearNumberOfRobotsKilledByPlayerInThisTurn");
        numberOfRobotsKilledByPlayerInThisTurn.Clear();
    }

    public Dictionary<int, int> numberOfDeadByRobot;
    public void RenewNumberOfDeadByRobot(int deadPlayer)
    {
        if (isPracticeMode) return;
        //Debug.Log("RenewNumberOfDeadByRobot");
        if (numberOfDeadByRobot.ContainsKey(deadPlayer))
            numberOfDeadByRobot[deadPlayer]++;
        //else
            //Debug.Log($"numberOfDeadByRobot not has {deadPlayer} key");
    }

    public Dictionary<int, Dictionary<ModuleType, int>> usedModuleCardTypeCounts; // 각 로봇별로 모듈 카드 사용횟수 체크(타입별로 체크)
    public Dictionary<int, Dictionary<Card, int>> usedModuleCardCounts; // 각 로봇별로 모듈 카드 사용횟수 체크(카드별로)

    public Dictionary<int, Dictionary<Volt_ModuleCardBase, int>> moduleUsedAndFrequencyUsedByEachRobot;
    /// <summary>
    /// 모듈을 쓴 플레이어와 사용된 해당 모듈의 타입과 횟수를 저장 및 갱신한다.
    /// </summary>
    /// <param name="moduleUsedPlayer"></param>
    /// <param name="usedModuleType"></param>
    public void RenewModuleUsedAndFrequencyUsedByEachRobot(int moduleUsedPlayer, Volt_ModuleCardBase usedModuleCard)
    {
        if (isPracticeMode)
        {
            //Debug.Log("연습모드였다고 한다.");
            return;
        }

        UpdateUsedModuleCardCount(moduleUsedPlayer, usedModuleCard.card); // 카드별 횟수
        UpdateUsedModuleCardTypeCount(moduleUsedPlayer, usedModuleCard.moduleType); // 카드타입별 횟수
    }

    public void UpdateUsedModuleCardCount(int playerID, Card card)
    {
        usedModuleCardCounts[playerID][card]++;
//#if UNITY_EDITOR
//        Debug.Log($"Player[{playerID}] use card[{card}] count:[{usedModuleCardCounts[playerID][card]}");
//#endif

        // 나머지 모듈은 여기서 사용금지!! 조건상 여기서 사용될 수 없음!!
        if (playerID != Volt_PlayerManager.S.I.playerNumber)
            return;
        
        switch (card)
        {
            case Card.TELEPORT:
                PacketTransmission.SendAchievementProgressPacket(2000027, playerID,true);
                break;
            case Card.ANCHOR:
                PacketTransmission.SendAchievementProgressPacket(2000032, playerID,true);
                break;
            case Card.HACKING:
                PacketTransmission.SendAchievementProgressPacket(2000030, playerID,true);
                break;
            case Card.DODGE:
                PacketTransmission.SendAchievementProgressPacket(2000026, playerID,true);
                break;
            case Card.EMP:
                PacketTransmission.SendAchievementProgressPacket(2000033, playerID,true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 모듈을 쓴 플레이어와 사용된 해당 모듈의 타입과 횟수를 저장 및 갱신한다.
    /// </summary>
    /// <param name="moduleUsedPlayer"></param>
    /// <param name="usedModuleType"></param>
    public void UpdateUsedModuleCardTypeCount(int moduleUsedPlayer, ModuleType moduleType)
    {
        usedModuleCardTypeCounts[moduleUsedPlayer][moduleType]++;
//#if UNITY_EDITOR
//            Debug.Log($"Player[{moduleUsedPlayer}] use moduletype[{moduleType}] count:[{usedModuleCardTypeCounts[moduleUsedPlayer][moduleType]}]");
//#endif

        if (IsRobotUseAllTypeModuleInThisGame(moduleUsedPlayer))
        {
            //DB 2000019 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000019, moduleUsedPlayer,true);
        }
        if (IsRobotUseAttackModuleMoreThanFiveTimesInGame(moduleUsedPlayer))
        {
            //DB 2000017 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000017, moduleUsedPlayer,true);
        }
        else if (IsRobotUseMovementModuleMoreThanFiveTimesInGame(moduleUsedPlayer))
        {
            //DB 2000018 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000018, moduleUsedPlayer,true);
        }
        else if (IsRobotUseTacticModuleMoreThanFiveTimesInGame(moduleUsedPlayer))
        {
            //DB 2000016 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000016, moduleUsedPlayer,true);
        }
    }

    public Dictionary<int, Dictionary<int, int>> otherRobotsAttackedByRobotsOnThisTurn;
    /// <summary>
    /// 해당 턴에 공격을 하는 로봇이 공격에 성공한 로봇들과 그 로봇들의 HitCount를 저장 및 갱신한다.
    /// </summary>
    /// <param name="attackPlayer"></param>
    /// <param name="hittedPlayer"></param>
    /// <param name="hittedPlayerHitCount"></param>
    public void RenewOtherRobotsAttackedByRobotsOnThatTurn(int attackPlayer, int hittedPlayer, int hittedPlayerHitCount)
    {
        if (attackPlayer == 0)
            return;

        if (isPracticeMode)
        {
            //Debug.Log("연습게임이었다고 한다.");
            return;
        }
        

        otherRobotsAttackedByRobotsOnThisTurn[attackPlayer][hittedPlayer] = hittedPlayerHitCount;
        
        if (IsAttackAllTheOtherRobotsOnThatTurn(attackPlayer))
        {
            //DB 2000020 업적진행
            PacketTransmission.SendAchievementProgressPacket(2000020, attackPlayer,true);
        }
    }
    /// <summary>
    /// 한턴이 끝나면 호출하여 해당턴에서 공격한 정보는 초기화 한다.
    /// </summary>
    public void ClearOtherRobotsAttackedByRobotsOnThisTurn()
    {
        if (isPracticeMode) return;

        for (int i = 1; i < 5; i++)
        {
            otherRobotsAttackedByRobotsOnThisTurn[i].Clear();
            numberOfRobotsKilledByPlayerInThisTurn[i].Clear();
        }
    }
    /// <summary>
    /// 현재 Attacker가 모든 로봇들을 공격하는데 성공했는지?
    /// </summary>
    /// <param name="currentAttacker"></param>
    /// <returns></returns>
    public bool IsAttackAllTheOtherRobotsOnThatTurn(int currentAttacker)
    {
        if (currentAttacker == 0) return false;
        if (otherRobotsAttackedByRobotsOnThisTurn[currentAttacker].Count >= 3)
        {
            //Debug.Log($"{currentAttacker}는 이번턴에 모든 플레이어를 때렸어요!");
            return true;
        }
        //Debug.Log($"{currentAttacker}는 이번턴에 {otherRobotsAttackedByRobotsOnThisTurn[currentAttacker].Count} 만큼의 플레이어를 때렸어요!");
        return false;
        
    }
    /// <summary>
    /// 현재 Attacker가 모든 로봇들 죽이는데 성공했는지?
    /// </summary>
    /// <param name="currnetAttacker"></param>
    /// <returns></returns>
    public bool IsKillAllTheOtherRobotsInThatTurn(int currnetAttacker)
    {
        if (numberOfRobotsKilledByPlayerInThisTurn[currnetAttacker].Count >= 3)
        {
            //Debug.Log($"{currnetAttacker}는 이번턴에 모든 플레이어를 죽였어요!");
            return true;
        }
        else
        {
            //Debug.Log($"{currnetAttacker}는 이번턴에 {numberOfRobotsKilledByPlayerInThisTurn[currnetAttacker].Count} 만큼 죽였어요!");
        }
        return false;
    }
    /// <summary>
    /// 공격모듈을 이 경기에서 5번이상 사용하였는지?
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public bool IsRobotUseAttackModuleMoreThanFiveTimesInGame(int playerNumber)
    {
        Dictionary<ModuleType, int> tmpDic;
        if (!usedModuleCardTypeCounts.TryGetValue(playerNumber, out tmpDic))
        {
            //Debug.Log($"usedModuleCardCounts not has {playerNumber} key");
            return false;
        }

        if (tmpDic[ModuleType.Attack] >= 5)
        {
            //Debug.Log($"{playerNumber} 는 공격 모듈을 5회 이상 사용했어요~");
            return true;
        }
        //Debug.Log($"{playerNumber} 는 공격 모듈을 5회 이상 사용하지 못했어요~");
        return false;
    }
    /// <summary>
    /// 이동모듈을 이 경기에서 5번이상 사용하였는지?
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public bool IsRobotUseMovementModuleMoreThanFiveTimesInGame(int playerNumber)
    {
        Dictionary<ModuleType, int> tmpDic;
        if (!usedModuleCardTypeCounts.TryGetValue(playerNumber, out tmpDic))
        {
            //Debug.Log($"usedModuleCardCounts not has {playerNumber} key");
            return false;
        }

        if (tmpDic[ModuleType.Movement] >= 5)
        {
            //Debug.Log($"{playerNumber} 는 이동 모듈을 5회 이상 사용했어요~");
            return true;
        }
        //Debug.Log($"{playerNumber} 는 이동 모듈을 5회 이상 사용하지 못했어요~");
        return false;
    }
    /// <summary>
    /// 전략모듈을 이 경기에서 5번이상 사용하였는지?
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public bool IsRobotUseTacticModuleMoreThanFiveTimesInGame(int playerNumber)
    {
        Dictionary<ModuleType, int> tmpDic;
        if (!usedModuleCardTypeCounts.TryGetValue(playerNumber, out tmpDic))
        {
            //Debug.Log($"usedModuleCardCounts not has {playerNumber} key");
            return false;
        }
        
        if (tmpDic[ModuleType.Tactic] >= 5)
        {
            //Debug.Log($"{playerNumber} 는 전략 모듈을 5회 이상 사용했어요~");
            return true;
        }
        //Debug.Log($"{playerNumber} 는 전략 모듈을 5회 이상 사용하지 못했어요~");
        return false;
    }
    /// <summary>
    /// 모든 타입의 모듈을 이 경기에서 사용하였는지?
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public bool IsRobotUseAllTypeModuleInThisGame(int playerNumber)
    {
        int tacticCnt = 0;
        int attackCnt = 0;
        int movementCnt = 0;


        Dictionary<ModuleType, int> tmpDic;
        if (usedModuleCardTypeCounts.TryGetValue(playerNumber, out tmpDic))
        {
            foreach (var item in tmpDic)
            {
                if (item.Key == ModuleType.Attack)
                    attackCnt += item.Value;
                else if (item.Key == ModuleType.Tactic)
                    tacticCnt += item.Value;
                else if (item.Key == ModuleType.Movement)
                    movementCnt += item.Value;
            }
        }
        //else
        //{
        //    Debug.Log($"moduleUsedAndFrequencyUsedByEachRobot not has {playerNumber} key");
        //}
        if (tacticCnt >= 1 && attackCnt >= 1 && movementCnt >= 1)
        {
            //Debug.Log($"{playerNumber} 는 모든 모듈을 골고루 한번씩 사용했어요~");
            return true;
        }
        //Debug.Log($"{playerNumber} 는 모듈을 attack : {attackCnt}, move : {movementCnt}, tactic : {tacticCnt}, 이렇게 사용했어요~");
        return false;
    }
    /// <summary>
    /// 모든 로봇들을 이 경기에서 처치하였는지?
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public bool IsKillAllTheOtherRobotsInThisGame(int playerNumber)
    {
        List<int> playerNumbers = new List<int>() { 1, 2, 3, 4 };
        playerNumbers.Remove(playerNumber);

        foreach (var item in playerNumbers)
        {
            if (numberOfRobotsKilledByPlayerInThisGame[playerNumber].Contains(item))
                continue;
            else
            {
                //Debug.Log($"{playerNumber} 는 이 게임에서 {item} 번 플레이어를 죽이지 못했어요~");
                return false;
            }
        }
        //Debug.Log($"{playerNumber} 는 모든 플레이어들을 한번씩 다 죽였어요~");
        return true;
    }
    /// <summary>
    /// 내 로봇이 아무도 죽인적이 없는지?
    /// </summary>
    /// <returns></returns>
    public bool IsRobotKillAnyone(int playerNumber)
    {
        List<int> tmpList;
        if (numberOfRobotsKilledByPlayerInThisGame[playerNumber].Count == 0)
        {
            //Debug.Log($"{playerNumber} 는 이 게임에서 아무도 죽이지 않았어요~");
            return true;
        }
        else
        {
            //Debug.Log($"{playerNumber} 는 이 게임에서 누군가를 죽이긴했어요~");
        }
        return false;
    }
    /// <summary>
    /// 내 로봇이 죽은적이 없는지? 
    /// </summary>
    /// <returns></returns>
    public bool IsRobotEverDied(int playerNumber)
    {
        for (int i = 0; i < numberOfRobotsKilledByPlayerInThisGame.Count; i++)
        {
            if (numberOfRobotsKilledByPlayerInThisGame[i + 1].Contains(playerNumber))
            {
                //Debug.Log($"{playerNumber} 는 {i + 1} 번 플레이어에게 죽었었네요~");
                return false;
            }
            else
            {
                //Debug.Log($"{playerNumber} 는 아무에게도 죽지 않았네요~");
                return true;
            }
        }
        return false;
    }



    //캐릭터 타입
    //승점
    //랭크
    //킬
    //데스
    [SerializeField]
    private RobotType robotType;
    public RobotType RobotType
    {
        get { return robotType; }
        set { robotType = value; }
    }

    private int vp;
    public int Coin
    {
        get { return vp; }
        set
        {
            vp = value;
        }
    }

    private int rank;
    public int Rank
    {
        get { return rank; }
        set
        {
            rank = value;
        }
    }

    private int kill;
    public int Kill
    {
        get { return kill; }
        set
        {
            kill = value;
        }
    }

    private int death;
    public int Death
    {
        get { return death; }
        set
        {
            death = value;
            //DB 데스 횟수 증가
        }
    }

    // Start is called before the first frame update

    private void Awake()
    {
        S = this;
        DontDestroyOnLoad(gameObject);
        numberOfRobotsKilledByPlayerInThisGame = new Dictionary<int, List<int>>()
        {
            { 1, new List<int>() },
            { 2, new List<int>() },
            { 3, new List<int>() },
            { 4, new List<int>()}
        };
        numberOfRobotsKilledByPlayerInThisTurn = new Dictionary<int, List<int>>()
        {
            { 1, new List<int>() },
            { 2, new List<int>() },
            { 3, new List<int>() },
            { 4, new List<int>()}
        };
        numberOfDeadByRobot = new Dictionary<int, int>();
        moduleUsedAndFrequencyUsedByEachRobot = new Dictionary<int, Dictionary<Volt_ModuleCardBase, int>>()
        {
            { 1, new Dictionary<Volt_ModuleCardBase, int>(){ } },
            { 2, new Dictionary<Volt_ModuleCardBase, int>(){ } },
            { 3, new Dictionary<Volt_ModuleCardBase, int>(){ } },
            { 4, new Dictionary<Volt_ModuleCardBase, int>(){ } }
        };

        otherRobotsAttackedByRobotsOnThisTurn = new Dictionary<int, Dictionary<int, int>>()
        {
            { 1, new Dictionary<int, int>() },
            { 2, new Dictionary<int, int>() },
            { 3, new Dictionary<int, int>() },
            { 4, new Dictionary<int, int>() }
        };

        usedModuleCardTypeCounts = new Dictionary<int, Dictionary<ModuleType, int>>();
        for (int i = 1; i <= 4; ++i)
        {
            usedModuleCardTypeCounts.Add(i, new Dictionary<ModuleType, int>());
            for(int j = 0; j < (int)ModuleType.Max; ++j)
            {
                usedModuleCardTypeCounts[i].Add((ModuleType)j, 0);
            }
        }

        usedModuleCardCounts = new Dictionary<int, Dictionary<Card, int>>();
        for(int i = 1; i <= 4; ++i)
        {
            usedModuleCardCounts.Add(i, new Dictionary<Card, int>());
            for(int j = 1; j < (int)Card.MAX; ++j)
            {
                usedModuleCardCounts[i].Add((Card)j, 0);
            }
        }
    }

    public void InitModuleUsedAndFrequencyUsedByEachRobot()
    {
        for (int i = 0; i < moduleUsedAndFrequencyUsedByEachRobot.Count; i++)
        {
            foreach (var item in Volt_ModuleDeck.S.attackCardPrefabs)
            {
                moduleUsedAndFrequencyUsedByEachRobot[i + 1].Add(item, 0);
            }
            foreach (var item in Volt_ModuleDeck.S.moveCardPrefabs)
            {
                moduleUsedAndFrequencyUsedByEachRobot[i + 1].Add(item, 0);
            }
            foreach (var item in Volt_ModuleDeck.S.tacticCardPrefabs)
            {
                moduleUsedAndFrequencyUsedByEachRobot[i + 1].Add(item, 0);
            }

        }
    }
}
