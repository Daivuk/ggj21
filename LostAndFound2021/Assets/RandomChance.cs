using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChance : MonoBehaviour
{
    public float Chances = 0.5f; // 50%

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // I know this is kind of hacky, but it allows me to quickly pick a chance of ANYTYHING in a room.
        // I wouldnt publish a product with this, but.. gamejam!
        if (Random.Range(0.0f, 1.0f) > Chances)
        {
            Destroy(gameObject);
        }
        Destroy(this);
    }
}
