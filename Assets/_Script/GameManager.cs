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

    public Camera Camera => _camera;
    public PlayerController Player => _player;

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
}
