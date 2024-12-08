using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Space]

    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private PlayerController _player;
    [SerializeField] private Text _scoreText;

    [Space]

    [SerializeField] private float _horizontalBounds = 3f;

    public float Score { get; set; }

    public Camera Camera => _cameraFollow.Camera;
    public PlayerController Player => _player;
    public float HorizontalBounds => _horizontalBounds;

    public float CameraBottomBorderY => _cameraFollow.Camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;

    private void OnEnable()
    {
        CameraFollow.OnCameraMove += UpdateScore;
    }

    private void OnDisable()
    {
        CameraFollow.OnCameraMove -= UpdateScore;
    }

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

    private void UpdateScore()
    {
        Score = _cameraFollow.Height * 20f;
        _scoreText.text = Mathf.Floor(Score).ToString();
    }

    private void OnDrawGizmos()
    {
        if (_player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_horizontalBounds, _player.transform.position.y + 5f, 0f), new Vector3(_horizontalBounds, _player.transform.position.y - 5f, 0f));
        Gizmos.DrawLine(new Vector3(-_horizontalBounds, _player.transform.position.y + 5f, 0f), new Vector3(-_horizontalBounds, _player.transform.position.y - 5f, 0f));
    }
}
