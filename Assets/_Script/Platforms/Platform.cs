using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Space]

    [SerializeField] private Transform _bonusParent;

    public Transform BonusParent => _bonusParent;

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
