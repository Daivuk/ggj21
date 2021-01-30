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

    public Sprite fullHeartImage;
    public float precentageOfShine;
    public float TimeBeforeAsking;
    private float currentCount;
    private bool fullHearts;

    private bool AcitveShine;
    public List<Sprite> Shine;
    public float shineFrameSpeed;
    private float currentShineFrameSpeed;
    private int shineFrameIndex;

    public Animator animator;
    // Update is called once per frame

    public void Init()
    {
        AcitveShine = false;
        fullHearts = false;

        /*
        if (healthCount == totalHealthCount)
        {
            fullHearts = true;
        }
        */
    }

    void FixedUpdate()
    {
        if(AcitveShine == false && fullHearts)
        {
            currentCount -= Time.deltaTime;
            if (currentCount < 0)
            {
                float random = Random.Range(0.0f, 1.0f);
                if (fullHearts && random <= precentageOfShine)
                {
                    AcitveShine = true;
                    shineFrameIndex = 0;
                }
                currentCount = TimeBeforeAsking;
            }
        }
        else if(fullHearts && AcitveShine)
        {
            currentShineFrameSpeed -= Time.deltaTime;
            if(currentShineFrameSpeed < 0)
            {
                currentShineFrameSpeed = shineFrameSpeed;
                shineFrameIndex++;

                if(shineFrameIndex < Shine.Count)
                {
                    Image.sprite = Shine[shineFrameIndex];
                }
                else
                {
                    shineFrameIndex = 0;
                    AcitveShine = false;
                }
            }
        }
       
       
    }

    private void AnimateHeartShine()
    {
        animator.SetTrigger("shine");
    }

    public void UpdateImage()
    {
        if(healthCount < HeartPeaces.Count)
        {
           // animator.StopPlayback();
            fullHearts = false;
            if(healthCount > -1) Image.sprite = HeartPeaces[healthCount];
        }
        else
        {
           // animator.StartPlayback();
            Image.sprite = fullHeartImage;
            fullHearts = true;
        }

    }
}
