using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOption : MonoBehaviour
{
    public static GameOption S;

    bool isOn60Frame = true;
    public bool IsOn60Frame
    {
        get { return isOn60Frame; }
        set
        {
            isOn60Frame = value;
            //if (isOn60Frame)
            //{
            //    Application.targetFrameRate = 60;
            //    PlayerPrefs.SetInt("Volt_FrameRateInfo", 60);
            //}
            //else
            //{
            //    Application.targetFrameRate = 30;
            //    PlayerPrefs.SetInt("Volt_FrameRateInfo", 30);
            //}
        }
    }

    float musicVolume;
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("Volt_MusicVolume", musicVolume);
        }
    }

    float soundVolume;
    public float SoundVolume
    {
        get { return soundVolume; }
        set
        {
            soundVolume = value;
            PlayerPrefs.SetFloat("Volt_SoundVolume", soundVolume);
        }
    }

    private void Awake()
    {
        if (S == null)
        {
            DontDestroyOnLoad(this.gameObject);
            S = this;
        }
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
        //if (PlayerPrefs.HasKey("Volt_FrameRateInfo"))
        //{
        //    if (PlayerPrefs.GetInt("Volt_FrameRateInfo") == 60)
        //        IsOn60Frame = true;
        //    else
        //        IsOn60Frame = false;
        //}
        //else
        //    IsOn60Frame = false;

        if (PlayerPrefs.HasKey("Volt_SoundVolume"))
            SoundVolume = PlayerPrefs.GetFloat("Volt_SoundVolume");
        else
            SoundVolume = 1f;

        if (PlayerPrefs.HasKey("Volt_MusicVolume"))
            MusicVolume = PlayerPrefs.GetFloat("Volt_MusicVolume");
        else
            MusicVolume = 1f;

        SceneManager.sceneLoaded += SceneChanged;
    }
    public void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene Changed!");
        Volt_SoundManager.S.sounds.Clear();
        foreach (var item in FindOptionPanel())
        {
            //Debug.Log("@@@@@@@@@@@@@@@@@@@@@@패널 찾아썽!");
            item.Init(IsOn60Frame,MusicVolume,SoundVolume);
        }
    }
    Volt_GameOptionPanel[] FindOptionPanel()
    {
        //Debug.Log("Find Option Panel!");
        return Resources.FindObjectsOfTypeAll<Volt_GameOptionPanel>();
    }
    public void OnChangedMusicVolume(float value)
    {
        MusicVolume = value;
    }
    public void OnChangedSoundVolume(float value)
    {
        soundVolume = value;
    }
    public void OnChangedFrameRate(bool on)
    {
        IsOn60Frame = on;
    }

}
