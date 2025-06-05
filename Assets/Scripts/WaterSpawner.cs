using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterPrefab;
    public float baseSpawnRate = 0.1f;
    public float spawnRateVariance = 0.05f; // how much to vary
    public float xOffsetRange = 0.5f; // how far from center

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWater();
            ScheduleNextSpawn();
        }
    }

    void SpawnWater()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x += Random.Range(-xOffsetRange, xOffsetRange);
        Instantiate(waterPrefab, spawnPos, Quaternion.identity);
    }

    void ScheduleNextSpawn()
    {
        float variance = Random.Range(-spawnRateVariance, spawnRateVariance);
        nextSpawnTime = Time.time + Mathf.Max(0.01f, baseSpawnRate + variance);
    }
}