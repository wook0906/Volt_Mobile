using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Volt_LoadingSceneManager : MonoBehaviour
{
    public static Volt_LoadingSceneManager S;
    
    public float fadeDuration;
    public ParticleSystem touchFeedbackEffect;
    //public UIProgressBar progressBar;
    public Stack<Scene> sceneStack;
    //public Image fadeScreen;
    public UIPanel fadeScreen;
    AsyncOperation async;
    public GameObject letterBoxPrefab;
    GameObject letterBox;
    public bool isWorking = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (S == null)
        {
            S = this;
            DontDestroyOnLoad(S);
        }
        else
            Destroy(this.gameObject);

        async = null;

        //if (letterBox == null)
        //{
        //    letterBox = Instantiate(letterBoxPrefab);
        //    DontDestroyOnLoad(letterBox);
        //}
        //else
        //    Destroy(letterBox);
    }
    void Start()
    {
        sceneStack = new Stack<Scene>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = transform.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            Instantiate(touchFeedbackEffect, pos, Quaternion.Euler(Vector3.zero),this.transform);
            PacketTransmission.SendInternetConnectionCheckPacket();
        }
    }
    public void RequestLoadScene(string newSceneName, float delayTime = 0f)
    {
        if (async != null || isWorking)
        {
            //print("RequestLoadScene Deny!");
            return;
        }
        isWorking = true;
        StartCoroutine(SceneChangeStart(newSceneName,delayTime));
    }
    IEnumerator SceneChangeStart(string newSceneName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(SceneLoadAsync(newSceneName));
    }
    IEnumerator SceneLoadAsync(string newSceneName)
    {
        if (!sceneStack.Contains(SceneManager.GetActiveScene()))
            sceneStack.Push(SceneManager.GetActiveScene());
        yield return StartCoroutine(FadeOut());
        async = SceneManager.LoadSceneAsync("LoadingScene");
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            yield return null;
        }
        
        yield return StartCoroutine(FadeIn());

        //if (newSceneName == "GameScene")
        //    letterBox.SetActive(false);
        //else
        //    letterBox.SetActive(true);

        StartCoroutine(AsyncLoadNewScene(newSceneName));
    }
    IEnumerator AsyncLoadNewScene(string newSceneName)
    {
        async = SceneManager.LoadSceneAsync(newSceneName);
        async.allowSceneActivation = false;
        UIProgressBar bar = GameObject.Find("ProgressBarBG").GetComponent<UIProgressBar>();
        
        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                bar.value = 1;
                yield return StartCoroutine(FadeOut());
                async.allowSceneActivation = true;
            }
            else
            {
                bar.value = async.progress;
                yield return null;
            }
        }
        yield return StartCoroutine(FadeIn());
        async = null;
        isWorking = false;
    }
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeDuration) {
            timer += Time.fixedDeltaTime;
            //fadeScreen.color = Color.Lerp(fadeScreen.color, Color.clear, Time.fixedDeltaTime*5f);
            fadeScreen.alpha = Mathf.Lerp(fadeScreen.alpha, 0, Time.fixedDeltaTime * 5f);
            yield return null;
        }
        fadeScreen.alpha = 0;
        fadeScreen.gameObject.SetActive(false);
    }
    IEnumerator FadeOut()
    {
        fadeScreen.gameObject.SetActive(true);
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.fixedDeltaTime;
            fadeScreen.alpha = Mathf.Lerp(fadeScreen.alpha, 1, Time.fixedDeltaTime*5f);
            yield return null;
        }
        fadeScreen.alpha = 1;
        
    }
}
