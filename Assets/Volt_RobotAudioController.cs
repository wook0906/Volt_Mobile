using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_RobotAudioController : MonoBehaviour
{
    //tmp
    public AudioClip attack;
    public AudioClip movement;
    public AudioClip dodge;
    public AudioClip death;
    public AudioClip victory;
    public AudioClip lose;
    public AudioClip hit;
    public AudioClip stun;
    public AudioClip select;
    public AudioClip guard;
    public AudioClip doubleAttack;
    //tmpEnd
    AudioSource audio;
    private void Awake()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetDefaultVolume()
    {
        audio.volume = PlayerPrefs.GetFloat("Volt_SoundVolume");
    }
    public void SetVolumeMult(float mult)
    {
        audio.volume = PlayerPrefs.GetFloat("Volt_SoundVolume");
        audio.volume *= mult;
    }
    public void PlayOneShot(AudioClip clip)
    {
        SetDefaultVolume();
        audio.PlayOneShot(clip);
    }
    public void PlayOneShot(AudioClip clip,float VolumeMult)
    {
        SetVolumeMult(VolumeMult);
        audio.PlayOneShot(clip);
    }
    public void ModelSoundPlay(string clipName)
    {
        SetDefaultVolume();
        switch (clipName)
        {
            case "attack":
                audio.PlayOneShot(attack);
                break;
            case "movement":
                audio.PlayOneShot(movement);
                break;
            case "dodge":
                audio.PlayOneShot(dodge);
                break;
            case "death":
                audio.PlayOneShot(death);
                break;
            case "victory":
            case "win":
                audio.PlayOneShot(victory);
                break;
            case "lose":
                audio.PlayOneShot(lose);
                break;
            case "hit":
                audio.PlayOneShot(hit);
                break;
            case "stun":
                audio.PlayOneShot(stun);
                break;
            case "select":
                audio.PlayOneShot(select);
                break;
            case "guard":
                audio.PlayOneShot(guard);
                break;
            case "doubleAttack":
                audio.PlayOneShot(doubleAttack);
                break;
            default:
                break;
        }
    }


}
