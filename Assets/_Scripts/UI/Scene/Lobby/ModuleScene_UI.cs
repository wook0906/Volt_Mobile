using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleScene_UI : UI_Scene
{
    enum Textures
    {
        BG
    }

    enum Buttons
    {
        AttackTap_Btn,
        MoveTap_Btn,
        TacticTap_Btn,
        CROSSFIRE,
        TIMEBOMB,
        POWERBEAM,
        GRENADES,
        SHOCKWAVE,
        DOUBLEATTACK,
        PERNERATE,
        SAWBLADE,
        DODGE,
        REPULSIONBLAST,
        TELEPORT,
        STEERINGNOZZLE,
        AMARGEDDON,
        EMP,
        DUMMYGEAR,
        SHIELD,
        ANCHOR,
        HACKING,
        BOMB,
        Back_Btn
    }

    enum Labels
    {
        AttackTap_Label,
        MoveTap_Label,
        TacticTap_Label,
        CrossFire_Label,
        TimeBomb_Label,
        Powerbeam_Label,
        Grenades_Label,
        Shockwave_Label,
        DoubleAttack_Label,
        Pernerate_Label,
        Sawblade_Label,
        Dodge_Label,
        RepulsionBlast_Label,
        Teleport_Label,
        SteeringNozzle_Label,
        Amargeddon_Label,
        EMP_Label,
        DummyGear_Label,
        Shield_Label,
        Anchor_Label,
        Hacking_Label,
        Bomb_Label
    }

    enum Sprites
    {
        CrossFire_BG, CrossFire_Edge, CrossFire_Icon,
        TimeBomb_BG, TimeBomb_Edge, TimeBomb_Icon,
        Powerbeam_BG, Powerbeam_Edge, Powerbeam_Icon,
        Grenades_BG, Grenades_Edge, Grenades_Icon,
        Shockwave_BG, Shockwave_Edge, Shockwave_Icon,
        DoubleAttack_BG, DoubleAttack_Edge, DoubleAttack_Icon,
        Pernerate_BG, Pernerate_Edge, Pernerate_Icon,
        Sawblade_BG, Sawblade_Edge, Sawblade_Icon,
        Dodge_BG, Dodge_Edge, Dodge_Icon,
        RepulsionBlast_BG, RepulsionBlast_Edge, RepulsionBlast_Icon,
        Teleport_BG, Teleport_Edge, Teleport_Icon,
        SteeringNozzle_BG, SteeringNozzle_Edge, SteeringNozzle_Icon,
        Amargeddon_BG, Amargeddon_Edge, Amargeddon_Icon,
        EMP_BG, EMP_Edge, EMP_Icon,
        DummyGear_BG, DummyGear_Edge, DummyGear_Icon,
        Shield_BG, Shield_Edge, Shield_Icon,
        Anchor_BG, Anchor_Edge, Anchor_Icon,
        Hacking_BG, Hacking_Edge, Hacking_Icon,
        Bomb_BG, Bomb_Edge, Bomb_Icon,
        Board_Sprite,

    }

    enum GameObjects
    {
        AttakcModuleCards,
        MoveModuleCards,
        TacticModuleCards
    }

    public ModuleExplaination_Popup ExplainationPopup { set; get; }

    private LobbyScene lobbyScene;
    public override void Init()
    {
        base.Init();

        Bind<UITexture>(typeof(Textures));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));
        Bind<GameObject>(typeof(GameObjects));

        lobbyScene = Managers.Scene.CurrentScene as LobbyScene;

        GetButton((int)Buttons.AttackTap_Btn).onClick.Add(new EventDelegate(OnPressdownAttackTapButton));
        GetButton((int)Buttons.MoveTap_Btn).onClick.Add(new EventDelegate(OnPressdownMovementTapButton));
        GetButton((int)Buttons.TacticTap_Btn).onClick.Add(new EventDelegate(OnPressdownTacticTapButton));
        GetButton((int)Buttons.Back_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.CloseSceneUI();
            Managers.UI.CloseAllPopupUI();
            lobbyScene.ChangeToLobbyCamera();
        }));

        string[] buttonNames = typeof(Buttons).GetEnumNames();
        for (int i = (int)Buttons.CROSSFIRE; i < (int)Buttons.Back_Btn; ++i)
        {
            EventDelegate onClick = new EventDelegate(this, "OnClickModuleCard");
            onClick.parameters[0] = Util.MakeParameter(GetButton(i), typeof(UIButton));
            GetButton(i).onClick.Add(onClick);
        }

        LobbyScene scene = Managers.Scene.CurrentScene as LobbyScene;
        scene.OnLoadedModuleSceneUI();
    }

    public override void OnActive()
    {
        Managers.UI.ShowPopupUIAsync<ModuleExplaination_Popup>();
        
        OnPressdownAttackTapButton();
    }

    private void OnClickModuleCard(UIButton button)
    {
        ExplainationPopup.gameObject.SetActive(true);
        ExplainationPopup.ShowPopup(button.name);
    }

    public void OnPressdownAttackTapButton()
    {
        GetGameObject((int)GameObjects.MoveModuleCards).SetActive(false);
        GetButton((int)Buttons.MoveTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.MoveTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.TacticModuleCards).SetActive(false);
        GetButton((int)Buttons.TacticTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.TacticTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.AttakcModuleCards).SetActive(true);
        GetButton((int)Buttons.AttackTap_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.AttackTap_Btn).normalSprite = "Btn_button01_p";
    }

    public void OnPressdownMovementTapButton()
    {
        GetGameObject((int)GameObjects.AttakcModuleCards).SetActive(false);
        GetButton((int)Buttons.AttackTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.AttackTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.TacticModuleCards).SetActive(false);
        GetButton((int)Buttons.TacticTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.TacticTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.MoveModuleCards).SetActive(true);
        GetButton((int)Buttons.MoveTap_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.MoveTap_Btn).normalSprite = "Btn_button01_p";
    }

    public void OnPressdownTacticTapButton()
    {
        GetGameObject((int)GameObjects.MoveModuleCards).SetActive(false);
        GetButton((int)Buttons.MoveTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.MoveTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.AttakcModuleCards).SetActive(false);
        GetButton((int)Buttons.AttackTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.AttackTap_Btn).normalSprite = "Btn_button01_n";

        GetGameObject((int)GameObjects.TacticModuleCards).SetActive(true);
        GetButton((int)Buttons.TacticTap_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.TacticTap_Btn).normalSprite = "Btn_button01_p";
    }
}
