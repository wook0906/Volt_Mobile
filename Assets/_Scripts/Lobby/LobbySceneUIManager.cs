using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneUIManager : MonoBehaviour
{
    public void OnClickedLobbySceneStartBtn()
    {
        LobbyScene_UI scene_UI = Managers.UI.GetSceneUI<LobbyScene_UI>();
        if(!scene_UI)
        {
            Debug.LogError("Not Find LobbyScene_UI");
            return;
        }

        scene_UI.OnClickStartGame();
    }
}
