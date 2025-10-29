using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] bushPrefabs;

    [Header("Settings")]
    [SerializeField] private int treeCount = 20;
    [SerializeField] private int bushCount = 30;
    [SerializeField] private float mapRadius = 40f;
    [SerializeField] private float minDistanceFromPlayer = 5f;
    [SerializeField] private LayerMask obstacleLayer = -1;   // ��

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private Transform player;

    private void Start()
    {
        Debug.Log("=== ObstacleSpawner: ������ ===");

        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("����� � ����� 'Player' �� ������!");
            enabled = false;
            return;
        }

        Debug.Log($"����� ������: {player.position}");

        // ---------- �������� PREFABS ----------
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("TreePrefabs ������ ������!");
            return;
        }
        if (bushPrefabs == null || bushPrefabs.Length == 0)
        {
            Debug.LogError("BushPrefabs ������ ������!");
            return;
        }

        Debug.Log($"Prefabs OK: Trees={treePrefabs.Length}, Bushes={bushPrefabs.Length}");

        SpawnObstacles(treePrefabs, treeCount, "Tree");
        SpawnObstacles(bushPrefabs, bushCount, "Bush");

        Debug.Log("=== ��������� ��������� ===");
    }

    // ---------- ������ ----------
    private void SpawnObstacles(GameObject[] prefabs, int count, string type)
    {
        Debug.Log($"���������� {count} {type}...");

        int spawned = 0;
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetValidPosition();
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

            GameObject obj = Instantiate(prefab, (Vector3)pos, Quaternion.identity);
            obj.name = $"{type}_{spawned + 1}";
            spawned++;

            if (showDebug)
                Debug.Log($"[{type}_{spawned}] �������: {pos}");
        }

        Debug.Log($"������� ������� {spawned} {type}");
    }

    // ---------- ����� ������� ----------
    private Vector2 GetValidPosition()
    {
        Vector2 pos;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minDistanceFromPlayer, mapRadius);

            // (Vector2)player.position + ������ �����������
            pos = (Vector2)player.position + new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance);

            // �������� �����������
            bool overlap = Physics2D.OverlapCircle(pos, 2f, obstacleLayer) != null;
            attempts++;

            if (showDebug && attempts % 10 == 0)
                Debug.Log($"������� {attempts}: {(overlap ? "Overlap" : "OK")}");

        } while (Physics2D.OverlapCircle(pos, 2f, obstacleLayer) != null && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("�� ������� ����� �������� ������� ����� 100 �������!");
            return (Vector2)player.position + Vector2.up * mapRadius; // ��������
        }

        return pos;
    }

    // ---------- ������� � SCENE VIEW ----------
    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minDistanceFromPlayer);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, mapRadius);
    }
}