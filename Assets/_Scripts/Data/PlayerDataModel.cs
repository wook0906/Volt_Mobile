using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string nickname;
    public int battery;
    public int gold;
    public int diamond;
    
    public UserData(string nickname, int battery, int gold, int diamond)
    {
        this.nickname = nickname;
        this.battery = battery;
        this.gold = gold;
        this.diamond = diamond;
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("Nickname: " + this.nickname);
        builder.Append(" /Battery: " + this.battery);
        builder.Append(" /Gold: " + this.gold);
        builder.Append(" /Diamond: " + this.diamond);
        return builder.ToString();
    }
}

public class UserExtraData
{
    public int gamePlay;
    public int kill;
    public int victoryCoin;
    public int dead;
    public int attackTry;
    public int attackSuccess;
    public int victoryCount;

    public UserExtraData(int gamePlay, int kill, int victoryCoin, int dead, int attackTry, int attackSuccess, int victoryCount)
    {
        this.gamePlay = gamePlay;
        this.kill = kill;
        this.victoryCoin = victoryCoin;
        this.dead = dead;
        this.attackTry = attackTry;
        this.attackSuccess = attackSuccess;
        this.victoryCount = victoryCount;
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("GamePlay: " + this.gamePlay);
        builder.Append(" /Kill: " + this.kill);
        builder.Append(" /VictoryCoin: " + this.victoryCoin);
        builder.Append(" /Dead: " + this.dead);
        builder.Append(" /AttackTry: " + this.attackTry);
        builder.Append(" /AttackSuccess: " + this.attackSuccess);
        builder.Append(" /VictoryCount: " + this.victoryCount);
        return builder.ToString();
    }
}

public class UserACHCondition
{
    public List<InfoACH> achInfos = new List<InfoACH>();

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        foreach (var item in achInfos)
        {
            builder.Append(item.ToString() + "/");
        }

        return builder.ToString();
    }
    public class InfoACH
    {
        public int ID;
        public int count;
        public bool isAccomplish;

        public InfoACH(int ID, int count)
        {
            this.ID = ID;
            this.count = count;
        }
        
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("[ACH ID: " + this.ID);
            builder.Append(", ACH Count: " + this.count);
            builder.Append(", IsAccomplish: " + this.isAccomplish + "]");
            return builder.ToString();
        }
    }
}