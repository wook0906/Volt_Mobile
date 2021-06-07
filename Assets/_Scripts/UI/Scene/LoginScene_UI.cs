using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using System.Xml;
using UnityEngine.AddressableAssets;
using System.IO;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SignInWithApple;

public class LoginScene_UI : UI_Scene
{
    enum Buttons
    {
        GameStart_Btn,
        Logout_Btn,
        TutorialSkip_Btn,
        Google_Btn,
        Facebook_Btn,
        SignInApple_Btn
    }

    enum ProgressBars
    {
        ResourcesLoadingBar
    }

    enum GameObjects
    {
        LoginButtons_BG
    }

    enum Labels
    {
        GameStart_Label,
        Logout_Label,
        TutorialSkip_Label
    }

    enum Sprites
    {
        ForeGround_Sprite
    }

    private string myID;
    private bool isClickStartGameBtn = false;

    public override void Init()
    {
        base.Init();

#if UNITY_EDITOR
        PlayerPrefs.DeleteKey("Volt_LoginType");
#endif

        Bind<UIButton>(typeof(Buttons));
        Bind<UISlider>(typeof(ProgressBars));
        Bind<GameObject>(typeof(GameObjects));
        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UIButton>(typeof(Buttons));
        //BindSprite<UISlider>(typeof(ProgressBars));
        //BindSprite(typeof(GameObjects));
        //BindSprite<UISprite>(typeof(Sprites));

        //SetButtonSwap(typeof(Buttons));

        Get<GameObject>((int)GameObjects.LoginButtons_BG).SetActive(false);
        Get<UISlider>((int)ProgressBars.ResourcesLoadingBar).gameObject.SetActive(false);
        Get<UIButton>((int)Buttons.Logout_Btn).gameObject.SetActive(false);
        if(Get<UIButton>((int)Buttons.TutorialSkip_Btn))
            Get<UIButton>((int)Buttons.TutorialSkip_Btn).gameObject.SetActive(false);
        Get<UIButton>((int)Buttons.GameStart_Btn).gameObject.SetActive(false);

        Get<UIButton>((int)Buttons.Google_Btn).onClick.Add(new EventDelegate(OnPressedGoogleLoginBtn));
        Get<UIButton>((int)Buttons.Facebook_Btn).onClick.Add(new EventDelegate(OnPressedFacebookLoginBtn));
        Get<UIButton>((int)Buttons.SignInApple_Btn).onClick.Add(new EventDelegate(OnClickSignInWithAppleBtn));
        Get<UIButton>((int)Buttons.Logout_Btn).onClick.Add(new EventDelegate(OnPressdownLogOutBtn));
        Get<UIButton>((int)Buttons.GameStart_Btn).onClick.Add(new EventDelegate(OnClickStartGame));
        if (Get<UIButton>((int)Buttons.TutorialSkip_Btn))
            Get<UIButton>((int)Buttons.TutorialSkip_Btn).onClick.Add(new EventDelegate(OnPressdownTutorialSkipBtn));
#if UNITY_ANDROID
                PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().Build());
                PlayGamesPlatform.DebugLogEnabled = true;
                PlayGamesPlatform.Activate();

                Get<UIButton>((int)Buttons.Google_Btn).gameObject.SetActive(true);
                Get<UIButton>((int)Buttons.SignInApple_Btn).gameObject.SetActive(false);
#elif UNITY_IOS
        Get<UIButton>((int)Buttons.Google_Btn).gameObject.SetActive(false);
        Get<UIButton>((int)Buttons.SignInApple_Btn).gameObject.SetActive(true);
#endif

        if (!PlayerPrefs.HasKey("Volt_TutorialDone"))
        {
            PlayerPrefs.SetInt("Volt_TutorialDone", 0); //1 : true 0 : false;
        }

        LoginType loginType = LoginType.None;
        if (PlayerPrefs.HasKey("Volt_LoginType"))
        {
            loginType = (LoginType)PlayerPrefs.GetInt("Volt_LoginType");
        }

        if (!FB.IsInitialized)
        {
            FB.Init(FacebookInitCallback, OnHideUnity);
            Debug.Log("Init Facebook sdk");
        }
        else
        {
            FB.ActivateApp();
        }

        switch (loginType)
        {
            case LoginType.None:
                Get<GameObject>((int)GameObjects.LoginButtons_BG).SetActive(true);
                break;
            case LoginType.Google:
                OnPressedGoogleLoginBtn();
                break;
            case LoginType.Apple:
                if (PlayerPrefs.HasKey("APPLE_SIGNIN"))
                {
                    Debug.Log("Sign in Apple");
                    string userID = PlayerPrefs.GetString("APPLE_SIGNIN");
                    Volt_PlayerData.instance.UserToken = userID;
                    Debug.Log($"UserID:{userID}");
                    var siwa = FindObjectOfType<SignInWithApple>();
                    siwa.onError.AddListener((SignInWithApple.CallbackArgs arg0) =>
                    {
                        print($"Error GetCredentialState msg:{arg0.error}");
                    });

                    siwa.onCredentialState.AddListener(
                        (SignInWithApple.CallbackArgs args) =>
                        {
                            switch (args.credentialState)
                            {
                                case UserCredentialState.Authorized:
                                    Debug.Log("Authorized");
                                    PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
                                    break;
                                case UserCredentialState.NotFound:
                                case UserCredentialState.Revoked:
                                    Debug.Log("NotFound or Revoked");
                                    break;
                                default:
                                    break;
                            }
                        });
                    siwa.GetCredentialState(userID);
                }
                else
                {
                    Debug.LogError("[Error] APPLE_SIGNIN no data");
                }
                break;
            default:
                break;
        }
    }

    private void ShowLoginPanel(bool bShow = true)
    {
        if(bShow)
            Get<GameObject>((int)GameObjects.LoginButtons_BG).SetActive(true);
        else
            Get<GameObject>((int)GameObjects.LoginButtons_BG).SetActive(false);
    }

    private IEnumerator CoRunPregressBarAnimation()
    {
        UISlider progressBar = Get<UISlider>((int)ProgressBars.ResourcesLoadingBar);
        progressBar.gameObject.SetActive(true);

        yield return new WaitUntil(() => { return DBManager.instance != null; });

        while (progressBar.value < 1)
        {
            progressBar.value = DBManager.instance.Progress;
            yield return null;
        }

        progressBar.gameObject.SetActive(false);

        Get<UIButton>((int)Buttons.GameStart_Btn).gameObject.SetActive(true);
        Get<UIButton>((int)Buttons.Logout_Btn).gameObject.SetActive(true);

        //TODO: 릴리즈 전 스킵 버튼 없애야됨
        if (Get<UIButton>((int)Buttons.TutorialSkip_Btn))
            Get<UIButton>((int)Buttons.TutorialSkip_Btn).gameObject.SetActive(true);
    }

    public void OnSuccessSignIn()
    {
        Managers.UI.CloseAllPopupUI();
        ShowLoginPanel(false);

        StartCoroutine(CoRunPregressBarAnimation());
    }

    public void OnFailSignIn()
    {
        LoginProgress_Popup popup = FindObjectOfType<LoginProgress_Popup>();
        if(popup != null)
            Managers.UI.ClosePopupUI(popup);

        ShowLoginPanel(false);
        Managers.UI.ShowPopupUIAsync<AccountMaker_Popup>();
    }

    public void OnPressdownLogOutBtn()
    {

#if UNITY_ANDROID
        if (Volt_PlayerData.instance.LoginType == LoginType.Facebook)
        {
            FB.LogOut();
        }
        else if (Volt_PlayerData.instance.LoginType == LoginType.Google)
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
        }
#endif

#if UNITY_IOS
        //TODO: 애플 로그아웃 구현
#endif
        Debug.Log("Logout");
        //add type
        Volt_PlayerData.instance.Clear();

        Managers.UI.CloseAllPopupUI();
        ShowLoginPanel();
    }

    public void OnPressdownTutorialSkipBtn() // tmp
    {
        PlayerPrefs.SetInt("Volt_TutorialDone", 1);
    }

    public void OnClickStartGame()
    {
        if(!isClickStartGameBtn)
        {
            isClickStartGameBtn = true;
            Debug.Log("Load Scene!!");
            Managers.Scene.LoadSceneAsync(Define.Scene.Lobby);
        }
    }

    #region facebook sdk
    private void FacebookInitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            //Debug.Log("Facebook Activate App 2");
            if (FB.IsLoggedIn)
            {
                Volt_PlayerData.instance.LoginType = LoginType.Facebook;
                //Debug.Log("Already Logged in");
                Debug.Log($"Token:{AccessToken.CurrentAccessToken.UserId}");
                //infoLog.text = AccessToken.CurrentAccessToken.UserId;
                Volt_PlayerData.instance.UserToken = AccessToken.CurrentAccessToken.UserId;

                PacketTransmission.SendSignInPacket(AccessToken.CurrentAccessToken.UserId.Length, AccessToken.CurrentAccessToken.UserId);
            }
        }
        else
        {
            Debug.Log("Failed to Init facebook sdk");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OnPressedFacebookLoginBtn()
    {
        Managers.UI.ShowPopupUIAsync<LoginProgress_Popup>();
        //loginProgressPanel.SetActive(true);
        if (!FB.IsInitialized)
        {
            FB.Init(FacebookInitCallback, OnHideUnity);
            //Debug.Log("Init Facebook sdk");
        }
        else
        {
            FB.ActivateApp();
            //Debug.Log("시부레뭐였더라");
        }


        //List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
        //FB.LogInWithReadPermissions(permissions, FacebookAuthCallback);
        StartCoroutine(FBInit());

    }
    IEnumerator FBInit()
    {
        yield return new WaitUntil(() => FB.IsInitialized);
        FB.LogInWithPublishPermissions(null, FacebookAuthCallback);
    }

    private void FacebookAuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;
            myID = aToken.UserId;
            Volt_PlayerData.instance.UserToken = myID;
            Debug.Log($"Welcome,{myID}");

            PlayerPrefs.SetInt("Volt_LoginType", (int)LoginType.Facebook);
            PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
        }
        else
        {
            LoginProgress_Popup popup = FindObjectOfType<LoginProgress_Popup>();
            Managers.UI.ClosePopupUI(popup);
            Debug.Log("User cancelled login");
        }
    }
    #endregion

    #region google sdk
    public void OnPressedGoogleLoginBtn()
    {
        Managers.UI.CloseAllPopupUI();
        ShowLoginPanel(false);
        StartCoroutine(GoogleLoginProgress());
    }
    IEnumerator GoogleLoginProgress()
    {
#if UNITY_ANDROID
        //loginProgress_Popup = Managers.UI.ShowPopupUI<LoginProgress_Popup>();
        yield return new WaitUntil(() => PlayGamesPlatform.Instance != null);
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    //Debug.Log("Google authentication Success : " + PlayGamesPlatform.Instance.GetIdToken());
                    //Debug.Log(PlayGamesPlatform.Instance.GetIdToken().Length);
                    string googleEmail = PlayGamesPlatform.Instance.GetUserEmail();
                    Debug.Log($"google email : {googleEmail}, Length : {googleEmail.Length}");
                    ShowLoginPanel(false);
                    Volt_PlayerData.instance.LoginType = LoginType.Google;
                    Volt_PlayerData.instance.UserToken = googleEmail;

                    PlayerPrefs.SetInt("Volt_LoginType", (int)LoginType.Google);
                    PacketTransmission.SendSignInPacket(googleEmail.Length, googleEmail);

                }
                else
                {
                    Debug.Log("Google Faild");
                    //TODO: 로그인 실패 팝업
                }
            });
        }
#else
        yield break;
#endif
    }
    #endregion

    #region Apple sdk
    public void OnClickSignInWithAppleBtn()
    {
        Managers.UI.ShowPopupUIAsync<LoginProgress_Popup>();
        //loginProgressPanel.SetActive(true);
        var siwa = FindObjectOfType<SignInWithApple>();
        siwa.Login(OnLogin);
    }

    private void OnLogin(SignInWithApple.CallbackArgs args)
    {
        if (args.error != null)
        {
            Debug.Log("Errors occurred: " + args.error);
            return;
        }

        UserInfo userInfo = args.userInfo;

        // Save the userId so we can use it later for other operations.
        string userId = userInfo.userId;

        Debug.Log($"Token:{userId}");
        ShowLoginPanel(false);
        Volt_PlayerData.instance.LoginType = LoginType.Apple;
        Volt_PlayerData.instance.UserToken = userId;

        PlayerPrefs.SetInt("Volt_LoginType", (int)LoginType.Apple);
        PlayerPrefs.SetString("APPLE_SIGNIN", userInfo.userId);
        PacketTransmission.SendSignInPacket(userId.Length, userId);
    }
    #endregion
}
