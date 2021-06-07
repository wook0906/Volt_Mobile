using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Volt_PlayerPanel : MonoBehaviour
{
    [Header("Set in Inspector")]
    public List<UISprite> moduleSprites;
    public List<GameObject> moduleActiveEffects;
    //public List<UISprite> shieldSprites;
    public UISprite charIcon;
    public UILabel IDLabel;
    public UILabel pointLabel;
    bool isAnimationDone = true;

    [Header("Set in Script")]
    public Volt_PlayerInfo ownerPlayer;

    private void Start()
    {
        
    }
    public void SetOwner(Volt_PlayerInfo player)
    {
        ownerPlayer = player;
        //HpRenew(ownerRobot.HitCount);
    }
    public void Init(Volt_PlayerInfo playerInfo)
    {
        //SetPicture(playerInfo.playerNumber, playerInfo.PlayerType);
        //Debug.LogError(this.name + " is " + playerInfo.playerNumber + "\'s Panel");
        IDLabel.text = playerInfo.NickName;
        SetOwner(playerInfo);
    }

    public void RenewPoint(int VP)
    {
        if (!gameObject.activeInHierarchy) return;

        pointLabel.text = VP.ToString();
        if(isAnimationDone)
            StartCoroutine(RenewPointAnimation());
    }
    IEnumerator RenewPointAnimation()
    {
        isAnimationDone = false;
        Vector3 originScale = pointLabel.transform.localScale;
        pointLabel.transform.localScale *= 2f;
        pointLabel.color = Color.yellow;
        float t = 0f;
        while (t <= 1f)
        {
            pointLabel.transform.localScale = Vector3.Lerp(pointLabel.transform.localScale, originScale, 0.1f);
            pointLabel.color = Color.Lerp(pointLabel.color, Color.white, 0.1f);
            t += Time.fixedDeltaTime;
            yield return null;
        }
        pointLabel.transform.localScale = originScale;
        pointLabel.color = Color.white;
        isAnimationDone = true;
    }

    public void ModuleIconChange(int slotNumber, Card card)
    {
        if (card == Card.NONE)
        {
            moduleSprites[slotNumber].spriteName = "NoneFrame";
            return;
        }
        moduleSprites[slotNumber].spriteName = card.ToString();
    }
    public void SetPicture(int playerNumber, PlayerType playerType)
    {
        Volt_PlayerInfo playerInfo = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        switch (playerInfo.RobotType)
        {
            case RobotType.Volt:
                if (Volt_PlayerManager.S.I == playerInfo)
                    charIcon.spriteName = "Player_VoltPic";
                else
                {
                    if (playerInfo.PlayerType == PlayerType.AI)
                        charIcon.spriteName = "VoltPic_Gray";
                    else
                        charIcon.spriteName = "VoltPic";
                }
                break;
            case RobotType.Mercury:
                if (Volt_PlayerManager.S.I == playerInfo)
                    charIcon.spriteName = "Player_MercuryPic";
                else
                {
                    if (playerInfo.PlayerType == PlayerType.AI)
                        charIcon.spriteName = "MercuryPic_Gray";
                    else
                        charIcon.spriteName = "MercuryPic";
                }
                break;
            case RobotType.Hound:
                if (Volt_PlayerManager.S.I == playerInfo)
                    charIcon.spriteName = "Player_HoundPic";
                else
                {
                    if (playerInfo.PlayerType == PlayerType.AI)
                        charIcon.spriteName = "HoundPic_Gray";
                    else
                        charIcon.spriteName = "HoundPic";
                }
                break;
            case RobotType.Reaper:
                if (Volt_PlayerManager.S.I == playerInfo)
                    charIcon.spriteName = "Player_ReaperPic";
                else
                {
                    if (playerInfo.PlayerType == PlayerType.AI)
                        charIcon.spriteName = "ReaperPic_Gray";
                    else
                        charIcon.spriteName = "ReaperPic";
                }
                break;
            default:
                Debug.Log("Player panel Init Error!");
                break;
        }
    }
    
}
