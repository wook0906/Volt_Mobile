using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    public ParticleSystem particle;
    public AudioSource audioSource;
    public List<AudioClip> clips;
    private void Awake()
    {
        particle.Play();
        int ran = Random.Range(0, clips.Count - 1);
        audioSource.PlayOneShot(clips[ran]);
    }

    private void Update()
    {
        if (!particle.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }


}
