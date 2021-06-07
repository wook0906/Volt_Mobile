using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Volt_SafeInt
{
    public static int defaultValue = Random.Range(0, 50000);
    public static int SetValue(int inputValue)
    {
        int safeValue = inputValue + defaultValue;
        return safeValue;
    }
    public static int UnpackValue(int safeValue)
    {
        return safeValue - defaultValue;
    }
}
