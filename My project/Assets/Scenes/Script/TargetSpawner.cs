using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameConfigSO config;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private float spawnTimer;

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnTarget();
            spawnTimer = Random.Range(config.MinSpawnInterval, config.MaxSpawnInterval);
        }
    }

    private void SpawnTarget()
    {
        if (spawnPoints.Length == 0) return;
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject obj = Instantiate(targetPrefab, point.position, Quaternion.identity);

        // Target 이름 그대로 사용
        Target targetScript = obj.GetComponent<Target>();
        if (targetScript != null)
        {
            targetScript.Initialize(config);
        }
    }
}