using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volt_ModuleBtn : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float blinkDuration = 1f;

    [Header("Set in Script")]
    Volt_ModuleCardBase curModuleCard;
    public Volt_ModuleCardBase CurModuleCard { get; }
    UIButton button;

    UISprite btnBG;
    UISprite frameImage;
    UISprite iconImage;

    public bool isEquipped = false;
    public bool isActive = false;
    bool isBtnShowing = true;
    [SerializeField]
    float lastInteractionTime = 0f;
    Volt_Robot robot;
    public int slotNumber;

    //public bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        //UIEventListener.Get(this.gameObject).onPress = OnPressed;

        button = GetComponent<UIButton>();
        btnBG = GetComponent<UISprite>();
        iconImage = transform.Find("Icon").GetComponent<UISprite>();
        frameImage = transform.Find("Frame").GetComponent<UISprite>();
        Interactable(false);
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ActiveButtonRotate(isActive);
    }
    void ActiveButtonRotate(bool on)
    {
        if (on)
        {
            frameImage.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, Time.time*150f);
        }
    }
    public void OnClickModuleBtn()
    {
        if (curModuleCard.skillType == SkillType.Passive)
            return;
        if (Time.time - lastInteractionTime > 1f)
        {
            if (Volt_GameManager.S.pCurPhase == Phase.rangeSelect)
            {
                if (!isActive)
                {
                    if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                    {
                        isActive = true;
                        robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
                        robot.moduleCardExcutor.SetOnActiveCard(robot.moduleCardExcutor.GetCurEquipCards()[slotNumber], slotNumber);
                    }
                    else
                    {
                        PacketTransmission.SendModuleActivePacket(Volt_PlayerUI.S.owner.playerNumber, slotNumber); //몇번째 모듈카드인지도 보내고, Unpack부분 함수 만들어야함.
                        lastInteractionTime = Time.time;
                    }
                }
                else
                {
                    if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                    {
                        isActive = false;
                        robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>(); robot.moduleCardExcutor.SetOffActiveCard(slotNumber);
                    }
                    else
                    {
                        PacketTransmission.SendModuleUnActivePacket(Volt_PlayerUI.S.owner.playerNumber, slotNumber);//몇번째 모듈카드인지도 보내고, Unpack부분 함수 만들어야함.
                        lastInteractionTime = Time.time;
                    }
                }
            }
            else if (Volt_GameManager.S.pCurPhase == Phase.behavoiurSelect)
            {
                Volt_GameManager.S.SelectBehaviourDoneCallback2(curModuleCard.behaviourType);

                if (!isActive)
                {
                    if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                    {
                        isActive = true;
                        robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>();
                        robot.moduleCardExcutor.SetOnActiveCard(robot.moduleCardExcutor.GetCurEquipCards()[slotNumber], slotNumber);
                    }
                    else
                    {
                        PacketTransmission.SendModuleActivePacket(Volt_PlayerUI.S.owner.playerNumber, slotNumber); //몇번째 모듈카드인지도 보내고, Unpack부분 함수 만들어야함.
                        lastInteractionTime = Time.time;
                    }
                }
                else
                {
                    if (Volt_GameManager.S.IsTutorialMode || Volt_GameManager.S.IsTrainingMode)
                    {
                        isActive = false;
                        robot = Volt_PlayerManager.S.I.playerRobot.GetComponent<Volt_Robot>(); robot.moduleCardExcutor.SetOffActiveCard(slotNumber);
                    }
                    else
                    {
                        PacketTransmission.SendModuleUnActivePacket(Volt_PlayerUI.S.owner.playerNumber, slotNumber);//몇번째 모듈카드인지도 보내고, Unpack부분 함수 만들어야함.
                        lastInteractionTime = Time.time;
                    }
                }
            }
        }
        else
        {
            //print("너무 잦은 조작입니다!"); //팝업 표시 필요
        }
    }
    //public void OnPressed(GameObject sender, bool state)
    //{
        
    //    if (state)
    //    {
    //        isPressed = true;
    //        //print("Press down");
    //        StartCoroutine(OnPressTimer(iconImage.spriteName));
    //    }
    //    else
    //    {
    //        isPressed = false;
    //        //print("press up");
    //    }
    //}
   
    //IEnumerator OnPressTimer(string cardName)
    //{
    //    float timer = 0;
    //    while (isPressed)
    //    {
    //        if (Volt_GameManager.S.pCurPhase == Phase.behavoiurSelect ||
    //            Volt_GameManager.S.pCurPhase == Phase.rangeSelect)
    //        {

    //            timer += Time.fixedDeltaTime;
    //            if (timer > 0.5f)
    //            {
    //                Volt_ModuleTooltip.S.ShowTooltip(cardName, true);
    //            }
    //            //print("OnPressTimer");
    //            yield return null;
    //        }
    //        else
    //            break;
    //    }
    //    Volt_ModuleTooltip.S.ShowTooltip(cardName, false);
    //}

    public void EquipModule(Volt_ModuleCardBase newModuleCard)
    {
        curModuleCard = newModuleCard;
        isEquipped = true;
        //if (newModuleCard.skillType == SkillType.Active)
        Interactable(true);
        if (newModuleCard.skillType == SkillType.Passive)
            isActive = true;
        ModuleIconUpdate(curModuleCard.card, newModuleCard.moduleType, newModuleCard.skillType);

    }
    public void UnEquipModule()
    {
        curModuleCard = null;
        isEquipped = false;
        Interactable(false);
        ModuleIconInit();
        isActive = false;
    }
    public void Interactable(bool on)
    {
        if (on)
            button.isEnabled = true;
        else
            button.isEnabled = false;
    }
    public void ModuleIconInit()
    {
        iconImage.spriteName = "NONE";
        btnBG.spriteName = "Frame_Body";
        if (btnBG)
        {
            UIButton thisBtn = btnBG.gameObject.GetComponent<UIButton>();
            thisBtn.normalSprite = "Frame_Body";
            thisBtn.hoverSprite = "Frame_Body";
            thisBtn.pressedSprite = "Frame_Body";
            thisBtn.disabledSprite = "Frame_Body";
        }
        frameImage.spriteName = "NoneFrame";
    }
    public void ModuleIconUpdate(Card newModule, ModuleType moduleType, SkillType skillType)
    {
        
        switch (moduleType)
        {
            case ModuleType.Attack:
                if (skillType == SkillType.Active)
                    frameImage.spriteName = "RedFrame_Active";
                else
                    frameImage.spriteName = "RedFrame_Passive";
                break;
            case ModuleType.Movement:
                if (skillType == SkillType.Active)
                    frameImage.spriteName = "BlueFrame_Active";
                else
                    frameImage.spriteName = "BlueFrame_Passive";
                break;
            case ModuleType.Tactic:
                if (skillType == SkillType.Active)
                    frameImage.spriteName = "YellowFrame_Active";
                else
                    frameImage.spriteName = "YellowFrame_Passive";
                break;
            default:
                break;
        }
        if(skillType == SkillType.Passive)
        {
            btnBG.spriteName = "Frame_Body_Passive";
            UIButton thisBtn = btnBG.gameObject.GetComponent<UIButton>();
            thisBtn.normalSprite = "Frame_Body_Passive";
            thisBtn.hoverSprite = "Frame_Body_Passive";
            thisBtn.pressedSprite = "Frame_Body_Passive";
            thisBtn.disabledSprite = "Frame_Body_Passive";
        }
        iconImage.spriteName = newModule.ToString();
    }
    public void Init()
    {
        ModuleIconInit();
    }

    public void ShowModuleButton(bool needShow)
    {
        StartCoroutine(WaitBtnAnimation(needShow));
    }
    IEnumerator WaitBtnAnimation(bool needShow)
    {
        UISprite bg = GetComponent<UISprite>();
        float v = 0f;
        if (needShow)
        {
            isBtnShowing = true;
            while (v < 268f)
            {
                v += Time.fixedDeltaTime * 1000f;
                bg.topAnchor.absolute = Mathf.Clamp(bg.topAnchor.absolute + Time.fixedDeltaTime * 1000f, 0f, 200f);
                bg.bottomAnchor.absolute = Mathf.Clamp(bg.bottomAnchor.absolute + Time.fixedDeltaTime * 1000f, -192f, 46f);
                yield return null;
            }
        }
        else
        {
            isBtnShowing = false;
            while (v < 268f)
            {
                v += Time.fixedDeltaTime * 1000f;
                bg.topAnchor.absolute = Mathf.Clamp(bg.topAnchor.absolute - Time.fixedDeltaTime * 1000f, 0f, 200f);
                bg.bottomAnchor.absolute = Mathf.Clamp(bg.bottomAnchor.absolute - Time.fixedDeltaTime * 1000f, -192f, 46f);
                yield return null;
            }
        }
    }
}
