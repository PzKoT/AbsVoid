// Assets/Scripts/Systems/ObstacleSpawner.cs
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private int treeCount = 20;
    [SerializeField] private int bushCount = 30;
    [SerializeField] private float mapRadius = 40f;
    [SerializeField] private float minDistanceFromPlayer = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    private Transform player;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError($"Игрок с тегом 'Player' не найден для слайма {gameObject.name}!");
                enabled = false;
            }
        }
        if (!player) return;

        SpawnObstacles(treePrefabs, treeCount);
        SpawnObstacles(bushPrefabs, bushCount);
    }

    void SpawnObstacles(GameObject[] prefabs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetValidPosition();
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    Vector2 GetValidPosition()
    {
        Vector2 pos;
        int attempts = 0;
        do
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(10f, mapRadius);
            pos = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            attempts++;
        }
        while ((Physics2D.OverlapCircle(pos, 2f, obstacleLayer) || Vector2.Distance(pos, player.position) < minDistanceFromPlayer) && attempts < 50);

        return pos;
    }
}