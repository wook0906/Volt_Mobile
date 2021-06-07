using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_TutorialContents : MonoBehaviour
{
    private UISprite blockPanel;

    public bool isNeedInput;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoneCallback()
    {
        gameObject.SetActive(false);
        HideBlockPanel();
        if(!isNeedInput)
            Volt_TutorialManager.S.DoNextTutorial();
    }
    
    public void HideBlockPanel()
    {
        blockPanel = GetComponentInChildren<UISprite>();
        blockPanel.alpha = 0f;
        Volt_TutorialManager.S.isShowingTutorial = false;
    }
    public void ShowBlockPanel()
    {
        blockPanel.alpha = 0.9f;
    }

    public void OnClickMoveBtn(GameObject go)
    {
        Volt_PlayerUI playerUI = FindObjectOfType<Volt_PlayerUI>();
        playerUI.OnClickMoveButton(go);
    }

    public void OnClickStartGame()
    {
        LobbyScene_UI sceneUI = FindObjectOfType<LobbyScene_UI>();
        sceneUI.OnClickStartGame();
    }
}
