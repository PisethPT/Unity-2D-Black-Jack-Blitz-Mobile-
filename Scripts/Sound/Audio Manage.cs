using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManage : MonoBehaviour
{
    public SoundManage[] MusicSound, SFXSound;
    public AudioSource MusicSource, SFXSource;
    public static AudioManage Ins;
    public Button MusicBtnNo, SFXBtnOn;
    public Sprite[] buttonImage;
    private bool MusicBntIsOn, SFXBtnIsOn;
    //  public Sprite musicImgOn, musicImgOff, sfxImgOn, sfxImgOff;

    // Awake()
    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // background sound manage
    public void StartPlayMusic(string name)
    {
        SoundManage sound = Array.Find(MusicSound, x => x.soundName == name);
        if (sound == null) Debug.Log("Sound not found.");
        else
        {
            MusicSource.clip = sound.AudioClip;
            MusicSource.Play();
        }
    }
    // sfx sounds manage
    public void StartPlaySFX(string name)
    {
        SoundManage sound = Array.Find(SFXSound, x => x.soundName == name);
        if (sound == null) Debug.Log("SFX not found");
        else SFXSource.PlayOneShot(sound.AudioClip);
    }

    // Toggle Background sound controll
    public void toggleBackgroundSound()
    {
        if (MusicBntIsOn)
        {
            MusicBtnNo.image.sprite = buttonImage[0];
            MusicBntIsOn = false;
            MusicSource.mute = true;
        }
        else
        {
            MusicBtnNo.image.sprite = buttonImage[1];
            MusicBntIsOn = true;
            MusicSource.mute = false;
        }
    }

    // Toggle SFX sound controll
    public void toggleSFXSound()
    {
        if (SFXBtnIsOn)
        {
            SFXBtnOn.image.sprite = buttonImage[3];
            SFXBtnIsOn = false;
            SFXSource.mute = true;
        }
        else
        {
            SFXBtnOn.image.sprite = buttonImage[2];
            SFXBtnIsOn = true;
            SFXSource.mute = false;
        }
    }
    
}
