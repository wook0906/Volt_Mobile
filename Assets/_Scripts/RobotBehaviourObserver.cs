using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorFlag
{
    None = 0,
    Player1 = 1 << 0,
    Player2 = 1 << 1,
    Player3 = 1 << 2,
    Player4 = 1 << 3
}
public class RobotBehaviourObserver : MonoBehaviour
{
    public int playerInAction;
    public static RobotBehaviourObserver Instance;
    public Volt_Robot currentPusher;
    public BehaviorFlag behaviorFlag { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnBehaviorFlag(int playerNum)
    {
       // Debug.Log(playerNum + " behaviour on");
        playerInAction = playerNum;
        switch (playerNum)
        {
            case 1:
                behaviorFlag |= BehaviorFlag.Player1;
                break;
            case 2:
                behaviorFlag |= BehaviorFlag.Player2;
                break;
            case 3:
                behaviorFlag |= BehaviorFlag.Player3;
                break;
            case 4:
                behaviorFlag |= BehaviorFlag.Player4;
                break;
            default:
                break;
        }
    }

    public void OffBehaviorFlag(int playerNum)
    {
        if (playerInAction == playerNum)
            playerInAction = 0;
       // Debug.Log(playerNum + " behaviour off");
        switch (playerNum)
        {
            case 1:
                behaviorFlag &= ~BehaviorFlag.Player1;
                break;
            case 2:
                behaviorFlag &= ~BehaviorFlag.Player2;
                break;
            case 3:
                behaviorFlag &= ~BehaviorFlag.Player3;
                break;
            case 4:
                behaviorFlag &= ~BehaviorFlag.Player4;
                break;
            default:
                break;
        }
    }

    public bool IsAllRobotBehaviourFlagOff()
    {
        if (behaviorFlag == BehaviorFlag.None)
            return true;
        return false;
    }

    public  bool IsRobotBehaviourFlagOff(int playerNumber)
    {
        bool isOff = true;
        switch (playerNumber)
        {
            case 1:
                isOff = (behaviorFlag & BehaviorFlag.Player1) == 0 ? true : false;
                break;
            case 2:
                isOff = (behaviorFlag & BehaviorFlag.Player2) == 0 ? true : false;
                break;
            case 3:
                isOff = (behaviorFlag & BehaviorFlag.Player3) == 0 ? true : false;
                break;
            case 4:
                isOff = (behaviorFlag & BehaviorFlag.Player4) == 0 ? true : false;
                break;
            default:
                break;
        }
        return isOff;
    }

    public Volt_PlayerInfo GetCurrentBehaviourRobotOwnerPlayer()
    {
        switch (behaviorFlag)
        {
            case BehaviorFlag.Player1:
                return Volt_PlayerManager.S.GetPlayerByPlayerNumber(1);
            case BehaviorFlag.Player2:
                return Volt_PlayerManager.S.GetPlayerByPlayerNumber(2);
            case BehaviorFlag.Player3:
                return Volt_PlayerManager.S.GetPlayerByPlayerNumber(3);
            case BehaviorFlag.Player4:
                return Volt_PlayerManager.S.GetPlayerByPlayerNumber(4);
            default:
                Debug.Log("GetCurrentBehaviourRobotOwnerPlayer Error1");
                break;
        }
        Debug.Log("GetCurrentBehaviourRobotOwnerPlayer Error2");
        return null;
    }

}
