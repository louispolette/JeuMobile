using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Space]

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 1.5f;

    private Rigidbody2D rb;

    private float _age;

    public float Speed { get =>  _speed; set => _speed = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = Vector3.up * _speed;
    }

    private void Update()
    {
        _age += Time.deltaTime;

        if (_age > _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
