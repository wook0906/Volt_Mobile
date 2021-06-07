using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/AttackData")]
public class AttackData : ScriptableObject
{
    public EffectData[] effectData;
    public AudioClip audioClip;
    public int damage;
    public CameraShakeType cameraShakeType;
}
