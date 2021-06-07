using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ModuleDescriptionInfo
{
    public string title;
    public string description;
    public string semiDescription;
    public Card card;
    public SkillType skillType;
    public ModuleType moduleType;
}

public static class Volt_ModuleDescriptionInfos
{
    public static ModuleDescriptionInfo GetModuleDescriptionInfo(string name, SystemLanguage language)
    {
        ModuleDescriptionInfo info = new ModuleDescriptionInfo();
        info.title = "title";
        info.description = "description";
        info.card = Card.NONE;
        info.skillType = SkillType.None;
        info.moduleType = ModuleType.Attack;
        switch (name)
        {
            case "CROSSFIRE":
                info.card = Card.CROSSFIRE;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "DOUBLEATTACK":

                info.card = Card.DOUBLEATTACK;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "GRENADES":
                info.card = Card.GRENADES;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "PERNERATE":
                info.card = Card.PERNERATE;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "POWERBEAM":
                info.card = Card.POWERBEAM;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "SAWBLADE":
                info.card = Card.SAWBLADE;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Attack;
                break;
            case "SHOCKWAVE":
                info.card = Card.SHOCKWAVE;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Attack;
                break;
            case "TIMEBOMB":
                info.card = Card.TIMEBOMB;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Attack;
                break;
            case "DODGE":
                info.card = Card.DODGE;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Movement;
                break;
            case "REPULSIONBLAST":
                info.card = Card.REPULSIONBLAST;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Movement;
                break;
            case "STEERINGNOZZLE":
                info.card = Card.STEERINGNOZZLE;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Movement;
                break;
            case "TELEPORT":
                info.card = Card.TELEPORT;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Movement;
                break;
            case "AMARGEDDON":
                info.card = Card.AMARGEDDON;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            case "ANCHOR":
                info.card = Card.ANCHOR;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            case "BOMB":
                info.card = Card.BOMB;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            case "DUMMYGEAR":
                info.card = Card.DUMMYGEAR;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            case "EMP":
                info.card = Card.EMP;
                info.skillType = SkillType.Active;
                info.moduleType = ModuleType.Tactic;
                break;
            case "HACKING":
                info.card = Card.HACKING;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            case "SHIELD":
                info.card = Card.SHIELD;
                info.skillType = SkillType.Passive;
                info.moduleType = ModuleType.Tactic;
                break;
            default:
                Debug.Log("GetModuleDescriptionInfo Error");
                break;
        }
        switch (language)
        {
            case SystemLanguage.French:
                //TODO
                break;
            case SystemLanguage.German:
                switch (name)
                {
                    case "CROSSFIRE":
                        info.title = "Ausweich \n manöver";
                        info.description = "Beim [Angreifen] greifst du in vier Richtungen gleichzeitig an.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Beim [Angreifen]\n greifst du in vier\n Richtungen gleichzeitig an.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "DOUBLEATTACK":
                        info.title = "Schnellfeuer";
                        info.description = "Beim [Angreifen] greifst du einen gegnerischen Robot zweimal an.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Beim [Angreifen]\n greifst du einen\n gegnerischen Robot\n zweimal an.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "GRENADES":
                        info.title = "Handgranate";
                        info.description = "Beim [Angreifen] wirfst du eine Granate\n und fügst dem getroffenen Robot 2 Schaden zu\n und jedem angrenzenden Feld 1 Schaden.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Beim [Angreifen]\n wirfst du eine Granate\n und fügst dem getroffenen Robot\n 2 Schaden zu \n und jedem\n angrenzenden Feld\n 1 Schaden.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "PERNERATE":
                        info.title = "Fokussierter Laser";
                        info.description = "Beim [Angreifen] fügst du jedem gegnerischen Robot in Angriffslinie Schaden zu.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Beim [Angreifen]\n fügst du jedem \n gegnerischen Robot \n in Angriffslinie\n Schaden zu. \n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "POWERBEAM":
                        info.title = "Power beam";
                        info.description = "Schiebt den feindlichen Roboter, wenn [Angreift].\n Schiebt 1 feld weg und schaden 1\n wenn 1~3[Angreift].\n Schiebt 2 felder weg und schaden 2\n wenn 4~6[Angriffe].\n Das Modul läuft nach Gebrauch ab.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Schiebt 1 feld weg \nund schaden 1 \n wenn 1~3[Angreift].\n Schiebt 2 felder weg und schaden 2 \n wenn 4~6[Angriffe].\n Das Modul läuft nach Gebrauch ab.";
                        break;
                    case "SAWBLADE":
                        info.title = "Sägeblatt";
                        info.description = "Schaden beim [Bewegen] und stößt den feindlichen Roboter weg.\n Schaden 1 bei 1 ~3[Bewegung]\n Schaden 2 bei 4 ~6[Bewegung]\n Das Modul läuft nach Beschädigung ab.";
                        info.semiDescription = "Schaden beim [Bewegen] und stößt den feindlichen Roboter weg.\n Das Modul läuft nach Beschädigung ab.";
                        break;
                    case "SHOCKWAVE":
                        info.title = "Elektronen\nschockwelle";
                        info.description = "Fügt nach dem [Bewegen] den angrenzenden Feldern 1 Schaden zu.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Fügt nach dem [Bewegen]\n den angrenzenden\n Feldern 1 Schaden zu.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "TIMEBOMB":
                        info.title = "Zeitbombe";
                        info.description = "Installiere bei [Angriffen] die Zeitbombe am ausgewählten Ort.\n Nach 3 Runden wird an diesem Ort 2 Schaden zugefügt und an jedem angrenzenden Feld 1 Schaden.\n Das Modul ist nach dem Einsatz verbraucht.\n Falls zwei Zeitbomben am Ort eines Roboters sind,\n explodiert die erste Zeitbombe sofort\n und fügt Schaden zu.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Installiere bei [Angriffen]\n die Zeitbombe am\n ausgewählten Ort.\n Nach 3 Runden wird an\n diesem Ort 2 Schaden zugefügt\n und an jedem angrenzenden Feld 1 Schaden.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "DODGE":
                        info.title = "Ausweich\nmanöver";
                        info.description = "Weiche 1 gegnerischen Angriff aus.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Weiche 1 gegnerischen\n Angriff aus.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "REPULSIONBLAST":
                        info.title = "Rückprall \n Stoßwelle";
                        info.description = "Stoße jeden gegnerischen Roboter, der nach der [Bewegung] angrenzend ist.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Stoße jeden\n gegnerischen Roboter,\n der nach der [Bewegung]\n angrenzend ist.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "STEERINGNOZZLE":
                        info.title = "Diagnoale Mobilität";
                        info.description = "Du kannst dich diagonal bewegen.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Du kannst dich\n diagonal bewegen.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "TELEPORT":
                        info.title = "Position \n stauscher";
                        info.description = "Bewege dich an den Ort eines Robots anstatt ihn [Anzugreifen].\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Bewege dich an\n den Ort eines Robots\n anstatt ihn [Anzugreifen].\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "AMARGEDDON":
                        info.title = "Bewaffnete Drohne";
                        info.description = "Ordne einen Bombenangriff an.\n Dies schickt 8-mal zufällig eine Bombe,\n die 1 Schaden verursacht,\n wenn sich ein Robot bewegt oder angreift.";
                        info.semiDescription = "Dies schickt 8-mal\n zufällig eine Bombe,\n die 1 Schaden verursacht,\n wenn sich ein Robot bewegt oder angreift.";
                        break;
                    case "ANCHOR":
                        info.title = "Block";
                        info.description = "Du wirst einmalig nicht durch eine gegnerische [Bewegung] gestoßen.";
                        info.semiDescription = "Du wirst einmalig nicht\n durch eine gegnerische\n [Bewegung] gestoßen.";
                        break;
                    case "BOMB":
                        info.title = "Selbst \n zerstörung";
                        info.description = "Wenn die Gesundheit des Robots 0 erreicht,\n explodiert er und fügt jedem angrenzenden Feld 1 Schaden zu.\n (Außer der Grube)";
                        info.semiDescription = "Wenn die Gesundheit des\n Robots 0 erreicht,\n explodiert er und fügt\n jedem angrenzenden Feld 1 Schaden zu.\n (Außer der Grube)";
                        break;
                    case "DUMMYGEAR":
                        info.title = "Attrappe";
                        info.description = "Dein Gegner erhält keine Punkte,\n wenn er den Robot mit diesem Modul tötet.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Dein Gegner erhält keine Punkte,\n wenn er den Robot mit diesem Modul tötet.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "EMP":
                        info.title = "EMP \n Vorrichtung";
                        info.description = "Zerstöre mit deinem [Angriff] jedes Modul,\n das ein gegnerischer Robot ausgerüstet hat.";
                        info.semiDescription = "[ffff00][Klicken zur Verwendung auf module icon][-]\n Zerstöre mit deinem\n [Angriff] jedes Modul,\n das ein gegnerischer Robot ausgerüstet hat.";
                        break;
                    case "HACKING":
                        info.title = "Hacking \n Vorrichtung";
                        info.description = "Ermöglicht sofort das Sammeln der Münze.\n Das Modul ist nach dem Einsatz verbraucht.";
                        info.semiDescription = "Ermöglicht sofort das\n Sammeln der Münze.\n Das Modul ist nach dem\n Einsatz verbraucht.";
                        break;
                    case "SHIELD":
                        info.title = "Schild";
                        info.description = "Erzeugt einen Schild,\n der 2 Schadenspunkte verteidigt.\n Nachdem das Schild zerstört wurde,\n ist das Modul verbraucht.";
                        info.semiDescription = "Erzeugt einen Schild,\n der 2 Schadenspunkte \n verteidigt.\n Nachdem das Schild \n zerstört wurde,\n ist das Modul verbraucht.";
                        break;
                    default:
                        break;
                }
                break;
            case SystemLanguage.Korean:
                switch (name)
                {
                    case "CROSSFIRE":
                        info.title = "일제사격";
                        info.description = "[공격]시 네 방향으로 공격합니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 네 방향으로\n 공격합니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "DOUBLEATTACK":
                        info.title = "속사";
                        info.description = "[공격]시 빠르게  2회 공격합니다. \n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 빠르게  2회 공격합니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "GRENADES":
                        info.title = "수류탄";
                        info.description = "[공격]시 수류탄을 던져 적 로봇에 피해 2를 입히고,\n 주변에 피해 1을 줍니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[FFFF00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 수류탄을 던져\n 적 로봇에 피해 2를 입히고,\n 주변에 피해 1을 줍니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "PERNERATE":
                        info.title = "관통탄";
                        info.description = "[공격]시 공격선 상의 모든 로봇에 피해를 줍니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 공격선 상의\n 모든 로봇에 피해를 줍니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "POWERBEAM":
                        info.title = "파워빔";
                        info.description = "[공격]시 적 로봇을 뒤로 밀어냅니다.\n 1~3[공격] 시 1칸 밀어내고 피해 1을 줍니다.\n 4~6[공격] 시 2칸 밀어내고 피해 2를 줍니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 적 로봇을\n 뒤로 밀어냅니다.\n 1~3[공격] 시 1칸 밀어내고\n 피해 1을 줍니다.\n 4~6[공격] 시 2칸 밀어내고\n 피해 2를 줍니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "SAWBLADE":
                        info.title = "톱날검";
                        info.description = "[이동]시 피해를 주고 적 로봇을 뒤로 밀어냅니다.\n 1~3[이동] 시 피해 1을 줍니다.\n 4~6[이동] 시 피해 2를 줍니다.\n 데미지를 주면 해당 모듈은 사라집니다.";
                        info.semiDescription = "[이동]시 피해를 주고\n 적 로봇을 뒤로 밀어냅니다.\n 데미지를 주면 해당 모듈은 \n 사라집니다.";
                        break;                 
                    case "SHOCKWAVE":
                        info.title = "전자충격파";
                        info.description = "[이동]이 끝난 후 주변에 피해 1을 줍니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[이동]이 끝난 후\n 주변에 피해 1을 줍니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "TIMEBOMB":
                        info.title = "시한폭탄";
                        info.description = "[공격]시 지정한 위치에 시한폭탄을 설치합니다.\n 3턴 후 중앙에 피해 2, 주변에 피해 1을 줍니다.\n 2개의 시한 폭탄이 같은 로봇에 부착되면\n 첫 번째 폭탄이 바로 폭발하여 피해를 줍니다.\n 사용 후 해당 모듈은 사라집니다";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 지정한 위치에\n 시한폭탄을 설치합니다.\n 3턴 후 중앙에 피해 2,\n 주변에 피해 1을 줍니다.\n 사용 후 해당 모듈은\n 사라집니다";
                        break;
                    case "DODGE":
                        info.title = "회피기동";
                        info.description = "적 로봇의 공격을 1회 회피합니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "적 로봇의 공격을\n 1회 회피합니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "REPULSIONBLAST":
                        info.title = "반동충격파";
                        info.description = "[이동]이 끝난 후 주변의\n 모든 적 로봇을 1칸 밀어냅니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[이동]이 끝난 후\n  주변의 모든 적 로봇을\n 1칸 밀어냅니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "STEERINGNOZZLE":
                        info.title = "사선기동";
                        info.description = "대각선으로 [이동]할 수 있습니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "대각선으로 \n [이동]할 수 있습니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "TELEPORT":
                        info.title = "위치교환기";
                        info.description = "[공격]시 해당 위치로 이동합니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시\n 해당 위치로 이동합니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "AMARGEDDON":
                        info.title = "폭격장치";
                        info.description = "경기장에 폭격을 요청합니다.\n 모든 로봇의 이동과 공격마다 무작위 위치에\n 8회 떨어지며, 피해 1을 줍니다.";
                        info.semiDescription = "폭격을 요청합니다.\n 모든 로봇의 이동과 공격마다\n 무작위 위치에 8회 떨어지며,\n 피해 1을 줍니다.";
                        break;
                    case "ANCHOR":
                        info.title = "고정장치";
                        info.description = "적 로봇의 [이동]에 의해 1회 뒤로 밀려나지 않습니다.";
                        info.semiDescription = "적 로봇의 [이동]에 의해\n 1회 뒤로 밀려나지 않습니다.";
                        break;
                    case "BOMB":
                        info.title = "자폭장치";
                        info.description = "로봇의 체력이 0이 되면 폭발하며\n 주변에 피해 1을 줍니다.(구덩이 제외)";
                        info.semiDescription = "로봇의 체력이\n 0이 되면\n 폭발하며 주변에\n 피해 1을 줍니다.\n (구덩이 제외)";
                        break;
                    case "DUMMYGEAR":
                        info.title = "더미장치";
                        info.description = "해당 모듈을 장착한 로봇을 처치하면\n 점수를 획득하지 않습니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "해당 모듈을\n 장착한 로봇을 처치하면\n 점수를 획득하지 않습니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "EMP":
                        info.title = "EMP장치";
                        info.description = "[공격]시 적 로봇에 장착 된 모든 모듈을 파괴합니다.\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "[ffff00][모듈 아이콘을 눌러서 사용][-]\n [공격]시 적 로봇에 장착 된\n 모든 모듈을 파괴합니다.\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "HACKING":
                        info.title = "해킹장치";
                        info.description = "코인을 바로 획득 할 수 있으며,\n 사용 후 해당 모듈은 사라집니다.";
                        info.semiDescription = "코인을 바로 획득 할 수 있으며,\n 사용 후 해당 모듈은\n 사라집니다.";
                        break;
                    case "SHIELD":
                        info.title = "쉴드장치";
                        info.description = "피해 2를 막아주는 보호막을 생성합니다.\n 쉴드가 모두 파괴되면, 해당 모듈은 사라집니다.";
                        info.semiDescription = "피해 2를 막아주는\n 보호막을 생성합니다.\n 쉴드가 모두 파괴되면,\n 해당 모듈은\n 사라집니다.";
                        break;
                    default:
                        break;
                }
                break;
            default:
                switch (name)
                {
                    case "CROSSFIRE":
                        info.title = "CROSSFIRE";
                        info.description = "Attacks in four directions when [Attacks].\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Attacks in four directions \n when [Attacks].\n The module expires \n after usage.";
                        break;
                    case "DOUBLEATTACK":
                        info.title = "DOUBLEATTACK";
                        info.description = "Attacks a enemy robot twice\n rapidly when [Attack].\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Attacks a enemy robot\n twice rapidly \n when [Attack].\n The module expires \n after usage.";
                        break;
                    case "GRENADES":
                        info.title = "GRENADES";
                        info.description = "When [Attacks], throws grenade and damage 2 to enemy robot, and damage 1 to the surroundings.\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n When [Attacks],\n throws grenade and damage 2\n to enemy robot,\n and damage 1 to the surroundings.\n The module expires\n after usage.";
                        break;
                    case "PERNERATE":
                        info.title = "PERNERATE";
                        info.description = "Damage to every enemy robot on the attack line when [Attack].\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Damage to every enemy robot \n on the attack line \n when [Attack].\n The module expires\n after usage.";
                        break;
                    case "POWERBEAM":
                        info.title = "POWERBEAM";
                        info.description = "Pushes enemy robot when [Attacks].\n Pushes 1 space away and damage 1\n when 1~3 [Attacks].\n Pushes 2 space away and damage 2\n when 4~6 [Attacks].\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Pushes 1 space away\n and damage 1\n when 1~3 [Attacks].\n Pushes 2 space away\n and damage 2\n when 4~6 [Attacks].\n The module expires \n after usage.";
                        break;
                    case "SAWBLADE":
                        info.title = "SAWBLADE";
                        info.description = "Damage when [Moving] and pushes enemy robot away.\n Damage 1 when 1~3 [Moving]\n Damage 2 when 4~6 [Moving]\n The module expires after damages.";
                        info.semiDescription = "Damage when [Moving]\n and pushes\n enemy robot away.\n The module expires\n after damages.";
                        break;
                    case "SHOCKWAVE":
                        info.title = "SHOCKWAVE";
                        info.description = "Push every enemy robot\n nearby after [Movement].\n The module expires after usage.";
                        info.semiDescription = "Push every enemy robot\n nearby after [Movement].\n The module expires\n after usage.";
                        break;
                    case "TIMEBOMB":
                        info.title = "TIMEBOMB";
                        info.description = "When [Attacks], install the time bomb\n to the chosen location.\n After 3 turns, damages 2 to the center, damage 1 to the surroundings.\n The module expires after usage.\n When two timebombs are attached to same robot, first time bomb ";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n When [Attacks],\n install the timebomb \n to the chosen location.\n After 3 turns,\n damages 2 to the center,\n damage 1\n to the surroundings.\n The module expires\n after usage.";
                        break;
                    case "DODGE":
                        info.title = "DODGE";
                        info.description = "Evade 1 attack from enemy.\n The module expires after usage.";
                        info.semiDescription = "Evade 1 attack\n from enemy.\n The module expires\n after usage.";
                        break;
                    case "REPULSIONBLAST":
                        info.title = "REPULSIONBLAST";
                        info.description = "Push every enemy robot\n nearby after [Movement].\n The module expires after usage.";
                        info.semiDescription = "Push every enemy robot\n nearby after [Movement].\n The module expires\n after usage.";
                        break;
                    case "STEERINGNOZZLE":
                        info.title = "STEERINGNOZZLE";
                        info.description = "Able to [Move] diagonal.\n The module expires after usage.";
                        info.semiDescription = "Able to [Move] diagonal.\n The module expires\n after usage.";
                        break;
                    case "TELEPORT":
                        info.title = "TELEPORT";
                        info.description = "Moves to the location when [Attacks].\n The module expires after usage.";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Moves to the location\n when [Attacks].\n The module expires\n after usage.";
                        break;
                    case "AMARGEDDON":
                        info.title = "AMARGEDDON";
                        info.description = "Request bombing to the field.\n Bombs 8 times randomly whenever every robot moves or attacks, doing 1 damage.";
                        info.semiDescription = "Request bombing \n to the field.\n Bombs 8 times randomly\n whenever every robot moves or attacks,\n doing 1 damage.";
                        break;
                    case "ANCHOR":
                        info.title = "ANCHOR";
                        info.description = "Not pushed by enemies' [Movement] once.";
                        info.semiDescription = "Not pushed by enemies'\n [Movement] once.";
                        break;
                    case "BOMB":
                        info.title = "BOMB";
                        info.description = "When the health of the robot reaches 0,\n it explodes and damage 1 to surroundings.\n ( Except the hole)";
                        info.semiDescription = "When the health\n of the robot reaches 0,\n it explodes and damage 1\n to surroundings.\n ( Except the hole)";
                        break;
                    case "DUMMYGEAR":
                        info.title = "DUMMYGEAR";
                        info.description = "The enemy does not earn points when slaying the robot with the module attached.\n The module expires after usage.";
                        info.semiDescription = "The enemy\n does not earn points\n when slaying the robot\n with the module attached.\n The module expires\n after usage.";
                        break;
                    case "EMP":
                        info.title = "EMP";
                        info.description = "Destroy every module that is attached on enemy robot when [Attacks].";
                        info.semiDescription = "[ffff00][Touch the module icon][-]\n Destroy every module\n that is attached on\n enemy robot\n when [Attacks].";
                        break;
                    case "HACKING":
                        info.title = "HACKING";
                        info.description = "Immediately allows to earn the coin, and the module expires after usage.";
                        info.semiDescription = "Immediately\n allows to earn the coin,\n and the module expires\n after usage.";
                        break;
                    case "SHIELD":
                        info.title = "SHIELD";
                        info.description = "Creates shield that defends 2 damage.\n After shield is destroyed, the module expires.";
                        info.semiDescription = "Creates shield that defends 2 damage.\n After shield is destroyed,\n the module expires.";
                        break;
                    default:
                        break;
                }
                break;
        }
        return info;
    }
    
}
