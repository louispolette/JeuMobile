using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    [Header("Platform Prefabs")]

    [SerializeField] private Platform _basicPlatform;
    [SerializeField] private Platform _breakablePlatform;
    [SerializeField] private Platform _movingPlatform;

    [Header("Platform Objects Prefabs")]

    [SerializeField] private PlatformObject _springPrefab;
    [SerializeField] private PlatformObject _propellerPrefab;

    [Header("Enemies Prefabs")]

    [SerializeField] private PlatformObject _ratPrefab;
    [SerializeField] private EnemyFlying _birdPrefab;

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

    [SerializeField, Min(0)] private float _springFrequency = 25f;
    [SerializeField, Min(0)] private float _springFrequencyVariation = 7.5f;

    [Space]

    [SerializeField, Min(0)] private float _propellerFrequency = 35f;
    [SerializeField, Min(0)] private float _propellerFrequencyVariation = 10f;

    [Space]

    [SerializeField, Min(0)] private float _ratFrequency = 40f;
    [SerializeField, Min(0)] private float _ratFrequencyVariation = 10f;

    [Space]

    [SerializeField, Min(0)] private float _birdFrequency = 50f;
    [SerializeField, Min(0)] private float _birdFrequencyVariation = 10f;

    private int _platformsSpawned;
    private float _heightUntilNextPlatform = 1f;
    private float _previousHeight;
    private int _nextBreakablePlatformNumber = 10;
    private int _nextMovingPlatformNumber = 40;
    private int _nextSpringNumber = 10;
    private int _nextPropellerNumber = 50;
    private int _nextBirdNumber = 80;
    private int _nextRatNumber = 60;

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
        _platformsSpawned++;

        Platform platformToSpawn = GetPlatformType();
        Vector3 randomPosition = new Vector3(Random.Range(-_spawnRange, _spawnRange), transform.position.y, transform.position.z);
        
        Platform platform = Instantiate(platformToSpawn, randomPosition, Quaternion.identity);

        PlatformObject platformObject = GetPlatformObject();

        if (platformObject != null)
        {
            Instantiate(platformObject, platform.BonusParent, false);
        }
    }

    private Platform GetPlatformType()
    {
        if (_platformsSpawned >= _nextBreakablePlatformNumber)
        {
            _nextBreakablePlatformNumber += Mathf.FloorToInt(_breakablePlatformFrequency
                                                             + Random.Range(-_breakablePlatformFrequencyVariation,
                                                                             _breakablePlatformFrequencyVariation));

            _heightUntilNextPlatform = _baseHeightBetweenPlatforms / 2f;

            return _breakablePlatform;
        }
        else if (_platformsSpawned >= _nextMovingPlatformNumber)
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

    private PlatformObject GetPlatformObject()
    {
        if (_platformsSpawned >= _nextSpringNumber)
        {
            _nextSpringNumber += Mathf.FloorToInt(_springFrequency
                                                  + Random.Range(-_springFrequencyVariation,
                                                                  _springFrequencyVariation));

            return _springPrefab;
        }
        else if (_platformsSpawned >= _nextPropellerNumber)
        {
            _nextPropellerNumber += Mathf.FloorToInt(_propellerFrequency
                                                     + Random.Range(-_propellerFrequencyVariation,
                                                                     _propellerFrequencyVariation));

            return _propellerPrefab;
        }
        else if (_platformsSpawned >= _nextRatNumber)
        {
            _nextRatNumber += Mathf.FloorToInt(_ratFrequency
                                               + Random.Range(-_ratFrequencyVariation,
                                                               _ratFrequencyVariation));
            return _ratPrefab;
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-_spawnRange, transform.position.y, 0f), new Vector3(_spawnRange, transform.position.y, 0f));
    }
}
