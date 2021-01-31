using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource audio;
    public bool muteAudio;
    private bool currentlyMuted;
    public List<AudioClip> Themes;

    public List<AudioClip> SoundEffects;

    [SerializeField] private float MasterVolume;

    public void MuteAudio(bool mute)
    {
        audio.mute = mute;
    }

    public void FixedUpdate()
    {
        if (muteAudio != currentlyMuted)
        {
            currentlyMuted = muteAudio;
            MuteAudio(currentlyMuted);
        }
    }

    public void playTheme(string theme)
    {
        switch (theme)
        {
            case "dungeon":
                audio.clip = Themes[0];
                break;
            case "title":
                audio.clip = Themes[1];
                break;
            case "combat":
                audio.clip = Themes[2];
                break;
            case "gameOver":
                audio.clip = Themes[3];
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
            case "crate":
                audio.PlayOneShot(SoundEffects[5]);
                break;
        }
    }

    public void setMasterVolume(float volume)
    {
        MasterVolume = volume;
        audio.volume = MasterVolume;
    }

}
