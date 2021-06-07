using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.Networking;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SignInWithApple;


[System.Serializable]
public enum LoginType
{
    None, Facebook, Google, Apple
}
public class Volt_TitleSceneManager : MonoBehaviour
{
    public static Volt_TitleSceneManager S;
    public GameObject selectLoginTypePanel;
    public GameObject createAccountUI;
    public GameObject nameOverlapUI;
    public GameObject networkErrorUI;
    public GameObject exitUI;
    public GameObject startBtn;
    public GameObject facebookLoginBtn;
    public GameObject googleLoginBtn;
    public GameObject appleLoginBtn;
    public GameObject logOutBtn;
    public GameObject loginBtn;
    public GameObject loginProgressPanel;
    public UISlider resourceLoadProgressBar;
    public GameObject accountCreateErrorPanel;
    //public string tmpToken;
    //public string appleAccountNickname;


    public UILabel infoLog;

    [SerializeField]
    string myID;

    bool sceneOver = false;
    // For Test
    
    private void Awake()
    {
        S = this;
        
#if UNITY_ANDROID
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
        if (!PlayerPrefs.HasKey("Volt_TutorialDone"))
        {
            PlayerPrefs.SetInt("Volt_TutorialDone",0); //1 : true 0 : false;
        }
        if (PlayerPrefs.HasKey("Volt_LoginType"))
        {
            if(PlayerPrefs.GetInt("Volt_LoginType") == (int)LoginType.Facebook)
            {
                if (!FB.IsInitialized)
                {
                    FB.Init(FacebookInitCallback, OnHideUnity);
                    Debug.Log("Init Facebook sdk");
                }
                else 
                {
                    FB.ActivateApp();
                    //Debug.Log("시부레뭐였더라");
                }
            }
            else if(PlayerPrefs.GetInt("Volt_LoginType") == (int)LoginType.Google)
            {
                OnPressedGoogleLoginBtn();
            }
            else
            {
                DisplayLogin(false);
            }
        }
        else
        {
            if (!FB.IsInitialized)
            {
                FB.Init(FacebookInitCallback, OnHideUnity);
                Debug.Log("Init Facebook sdk");
            }
            else
            {
                FB.ActivateApp();
                //Debug.Log("시부레뭐였더라");
            }
#if UNITY_ANDROID
            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
#endif      
            DisplayLogin(false);
        }
        infoLog.text = "no Login";
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
                infoLog.text = AccessToken.CurrentAccessToken.UserId;
                Volt_PlayerData.instance.UserToken = AccessToken.CurrentAccessToken.UserId;

                PacketTransmission.SendSignInPacket(AccessToken.CurrentAccessToken.UserId.Length, AccessToken.CurrentAccessToken.UserId);
            }
            else
            {
                //Debug.Log("not logged in yet");
                //SetActivePlatformBtn(true);
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
        loginProgressPanel.SetActive(true);
#if UNITY_EDITOR
        myID = "me";
        Volt_PlayerData.instance.UserToken = myID;
        infoLog.text = "Welcome," + myID;
        PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
#else
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
#endif

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
            //Debug.Log(aToken.UserId);
            //foreach (var item in aToken.Permissions)
            //{
            //    Debug.Log(item);
            //}
            myID = aToken.UserId;
            Volt_PlayerData.instance.UserToken = myID;
            infoLog.text = "Welcome," + myID;
#if UNITY_ANDROID
            PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
#elif UNITY_IOS
            PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
#endif

        }
        else
        {
            loginProgressPanel.SetActive(false);
            Debug.Log("User cancelled login");
            DisplayLogin(false);
        }
    }
#endregion

    void Start()
    {
        if(ClientSocketModule.Instance)
            DontDestroyOnLoad(ClientSocketModule.Instance);

        if (Volt_SoundManager.S)
        {
            Volt_SoundManager.S.ChangeBGM((AudioClip)Resources.Load("BGMS/Lobby"));
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.InternetNonReachable);
        }
#if UNITY_ANDROID
        googleLoginBtn.SetActive(true);
        appleLoginBtn.SetActive(false);
#elif UNITY_IOS
        googleLoginBtn.SetActive(false);
        appleLoginBtn.SetActive(true);
        if(PlayerPrefs.HasKey("APPLE_SIGNIN"))
        {
            string userID = PlayerPrefs.GetString("APPLE_SIGNIN");
            //if (userID.Length > 40)
            //    Volt_PlayerData.instance.UserToken = userID.Substring(0, 40);
            //else
            //    Volt_PlayerData.instance.UserToken = userID;
            Volt_PlayerData.instance.UserToken = userID;
            var siwa = appleLoginBtn.GetComponent<SignInWithApple>();
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
                            PacketTransmission.SendSignInPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken);
                            break;
                        case UserCredentialState.NotFound:
                        case UserCredentialState.Revoked:
                            break;
                        default:
                            break;
                    }
                });
            siwa.GetCredentialState(userID);
        }
#endif
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitUI.activeSelf)
                exitUI.SetActive(false);
            else
                exitUI.SetActive(true);
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            networkErrorUI.SetActive(true);
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
    IEnumerator InternetConnectFailQuit()
    {
        //인터넷이 없다는 메시지 출력
       
        networkErrorUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        print("인터넷이 없어서 종료함.");
        Application.Quit();
    }
    
    public void OnPressCreateNewAccountConfirmBtn(GameObject info)
    {
        string nickName = info.GetComponent<UILabel>().text;

        if (nickName.Length > 12 || nickName.Length < 2)
        {
            accountCreateErrorPanel.SetActive(true);
            UILabel errorMsg = accountCreateErrorPanel.GetComponentInChildren<UILabel>();
            switch (Application.systemLanguage)
            {
                case SystemLanguage.French:
                    errorMsg.text = "Le nombre de caractères est limité à 2 ~ 12.";
                    break;
                case SystemLanguage.German:
                    errorMsg.text = "Die Anzahl der Buchstaben ist auf 2 ~ 12 begrenzt.";
                    break;
                case SystemLanguage.Korean:
                    errorMsg.text = "글자 수는 2 ~ 12자로 제한되어 있습니다.";
                    break;
                default:
                    errorMsg.text = "The number of characters is limited to 2 ~ 12.";
                    break;
            }
            //Debug.LogError("글자 수 제한 되어있음.");
            return;
        }

        for (int i = 0; i < nickName.Length; i++)
        {
            //bool containsAlphabet = false;

            if ((int)nickName[i] >= 65 && (int)nickName[i] <= 90)
            {
                //containsAlphabet = true;
                continue;
            }
            else if ((int)nickName[i] >= 97 && (int)nickName[i] <= 122)
            {
                //containsAlphabet = true;
                continue;
            }
            else if ((int)nickName[i] >= 48 && (int)nickName[i] <= 57)
                continue;
            else
            {
                accountCreateErrorPanel.SetActive(true);
                UILabel errorMsg = accountCreateErrorPanel.GetComponentInChildren<UILabel>();
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.French:
                        errorMsg.text = "Le surnom doit être composé d'une combinaison d'anglais et de chiffres.";
                        break;
                    case SystemLanguage.German:
                        errorMsg.text = "Der Nickname muss aus einer Kombination aus Englisch und Zahlen bestehen.";
                        break;
                    case SystemLanguage.Korean:
                        errorMsg.text = "닉네임은 영문과 숫자의 조합으로 이루어져야 합니다.";
                        break;
                    default:
                        errorMsg.text = "Nicknames must consist of a combination of English and numbers.";
                        break;
                }
                //Debug.LogError("아이디는 영문과 숫자로만 조합되어야함.");
                return;
            }
        }
        //Debug.Log("SignUp 조건 충족");
        PacketTransmission.SendSignUpPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken, nickName.Length, nickName);
    }
    public void OnPressStartBtn()
    {
        //startBtn.GetComponent<BoxCollider>().enabled = false;
        if (!sceneOver)
        {
            sceneOver = true;
            Volt_LoadingSceneManager.S.RequestLoadScene("Lobby2");
        }
    }
    public void OnPressCloseBtn(GameObject self)
    {
        self.SetActive(false);
    }
    public void OnPressExitBtn()
    {
        Application.Quit();
    }

   
    void SetActivePlatformPanel(bool b)
    {
        //facebookLoginBtn.SetActive(b);
        //googleLoginBtn.SetActive(b);
        selectLoginTypePanel.SetActive(b);
        //페이스북, 구글 로그인버튼 활성화.
    }
    
    public void OnPressedGoogleLoginBtn()
    {

        StartCoroutine(GoogleLoginProgress());
    }
    IEnumerator GoogleLoginProgress()
    {
#if UNITY_ANDROID
        loginProgressPanel.SetActive(true);
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
                    infoLog.text = googleEmail;
                    SetActivePlatformPanel(false);
                    Volt_PlayerData.instance.LoginType = LoginType.Google;
                    Volt_PlayerData.instance.UserToken = googleEmail;

                    PacketTransmission.SendSignInPacket(googleEmail.Length, googleEmail);

                }
                else
                {
                    //Debug.Log("Google Faild");
                    infoLog.text = "Google Failed";
                    loginProgressPanel.SetActive(false);
                    DisplayLogin(false);
                }
            });
        }
#else
        yield break;
#endif
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
        Volt_PlayerData.instance.UserToken = "";
        Volt_PlayerData.instance.LoginType = LoginType.None;
#endif

#if UNITY_IOS

#endif
        infoLog.text = "Logout";
        //add type
        Volt_PlayerData.instance.LoginType = LoginType.None;
        DisplayLogin(false);
    }
    public void OnPressdownTutorialSkipBtn() // tmp
    {
        PlayerPrefs.SetInt("Volt_TutorialDone", 1);
    }

    void DisplayLogin(bool on)
    {
        if (on)
        {
            SetActivePlatformPanel(false);
            startBtn.SetActive(true);
            logOutBtn.SetActive(true);
            loginBtn.SetActive(false);
        }
        else
        {
            SetActivePlatformPanel(true);
            startBtn.SetActive(false);
            logOutBtn.SetActive(false);
            loginBtn.SetActive(true);
            infoLog.text = "No login yet";
        }
    }
    public void ProgressLoadData()
    {
        if (resourceLoadProgressBar == null) return;
        resourceLoadProgressBar.value += (1f / 11f);
        if (resourceLoadProgressBar.value >= 0.99f)
        {
            resourceLoadProgressBar.gameObject.SetActive(false);
            DisplayLogin(true);
        }
    }
    public void DoReadyToStart()
    {
        createAccountUI.SetActive(false);
        resourceLoadProgressBar.gameObject.SetActive(true);
        selectLoginTypePanel.SetActive(false);
        loginBtn.SetActive(false);
        loginProgressPanel.SetActive(false);
    }
    public void OnClickLoginBtn()
    {
        DisplayLogin(false);
    }
    public void CloseLoginTypePanel()
    {
        SetActivePlatformPanel(false);
        startBtn.SetActive(false);
        logOutBtn.SetActive(false);
        loginBtn.SetActive(true);
    }
    
    //Tmp
    public void OnClickSignUpBtn(GameObject info)
    {
        string nickName = info.GetComponent<UILabel>().text;
        PacketTransmission.SendSignUpPacket(Volt_PlayerData.instance.UserToken.Length, Volt_PlayerData.instance.UserToken, nickName.Length, nickName);
    }

    public void OnClickSignInWithAppleBtn()
    {
        loginProgressPanel.SetActive(true);
        var siwa = appleLoginBtn.GetComponent<SignInWithApple>();
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
        //if (userId.Length > 40)
        //{
        //    userId = userId.Substring(0, 40);
        //}

        infoLog.text = userId;
        SetActivePlatformPanel(false);
        Volt_PlayerData.instance.LoginType = LoginType.Apple;
        Volt_PlayerData.instance.UserToken = userId;

        PlayerPrefs.SetString("APPLE_SIGNIN", userInfo.userId);
        Debug.Log($"Token:{userId}");
        PacketTransmission.SendSignInPacket(userId.Length, userId);
    }
    //public void SendTmpToken()
    //{
    //    PacketTransmission.SendSignUpPacket(tmpToken.Length, tmpToken, appleAccountNickname.Length, appleAccountNickname);
    //}
    //public void SendTmpTokenLogin()
    //{
    //    PacketTransmission.SendSignInPacket(3, "bbb");
    //}
}
