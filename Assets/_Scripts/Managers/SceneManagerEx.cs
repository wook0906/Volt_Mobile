using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    private AsyncOperationHandle<SceneInstance> sceneHandle;
    private AsyncOperation asyncOperation;
    public float SceneLoadingProgress
    {
        get
        {
            if (asyncOperation == null)
                return 0f;
            return asyncOperation.progress;
        }
    }
    public bool IsDoneLoadScene
    {
        get
        {
            if (asyncOperation == null)
                return false;
            return asyncOperation.isDone;
        }
    }
    public BaseScene CurrentScene
    {
        get
        {
            BaseScene[] scenes = GameObject.FindObjectsOfType<BaseScene>();
            foreach (var scene in scenes)
            {
                if (scene.SceneType != Define.Scene.UnKnown)
                    return scene;
            }
            return null;
        }
    }

    public void LoadSceneAsync(Define.Scene type, float delayTime = 0)
    {
        //Managers.Clear();
        Managers.UI.ShowPopupUIAsync<Fade_Popup>();
        Managers.Instance.StartCoroutine(CoStartChangeScene(type, delayTime));
    }

    private IEnumerator CoStartChangeScene(Define.Scene type, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Managers.Instance.StartCoroutine(CoLoadSceneAsync(type));
    }

    private IEnumerator CoLoadSceneAsync(Define.Scene type)
    {
        Fade_Popup fadePopup = null;
        yield return new WaitUntil(() =>
        {
            fadePopup = GameObject.FindObjectOfType<Fade_Popup>();

            return fadePopup != null;
        });

        fadePopup.FadeOut(1f);
        yield return new WaitUntil(() => { return fadePopup.IsDone; });

        sceneHandle = Addressables.LoadSceneAsync($"Assets/_Scenes/{type}.unity", LoadSceneMode.Single, false);

        while (sceneHandle.PercentComplete < 1.0f) yield return null;
        asyncOperation = sceneHandle.Result.ActivateAsync();

        while (asyncOperation.progress < 0.9f) yield return null;

        if (Managers.Scene.CurrentScene != null)
            Managers.Scene.CurrentScene.Clear();
        else
            Managers.UI.CloseAllPopupUI();
        
        asyncOperation.allowSceneActivation = true;
        asyncOperation = null;
    }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string sceneName = System.Enum.GetName(typeof(Define.Scene), type);
        return sceneName;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    public void ActiveScene()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
