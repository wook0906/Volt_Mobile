using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Volt_ResultSceneManager : MonoBehaviour
{

    static int vpMult = 25;
    static int killMult = 25;
    static int victory = 75;
    static int lose = 40;

    public Texture2D defeatBG;
    public Texture2D victoryBG;
    public GameObject exitPanel;
    public UILabel vpLabel;
    public UILabel vpScoreLabel;
    public UILabel rankLabel;
    public UILabel rankScoreLabel;
    public UILabel killLabel;
    public UILabel killScoreLabel;
    public UILabel deathLabel;
    public UILabel totalPointLabel;
    public UILabel nickNameLabel;
    public UITexture sceneBG;
    public UISprite resultIcon;
    public ParticleSystem fireWorksParticle;
    int totalScore;
    public List<GameObject> RobotModels;
    public Camera[] robotViewCameras;
    public Transform robotRoot;

    private GameObject robot;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in robotViewCameras)
        {
            float ratio = (float)Screen.width / (float)Screen.height;
            float value1 = 4 / 3f;
            float value2 = 16 / 9f;

            float differ1 = Mathf.Abs(ratio - value1);
            float differ2 = Mathf.Abs(ratio - value2);

            item.orthographicSize = differ1 < differ2 ? 2.2f : 2f;
        }
        DisPlayGamePlayData();
        //Invoke("OnClickGoToLobbyBtn", 15f);
        //Invoke("DelayedDestroy", 3f);
        Volt_SoundManager.S.environmentSound.Stop();
        Volt_SoundManager.S.bgm.Stop();
        //결과화면 음악재생.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickGoToLobbyBtn();
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(touchFeedbackEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.Euler(Vector3.zero));
        }
        */
    }
    public void OnClickGoToLobbyBtn()
    {
        Managers.Scene.LoadSceneAsync(Define.Scene.Lobby);
        //Volt_LoadingSceneManager.S.RequestLoadScene("Lobby2");
        CancelInvoke("OnClickGoToLobbyBtn");
    }
    public void OnClickApplicationExitBtn(bool agree)
    {
        if (agree)
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.Quit();
            //else if (Application.platform == RuntimePlatform.WindowsEditor)
                //UnityEditor.EditorApplication.isPlaying = false;
        }
        else
            exitPanel.SetActive(false);
    }
    public void DisPlayGamePlayData()
    {
        nickNameLabel.text = Volt_PlayerData.instance.NickName;
        vpLabel.text = Volt_GamePlayData.S.Coin.ToString();
        vpScoreLabel.text = (Volt_GamePlayData.S.Coin * vpMult).ToString();
        totalScore += Volt_GamePlayData.S.Coin * vpMult;
        


        killLabel.text = Volt_GamePlayData.S.Kill.ToString();
        killScoreLabel.text = (Volt_GamePlayData.S.Kill * killMult).ToString();
        totalScore += Volt_GamePlayData.S.Kill * killMult;

        deathLabel.text = Volt_GamePlayData.S.Death.ToString();

        RobotType robotType = Volt_GamePlayData.S.RobotType;
        SkinType skinType = Volt_PlayerData.instance.selectdRobotSkins[robotType].SkinType;

        Managers.Resource.InstantiateAsync($"Robots/{robotType}/{robotType}_{skinType}.prefab",
            (result) =>
            {
                robot = result.Result;
                robot.transform.parent = robotRoot;
                robot.GetOrAddComponent<Volt_ModelRobot>().Init(robotType);
                if(Volt_GamePlayData.S.Rank == 1)
                {
                    robot.GetComponent<Animator>().CrossFade("Win", 0.1f);
                }
                else
                {
                    robot.GetComponent<Animator>().CrossFade("Lose", 0.1f);
                }
            });
        

        //RobotModels[(int)Volt_GamePlayData.S.RobotType].SetActive(true);
        //Material[] skin = Volt_PrefabFactory.S.GetSkinDatas()[Volt_GamePlayData.S.RobotType].GetSkinMaterial(Volt_PlayerData.instance.selectdRobotSkins[Volt_GamePlayData.S.RobotType].SkinType);
        //RobotModels[(int)Volt_GamePlayData.S.RobotType].GetComponentInChildren<Renderer>().materials = skin;
        if (Volt_GamePlayData.S.Rank == 1)
        {
            
            //RobotModels[(int)Volt_GamePlayData.S.RobotType].GetComponent<Animator>().CrossFade("win", 0.1f);
            totalScore += victory;
            rankScoreLabel.text = victory.ToString();
            sceneBG.mainTexture = victoryBG;
            resultIcon.spriteName = "Icon_Victory";
            fireWorksParticle.Play();
        }
        else
        {
            
            //RobotModels[(int)Volt_GamePlayData.S.RobotType].GetComponent<Animator>().CrossFade("lose", 0.1f);
            totalScore += lose;
            rankScoreLabel.text = lose.ToString();
            sceneBG.mainTexture = defeatBG;
            resultIcon.spriteName = "Icon_Defeat";
        }
        rankLabel.text = Volt_GamePlayData.S.Rank.ToString();

        totalPointLabel.text = totalScore.ToString();



        if (Volt_GamePlayData.S.isPracticeMode)
        {
            totalPointLabel.text = "0";
            killScoreLabel.text = "0";
            rankScoreLabel.text = "0";
            vpScoreLabel.text = "0";
        }
        else
        {
            //Debug.LogError("Total Score : " + totalScore);
            PacketTransmission.SendGoldPacket(totalScore);
        }
    }
    void DelayedDestroy()
    {
        Destroy(Volt_GamePlayData.S.gameObject);
    }
    
}
