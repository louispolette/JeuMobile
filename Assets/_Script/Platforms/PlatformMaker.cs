using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    [Header("Platform Prefabs")]

    [SerializeField] private Platform _basicPlatform;
    [SerializeField] private Platform _breakablePlatform;
    [SerializeField] private Platform _movingPlatform;

    [Header("Settings")]

    [SerializeField] private float _maxDifficultyHeight = 100f;

    [Space]

    [SerializeField] private float _spawnRange = 3f;
    [SerializeField] private float _baseHeightBetweenPlatforms = 2f;
    [SerializeField, Min(0f)] private float _heightBetweenPlatformsVariation = 0.25f;
    [SerializeField] private AnimationCurve _platformSpawnDistanceOverTime;

    [Space]

    [SerializeField, Min(0)] private float _breakablePlatformFrequency = 10f;
    [SerializeField, Min(0)] private float _breakablePlatformFrequencyVariation = 2.5f;

    [Space]

    [SerializeField, Min(0)] private float _movingPlatformFrequency = 15f;
    [SerializeField, Min(0)] private float _movingPlatformFrequencyVariation = 2.5f;

    [Space]

    [SerializeField] private float _currentCoef;

    private int _platformsSpawned;
    private float _heightUntilNextPlatform = 1f;
    private float _previousHeight;
    private int _nextBreakablePlatformNumber = 10;
    private int _nextMovingPlatformNumber = 40;

    private float CurrentPlatformDistanceCoef => _platformSpawnDistanceOverTime.Evaluate(transform.position.y / _maxDifficultyHeight);

    private void Awake()
    {
        _previousHeight = transform.position.y;
    }

    private void OnEnable()
    {
        CameraFollow.OnCameraMove += OnPositionUpdated;
    }

    private void OnDisable()
    {
        CameraFollow.OnCameraMove -= OnPositionUpdated;
    }

    private void OnPositionUpdated()
    {
        _heightUntilNextPlatform -= transform.position.y - _previousHeight;

        if (_heightUntilNextPlatform <= 0f)
        {
            float nextHeight = GetNextSpawnHeight();
            _heightUntilNextPlatform = nextHeight;

            MakePlatform();
        }

        _currentCoef = CurrentPlatformDistanceCoef;

        _previousHeight = transform.position.y;
    }

    private float GetNextSpawnHeight()
    {
        float nextHeight = _baseHeightBetweenPlatforms * CurrentPlatformDistanceCoef;
        nextHeight += Random.Range(-_heightBetweenPlatformsVariation, _heightBetweenPlatformsVariation);

        return nextHeight;
    }

    public void MakePlatform()
    {
        Platform platformToSpawn = GetPlatformType();
        Vector3 randomPosition = new Vector3(Random.Range(-_spawnRange, _spawnRange), transform.position.y, transform.position.z);
        
        Platform platform = Instantiate(platformToSpawn, randomPosition, Quaternion.identity);
        _platformsSpawned++;
    }

    private Platform GetPlatformType()
    {
        int nextSpawnNumber = _platformsSpawned + 1;

        if (nextSpawnNumber >= _nextBreakablePlatformNumber)
        {
            _nextBreakablePlatformNumber += Mathf.FloorToInt(_breakablePlatformFrequency
                                                             + Random.Range(-_breakablePlatformFrequencyVariation,
                                                                             _breakablePlatformFrequencyVariation));

            _heightUntilNextPlatform = _baseHeightBetweenPlatforms / 2f;

            return _breakablePlatform;
        }
        else if (nextSpawnNumber >= _nextMovingPlatformNumber)
        {
            _nextMovingPlatformNumber += Mathf.FloorToInt(_movingPlatformFrequency
                                                             + Random.Range(-_movingPlatformFrequencyVariation,
                                                                             _movingPlatformFrequencyVariation));

            return _movingPlatform;
        }
        else
        {
            return _basicPlatform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-_spawnRange, transform.position.y, 0f), new Vector3(_spawnRange, transform.position.y, 0f));
    }
}
