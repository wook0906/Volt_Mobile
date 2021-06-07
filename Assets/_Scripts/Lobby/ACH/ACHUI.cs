using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACHUI : MonoBehaviour
{
    public static ACHUI instance;

    public GameObject dailyACHViewRootGO;
    public GameObject normalACHViewRootGO;

    public ScrollViewItemCreator dailyScrollViewItemCreator;
    public ScrollViewItemCreator normalScrollViewItemCreator;

    public UISprite dailyTapSprite;
    public UISprite normalTapSprite;

    private void Awake()
    {
        instance = this;
    }
    private IEnumerator Start()
    {
        if(PlayerPrefs.GetInt("Volt_TutorialDone") == 1)
        {
            //Volt_TutorialManager.S.FindContentsByName("WaitAchievementPanelOpen").gameObject.SetActive(false);
            //Volt_TutorialManager.S.TutorialStart("DailyAchievementTap");
        }
        yield return new WaitUntil(() => { return dailyScrollViewItemCreator.IsInit &&
            normalScrollViewItemCreator.IsInit; });
        //gameObject.SetActive(false);
        //dailyACHViewRootGO.SetActive(false);
        normalACHViewRootGO.SetActive(false);
    }

    private void OnDisable()
    {
        //print("OnDisable ACH");
    }

    public void OnPressdownDailyTapButton()
    {
        normalACHViewRootGO.SetActive(false);
        normalTapSprite.depth = 2;

        dailyACHViewRootGO.SetActive(true);
        dailyTapSprite.depth = 3;
    }

    public void OnPressdownNormalTapButton()
    {
        dailyACHViewRootGO.SetActive(false);
        dailyTapSprite.depth = 2;

        normalACHViewRootGO.SetActive(true);
        normalTapSprite.depth = 3;

        if (PlayerPrefs.GetInt("Volt_TutorialDone") == 1)
        {
            //Volt_TutorialManager.S.FindContentsByName("WaitNormalAchievementTap").gameObject.SetActive(false);
            //Volt_TutorialManager.S.TutorialStart("NormalAchievementCondition");
        }
    }
    

}
