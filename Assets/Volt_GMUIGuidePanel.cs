using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GuideMSGType
{
    RobotSetup, WaitPlaceOtherPlayerRobot, BehaviourSelect, RangeSelect, Synchronization, SynchronizationWait, Victory,
    Defeat, RoundEnd, SuddenDeath
}

public class Volt_GMUIGuidePanel : MonoBehaviour
{
    public UISprite guideTexture;
    public Volt_SpriteAnimationMSG spriteAnimationPrefab;
    public UILabel guideText;
    public UIPanel guidePanel;

    // Start is called before the first frame update
    
    public void ShowSpriteAnimationMSG(GuideMSGType msgType, bool isNeedFadeOut)
    {
        Volt_SpriteAnimationMSG spriteAnimationInstance = Instantiate(spriteAnimationPrefab,transform);
        spriteAnimationInstance.SetMSG(msgType, isNeedFadeOut);
    }
    public void ShowGuideTextMSG(GuideMSGType mSGType, bool isNeedFadeOut, SystemLanguage language)
    {
        
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
        {
            guideText.gameObject.SetActive(true);
            guideTexture.gameObject.SetActive(true);
            switch (mSGType)
            {
                case GuideMSGType.RobotSetup:
                    switch (language)
                    {
                        case SystemLanguage.French:
                            guideText.text = "?";
                            break;
                        case SystemLanguage.German:
                            guideText.text = "[FF0000]Stelle[-] den Robot";
                            break;
                        case SystemLanguage.Korean:
                            guideText.text = "로봇을 [FF0000]배치[-]하세요!";
                            break;
                        default:
                            guideText.text = "Choose your Tile to [FF0000]place[-] the robot";
                            break;
                    }
                    break;
                case GuideMSGType.BehaviourSelect:
                    switch (language)
                    {
                        case SystemLanguage.French:
                            guideText.text = "?";
                            break;
                        case SystemLanguage.German:
                            guideText.text = "Wähle deine Aktionsart.";
                            break;
                        case SystemLanguage.Korean:
                            guideText.text = "[FF0000]행동[-]을 선택하세요!";
                            break;
                        default:
                            guideText.text = "Choose your action. ";
                            break;
                    }
                    break;
                case GuideMSGType.RangeSelect:
                    switch (language)
                    {
                        case SystemLanguage.French:
                            guideText.text = "?";
                            break;
                        case SystemLanguage.German:
                            guideText.text = "Wähle Ausrichtung und Entfernung.";
                            break;
                        case SystemLanguage.Korean:
                            guideText.text = "방향과 거리를 선택하세요!";
                            break;
                        default:
                            guideText.text = "Choose your direction and distance. ";
                            break;
                    }
                    break;
                case GuideMSGType.Synchronization:
                    switch (language)
                    {
                        case SystemLanguage.French:
                            guideText.text = "?";
                            break;
                        case SystemLanguage.German:
                            guideText.text = "Ich bin dabei, mich zu synchronisieren.";
                            break;
                        case SystemLanguage.Korean:
                            guideText.text = "동기화 중입니다...";
                            break;
                        default:
                            guideText.text = "Sync in progress.";
                            break;
                    }
                    break;
                case GuideMSGType.SynchronizationWait:
                    switch (language)
                    {
                        case SystemLanguage.French:
                            guideText.text = "?";
                            break;
                        case SystemLanguage.German:
                            guideText.text = "Ich warte auf die Synchronisierungsdaten.";
                            break;
                        case SystemLanguage.Korean:
                            guideText.text = "동기화를 기다리는 중입니다...";
                            break;
                        default:
                            guideText.text = "Waiting for synchronization phase.";
                            break;
                    }
                    break;
                default:
                    break;
            }
            
            if (isNeedFadeOut)
                StartCoroutine(FadeOutDestroy());
        }
    }
    public void HideGuideText()
    {
        guideText.gameObject.SetActive(false);
        guideTexture.gameObject.SetActive(false);
    }
    IEnumerator FadeOutDestroy()
    {
        yield return new WaitForSeconds(2f);
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            guideText.alpha -= Time.deltaTime;
            Color color = guideTexture.color;
            color.a -= Time.deltaTime;
            guideTexture.color = color;
            yield return null;
        }
        guideText.gameObject.SetActive(false);
        guideTexture.gameObject.SetActive(false);
        guideText.alpha = 1f;
        guideTexture.color = Color.white;
    }
    
}
