using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Space]

    [SerializeField] private ControlMode _controlMode = ControlMode.Mobile;

    [SerializeField] private float _MouseSensitivity = 0.1f;
    [SerializeField] private float _gyroSensitivity = 0.1f;

    [Space]

    [SerializeField, Range(-1f, 1f)] private float _horizontalInput;

    [SerializeField] private float _movementSpeed = 1f;

    [Space]

    [SerializeField] private float _jumpForce = 5f;

    [Space]

    [SerializeField] private float _bounds = 3f;

    private Rigidbody2D _rb2d;
    private Camera _camera;

    private enum ControlMode
    {
        Mobile,
        Desktop
    }

    private void Awake()
    {
        _camera = Camera.main;
        Application.targetFrameRate = 60;
        _rb2d = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Locked;
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        GetHorizontalInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckBounds();
    }

    private void GetHorizontalInput()
    {
        switch (_controlMode)
        {
            case ControlMode.Mobile:
                GetGyroInput();
                break;
            case ControlMode.Desktop:
                GetMouseInput();
                break;
        }
    }

    private void GetMouseInput()
    {
        _horizontalInput = Mathf.Clamp(_horizontalInput + Input.GetAxis("Mouse X") * _MouseSensitivity, -1f, 1f);
    }

    private void GetGyroInput()
    {
        _horizontalInput = Input.acceleration.x;
    }

    private void HandleMovement()
    {
        Vector2 movement = Vector2.right * _horizontalInput * _movementSpeed;
        Vector2 newPosition = _rb2d.position + movement;

        _rb2d.position = new Vector2(newPosition.x, _rb2d.position.y);
    }

    private void CheckBounds()
    {
        if (_rb2d.position.y <= GameManager.Instance.CameraBottomBorderY)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (_rb2d.position.x > _bounds)
        {
            float extraDistance = Mathf.Abs(_rb2d.position.x - _bounds);
            _rb2d.position = new Vector2(-_bounds + extraDistance, _rb2d.position.y);
        }
        else if (_rb2d.position.x < -_bounds)
        {
            float extraDistance = Mathf.Abs(_rb2d.position.x - -_bounds);
            _rb2d.position = new Vector2(_bounds - extraDistance, _rb2d.position.y);
        }
    }

    private void Jump()
    {
        _rb2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_rb2d.velocity.y > 0) return;

        var platform = collision.collider.GetComponent<Platform>();
        platform.GetJumped();

        _rb2d.velocity = Vector2.zero;
        Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_bounds, transform.position.y + 5f, 0f), new Vector3(_bounds, transform.position.y - 5f, 0f));
        Gizmos.DrawLine(new Vector3(-_bounds, transform.position.y + 5f, 0f), new Vector3(-_bounds, transform.position.y - 5f, 0f));
    }
}
