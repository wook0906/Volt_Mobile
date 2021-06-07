using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUI : MonoBehaviour
{
    public GameObject attackModuleRootGO;
    public GameObject movementModuleRootGO;
    public GameObject tacticModuleRootGO;

    public UISprite attackTapSprite;
    public UISprite movementTapSprite;
    public UISprite tacticTapSprite;

    public GameObject moduleExplainationPopupPanel;
    public UILabel cardTitle;
    public UILabel cardDescription;
    public UITexture moduleExplainationImage;
    
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Volt_TutorialDone") == 1)
        {
            //Volt_TutorialManager.S.FindContentsByName("WaitIntroduceAllModuleBtn").gameObject.SetActive(false);
            //Volt_TutorialManager.S.TutorialStart("ModuleTypeTap");
        }
        OnPressdownAttackTapButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressdownAttackTapButton()
    {
        movementModuleRootGO.SetActive(false);
        movementTapSprite.depth = 1;

        tacticModuleRootGO.SetActive(false);
        tacticTapSprite.depth = 1;

        attackModuleRootGO.SetActive(true);
        attackTapSprite.depth = 3;
    }

    public void OnPressdownMovementTapButton()
    {
        attackModuleRootGO.SetActive(false);
        attackTapSprite.depth = 1;

        tacticModuleRootGO.SetActive(false);
        tacticTapSprite.depth = 1;

        movementModuleRootGO.SetActive(true);
        movementTapSprite.depth = 3;
    }

    public void OnPressdownTacticTapButton()
    {
        movementModuleRootGO.SetActive(false);
        movementTapSprite.depth = 1;

        attackModuleRootGO.SetActive(false);
        attackTapSprite.depth = 1;

        tacticModuleRootGO.SetActive(true);
        tacticTapSprite.depth = 3;
    }
    public void OnClickModuleCard(GameObject go)
    {
        PopupPanelInit(go);
    }

    void PopupPanelInit(GameObject go)
    {
        moduleExplainationImage.GetComponent<UITexture>().mainTexture = (Texture)Resources.Load("Images/ModuleIcons/" + go.name);
        ModuleDescriptionInfo moduleDesInfo = Volt_ModuleDescriptionInfos.GetModuleDescriptionInfo(go.name, Application.systemLanguage); //Application.systemLanguage);
        cardTitle.text = moduleDesInfo.title;
        cardDescription.text = moduleDesInfo.description;
    }
}
