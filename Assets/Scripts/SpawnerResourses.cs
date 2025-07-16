using System.Collections;
using UnityEngine;

public class SpawnerResourses : MonoBehaviour
{
    public GameObject resourcePrefab;
    public float spawnInterval = 2f;
    public int maxAttempts = 10; // сколько раз пробовать найти свободное место при каждом спавне
    public int maxResources = 20;
    public Vector2 spawnAreaMin = new Vector2(-10, -10);
    public Vector2 spawnAreaMax = new Vector2(10, 10);
    public float checkRadius = 1f; // радиус проверки на столкновения

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Считаем только если лимит не достигнут
            //   if (GameObject.FindGameObjectsWithTag("Resource").Length < maxResources)
            TrySpawnResource();
        }
    }

    private void TrySpawnResource()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPos = GetRandomPosition();

            // Проверяем, нет ли коллайдеров вокруг точки спавна
            Collider[] colliders = Physics.OverlapSphere(spawnPos, checkRadius);
            if (colliders.Length == 0)
            {
                GameObject res = Instantiate(resourcePrefab, spawnPos, Quaternion.identity);
                //  res.tag = "Resource"; // убедись, что у префаба тоже стоит этот тег
                return;
            }
        }
        // Не удалось найти свободное место — не спавним
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float y = 1f; // если карта плоская
        return new Vector3(x, y, z);
    }
}