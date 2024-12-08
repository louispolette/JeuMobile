using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Space]

    [SerializeField] private float _riseHeight = 0.75f;

    [Space]

    [SerializeField] private PlayerController _player;

    private Camera _camera;
    
    public float Height => transform.position.y;

    public Camera Camera => _camera;

    private float RiseHeightWorld => _camera.ViewportToWorldPoint(new Vector3(0f, _riseHeight, 0f)).y;

    public delegate void CameraMove();
    public static event CameraMove OnCameraMove;

    public Vector2 PlayerViewportPosition
    {
        get
        {
            return _camera.WorldToViewportPoint(_player.transform.position);
        }
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (PlayerViewportPosition.y > _riseHeight)
        {
            float heightDifference = _player.transform.position.y - RiseHeightWorld;
            transform.position = new Vector3(transform.position.x, transform.position.y + heightDifference, transform.position.z);
            
            if (heightDifference > 0f)
            {
                OnCameraMove?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        _camera = GetComponent<Camera>();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-3f, RiseHeightWorld, 0f), new Vector3(3f, RiseHeightWorld, 0f));
    }
}
