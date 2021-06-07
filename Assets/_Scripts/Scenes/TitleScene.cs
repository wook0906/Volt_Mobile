using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TitleScene : BaseScene
{
    enum Loads
    {
        None,
        CheckDLCSize = 1 << 0,
        DLC = 1 << 1,
        SceneUI = 1 << 2,
        All = DLC | SceneUI | CheckDLCSize
    };

    public UISlider progressbar;
    public GameObject dlcDownPopup;
    public UILabel downloadPercentLabel;
    public UILabel dlcSizeLabel;

    private Exit_Popup exitPopup;

    private AsyncOperationHandle dlcDownHandle;

    private Loads loads;
    private long dlcSize;
    private bool isStartDownloadDLC;

    public override float Progress
    {
        get
        {
            string[] loadList = typeof(Loads).GetEnumNames();
            int max = loadList.Length - 2; // Except none, all
            int count = 0;
            for(int i = 1; i <= max; ++i)
            {
                if ((loads & (Loads)Enum.Parse(typeof(Loads), loadList[i])) != 0)
                    count++;
            }
            return (float)count / max;
        }
    }

    public override bool IsDone
    {
        get
        {
            return loads == Loads.All;
        }
    }

    protected override void Init()
    {
        Managers.UI.ShowSceneUIAsync<LoginScene_UI>(() =>
        {
            loads |= Loads.SceneUI;
            Debug.Log("Load Title Scene UI");
        });
    }

    public override void Clear()
    {
        Managers.Clear();
        Camera.main.enabled = false;
    }

    private IEnumerator CoRunProgressAnimation(AsyncOperationHandle handle)
    {
        Debug.Log("RunProgressAnimation"); 
        progressbar.gameObject.SetActive(true);
        progressbar.value = 0f;
        yield return new WaitUntil(() => { return handle.IsValid(); });
        float min = handle.PercentComplete;
        while (!handle.IsDone)
        {
            float percentComplete = Mathf.Clamp(handle.PercentComplete, min, 1.0f);
            float percent = percentComplete * 100.0f;
            downloadPercentLabel.text = $"{percent.ToString("F01")}%";
            progressbar.value = percentComplete;
            yield return null;
        }
        progressbar.value = 1f;
        progressbar.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        SceneType = Define.Scene.Title;

        if (ClientSocketModule.Instance)
            DontDestroyOnLoad(ClientSocketModule.Instance);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.InternetNonReachable);
            //Managers.UI.ShowPopupUI<NetworkError_Popup>();
            //Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.InternetNonReachable);
            yield break;
        }

        #region Addressable Assets Download
        Addressables.GetDownloadSizeAsync("DLC").Completed += (result) =>
        {
            dlcSize = ((AsyncOperationHandle<long>)result).Result;
            Addressables.Release(result);
            loads |= Loads.CheckDLCSize;
        };
        yield return new WaitUntil(() => { return (loads & Loads.CheckDLCSize) != 0; });
        Debug.Log($"DLC Size:{dlcSize}");
        if (dlcSize > 0)
        {
            dlcDownPopup.SetActive(true);
            float mbyte = (dlcSize / 1024f) / 1024f;
            dlcSizeLabel.text = $"({mbyte.ToString("F02")}MB)";
            yield return new WaitUntil(() => { return isStartDownloadDLC; });

            dlcDownHandle = Addressables.DownloadDependenciesAsync("DLC", true);
            dlcDownHandle.Completed += (result) => { loads |= Loads.DLC; };

            StartCoroutine(CoRunProgressAnimation(dlcDownHandle));
        }
        else
        {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
                    LoginType loginType = LoginType.None;
                    if (PlayerPrefs.HasKey("Volt_LoginType"))
                        loginType = (LoginType)PlayerPrefs.GetInt("Volt_LoginType");

                    if (loginType != LoginType.None)
                        Managers.UI.ShowPopupUIAsync<LoginProgress_Popup>();
#endif
            loads |= Loads.DLC;
        }
        yield return new WaitUntil(() => { return (loads & Loads.DLC) != 0; });
        #endregion

        if (Volt_SoundManager.S)
        {
            Volt_SoundManager.S.ChangeBGM((AudioClip)Resources.Load("BGMS/Lobby"));
        }
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/BGMS/Lobby.wav",
            (result) =>
            {
                Volt_SoundManager.S.ChangeBGM(result.Result);
            });

        yield return new WaitUntil(() => { return Volt_SoundManager.S.bgm.isPlaying; });
        Init();
        Managers.Data.Init();

        yield return new WaitUntil(() => { return loads == Loads.All; });
        Managers.UI.CloseAllPopupUI();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            if(exitPopup == null)
            {
                AsyncOperationHandle<GameObject> handle = Managers.UI.ShowPopupUIAsync<Exit_Popup>();
                if (handle.IsValid())
                {
                   handle.Completed += (result) =>
                   {
                       exitPopup = result.Result.GetComponent<Exit_Popup>();
                   };
                }
            }
            else
            {
                Managers.UI.ClosePopupUI(exitPopup);
            }
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.InternetNonReachable);
            //Managers.UI.ShowPopupUI<NetworkError_Popup>();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.SetInt("Volt_TutorialDone", 0);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteKey("Volt_LoginType");
        }
    }

    public void OnClickDownloadDlcButton()
    {
        isStartDownloadDLC = true;
        dlcDownPopup.SetActive(false);
    }
}
