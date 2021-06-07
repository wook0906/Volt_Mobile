using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_Fade : MonoBehaviour
{
    public GameObject heidelLogo;
    public GameObject gfsUboLogo;
    [SerializeField]
    private int waitTime = 2;
    [SerializeField]
    private float fadeSpeed = 1f;
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width / 16 * 9, false);
    }
    void Start()
    {
        //StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ////IEnumerator StartScene()
    ////{
    ////    //print("Start");
    ////    Color c = heidelLogo.GetComponent<SpriteRenderer>().color;
    ////    while (heidelLogo.GetComponent<SpriteRenderer>().color.a <= 0.98f) {
    ////        c.a += Time.fixedDeltaTime * fadeSpeed;
    ////        heidelLogo.GetComponent<SpriteRenderer>().color = c;
    ////        //print("1");
    ////        yield return new WaitForFixedUpdate();
    ////    }
    ////    c.a = 1f;
    ////    heidelLogo.GetComponent<SpriteRenderer>().color = c;
    ////    print("2");
    ////    yield return new WaitForSeconds(waitTime);
    ////    while (heidelLogo.GetComponent<SpriteRenderer>().color.a >= 0.02f)
    ////    {
    ////        c.a -= Time.fixedDeltaTime * fadeSpeed;
    ////        heidelLogo.GetComponent<SpriteRenderer>().color = c;
    ////        print("3");
    ////        yield return new WaitForFixedUpdate();
    ////    }
    ////    c.a = 0f;
    ////    heidelLogo.GetComponent<SpriteRenderer>().color = c;
    ////    print("4");
    ////    yield return new WaitForSeconds(0.5f);

    ////    c = gfsUboLogo.GetComponent<SpriteRenderer>().color;
    ////    while (gfsUboLogo.GetComponent<SpriteRenderer>().color.a <= 0.98f)
    ////    {
    ////        c.a += Time.fixedDeltaTime * fadeSpeed;
    ////        gfsUboLogo.GetComponent<SpriteRenderer>().color = c;
    ////        print("5");
    ////        yield return new WaitForFixedUpdate();
    ////    }
    ////    c.a = 1f;
    ////    gfsUboLogo.GetComponent<SpriteRenderer>().color = c;
    ////    print("6");
    ////    yield return new WaitForSeconds(waitTime);
    ////    while (gfsUboLogo.GetComponent<SpriteRenderer>().color.a >= 0.02f)
    ////    {
    ////        c.a -= Time.fixedDeltaTime * fadeSpeed;
    ////        gfsUboLogo.GetComponent<SpriteRenderer>().color = c;
    ////        print("7");
    ////        yield return new WaitForFixedUpdate();
    ////    }
    ////    c.a = 0f;
    ////    gfsUboLogo.GetComponent<SpriteRenderer>().color = c;
    ////    yield break;
    ////    //UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    ////}
}
