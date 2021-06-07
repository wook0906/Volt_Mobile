using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_AudioAutoDestroy : MonoBehaviour
{
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    void LateUpdate()
    {
        if (!audio.isPlaying)
        {
            Volt_SoundManager.S.sounds.Remove(audio);
            Managers.Resource.Release<AudioClip>(audio.clip);
            Destroy(gameObject);
        }
    }
}
