using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MSG2DEventType
{
    Kill, KillSelf, FallSelf, DeadToPlayer, DeadToTrap,
    UseDodge, UseSteeringNozzle, UseRepulsionBlast, UseTeleport, UseCrossFire,
    UseGrenade, UseTimeBomb, UsePowerBeam, UseSawBlade, UseShockWave,
    UseDoubleAttack, UsePernerate, UseAnchor, UseBomb, UseShield,
    UseHacking, UseEMP, UseDummy, UseAmargeddon
}
public class Volt_2dUIMsg : MonoBehaviour
{
    public bool isMoving = false;
    public float moveRange = 50f;
    public float showTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show()
    {
        GetComponent<UILabel>().color = Color.white;
        StartDisappear();
    }
    void StartDisappear()
    {
        StartCoroutine(DisappearMSG());
    }
    IEnumerator DisappearMSG()
    {
        yield return new WaitForSecondsRealtime(showTime);
        float t = 0f;
        UILabel label = GetComponent<UILabel>();
        Color color = label.color;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime;
            label.color = Color.Lerp(color, Color.clear, t);
            yield return null;
        }
        //Queue에서 제거해야함.
        Volt_GMUI.S.msg2dHandler.RemoveMsg();
    }
    public void MoveUpStart()
    {
        isMoving = true;
        StartCoroutine(MoveUp());
    }
    IEnumerator MoveUp()
    {
        Vector3 originPos = transform.localPosition;
        float targetY = originPos.y + moveRange;
        Vector3 tmpPos = originPos;
        while (transform.localPosition.y < targetY)
        {
            tmpPos.y += Time.fixedDeltaTime * 50f;
            transform.localPosition = tmpPos;
            yield return null;
        }
        tmpPos.y = targetY;
        transform.localPosition = tmpPos;
        isMoving = false;
    }

    public void SetMsg(MSG2DEventType msgType, int playerNumber = 0)
    {
        Volt_PlayerInfo player = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber);
        if (!player) return;

        //SystemLanguage tmpLanguage = SystemLanguage.German;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                break;
            case SystemLanguage.German:
                switch (msgType)
                {
                    case MSG2DEventType.Kill:
                        GetComponent<UILabel>().text = "Du hast [FF0000]" + player.NickName + "[-] zerstört.";
                        break;
                    case MSG2DEventType.KillSelf:
                        GetComponent<UILabel>().text = "Du hast [FF0000]selbst[-] zerstört.";
                        break;
                    case MSG2DEventType.DeadToPlayer:

                        GetComponent<UILabel>().text = "Du wurdest von [FF0000]" + player.NickName + "[-] zerstört.";
                        break;
                    case MSG2DEventType.DeadToTrap:
                        GetComponent<UILabel>().text = "Du wurdest von [FF0000]Trap[-] zerstört.";
                        break;
                    case MSG2DEventType.FallSelf:
                        GetComponent<UILabel>().text = "Du bist [FF0000]gefallen";
                        break;
                    case MSG2DEventType.UseDodge:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[00FFFF]Ausweichmanöver[-]\'";
                        break;
                    case MSG2DEventType.UseSteeringNozzle:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[00FFFF]Diagnoale Mobilität[-]\'";
                        break;
                    case MSG2DEventType.UseRepulsionBlast:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[00FFFF]Rückprall-Stoßwelle[-]\'";
                        break;
                    case MSG2DEventType.UseTeleport:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[00FFFF]Positionstauscher[-]\'";
                        break;
                    case MSG2DEventType.UseCrossFire:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Salvenfeuer[-]\'";
                        break;
                    case MSG2DEventType.UseGrenade:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Handgranate[-]\'";
                        break;
                    case MSG2DEventType.UseTimeBomb:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Zeitbombe[-]\'";
                        break;
                    case MSG2DEventType.UsePowerBeam:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Kraftstrahl[-]\'";
                        break;
                    case MSG2DEventType.UseSawBlade:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Sägeblatt[-]\'";
                        break;
                    case MSG2DEventType.UseShockWave:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Elektronenschockwelle[-]\'";
                        break;
                    case MSG2DEventType.UseDoubleAttack:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Schnellfeuer[-]\'";
                        break;
                    case MSG2DEventType.UsePernerate:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FF0000]Fokussierter Laser[-]\'";
                        break;
                    case MSG2DEventType.UseAnchor:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Block[-]\'";
                        break;
                    case MSG2DEventType.UseBomb:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Selbstzerstörung[-]\'";
                        break;
                    case MSG2DEventType.UseShield:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Schild[-]\'";
                        break;
                    case MSG2DEventType.UseHacking:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Hacking-Vorrichtung[-]\'";
                        break;
                    case MSG2DEventType.UseEMP:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]EMP-Vorrichtung[-]\'";
                        break;
                    case MSG2DEventType.UseDummy:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Attrappe[-]\'";
                        break;
                    case MSG2DEventType.UseAmargeddon:
                        GetComponent<UILabel>().text = player.NickName + " benutzte \'[FFFF00]Bewaffnete Drohne[-]\'";
                        break;
                    default:
                        break;
                }
                break;
            case SystemLanguage.Korean:
                switch (msgType)
                {
                    case MSG2DEventType.Kill:
                        GetComponent<UILabel>().text = "[FF0000]" + player.NickName + "[-] " + "을 처치했습니다.";
                        break;
                    case MSG2DEventType.KillSelf:
                        GetComponent<UILabel>().text = "[FF0000]자폭[-] 했습니다.";
                        break;
                    case MSG2DEventType.DeadToPlayer:
                        GetComponent<UILabel>().text = "[FF0000]" + player.NickName + "[-] " + "에게 처치 당했습니다.";
                        break;
                    case MSG2DEventType.DeadToTrap:
                        GetComponent<UILabel>().text = "[FF0000]함정[-]에 처치당했습니다.";
                        break;
                    case MSG2DEventType.FallSelf:
                        GetComponent<UILabel>().text = "[FF0000]낙사[-] 했습니다.";
                        break;
                    case MSG2DEventType.UseDodge:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[00FFFF]회피기동[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseSteeringNozzle:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[00FFFF]사선기동[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseRepulsionBlast:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[00FFFF]반동충격파[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseTeleport:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[00FFFF]위치교환기[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseGrenade:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]수류탄[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseCrossFire:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]일제사격[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseTimeBomb:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]시한폭탄[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UsePowerBeam:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]파워빔[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseSawBlade:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]톱날검[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseShockWave:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]전자충격파[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseDoubleAttack:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]속사[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UsePernerate:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FF0000]관통탄[-]\'을 사용했습니다.";
                        break;
                    case MSG2DEventType.UseAnchor:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]고정장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseBomb:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]자폭장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseShield:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]쉴드장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseHacking:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]해킹장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseEMP:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]EMP장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseDummy:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]더미장치[-]\'를 사용했습니다.";
                        break;
                    case MSG2DEventType.UseAmargeddon:
                        GetComponent<UILabel>().text = player.NickName + "가 \'[FFFF00]폭격장치[-]\'를 사용했습니다.";
                        break;
                    default:
                        break;
                }
                break;
            default:
                switch (msgType)
                {
                    case MSG2DEventType.Kill:
                        GetComponent<UILabel>().text = "You have slain [FF0000]" + player.NickName + "[-]";
                        break;
                    case MSG2DEventType.KillSelf:
                        GetComponent<UILabel>().text = "You slain [FF0000]yourself[-]";
                        break;
                    case MSG2DEventType.DeadToPlayer:

                        GetComponent<UILabel>().text = "You were slain by [FF0000]" + player.NickName + "[-]";
                        break;
                    case MSG2DEventType.DeadToTrap:
                        GetComponent<UILabel>().text = "You were slain by [FF0000]trap[-]";
                        break;
                    case MSG2DEventType.FallSelf:
                        GetComponent<UILabel>().text = "You [FF0000]fell[-] and died";
                        break;
                    case MSG2DEventType.UseDodge:
                        GetComponent<UILabel>().text = player.NickName + " used \'[00FFFF]Evasion mobility[-]\'";
                        break;
                    case MSG2DEventType.UseSteeringNozzle:
                        GetComponent<UILabel>().text = player.NickName + " used \'[00FFFF]diagonal mobility[-]\'";
                        break;
                    case MSG2DEventType.UseRepulsionBlast:
                        GetComponent<UILabel>().text = player.NickName + " used \'[00FFFF]rebound shock wave[-]\'";
                        break;
                    case MSG2DEventType.UseTeleport:
                        GetComponent<UILabel>().text = player.NickName + " used \'[00FFFF]Location changer[-]\'";
                        break;
                    case MSG2DEventType.UseCrossFire:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]Open fire[-]\'";
                        break;
                    case MSG2DEventType.UseGrenade:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]grenade[-]\'";
                        break;
                    case MSG2DEventType.UseTimeBomb:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]Time bomb[-]\'";
                        break;
                    case MSG2DEventType.UsePowerBeam:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]Power beam[-]\'";
                        break;
                    case MSG2DEventType.UseSawBlade:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]Saw blade[-]\'";
                        break;
                    case MSG2DEventType.UseShockWave:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]electron shock wave[-]\'";
                        break;
                    case MSG2DEventType.UseDoubleAttack:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]Rapid fire[-]\'";
                        break;
                    case MSG2DEventType.UsePernerate:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FF0000]penetrator[-]\'";
                        break;
                    case MSG2DEventType.UseAnchor:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]contraption device[-]\'";
                        break;
                    case MSG2DEventType.UseBomb:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]self-destruction device[-]\'";
                        break;
                    case MSG2DEventType.UseShield:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]Shield device[-]\'";
                        break;
                    case MSG2DEventType.UseHacking:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]Hacking device[-]\'";
                        break;
                    case MSG2DEventType.UseEMP:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]EMP device[-]\'";
                        break;
                    case MSG2DEventType.UseDummy:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]Dummy device[-]\'";
                        break;
                    case MSG2DEventType.UseAmargeddon:
                        GetComponent<UILabel>().text = player.NickName + " used \'[FFFF00]Bombing device[-]\'";
                        break;
                    default:
                        break;
                }
                break;
        }
        
        Volt_GMUI.S.msg2dHandler.EntryMsg(this);
    }
}
