using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] private float _pathLength = 2f;
    [SerializeField] private float _speed = 1f;

    private Vector3 _leftEnd;
    private Vector3 _rightEnd;
    
    private float _positionOnPath = 0.5f;
    private bool _endPointsInitialized = false;
    private bool _goRight;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _goRight = Random.Range(0, 2) == 1;
        _positionOnPath = Random.Range(0f, 1f);
        UpdatePathPosition();
    }

    private void Start()
    {
        SetEndPoints();
    }

    private void FixedUpdate()
    {
        if (_goRight)
        {
            _positionOnPath += _speed * 0.01f;
        }
        else
        {
            _positionOnPath -= _speed * 0.01f;
        }

        if (_positionOnPath >= 1f)
        {
            _positionOnPath = 1f;
            _goRight = false;
        }
        else if (_positionOnPath <= 0f)
        {
            _positionOnPath = 0f;
            _goRight = true;
        }

        UpdatePathPosition();
    }

    private void SetEndPoints()
    {
        _leftEnd = transform.position + Vector3.left * _pathLength / 2;
        _rightEnd = transform.position + Vector3.right * _pathLength / 2;

        float boundsExtent = GameManager.Instance.HorizontalBounds;

        if (_leftEnd.x < -boundsExtent)
        {
            float extraDistance = Mathf.Abs(_leftEnd.x + boundsExtent);
            _leftEnd += Vector3.right * extraDistance;
            _rightEnd += Vector3.right * extraDistance;
        }
        else if(_rightEnd.x > boundsExtent)
        {
            float extraDistance = Mathf.Abs(_rightEnd.x - boundsExtent);
            _leftEnd += Vector3.left * extraDistance;
            _rightEnd += Vector3.left * extraDistance;
        }

        _endPointsInitialized = true;
    }

    private void UpdatePathPosition()
    {
        Vector2 newPosition = Vector2.Lerp(_leftEnd, _rightEnd, _positionOnPath);
        _rb.MovePosition(newPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        if (_endPointsInitialized)
        {
            Gizmos.DrawLine(_leftEnd, _rightEnd);
        }
        else
        {
            Gizmos.DrawLine(transform.position + Vector3.left * _pathLength / 2f, transform.position + Vector3.right * _pathLength / 2f);
        }
    }
}
