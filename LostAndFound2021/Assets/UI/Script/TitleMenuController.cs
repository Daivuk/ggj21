using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class TitleMenuController : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset intro;
    public PlayableAsset outro;
    public Slider BGMSlider;
    public Text MusicMuteText;
    public Text VolumeAmount;
    private bool activeMenu;
    // Start is called before the first frame update
    public void Awake()
    {
        activeMenu = true; //introing at the begining
        
    }
    void Start()
    {
        BGMSlider.value = GameHandler.instance.audioSystem.audio.volume;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        OutroMenu();
    }
    public void MuteButtonClicked()
    {
        GameHandler.instance.audioSystem.muteAudio = !GameHandler.instance.audioSystem.muteAudio;
        UpdateUI();
    }
    public void changeVolume()
    {
        GameHandler.instance.audioSystem.setMasterVolume(BGMSlider.value);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (GameHandler.instance.audioSystem.muteAudio) MusicMuteText.text = "Unmute audio";
        if (GameHandler.instance.audioSystem.muteAudio == false) MusicMuteText.text = "Mute audio";

        VolumeAmount.text =  ((int)(GameHandler.instance.audioSystem.audio.volume * 100)).ToString();
    }

    public void IntroMenu()
    {
        GameHandler.instance.audioSystem.playSoundEffect("menu2");
        activeMenu = true;
        director.Play(intro);
    }
    public void OutroMenu()
    {
        GameHandler.instance.audioSystem.playSoundEffect("menu3");
        activeMenu = false;
        director.Play(outro);
    }

    public void FinishedAnimation()
    {
        if (activeMenu == false)
        {
            GameHandler.instance.UnPauseGame();
            Destroy(this.gameObject);
        }
    }
}
