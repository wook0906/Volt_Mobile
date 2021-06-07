using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_FlowText : MonoBehaviour
{
    [SerializeField]
    bool isPlayTyping = false;
    [SerializeField]
    bool isTouched = false;
    [TextArea]
    public List<string> korTexts;
    [TextArea]
    public List<string> engTexts;
    [TextArea]
    public List<string> gerTexts;
    [TextArea]
    public List<string> frenTexts;

    private List<string> texts;

    public UILabel label;
    public Volt_TutorialContents contents;
    int idx = 0;
    int touchCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        label = GetComponentInChildren<UILabel>();
        contents = transform.parent.GetComponent<Volt_TutorialContents>();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                texts = frenTexts;
                break;
            case SystemLanguage.German:
                texts = gerTexts;
                break;
            case SystemLanguage.Korean:
                texts = korTexts;
                break;
            default:
                texts = engTexts;
                break;
        }
        if (texts.Count == 0) return;
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        if (isPlayTyping) yield break;
        isPlayTyping = true;
        //GetComponent<UIButton>().enabled = false;
        if (texts[idx].Length >= 0)
        {
            for (int i = 0; i < texts[idx].Length; i++)
            {
                if (texts[idx].IndexOf('[', i) == i)
                {
                    i = texts[idx].IndexOf(']', i) + 1;
                }
                yield return new WaitForSeconds(Volt_TutorialManager.S.textFlowSpeed);
                if (isTouched)
                {
                    label.text = texts[idx];
                    isPlayTyping = false;
                    //GetComponent<UIButton>().enabled = true;
                    yield break;
                }
                else
                    label.text = texts[idx].Substring(0, i);
            }
            label.text = texts[idx];
        }
        isPlayTyping = false;
        //GetComponent<UIButton>().enabled = true;
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPressedNextBtn()
    {
        if (isPlayTyping)
        {
            //Debug.Log("왜?@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            isTouched = true;
            return;
        }
        else
        {
            GoNext();
        }
    }
    void GoNext()
    {
        idx++;
        if (idx < texts.Count)
        {
            
            isTouched = false;
            StartCoroutine(ShowText());
        }
        else
        {
            Exit();
        }
    }
    public void Exit()
    {
        this.gameObject.SetActive(false);
        contents.HideBlockPanel();
        contents.DoneCallback();
    }
}
