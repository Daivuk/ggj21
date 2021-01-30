using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public List<HealthDisplay> healthDisplays;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplay()
    {
        Health health = PlayerController.instance.characterHealth;
        int alocatedHealth = health.currentHealth;
        /*
        foreach (HealthDisplay UI in HealthDisplay)
        {

            if (alocatedHealth >= 4)
            {
                UI.gameObject.SetActive(true);
                alocatedHealth -= 4;
            }  
        }
        */
    }
}
