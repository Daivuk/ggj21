using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
//       oops, lower case
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.attacker.Heal(1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnAnimationDone()
    {
        Destroy(gameObject);
    }
}
