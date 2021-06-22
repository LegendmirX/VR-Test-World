using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour 
{
    [Space]
    [Header("Sounds")]
    public SoundAudioClip[] soundAudioClipArray;

    public static GameAssets i;

    public void SetUp()
    {
        i = this;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
