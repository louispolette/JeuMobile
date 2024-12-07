using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Space]

    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerController _player;

    [Space]

    [SerializeField] private float _horizontalBounds = 3f;

    public Camera Camera => _camera;
    public PlayerController Player => _player;
    public float HorizontalBounds => _horizontalBounds;

    public float CameraBottomBorderY => _camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDrawGizmos()
    {
        if (_player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_horizontalBounds, _player.transform.position.y + 5f, 0f), new Vector3(_horizontalBounds, _player.transform.position.y - 5f, 0f));
        Gizmos.DrawLine(new Vector3(-_horizontalBounds, _player.transform.position.y + 5f, 0f), new Vector3(-_horizontalBounds, _player.transform.position.y - 5f, 0f));
    }
}
