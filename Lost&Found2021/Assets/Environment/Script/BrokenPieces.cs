using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    public float lifeTime = 2f;
    [HideInInspector] public SpriteRenderer renderer;
    public float fadeOutSpeed = 1.5f;
    // Start is called before the first frame update
    public void setUp(Sprite shard, float strength)
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = shard;
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(Random.Range(-strength, strength), Random.Range(-strength, strength));
        rigidbody2D.AddForceAtPosition(force, transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        //transform.position += moveDirection * Time.deltaTime;
        //moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deacceleration);

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.MoveTowards(1, 0, fadeOutSpeed));
            if (renderer.color.a == 0f)
            {
                Destroy(gameObject);
            }

        }
    }
}
