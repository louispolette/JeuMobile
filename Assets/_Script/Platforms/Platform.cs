using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnEnable()
    {
        CameraFollow.OnCameraMove += CheckIfOffscreen;
    }

    private void OnDisable()
    {
        CameraFollow.OnCameraMove -= CheckIfOffscreen;
    }

    public virtual void GetJumped()
    {

    }

    private void CheckIfOffscreen()
    {
        if (transform.position.y <= GameManager.Instance.CameraBottomBorderY - 1f)
        {
            Destroy(gameObject);
        }
    }
}
