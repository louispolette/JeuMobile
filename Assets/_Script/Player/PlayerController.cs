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

    private float Bounds => GameManager.Instance.HorizontalBounds;

    private Rigidbody2D _rb2d;
    private Camera _camera;
    private Coroutine _propellerHatCoroutine;

    private bool _isUsingPropeller = false;

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

        if (_rb2d.position.x > Bounds)
        {
            float extraDistance = Mathf.Abs(_rb2d.position.x - Bounds);
            _rb2d.position = new Vector2(-Bounds + extraDistance, _rb2d.position.y);
        }
        else if (_rb2d.position.x < -Bounds)
        {
            float extraDistance = Mathf.Abs(_rb2d.position.x - -Bounds);
            _rb2d.position = new Vector2(Bounds - extraDistance, _rb2d.position.y);
        }
    }

    public void Jump()
    {
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
        _rb2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound("jump");
    }

    public void Jump(float force)
    {
        float initialJumpForce = _jumpForce;

        _jumpForce = force;
        Jump();
        _jumpForce = initialJumpForce;
    }

    public void UsePropellerHat(PropellerPowerUpStats stats)
    {
        if (_propellerHatCoroutine != null)
        {
            StopCoroutine(_propellerHatCoroutine);
        }

        StartCoroutine(PropellerHatCoroutine(stats));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_rb2d.velocity.y > 0) return;

        var platform = collision.collider.GetComponent<Platform>();
        platform.GetJumped();

        _rb2d.velocity = Vector2.zero;
        Jump();
    }

    private IEnumerator PropellerHatCoroutine(PropellerPowerUpStats stats)
    {
        float timeSinceStart = 0f;

        _isUsingPropeller = true;

        while (timeSinceStart < stats.duration)
        {
            if (_rb2d.velocity.y < stats.targetVelocity)
            {
                float thrustForce;

                if (_rb2d.velocity.y < 0f)
                {
                    thrustForce = stats.thrust * 2f;
                }
                else
                {
                    thrustForce = stats.thrust;
                }

                _rb2d.AddForce(Vector2.up * thrustForce * Time.deltaTime, ForceMode2D.Force);
            }
            
            timeSinceStart += Time.deltaTime;
            yield return null;
        }

        _isUsingPropeller = false;
    }
}
