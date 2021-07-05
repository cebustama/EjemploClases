using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Audio Clips")]
public class PlayerAudioclips : ScriptableObject
{
    public AudioClip attack;
    public AudioClip receiveDamage;
    public AudioClip die;
}
