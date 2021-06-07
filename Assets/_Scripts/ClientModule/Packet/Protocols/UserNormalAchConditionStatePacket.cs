using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Facebook.Unity;

public class UserNormalAchConditionStatePacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //int startIndex = PacketInfo.FromServerPacketSettingIndex;

        //int rowCount = ByteConverter.ToInt(buffer, ref startIndex);
        //int gamePlay = ByteConverter.ToInt(buffer, ref startIndex);
        //int kill = ByteConverter.ToInt(buffer, ref startIndex);
        //int victoryCoin = ByteConverter.ToInt(buffer, ref startIndex);
        //int dead = ByteConverter.ToInt(buffer, ref startIndex);
        //int attackTry = ByteConverter.ToInt(buffer, ref startIndex);
        //int attackSuccess = ByteConverter.ToInt(buffer, ref startIndex);
        //int victoryCount = ByteConverter.ToInt(buffer, ref startIndex);

        //Debug.Log(rowCount);
        //Debug.Log("Cumulative Game Play count : " + gamePlay);
        //Debug.Log("Cumulative kill count : " + kill);
        //Debug.Log("Cumulative victory Coin : " + victoryCoin);
        //Debug.Log("Cumulative Dead count : " + dead);
        //Debug.Log("Cumulative Attack Try count : " + attackTry);
        //Debug.Log("Cumulative Attack Success count : " + attackSuccess);
        //Debug.Log("Cumulative Victory count : " + victoryCount);

        //DBManager.instance.userExtraData = new UserExtraData(gamePlay, kill, victoryCoin, dead,
        //    attackTry, attackSuccess, victoryCount);

        //DBManager.instance.OnLoadedUserACHConditionState();




        /*
         UserDaily(Normal)Condition 2개 관련 코드들 전부 제거
         UserDaily(Normal)ConditionState를 추가하였음
         해당 클래스에서 UserDaily(Normal)Condition에서 진행하던 코드를 실행시키면 됨.
         */


        Debug.Log("UserNormalAchConditionStatePacket Unpack");
        int startIndex = PacketInfo.FromServerPacketSettingIndex;
        int id = 2000001;
        int length = 0;

        for (int i = 0; i < (int)ENormalConditionType.End; i++)
        {
            
            int value = ByteConverter.ToInt(buffer, ref startIndex);
            if (i == 0) // 게임 플레이 횟수가 들어온다.
            {
                Volt_PlayerData.instance.PlayCount = value;

                for (int j = 0; j < 2; ++j)
                {
                    if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + length))
                        Volt_PlayerData.instance.AchievementProgresses.Add(id + length, new ACHProgress());

                    Volt_PlayerData.instance.AchievementProgresses[id + length].SetAchievementProgress(value);
                    length++;
                }
            }
            else if (i == 1) // 적을 죽인 횟수
            {
                Volt_PlayerData.instance.KillCount = value;

                for (int j = 0; j < 2; ++j)
                {
                    if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + length))
                        Volt_PlayerData.instance.AchievementProgresses.Add(id + length, new ACHProgress());

                    Volt_PlayerData.instance.AchievementProgresses[id + length].SetAchievementProgress(value);
                    length++;
                }

            }
            else if (i == 2) // 승점 코인 획득 갯수
            {
                Volt_PlayerData.instance.CoinCount = value;
                for (int j = 0; j < 3; ++j)
                {

                    if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + length))
                        Volt_PlayerData.instance.AchievementProgresses.Add(id + length, new ACHProgress());

                    Volt_PlayerData.instance.AchievementProgresses[id + length].SetAchievementProgress(value);
                    length++;
                }
            }
            else if (i == 3) // 죽은 횟수
            {
                Volt_PlayerData.instance.DeathCount = value;
            }
            else if (i == 4) // 공격 횟수
            {
                Volt_PlayerData.instance.AttackTryCount = value;
            }
            else if (i == 5) // 공격 성공횟수{
            {
                Volt_PlayerData.instance.AttackSuccessCount = value;
            }
            else if (i == 6) // 승리 횟수
            {
                Volt_PlayerData.instance.VictoryCount = value;

                for(int j = 0; j < 3; j++)
                {
                    if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + length))
                        Volt_PlayerData.instance.AchievementProgresses.Add(id + length, new ACHProgress());

                    Volt_PlayerData.instance.AchievementProgresses[id + length].SetAchievementProgress(value);
                    length++;
                }
            }
            else // 나머지....
            {
                if (!Volt_PlayerData.instance.AchievementProgresses.ContainsKey(id + length))
                    Volt_PlayerData.instance.AchievementProgresses.Add(id + length, new ACHProgress());

                Volt_PlayerData.instance.AchievementProgresses[id + length].SetAchievementProgress(value);
                length++;
            }
        }
        DBManager.instance.OnLoadedUserNormalACHCondition();
    }
}