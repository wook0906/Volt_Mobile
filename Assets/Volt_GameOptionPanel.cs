using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_GameOptionPanel : MonoBehaviour
{
    public GameObject resetAccountPopupGO;
    public UISlider soundSlider;
    public UISlider musicSlider;
    public UIToggle frameRateControlToggle;
    public GameObject cheatActiveToggle;

    bool isToggleInit = false;

    bool isCheatModeOn = true;
    public bool IsCheatModeOn
    {
        get { return isCheatModeOn; }
        set { isCheatModeOn = value; }
    }

    public void Init(bool isOn60Frame, float musicVolume, float soundVolume)
    {
        //Debug.Log(gameObject.name + " Init!");
        if(frameRateControlToggle)
            frameRateControlToggle.Set(isOn60Frame);
        musicSlider.Set(musicVolume);
        soundSlider.Set(soundVolume);
        musicSlider.Start();
        soundSlider.Start();
        musicSlider.gameObject.SetActive(false);
        soundSlider.gameObject.SetActive(false);
        musicSlider.gameObject.SetActive(true);
        soundSlider.gameObject.SetActive(true);
        //Invoke("LateInit", 0.1f);
    }

    private void Awake()
    {
     //frameRateControlToggle.optionCanBeNone   
    }
    //private void OnEnable()
    //{
    //    if (Volt_UILayerManager.instance)
    //        Volt_UILayerManager.instance.Enqueue(gameObject);
    //    Debug.Log("OnEnabled");
    //}


    public void OnValueChanged(GameObject slider)
    {
        if (!Volt_SoundManager.S)
            return;

        if (slider.name == "Sound_Slider")
        {
            PlayerPrefs.SetFloat("Volt_SoundVolume", slider.GetComponent<UISlider>().value);
            foreach (var item in FindObjectsOfType<UIPlaySound>())
            {
                item.volume = PlayerPrefs.GetFloat("Volt_SoundVolume");
            }
            Volt_SoundManager.S.OnChangedSoundVolume(slider.GetComponent<UISlider>().value);
        }
        else
        {
            PlayerPrefs.SetFloat("Volt_MusicVolume", slider.GetComponent<UISlider>().value);
            Volt_SoundManager.S.OnChangedMusicVolume(slider.GetComponent<UISlider>().value);
        }
    }
    public void OnToggle60FrameMode()
    {
        if (!isToggleInit)
        {
            isToggleInit = true;
            return;
        }

        //Debug.Log("60 frame Toggle!");
        if (GameOption.S.IsOn60Frame)
            GameOption.S.IsOn60Frame = false;
        else
            GameOption.S.IsOn60Frame = true;
    }
    public void OnToggleCheat()
    {
        if (IsCheatModeOn)
        {
            IsCheatModeOn = false;
            Volt_GMUI.S.cheatBtn.GetComponent<UIRect>().alpha = 0f;
        }
        else
        {
            IsCheatModeOn = true;
            Volt_GMUI.S.cheatBtn.GetComponent<UIRect>().alpha = 1f;
        }
    }
}
