using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu(menuName = "Effect/EffectData")]
public class EffectData : ScriptableObject
{
    public Effects      effectType;
    public string       launchPointPath;
   
    public float scaleMultValue = 1;
}
