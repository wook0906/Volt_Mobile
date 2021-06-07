using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    etc, music
}
public class Volt_SoundController : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audio;
    public SoundType soundType;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<AudioSource>())
        {
            audio = GetComponent<AudioSource>();
        }
        if (audio && soundType == SoundType.etc)
        {
            if (PlayerPrefs.HasKey("Volt_SoundVolume"))
                audio.volume = PlayerPrefs.GetFloat("Volt_SoundVolume");
        }
        else if(audio && soundType == SoundType.music)
        {
            if (PlayerPrefs.HasKey("Volt_MusicVolume"))
                audio.volume = PlayerPrefs.GetFloat("Volt_MusicVolume");
        }
    }

}
