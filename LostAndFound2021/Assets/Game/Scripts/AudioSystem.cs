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
    public bool isPlayDungeonTheme()
    {
        if (audio.clip == null) return false;

        if (audio.clip.name == Themes[0].name || audio.clip.name == Themes[4].name) return true;

        return false;
    }
    public void playTheme(string theme)
    {
        if (theme == "dungeon" && GameHandler.instance.hasItemInInventory("Item18")) theme = "techno"; //item changes main theme

        bool playSong = false;

        //stop double play
        string audioClipName = "";
        if (audio.clip != null) audioClipName = audio.clip.name;

        switch (theme)
        {
            case "dungeon":
                if (audioClipName != Themes[0].name)
                {
                    audio.clip = Themes[0];
                    playSong = true;
                }
                break;
            case "title":
                if (audioClipName != Themes[1].name)
                {
                    audio.clip = Themes[1];
                    playSong = true;
                }
                break;
            case "win":
                audio.clip = Themes[2];
                playSong = true;
                break;
            case "gameOver":
                audio.clip = Themes[3];
                playSong = true;
                break;
            case "techno":
                if(audioClipName != Themes[4].name)
                {
                    audio.clip = Themes[4];
                    playSong = true;    
                }
                
                break;
        }
        audio.volume = MasterVolume;
        audio.loop = true;

        if(playSong) audio.Play();
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
            case "grabItem":
                audio.PlayOneShot(SoundEffects[6]);
                break;
            case "getItem":
                audio.PlayOneShot(SoundEffects[7]);
                break;
            case "upgrade":
                audio.PlayOneShot(SoundEffects[8]);
                break;
            case "keychain":
                audio.PlayOneShot(SoundEffects[9]);
                break;
            case "menu1":
                audio.PlayOneShot(SoundEffects[10]);
                break;
            case "menu2":
                audio.PlayOneShot(SoundEffects[11]);
                break;
            case "menu3":
                audio.PlayOneShot(SoundEffects[12]);
                break;
            case "step":
                audio.PlayOneShot(SoundEffects[13]);
                break;
            case "swim":
                audio.PlayOneShot(SoundEffects[14]);
                break;
            case "waterdrip":
                audio.PlayOneShot(SoundEffects[15]);
                break;
        }
    }

    public void setMasterVolume(float volume)
    {
        MasterVolume = volume;
        audio.volume = MasterVolume;
    }

}
