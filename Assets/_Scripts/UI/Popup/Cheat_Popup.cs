using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_Popup : UI_Popup 
{
    enum Buttons
    {	
        
        BackBtn,
        Player1,
        Player2,
        Player3,
        Player4,
        Amargeddon,
        Anchor,
        Bomb,
        Crossfire,
        Dodge,
        DoubleAttack,
        DummyGear,
        EMP,
        Grenade,
        Pernerate,
        PowerBeam,
        RepulsionBlast,
        SawBlade,
        Shield,
        ShockWave,
        TimeBomb,
        SteeringNozzle,
        Hacking,
        Teleport,
        AddHP,
        SubtractHP,
        AddVP,
        SubVP,
    }

    enum Labels
    {
        
        Player1_Label,
        Player2_Label,
        Player3_Label,
        Player4Label,
        AddHPLabel,
        SubtractHPLabel,
        AddVPLabel,
        SubVPLabel,
        EndlessToggleLabel,
        SuddenDeathModeLabel,
    }

    enum ScrollViews
    {
        
    }

    enum Sliders
    {
        
    }

    enum Progressbars
    {
        
    }

    enum Toggles
    {
        
        EndlessGameToggle,
        SuddenDeathModeToggle,
    }

    enum Sprites
    {
        
        BG,
        BackBtnIcon,
        AmargeddonIcon,
        AnchorIcon,
        BombIcon ,
        CrossfireIcon ,
        DodgeIcon ,
        DoubleAttackIcon ,
        DummyIcon ,
        EMPIcon,
        GrenadeIcon,
        PernerateIcon,
        PowerBeamIcon,
        RepulsionBlastIcon,
        SawBladeIcon ,
        ShieldIcon,
        ShockPulseIcon,
        TimeBombIcon,
        SteeringNozzleIcon ,
        HackingIcon,
        TeleportIcon ,
        EndlessToggleOn,
        EndlessToggleOff,
        SuddenDeathModeOn,
        SuddenDeathModeOff,
    }

    enum GameObjects
    {
        
    }

    public override void Init()
    {
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UIScrollView>(typeof(ScrollViews));
        Bind<UISlider>(typeof(Sliders));
        Bind<UIProgressBar>(typeof(Progressbars));
        Bind<UISprite>(typeof(Sprites));
        Bind<GameObject>(typeof(GameObjects));
     }
}
