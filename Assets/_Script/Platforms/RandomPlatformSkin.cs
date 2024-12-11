using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatformSkin : MonoBehaviour
{
    [Space]

    [SerializeField] private Sprite _altSprite;
    [SerializeField, Range(0f, 1f)] private float _altSpriteChance;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();

        if (Random.Range(0f, 1f) <= _altSpriteChance)
        {
            _renderer.sprite = _altSprite;
        }

        if (Random.Range(0, 2) == 1)
        {
            _renderer.flipX = true;
        }
    }
}
