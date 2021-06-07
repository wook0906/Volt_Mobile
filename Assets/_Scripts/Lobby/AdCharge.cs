using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCharge : MonoBehaviour
{
    public static AdCharge instance;
    bool isTimerPlaying = false;
    [SerializeField]
    private float _remainSecond;
    public float remainSecond
    {
        get { return _remainSecond; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetTimer(float remainSecond)
    {
        this._remainSecond = remainSecond;
        isTimerPlaying = true;
    }


    private void Update()
    {
        if (!isTimerPlaying) return;

        _remainSecond -= Time.deltaTime;
        //Debug.Log(_remainSecond);
        if (remainSecond <= 0f)
        {
            isTimerPlaying = false;
            _remainSecond = 0f;
        }
    }

    public override string ToString()
    {
        int minute = (int)((remainSecond + 1) / 60f);
        int second = (int)((remainSecond + 1) % 60f);
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(minute.ToString("D2"));
        builder.Append(":");
        builder.Append(second.ToString("D2"));
        return builder.ToString();
    }

    private void OnApplicationPause(bool pause)
    {
        
    }
}