using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab của enemy
    public float spawnRate = 1.5f; // Tần suất spawn
    public float spawnRadius = 5f; // Bán kính spawn
    private float spawnTimer = 0f; // Bộ đếm thời gian spawn

    // Start is called before the first frame update
    void Start()
    {
        // Nếu cần, có thể khởi tạo gì đó ở đây
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra thời gian spawn
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnEnemy();
            spawnTimer = 0f; // Reset bộ đếm thời gian
        }
    }

    void SpawnEnemy()
    {
        // Tạo một vị trí ngẫu nhiên trong bán kính spawn
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = transform.position.y; // Đảm bảo enemy spawn trên cùng mặt đất

        // Spawn enemy tại vị trí đã tính
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // Vẽ gizmos khi đối tượng được chọn trong editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // Đặt màu của gizmo
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Vẽ vòng tròn wireframe tại vị trí spawner
    }
}

