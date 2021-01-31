using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {        
        GameHandler.instance.ShowYouWinScreen();
    }
}
