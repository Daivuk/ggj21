﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public bool GamePaused;

    public AudioSource audio;
    public bool muteAudio;
    private bool currentlyMuted;
    public AudioClip CatTheme;
    public AudioClip dungeonTheme;

    public List<AudioClip> SoundEffects;

    [SerializeField] private float MasterVolume;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playTheme("title");
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    void Update()
    {
        if(muteAudio != currentlyMuted)
        {
            currentlyMuted = muteAudio;
            MuteAudio(currentlyMuted);
        }
    }

    public void MuteAudio(bool mute)
    {
        audio.mute = mute;
    }

    public void playTheme(string theme)
    {
        switch (theme)
        {
            case "dungeon":
                audio.clip = dungeonTheme;
                break;
            case "title":
                audio.clip = CatTheme;
                break;
            case "combat":
                break;
        }
        audio.volume = MasterVolume;
        audio.loop = true;
        audio.Play();
    }

    public void playSoundEffect(string soundEffect)
    {
        switch (soundEffect)
        {
            case "dash":
                audio.PlayOneShot(SoundEffects[0]);
                break;
            case "hit1":
                audio.PlayOneShot(SoundEffects[1]);
                break;
            case "hit2":
                audio.PlayOneShot(SoundEffects[2]);
                break;
            case "swing":
                audio.PlayOneShot(SoundEffects[3]);
                break;
            case "chime":
                audio.PlayOneShot(SoundEffects[4]);
                break;
        }
    }

    public void setMasterVolume(float volume)
    {
        MasterVolume = volume;
        audio.volume = MasterVolume;
    }

    public void UnPauseGame()
    {
        GamePaused = false;
        PlayerController.instance.LockedCharacter = false;
    }
    public void PauseGame()
    {
        GamePaused = true;
        PlayerController.instance.LockedCharacter = true;
    }
}