using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Attacker : MonoBehaviour
{
    [HideInInspector] public UnityEvent HealthUpdated;
    [HideInInspector] public UnityEvent Died;
    public int AttackDamage;
    public float invulnerableCoolDown;
    private float currentInvulnerableCount;
    private bool invulnerable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invulnerable)
        {
            currentInvulnerableCount -= Time.deltaTime;
            if(currentInvulnerableCount < 0)
            {
                invulnerable = false;
            }
        }
    }

    public void DamageTarget(int damageAmount)
    {
        if(invulnerable == false)
        {
            this.GetComponent<Health>().currentHealth -= damageAmount;
            //this.GetComponent<Mover>().KnockBack(dir, knockBackStrength);
            HealthUpdated.Invoke();

            if(invulnerableCoolDown > 0)
            {
                invulnerable = true;
                currentInvulnerableCount = invulnerableCoolDown;
            }

            if(this.GetComponent<Health>().currentHealth <= 0)
            {
                Died.Invoke();
            }
        }
       
    }
}
