using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSprite : MonoBehaviour
{    public void OnAnimationDone()
    {
        transform.parent.gameObject.GetComponent<heart>().OnAnimationDone(); // Dont ask...
    }
}
