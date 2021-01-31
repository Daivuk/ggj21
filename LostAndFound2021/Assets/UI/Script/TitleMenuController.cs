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
        BGMSlider.value = GameHandler.instance.audio.volume;
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
        GameHandler.instance.muteAudio = !GameHandler.instance.muteAudio;
        UpdateUI();
    }
    public void changeVolume()
    {
        GameHandler.instance.setMasterVolume(BGMSlider.value);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (GameHandler.instance.muteAudio) MusicMuteText.text = "Unmute audio";
        if (GameHandler.instance.muteAudio == false) MusicMuteText.text = "Mute audio";

        VolumeAmount.text =  ((int)(GameHandler.instance.audio.volume * 100)).ToString();
    }

    public void IntroMenu()
    {
        activeMenu = true;
        director.Play(intro);
    }
    public void OutroMenu()
    {
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
