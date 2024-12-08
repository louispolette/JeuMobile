using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : PlatformObject
{
    [Space]

    [SerializeField] private float _jumpForce = 60f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            SoundManager.Instance.PlaySound("spring");
            player.Jump(_jumpForce);
            enabled = false;
        }
    }
}
