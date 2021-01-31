using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Popup : MonoBehaviour
{
    public SpriteRenderer itemRef;
    // public PlayableDirector director;

    float delay = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay < 0) GameObject.Destroy(gameObject);
    }
}
