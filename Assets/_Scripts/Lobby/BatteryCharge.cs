using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCharge : MonoBehaviour
{
    public class BatteryChargeTimer
    {
        public float Seconds { get; private set; }
        bool isStartTimer;
        public bool IsStartTimer {
            get { return isStartTimer; }
            private set
            {
                isStartTimer = value;
                BatteryCharge.isCharging = isStartTimer;
                //Debug.Log($"Is charging battery @ {BatteryCharge.isCharging}");
            }
        }

        public void Reset()
        {
            //Debug.Log("Rest Battery Charge Timer");
            Seconds = 0f;
            IsStartTimer = false;
        }

        public bool IsDone()
        {
            if (IsStartTimer && Seconds <= 0f)
                return true;
            return false;
        }

        public override string ToString()
        {
            int minute = (int)((Seconds + 1) / 60f);
            int second = (int)((Seconds + 1) % 60f);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(minute.ToString("D2"));
            builder.Append(":");
            builder.Append(second.ToString("D2"));
            return builder.ToString();
        }

        public void SetTime(float seconds)
        {
            this.Seconds = seconds;
        }

        public void StartTimer(float seconds, DateTime timerStartTime)
        {
            RegisterTime(timerStartTime);
            this.Seconds = seconds;
            IsStartTimer = true;
        }


        public void StartTimer(float seconds)
        {
            StartTimer(seconds, Volt.Time.GetGoogleDateTime());
        }

        public void Play()
        {
            this.Seconds -= Time.deltaTime;
        }

        public bool IsRun()
        {
            if (IsStartTimer)
                return true;
            return false;
        }
    }

    public static BatteryCharge instance;

    BatteryChargeTimer timer = new BatteryChargeTimer();
    public BatteryChargeTimer Timer { get { return timer; } }

    public static int maxBatteryCount = 5;
    [SerializeField]
    float delayPerChargeBattey = 20;
    [SerializeField]
    bool isLoadedLastTimerStartTime = false;

    public DateTime lastTimerStartTime = new DateTime();
    public static bool isCharging = false;
    bool isInit = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (Volt_PlayerData.instance.GetBatteryCount() >= maxBatteryCount)
            return;
        
        //Debug.LogWarning("Send battery time request packet");
        //마지막으로 타이머를 가동한 시간을 요청한다.
        SendBatteryTimeRequestPacket();
    }

    /// <summary>
    /// 서버에서 받아온 마지막 타이머 시작 시간과 현재 시간을 이용해
    /// 배터리 개수를 추가하고, 남은 시간으로 타이머를 돌린다.
    /// </summary>
    public void AddBatteryAndStartTimer()
    {
        if(lastTimerStartTime.Year == 1)
        {
            //Debug.LogWarning($"Warning wrong DateTime @ {lastTimerStartTime.ToString()}");
            return;
        }

        DateTime now = Volt.Time.GetGoogleDateTime();
        TimeSpan timeSpan = now - lastTimerStartTime;

        //Debug.LogWarning($"Now: {now.ToString()}, TimerStartTIme: {lastTimerStartTime.ToString()}");

        int batteryCount = Mathf.Abs((int)(timeSpan.TotalSeconds / delayPerChargeBattey));
        int remainSecond = (int)delayPerChargeBattey - Mathf.Abs((int)(timeSpan.TotalSeconds % delayPerChargeBattey));

        // 배터리를 추가하기 전 추가되는 양 + 현재 배터리 개수의 값이
        // 최대 충전 가능한 양보다 많으면 배터리 개수를 최대 개수까지만 추가되도록 조정한다.
        if (Volt_PlayerData.instance.GetBatteryCount() + batteryCount > maxBatteryCount)
        {
            batteryCount = maxBatteryCount - Volt_PlayerData.instance.GetBatteryCount();
        }
        Volt_PlayerData.instance.AddBattery(batteryCount);
        //Debug.LogWarning($"Battery count:{batteryCount}");
        //Debug.LogWarning($"Remain second {remainSecond}");
        
        // 플레이어의 현재 배터리 소지 개수가 최대 충전 개수보다 적으면 타이머 재생
        if (Volt_PlayerData.instance.GetBatteryCount() < maxBatteryCount)
        {
            // 현재 시간에서 3분 타이머에서 흐른 시간만큼을 되돌리면 타이머의 시작 시간이다(<- 뭔소리인지...)
            DateTime timerStartTime = now.AddSeconds(-Mathf.Abs((int)(timeSpan.TotalSeconds % delayPerChargeBattey)));
            // 현재 시간 - 마지막으로 타이머를 시작한 시간의 값에 배터리 1개당 대기해야하는 시간을
            // 나눈 나머지 값들은 타이머 시간으로 적용된다.
            if (timer.IsRun())
                timer.Reset();
            timer.StartTimer(remainSecond, timerStartTime);
            //Debug.LogWarning($"Time: {timer.ToString()}");
        }
        else
        {
            RegisterTime();
        }
    }
    
    /// <summary>
    /// 서버로부터 마지막 타이머 시작 시간을 받오면 발생하는 콜백함수
    /// </summary>
    public void OnLoadedLastTimerStartTime()
    {
        //Debug.LogError("OnLoadedLastTimerStartTime");
        isLoadedLastTimerStartTime = true;
        AddBatteryAndStartTimer();
        isRequestTime = false;
    }

    /// <summary>
    /// 서버로 현재 시간을 등록한다.
    /// </summary>
    public static void RegisterTime(DateTime timerStartTime)
    {
        if (isCharging == false)
        {
            DateTime curTime = timerStartTime;
            //Debug.LogWarning($"RegisterTime Year:{curTime.Year} Month:{curTime.Month} Day:{curTime.Day} Hour:{curTime.Hour} Minute:{curTime.Minute} Second:{curTime.Second}");
            PacketTransmission.SendBatteryTimeRegisterPacket(curTime.Year, curTime.Month, curTime.Day,
                curTime.Hour, curTime.Minute, curTime.Second);
            return;
        }
        //Debug.Log("Don't registertime because timer is already running");
    }
    
    public static void RegisterTime()
    {
        RegisterTime(Volt.Time.GetGoogleDateTime());
    }
    
    void Update()
    {
        if (isRequestTime)
            return;

        if (Volt_PlayerData.instance.GetBatteryCount() >= maxBatteryCount)
            return;

        if (timer.IsDone())
        {
            Volt_PlayerData.instance.OnChargedBattery();
        }
        else
        {
            if(timer.Seconds > 0f)
            {
                timer.Play();
                //timerLabel.text = timer.ToString();
                // Debug.Log($"Timer @ {timer.ToString()}");
            }
            else
            {
                timer.StartTimer(delayPerChargeBattey);
            }
        }
    }

    private bool isRequestTime = false;
    private void SendBatteryTimeRequestPacket()
    {
        isRequestTime = true;
        //PacketTransmission.SendBatteryTimeRequestPacket();
    }

    private void OnApplicationPause(bool pause)
    {
        // 화면을 내린 뒤 다시 올리면 서버로부터 타이머를 시작한 시간을 받아온다.
        // 이 후 콜백함수를 통해 자동으로 후처리를 한다.
        if(pause == false)
        {
            //SendBatteryTimeRequestPacket();
            //현재 클라가 갖고있는 시간과 로컬타임을 계산하여 타이머를 돌린다.
        }
    }
}
