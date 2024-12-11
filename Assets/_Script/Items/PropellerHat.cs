using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerHat : PlatformObject
{
    [Space]

    [SerializeField] private PropellerPowerUpStats _stats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (player.IsUsingPropeller) return;

            player.UsePropellerHat(_stats);

            Destroy(gameObject);
        }
    }
}

[Serializable]
public struct PropellerPowerUpStats
{
    public float duration;
    public float targetVelocity;
    public float thrust;
}
