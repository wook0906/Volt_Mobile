using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameScene : BaseScene
{
    enum Loads
    {
        None = 0,
        PostProcessing = 1 << 0,
        GamePlayData = 1 << 1,
        ModuleDeck = 1 << 2,
        ParticleManager = 1 << 3,
        PlayerUI = 1 << 4,
        RobotObserver = 1 << 5,
        GMUI = 1 << 6,
        GameManager = 1 << 7,
        PrefabFactory = 1 << 8,
        MatchingBGTexutre = 1 << 9,
        All = PostProcessing | GamePlayData | ModuleDeck |
            ParticleManager | PlayerUI | RobotObserver | GMUI |
            GameManager | MatchingBGTexutre | PrefabFactory
    }

    private Loads load;
    public override float Progress
    {
        get
        {
            Array loads = typeof(Loads).GetEnumValues();
            int max = loads.Length - 2;
            int count = 0;
            for(int i = 1; i <= max; ++i)
            {
                if ((load & (Loads)loads.GetValue(i)) != 0)
                    count++;
            }
            return (float)count / max;
        }
    }

    public override bool IsDone
    {
        get { return load == Loads.All; }
    }

    
    AsyncOperationHandle<GameObject>[] preloadHandles;
    AsyncOperationHandle<GameObject> gameMgrHandle;
    List<GameObject> coreGOs = new List<GameObject>();
    Volt_GameManager gameMgr;
    private IEnumerator Start()
    {
        SceneType = Define.Scene.GameScene;

        AsyncOperationHandle<GameObject> fadePopupHandle = Managers.UI.ShowPopupUIAsync<Fade_Popup>();
        yield return new WaitUntil(() => { return fadePopupHandle.IsDone; });
        Fade_Popup fadePopup = fadePopupHandle.Result.GetComponent<Fade_Popup>();
        fadePopup.FadeIn(.5f, float.MaxValue);

        AsyncOperationHandle<GameObject> loadingPopupHandle = Managers.UI.ShowPopupUIAsync<Loading_Popup>();
        yield return new WaitUntil(() => { return loadingPopupHandle.IsDone; });
        Loading_Popup loadingPopup = loadingPopupHandle.Result.GetComponent<Loading_Popup>();
        yield return new WaitUntil(() => { return loadingPopup.IsInit; });


        string[] preloadEnumNames = typeof(Loads).GetEnumNames();
        int max = preloadEnumNames.Length - 3;
        preloadHandles = new AsyncOperationHandle<GameObject>[max];
        for (int i = 1; i <= max; ++i)
        {
            preloadHandles[i - 1] = Managers.Resource.InstantiateAsync($"Game/Core/{preloadEnumNames[i]}.prefab");
            preloadHandles[i - 1].Completed += (result) =>
              {
                  if (result.Result.name.Contains("(Clone)"))
                  {
                      result.Result.name = result.Result.name.Split('(')[0];
                      load |= (Loads)Enum.Parse(typeof(Loads), result.Result.name);
                  }
                  coreGOs.Add(result.Result);
                  if (!result.Result.GetComponent<Volt_GameManager>())
                      return;
                  gameMgr = result.Result.GetComponent<Volt_GameManager>();
              };
        }

        yield return new WaitUntil(() =>
        {
            return preloadHandles.Length == coreGOs.Count;
        });

        foreach (var coreGO in coreGOs)
        {
            coreGO.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitUntil(() => { return IsDone; });
        Managers.UI.CloseAllPopupUI();
        gameMgr.StartMatching();
    }

    public void OnLoadedMatchingBGTexture()
    {
        load |= Loads.MatchingBGTexutre;
    }
    public override void Clear()
    {
        Managers.Clear();
        Camera.main.enabled = false;
    }
}
