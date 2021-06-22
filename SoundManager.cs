using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        LightSaberConstant,
        LightSaberIgnite,
        LightSaberEnd
    }

    private static Dictionary<Sound, float> soundTimerDic;
    private static Dictionary<Sound, SoloAudio> soloSoundsDic;

    public static void SetUp()
    {
        soundTimerDic = new Dictionary<Sound, float>();
        soloSoundsDic = new Dictionary<Sound, SoloAudio>();
        for (int i = 0; i < Enum.GetValues(typeof(Sound)).Length; i++)
        {
            Sound sound = (Sound)i;
            switch (sound)
            {
                default:
                    continue;
                case Sound.LightSaberConstant:
                    soundTimerDic.Add(sound, 0);
                    break;
            }
        }
    }

    public static void PlaySound(Sound sound, Vector3? position = null, Transform parent = null)
    {
        if (IsSoloSound(sound) == true)
        {
            if (soloSoundsDic.ContainsKey(sound))
            {
                soloSoundsDic[sound].Play();
            }
            else
            {
                soloSoundsDic.Add(sound, new SoloAudio { audioSource = CreateSoundObj(), isPlay = true });
                soloSoundsDic[sound].audioSource.loop = true;
            }
            return;
        }

        if (position == null)
        {
            PlaySound(sound);
        }

        if (CanPlaySound(sound) == true)
        {
            GameObject obj = CreateSoundObj().gameObject;
            if(parent != null)
            {
                obj.transform.SetParent(parent);
            }
            obj.AddComponent<DestroySoundOnFinnish>();
        }

        AudioSource CreateSoundObj()
        {
            GameObject soundGameObject = new GameObject("Sound");
            if(position != null)
            {
                soundGameObject.transform.position = position.Value;
            }
            if (parent != null)
            {
                soundGameObject.transform.SetParent(parent);
            }
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
            return audioSource;
        }
    }
    
    private static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound) == true)
        {
            GameObject obj = CreateSoundObj().gameObject;
            obj.AddComponent<DestroySoundOnFinnish>();
        }

        AudioSource CreateSoundObj()
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
            return audioSource;
        }
    }

    public static void StopSound(Sound sound)
    {
        if(IsSoloSound(sound) == false)
        {
            Debug.Log("Cannot stop none solo sounds: calledfor: " + sound.ToString());
            return;
        }

        if (soloSoundsDic.ContainsKey(sound))
        {
            soloSoundsDic[sound].Stop();
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.LightSaberConstant:
                if (soundTimerDic.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDic[sound];
                    float playerMoveTimerMax = GetAudioClip(sound).length;
                    if(lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDic[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            default:
                return true;
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip audioClip in GameAssets.i.soundAudioClipArray)
        {
            if(audioClip.sound == sound)
            {
                return audioClip.audioClip;
            }
        }

        Debug.LogError(sound.ToString() + " not found in GameAssets.SoundAudioClipArray");
        return null;
    }

    private static bool IsSoloSound(Sound sound)
    {
        switch (sound)
        {
            case Sound.LightSaberConstant:
                return true;
            default:
                return false;
        }
    }

    public class SoloAudio
    {
        public AudioSource audioSource;
        public bool isPlay = true;

        public void Stop()
        {
            if(isPlay == true)
            {
                audioSource.Stop();
                isPlay = false;
            }
        }

        public void Play()
        {
            if(isPlay == false)
            {
                audioSource.Play();
                isPlay = true;
            }
        }
    }
}
