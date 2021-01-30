using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    public int healthCount;
    public int totalHealthCount;
    public Image Image;

    public List<Sprite> HeartPeaces;

    private bool AnimateShine;
    public float shineCycle;

    public List<Sprite> ShineFrames;

    private int shineIndex;
    private float count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimateShine)
        {
            count -= Time.deltaTime;
            if(count < 0)
            {
                count = shineCycle;
                shineIndex++;

                if(shineIndex >= ShineFrames.Count)
                {
                    shineIndex = 0;
                }
                Image.sprite = HeartPeaces[shineIndex];
            }
        }
    }

    public void UpdateImage()
    {
        if(healthCount < HeartPeaces.Count)
        {
            Image.sprite = HeartPeaces[healthCount];
        }
        else
        {
            if (AnimateShine == false)
            {
                count = shineCycle;
                shineIndex = 0;
            }
            AnimateShine = true;
             
        }

    }
}
