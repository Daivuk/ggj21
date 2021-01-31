using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class UpgradeInfoPopUp : MonoBehaviour
{

    public Text DisplayMessage;
    public PlayableDirector PlayableDirector;
    public PlayableAsset intro;

    public void SetUp(string text)
    {
        DisplayMessage.text = text;
    }
  
    public void FinishedAnimation()
    {
        Destroy(this.gameObject);
    }
}
