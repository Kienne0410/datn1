using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab vụ nổ
    public GameObject warningPrefab; // Prefab cảnh báo
    public Transform player; // Đối tượng người chơi

    // Tấn công
    public float explosionRadius = 5f; // Bán kính vụ nổ
    public int explosionCount = 10; // Số lượng vụ nổ
    public float delayBetweenExplosions = 0.2f; // Thời gian giữa các vụ nổ
    public float warningTime = 1f; // Thời gian cảnh báo trước khi nổ
    public int explosionDamage = 50; // Sát thương mỗi vụ nổ
    public float cooldownTime = 5f; // Thời gian hồi chiêu
    public Vector2 explosionSizeRange = new Vector2(0.5f, 2f); // Kích thước vụ nổ ngẫu nhiên (min, max)

    // Di chuyển
    public float moveSpeed = 3f; // Tốc độ di chuyển
    public float chaseRange = 8f; // Phạm vi đuổi theo người chơi
    public float attackRange = 5f; // Phạm vi để bắt đầu tấn công
    public float stopDistance = 2f; // Khoảng cách dừng trước người chơi

    private bool isAttacking = false;
    private bool isCooldown = false;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking && !isCooldown)
        {
            // Tấn công nếu người chơi trong phạm vi
            StartCoroutine(ActivateSkill());
        }
        else if (distanceToPlayer <= chaseRange && distanceToPlayer > stopDistance)
        {
            // Đuổi theo người chơi nếu trong phạm vi đuổi và ngoài phạm vi tấn công
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Xoay Boss hướng về phía người chơi
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private IEnumerator ActivateSkill()
    {
        isAttacking = true;
        isCooldown = true;

        // Tạo các vụ nổ xung quanh vị trí người chơi
        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * explosionRadius;
            Vector3 explosionPosition = player.position + new Vector3(randomPoint.x, randomPoint.y, 0);

            // Hiển thị cảnh báo trước khi phát nổ
            StartCoroutine(ShowWarningAndExplode(explosionPosition));

            yield return new WaitForSeconds(delayBetweenExplosions);
        }

        isAttacking = false;

        // Hồi chiêu
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
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
            if (hit.transform == player)
            {
                Debug.Log($"Player takes {explosionDamage} damage from explosion!");
                // Thêm logic gây sát thương cho người chơi tại đây
            }
        }

        // Hủy cảnh báo và vụ nổ
        Destroy(warning);
        Destroy(explosion, 2f);
    }

    private void OnDrawGizmos()
    {
        // Vẽ phạm vi đuổi theo và tấn công trong chế độ Scene
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, explosionRadius);
    }
}
