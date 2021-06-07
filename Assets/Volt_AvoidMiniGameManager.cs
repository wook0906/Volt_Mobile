using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Volt_AvoidMiniGameManager : MonoBehaviour
{
    public static Volt_AvoidMiniGameManager S;
    public List<MiniGameAvoidDifficultyData> difficultyDatas;
    private int difficulty = 0;
    public int Difficulty
    {
        get { return difficulty; }
        set
        {
            if (value >= difficultyDatas.Count) return;
            difficulty = value;
            generator.SetDifficulty(difficultyDatas[difficulty]);
        }
    }
    public Volt_MiniGameMissileGenerator generator;
    public float totalGamePlayTime = 40f;
    private float timer = 0f;
    public float Timer
    {
        get { return timer; }
        set
        {
            timer = value;
            if(timer >= 0)
                timerText.text = (totalGamePlayTime - timer).ToString("F02");
        }
    }
    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }
    public bool isGamePlaying = false;
    public Text startTimerText;
    public Text timerText;
    public Text scoreText;
    float startTime;
    // Start is called before the first frame update
    private void Awake()
    {
        S = this;
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => !Volt_LoadingSceneManager.S.isWorking);
        GameStart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    GameStart();
        //}
        if (isGamePlaying)
        {
            Timer += Time.fixedDeltaTime;
            //if (Timer >= totalGamePlayTime)
            //{
            //    Timer = 0f;
            //    isGamePlaying = false;
            //}
        }
    }
    void GameStart()
    {
        StartCoroutine(DealyedGameStart());
    }
    IEnumerator DealyedGameStart()
    {
        startTimerText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        int startTimer = 3;
        while(startTimer != 0)
        {
            startTimerText.text = startTimer.ToString();
            yield return new WaitForSeconds(1f);
            startTimer--;
        }
        startTimerText.text = "";
        scoreText.text = "0";
        Timer = 0f;
        isGamePlaying = true;
        generator.SetDifficulty(difficultyDatas[0]);
        generator.GenerateStart();

    }
    public void DropDoneCallback()
    {
        Difficulty++;
        generator.GenerateStart();
    }
    public void GameOver()
    {
        isGamePlaying = false;
        Timer = 0f;
        //Debug.Log("게임오버");
        Volt_LoadingSceneManager.S.RequestLoadScene("Lobby2",6f);
    }
}
