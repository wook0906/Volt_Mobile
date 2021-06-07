using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volt_PlayerUI : MonoBehaviour
{
    public static Volt_PlayerUI S;

    public Volt_PlayerInfo owner;
    public GameObject behaviourPanel;
    public Volt_ModuleBtn[] moduleBtns;
    public GameObject emoticonPanel;
    public List<UISprite> emoticonBtns;

    //public UILabel moduleInfoGuideText;

    public UIPanel newModuleGuidepanel;
    public UISprite newModuleGuideSkillType;
    public UISprite newModuleGuideTypeEdge;
    public UISprite newModuleGuideIcon;
    public UILabel newModuleGuideName;

    bool isEmoticonPanelOn = false;
    public bool IsEmoticonPanelOn
    {
        get { return isEmoticonPanelOn; }
        set
        {
            isEmoticonPanelOn = value;
            if (isEmoticonPanelOn)
                emoticonPanel.GetComponent<UIPanel>().alpha = 1f;
            else
                emoticonPanel.GetComponent<UIPanel>().alpha = 0f;
        }
    }

    private void Awake()
    {
        S = this;
    }
    void Start()
    {
        transform.SetParent(GameObject.Find("PlayerUI Root").transform);
        transform.localPosition = new Vector3(0f, 100f, 0f);
        transform.localScale = Vector3.one;
        BehaviourSelectOff();
    }

    public void Init()
    {
        owner = Volt_PlayerManager.S.I;
        //PlayerData에서 저장해놓은 이모티콘데이터(로비에서 어떤 이모티콘을 세팅하여 게임에 참가하였는지)
        //위 데이터를 이용하여 emoticonBtns를 만든다.
        EmoticonSetup();
    }
    void EmoticonSetup()
    {
        Dictionary<int, string> emoticonSet = new Dictionary<int, string>(Volt_PlayerData.instance.GetEmoticonSet());
        for (int i = 0; i < 6; i++)
        {
            emoticonBtns[i].spriteName = emoticonSet[i];
            emoticonBtns[i].gameObject.name = emoticonSet[i];
        }
    }

    
    public void OnClickShowEmoticonPanelBtn()
    {
        if (IsEmoticonPanelOn)
            IsEmoticonPanelOn = false;
        else
            IsEmoticonPanelOn = true;
    }

    public void OnClickEmoticonButton(GameObject go)
    {
        if (!owner.GetRobot() ||
            owner.GetRobot().panel.IsEmoticonShowing() ||
            go.name.Equals("EmoticonNone"))
        {
            IsEmoticonPanelOn = false;
            return;
        }

        for (Define.EmoticonType i = Define.EmoticonType.None; i <= Define.EmoticonType.Common_Surrender; i++)
        {
            if (go.name.Equals(i.ToString()))
            {
                if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                {
                    owner.GetRobot().panel.EmoticonPlay(i);
                    break;
                }
                else
                {
                    PacketTransmission.SendUseEmoticon(owner.playerNumber, (int)i);
                    break;
                }
            }
        }
        IsEmoticonPanelOn = false;
    }

    public void OnClickAttackButton(GameObject go)
    {
        Volt_GameManager.S.SelectBehaviourDoneCallback(go);
    }

    public void OnClickMoveButton(GameObject go)
    {
        Volt_GameManager.S.SelectBehaviourDoneCallback(go);
    }

    public void BehaviourSelectOn()
    {
        behaviourPanel.SetActive(true);
    }
    public void BehaviourSelectOff()
    {
        behaviourPanel.SetActive(false);

    }
    public void ShowModuleButton(bool needShow)
    {

        if (needShow && Volt_GMUI.S.isGetNewModule)
        {

            Volt_GMUI.S.NoticeNewModule();
        }
        foreach (var item in moduleBtns)
        {
            item.ShowModuleButton(needShow);
        }
    }
    public void GetNewModule(Card newCard)
    {
        if (Volt_GameManager.S.pCurPhase == Phase.synchronization) return;
        Volt_GMUI.S.isGetNewModule = true;
        StartCoroutine(ModuleGuideIconFadeOut(newCard));
    }

    IEnumerator ModuleGuideIconFadeOut(Card newCard)
    {
        float timer = 0f;
        ModuleDescriptionInfo moduleInfo = Volt_ModuleDescriptionInfos.GetModuleDescriptionInfo(newCard.ToString(), Application.systemLanguage);
        switch (moduleInfo.moduleType)
        {
            case ModuleType.Attack:
                if (moduleInfo.skillType == SkillType.Passive)
                {
                    newModuleGuideTypeEdge.spriteName += "RedFrame_Passive";
                    newModuleGuideSkillType.spriteName = "Frame_Body_Passive";
                }
                else
                {
                    newModuleGuideTypeEdge.spriteName = "RedFrame_Active";
                    newModuleGuideSkillType.spriteName = "Frame_Body";
                }
                break;
            case ModuleType.Movement:
                if (moduleInfo.skillType == SkillType.Passive)
                {
                    newModuleGuideTypeEdge.spriteName += "BlueFrame_Passive";
                    newModuleGuideSkillType.spriteName = "Frame_Body_Passive";
                }
                else
                {
                    newModuleGuideTypeEdge.spriteName = "BlueFrame_Active";
                    newModuleGuideSkillType.spriteName = "Frame_Body";
                }
                break;
            case ModuleType.Tactic:
                if (moduleInfo.skillType == SkillType.Passive)
                {
                    newModuleGuideTypeEdge.spriteName += "YellowFrame_Passive";
                    newModuleGuideSkillType.spriteName = "Frame_Body_Passive";
                }
                else
                {
                    newModuleGuideTypeEdge.spriteName = "YellowFrame_Active";
                    newModuleGuideSkillType.spriteName = "Frame_Body";
                }
                break;
            default:
                Debug.LogError("ModuleGuideIconFadeOut Error2");
                break;
        }
        newModuleGuideIcon.spriteName = moduleInfo.card.ToString();
        newModuleGuideName.text = moduleInfo.title;
        newModuleGuidepanel.alpha = 1f;
        while (timer < 3.5f)
        {
            if (timer >= 3f)
                newModuleGuidepanel.alpha -= Time.fixedDeltaTime * 2f;
            timer += Time.fixedDeltaTime;
            yield return null;
        }
        newModuleGuidepanel.alpha = 0f;
    }

    
    public void NewModuleEquip(Volt_ModuleCardBase newModuleCard)
    {
        //print("NewModuleEquip!");
        foreach (var btn in moduleBtns)
        {
            if (!btn.isEquipped)
            {
                btn.EquipModule(newModuleCard);
                return;
            }
        }
        //print("Active Card!");
    }
   
    public void UnEquipModuleCard(int slotNumber)
    {
        for (int i = 0; i < moduleBtns.Length; i++)
        {
            if (i == slotNumber)
            {
                moduleBtns[i].UnEquipModule();
                return;
            }
        }
    }
    public void RobotDeadModuleBtnInit()
    {
        foreach (var item in moduleBtns)
        {
            item.Init();
        }
    }

}
