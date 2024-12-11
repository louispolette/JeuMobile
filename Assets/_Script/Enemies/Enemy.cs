using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnEnable()
    {
        CameraFollow.OnCameraMove += CheckIfOffscreen;
    }

    private void OnDisable()
    {
        CameraFollow.OnCameraMove -= CheckIfOffscreen;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void CheckIfOffscreen()
    {
        if (transform.position.y <= GameManager.Instance.CameraBottomBorderY - 3f)
        {
            Destroy(gameObject);
        }
    }

    public void GetHit()
    {
        Die();
    }

    public void HitPlayer(PlayerController player)
    {
        player.GetHit();
    }
}
