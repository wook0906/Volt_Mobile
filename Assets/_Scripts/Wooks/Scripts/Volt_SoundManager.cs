using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Volt_SoundManager : MonoBehaviour
{
    public static Volt_SoundManager S;
    public AudioSource bgm;
    public AudioSource environmentSound;
    public AudioSource audioSourcePrefab;
    public List<AudioSource> sounds;
    public float musicVolume;
    public float soundVolume;

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
    }
    
    public void OnChangedMusicVolume(float value)
    {
        musicVolume = value * 0.55f;
        bgm.volume = musicVolume;
        environmentSound.volume = musicVolume;
        GameOption.S.OnChangedMusicVolume(value);
    }
    public void OnChangedSoundVolume(float value)
    {
        soundVolume = value;
        GameOption.S.OnChangedSoundVolume(value);
        foreach (var item in sounds)
        {
            item.volume = soundVolume;
        }
    }

    public void ChangeBGM(MapType mapType)
    {
        if(bgm.clip != null)
        {
            bgm.Stop();
            Managers.Resource.Release<AudioClip>(bgm.clip);
            bgm.clip = null;
        }
        if(environmentSound.clip != null)
        {
            environmentSound.Stop();
            Managers.Resource.Release<AudioClip>(environmentSound.clip);
            environmentSound.clip = null;
        }

        switch (mapType)
        {
            case MapType.Tutorial:
            case MapType.TwinCity:
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/BGMS/TwinCity.wav",
                    (result) =>
                    {
                        bgm.clip = result.Result;
                        bgm.Play();
                    });
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/BackgroundSound/twincity_Background.mp3",
                    (result) =>
                    {
                        environmentSound.clip = result.Result;
                        environmentSound.Play();
                    });
                break;
            case MapType.Rome:
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/BGMS/Rome.wav",
                    (result) =>
                    {
                        bgm.clip = result.Result;
                        bgm.Play();
                    });
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/BackgroundSound/roma_Background.mp3",
                    (result) =>
                    {
                        environmentSound.clip = result.Result;
                        environmentSound.Play();
                    });
                break;
            case MapType.Ruhrgebiet:
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/BGMS/Ruhrgebiet.wav",
                    (result) =>
                    {
                        bgm.clip = result.Result;
                        bgm.Play();
                    });
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/BackgroundSound/factory_Background.mp3",
                    (result) =>
                    {
                        environmentSound.clip = result.Result;
                        environmentSound.Play();
                    });
                break;
            case MapType.Tokyo:
                Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/BGMS/Tokyo.wav",
                    (result) =>
                    {
                        bgm.clip = result.Result;
                        bgm.Play();
                    });
                break;
            default:
                break;
        }
    }
    public void ChangeBGM(AudioClip newClip)
    {
        if (bgm.clip != null)
        {
            bgm.Stop();
            Managers.Resource.Release<AudioClip>(bgm.clip);
            bgm.clip = null;
        }
        if (environmentSound.clip != null)
        {
            environmentSound.Stop();
            Managers.Resource.Release<AudioClip>(environmentSound.clip);
            environmentSound.clip = null;
        }
        bgm.clip = newClip;
        bgm.Play();
    }
    public void RequestSoundPlay(AudioClip clip, bool isLoop, float delayTime = 0f)
    {
        if (delayTime != 0f)
        {
            AudioSource audioInstance = Instantiate(audioSourcePrefab);
            audioInstance.loop = isLoop;
            audioInstance.clip = clip;
            audioInstance.volume = soundVolume;
            audioInstance.Play();
            sounds.Add(audioInstance);
        }
        else
        {
            StartCoroutine(DelayedSoundPlay(clip, isLoop, delayTime));
        }
    }
    IEnumerator DelayedSoundPlay(AudioClip clip, bool isLoop, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioSource audioInstance = Instantiate(audioSourcePrefab);
        audioInstance.loop = isLoop;
        audioInstance.clip = clip;
        audioInstance.volume = soundVolume;
        audioInstance.Play();
        sounds.Add(audioInstance);
    }
}
