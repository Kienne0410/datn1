using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform player; // Người chơi, đối tượng mà boss sẽ theo đuổi
    [SerializeField] private float moveSpeed = 3f; // Tốc độ di chuyển của boss
    [SerializeField] private float chaseRange = 5f; // Khoảng cách để boss bắt đầu đuổi theo người chơi
    [SerializeField] private float attackRange = 1f; // Khoảng cách để boss tấn công người chơi
    [SerializeField] private float patrolRange = 10f; // Phạm vi tuần tra quanh boss spawn (đơn vị: mét)
    [SerializeField] private float timeBetweenPatrols = 2f; // Thời gian chờ trước khi boss tuần tra điểm mới

    private Vector3 patrolTarget; // Vị trí tuần tra ngẫu nhiên
    private Animator animator;
    private bool isPatrolling = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRandomPatrolTarget(); // Lấy mục tiêu tuần tra ngẫu nhiên ngay từ đầu
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Nếu boss gần người chơi, tấn công
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // Nếu boss đủ gần người chơi, bắt đầu đuổi theo
            ChasePlayer();
        }
        else
        {
            // Nếu không gần người chơi, boss sẽ tuần tra
            if (!isPatrolling)
            {
                StartCoroutine(Patrol()); // Nếu boss chưa đang tuần tra, bắt đầu tuần tra
            }
        }
    }

    private void ChasePlayer()
    {
        // Di chuyển boss về phía người chơi
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        animator.SetTrigger("Chasing"); // Gọi trigger chasing trong Animator
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("Attack_1"); // Gọi trigger Attack_1 trong Animator khi boss tấn công
    }

    private IEnumerator Patrol()
    {
        isPatrolling = true;

        // Di chuyển boss tới mục tiêu tuần tra
        while (Vector3.Distance(transform.position, patrolTarget) > 0.2f)
        {
            Vector3 direction = (patrolTarget - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            animator.SetTrigger("Move"); // Gọi animation Move trong Animator khi boss đang tuần tra

            yield return null; // Chờ đến frame tiếp theo
        }

        // Sau khi boss đến điểm tuần tra, chờ và tạo mục tiêu tuần tra mới
        yield return new WaitForSeconds(timeBetweenPatrols);
        SetRandomPatrolTarget(); // Tạo vị trí tuần tra mới
        isPatrolling = false;
    }

    private void SetRandomPatrolTarget()
    {
        // Tạo mục tiêu tuần tra ngẫu nhiên trong phạm vi xung quanh vị trí của boss
        Vector3 randomDirection = Random.insideUnitCircle * patrolRange;
        patrolTarget = new Vector3(transform.position.x + randomDirection.x, transform.position.y + randomDirection.y, transform.position.z);
    }
}
