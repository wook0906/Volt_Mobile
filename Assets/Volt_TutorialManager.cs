using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Volt_TutorialManager : MonoBehaviour
{
    public static Volt_TutorialManager S;
   
    public List<Volt_TutorialContents> tutorials;
    public Volt_TutorialContents curContents;
    int idx = 0;
    [HideInInspector]
    public bool isShowingTutorial = false;
    public float textFlowSpeed = 0.05f;
    private void Awake()
    {
        S = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //transform.SetParent(Managers.UI.Root.transform);
        //transform.localPosition = Vector3.zero;
    }
    public void TutorialStart(int order)
    {
        idx = order;
        tutorials[idx].gameObject.SetActive(true);
        curContents = tutorials[idx];
        isShowingTutorial = true;
    }
    public void TutorialStart(string name)
    {
        Volt_TutorialContents content = FindContentsByName(name);
        content.gameObject.SetActive(true);
        isShowingTutorial = true;
        curContents = content;
        idx = GetContentIndex(content);
    }
    int GetContentIndex(Volt_TutorialContents content)
    {
        int idx = 0;
        foreach (var item in tutorials)
        {
            if (item == content)
                return idx;
            idx++;
        }
        Debug.Log("Error");
        return -1;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public Volt_TutorialContents FindContentsByName(string name)
    {
        foreach (var item in tutorials)
        {
            if (item.name == name)
                return item;
        }
        Debug.Log("Error");
        return null;
    }
    public void DoNextTutorial()
    {
        isShowingTutorial = true;
        tutorials[idx].gameObject.SetActive(false);
        idx++;
        if (idx < tutorials.Count)
        {
            tutorials[idx].gameObject.SetActive(true);
            curContents = tutorials[idx];
        }
        else
        {
            TutorialOver();
        }
    }
    public void TutorialOver()
    {
    
        Destroy(this.gameObject);
    }

}
