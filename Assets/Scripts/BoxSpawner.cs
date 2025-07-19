using System;
using System.Collections;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private PoolBoxes _poolBoxes;
    [SerializeField] private Box _prefabBox;

    [SerializeField] private float _spawnInterval = 1f;

    private SpawnArea _spawnArea = new ();

    private int _maxAttempts = 10;
    private float _checkRadius = 1f;

    private readonly Collider[] _overlapResults = new Collider[16];

    public event Action<Vector3> BoxCreated;

    private void Awake()
    {
        _poolBoxes.Init(_prefabBox);
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_spawnInterval);

            TrySpawnResource();
        }
    }

    private void TrySpawnResource()
    {
        for (int attempt = 0; attempt < _maxAttempts; attempt++)
        {
            Vector3 spawnPos = _spawnArea.GetRandomPosition();
            int count = Physics.OverlapSphereNonAlloc(spawnPos, _checkRadius, _overlapResults);

            if (count == 0)
            {
                Box box = _poolBoxes.GetInstance();
                box.Init(spawnPos);
                BoxCreated?.Invoke(spawnPos);

                return;
            }
        }
    }
}