using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_LabelLanguage : MonoBehaviour
{
    public bool useDelayedTranslate = false;

    [TextArea]
    public string kor;
    [TextArea]
    public string eng;
    [TextArea]
    public string german;
    [TextArea]
    public string french;

    private UILabel label;
    // Start is called before the first frame update
    private void Awake()
    {
        label = GetComponent<UILabel>();
    }
    void Start()
    {
        if (useDelayedTranslate)
            Invoke("Init", 0.2f);
        else
            Init();
    }
    public void Init()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                label.text = french;
                break;
            case SystemLanguage.German:
                label.text = german;
                break;
            case SystemLanguage.Korean:
                label.text = kor;
                break;
            default:
                label.text = eng;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
