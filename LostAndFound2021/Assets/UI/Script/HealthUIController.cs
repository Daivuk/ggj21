using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public List<HealthDisplay> healthDisplays;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < healthDisplays.Count; i++)
        {
            healthDisplays[i].Init();
        }

        UpdateDisplay();

        PlayerController.instance.getFocusObject().GetComponent<Attacker>().HealthUpdated.AddListener(() => UpdateDisplay());
    }

    public void UpdateDisplay()
    {
        Health health = PlayerController.instance.characterHealth;
        int alocatedHealth = health.currentHealth;
        int totalHealthCount = health.TotalHealth;
        for(int i= 0; i < healthDisplays.Count;i++)
        {
            if (alocatedHealth >= 4)
            {
                healthDisplays[i].gameObject.SetActive(true);
                healthDisplays[i].healthCount = 4;
                healthDisplays[i].UpdateImage();
                alocatedHealth -= 4;
                totalHealthCount -= 4;
            }
            else
            {
                if(alocatedHealth != 0)
                {
                    healthDisplays[i].gameObject.SetActive(true);
                    healthDisplays[i].healthCount = alocatedHealth;
                    healthDisplays[i].UpdateImage();
                   
                    totalHealthCount -= 4;
                    alocatedHealth = 0;
                }
                else
                {
                    if (totalHealthCount == 0)
                    {
                        healthDisplays[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        //show max health if it empty
                        healthDisplays[i].gameObject.SetActive(true);
                        totalHealthCount -= 4;
                        healthDisplays[i].healthCount = 0;
                        healthDisplays[i].UpdateImage();
                    }
                    
                }

            }  
        }
        
    }
}
