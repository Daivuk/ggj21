using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTileLogic : MonoBehaviour
{
    public int damageAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Attacker>().DamageTarget(this.transform.position, damageAmount);
        }
        if(collision.gameObject.tag == "box")
        {

        }
    }
}
