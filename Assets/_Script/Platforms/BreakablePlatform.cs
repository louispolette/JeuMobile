using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : Platform
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.velocity.y < 0f)
        {
            Break();
        }
    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
