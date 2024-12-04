using System.Collections;
using TMPro;
using UnityEngine;

public class DropBomBossController : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab vụ nổ
    public GameObject warningPrefab; // Prefab cảnh báo

    // Tấn công
    public float explosionRadius = 5f; // Bán kính vụ nổ
    public int explosionCount = 10; // Số lượng vụ nổ
    public float delayBetweenExplosions = 0.2f; // Thời gian giữa các vụ nổ
    public float warningTime = 1f; // Thời gian cảnh báo trước khi nổ
    public int explosionDamage = 50; // Sát thương mỗi vụ nổ
    public float cooldownTime = 5f; // Thời gian hồi chiêu
    public Vector2 explosionSizeRange = new Vector2(0.5f, 2f); // Kích thước vụ nổ ngẫu nhiên (min, max)

    private Transform _playerTransform;
    protected void Start()
    {
        _playerTransform = PlayerController.Instance.transform;
    }

    public IEnumerator ActivateSkill()
    {

        // Tạo các vụ nổ xung quanh vị trí người chơi
        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * explosionRadius;
            Vector3 explosionPosition = _playerTransform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

            // Hiển thị cảnh báo trước khi phát nổ
            StartCoroutine(ShowWarningAndExplode(explosionPosition));

            yield return new WaitForSeconds(delayBetweenExplosions);
        }
    }

    private IEnumerator ShowWarningAndExplode(Vector3 position)
    {
        // Hiển thị cảnh báo
        GameObject warning = Instantiate(warningPrefab, position, Quaternion.identity);

        // Đợi thời gian cảnh báo
        yield return new WaitForSeconds(warningTime);

        // Tạo vụ nổ tại vị trí
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        // Random kích thước vụ nổ
        float randomSize = Random.Range(explosionSizeRange.x, explosionSizeRange.y);
        explosion.transform.localScale = new Vector3(randomSize, randomSize, 1);

        // Gây sát thương nếu người chơi trong vùng vụ nổ
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, randomSize / 2f);
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.transform == _playerTransform)
            {
                Debug.Log($"Player takes {explosionDamage} damage from explosion!");
                // Thêm logic gây sát thương cho người chơi tại đây
            }
        }

        // Hủy cảnh báo và vụ nổ
        Destroy(warning);
        Destroy(explosion, 2f);
    }
}
