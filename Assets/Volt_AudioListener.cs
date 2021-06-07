using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Volt_AudioListener : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //DontDestroyOnLoad(this);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("#######################OnSceneLoaded");
        check = false;
    }

    private float elapsedTime = 0f;
    private float delay = 5f;
    private bool check = false;
    private void Update()
    {
        if (check)
            return;

        if(elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            return;
        }

        UnityEngine.Object[] objects = GameObject.FindObjectsOfType<AudioListener>();
        for (int i = 0; i < objects.Length; i++)
        {
            AudioListener listener = objects[i] as AudioListener;
            if (listener.gameObject == gameObject)
                continue;

            //Debug.Log($"Name: {listener.name} destroied audiolistener");
            Destroy(listener);
        }
        check = true;
    }
}
