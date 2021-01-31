﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXAnim : MonoBehaviour
{
    float delay = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay < 0) GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
