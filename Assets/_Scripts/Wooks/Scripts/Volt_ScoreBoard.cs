using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volt_ScoreBoard : MonoBehaviour
{
    //public List<Text> playerNicknames;
    public List<UILabel> playerNicknames;
    //public List<Text> playerScores;
    public List<UILabel> playerScores;

    float timer;

    Animator animator;
    UISprite bg;

    bool isPopup = false;
    bool isPlaying = false;
    bool isTutorialDone = false;

    public static float popUpSpeed =1000f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bg = GetComponentInChildren<UISprite>();
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
    }
    public void SetScore(int playerNumber, int score)
    {
        playerScores[playerNumber - 1].text = score.ToString();
    }
    public void SetNickname(int playerNumber, string nickName)
    {
        playerNicknames[playerNumber - 1].text = nickName;
    }
    public void OnClicked()
    {
        if (PlayerPrefs.GetInt("Volt_TutorialDone") == 0 && !isTutorialDone)
        {
            isTutorialDone = true;
            Volt_TutorialManager.S.DoNextTutorial();
        }
        if (!isPlaying)
            StartCoroutine(PopAnimation());
        
    }
    IEnumerator PopAnimation()
    {
        float v = 0f;
        if (!isPopup)
        {
            isPlaying = true;
            isPopup = true;
            while (v < 300f)
            {
                v += Time.fixedDeltaTime * popUpSpeed;
                bg.leftAnchor.absolute += Time.fixedDeltaTime * popUpSpeed;
                bg.rightAnchor.absolute+= Time.fixedDeltaTime * popUpSpeed;
                yield return null;
            }
            isPlaying = false;
        }
        else
        {
            isPlaying = true;
            isPopup = false;
            while (v < 300f)
            {
                v += Time.fixedDeltaTime * popUpSpeed;
                bg.leftAnchor.absolute -= Time.fixedDeltaTime * popUpSpeed;
                bg.rightAnchor.absolute -= Time.fixedDeltaTime * popUpSpeed;
                yield return null;
            }
            isPlaying = false;
        }
    }
}
