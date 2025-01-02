using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ExplosionSkill : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab vụ nổ
    public GameObject warningPrefab; // Prefab cảnh báo
    public float radius = 10f; // Bán kính vòng tròn
    public int explosionCount = 10; // Số vụ nổ
    public float delayBetweenExplosions = 0.2f; // Thời gian giữa các vụ nổ
    public float warningTime = 1f; // Thời gian cảnh báo trước khi nổ
    public int damage = 100; // Sát thương mỗi vụ nổ
    public float cooldownTime = 5f; // Thời gian hồi chiêu
    public Vector2 explosionSizeRange = new Vector2(0.5f, 2.0f); // Kích thước vụ nổ ngẫu nhiên (min, max)

    public Image cooldownOverlay; // Image hiển thị cooldown (làm tối)
    public Button skillButton; // Nút bấm để kích hoạt kỹ năng
    public AudioClip explosionSkillSound; // Âm thanh khi kích hoạt kỹ năng
    public AudioClip warningSound; // Âm thanh cảnh báo
    public AudioClip explosionSound; // Âm thanh vụ nổ

    private AudioSource audioSource; // AudioSource để phát âm thanh
    private bool isSkillActive = false;
    private bool isCooldown = false; // Trạng thái hồi chiêu
    private float currentCooldownTime = 0f; // Thời gian còn lại của hồi chiêu
    private PlayerControls _playerControls;
    private Action<InputAction.CallbackContext> activeSkillAction;

    private void Awake()
    {
        _playerControls = InputManager.Instance.playerControls; 
        activeSkillAction = _ => OnActivateSkill();
        _playerControls.Combat.ActivateSkill.performed += activeSkillAction;
    }
    private void Start()
    {
        // Lấy AudioSource từ đối tượng
        audioSource = GetComponent<AudioSource>();
        

        // Đảm bảo Image ban đầu đầy (tối hoàn toàn)
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0; // Lớp phủ tắt ban đầu
        }

        if (skillButton != null)
        {
            skillButton.interactable = true; // Bật nút kỹ năng
        }
    }

    private void OnDestroy()
    {
        _playerControls.Combat.ActivateSkill.performed -= activeSkillAction;
    }

    public void OnActivateSkill()
    {
        if (!isSkillActive && !isCooldown)
        {
            // Phát âm thanh khi sử dụng kỹ năng
            if (explosionSkillSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSkillSound);
            }

            StartCoroutine(ActivateSkill());
        }
    }

    private IEnumerator ActivateSkill()
    {
        isSkillActive = true;
        isCooldown = true;
        currentCooldownTime = cooldownTime;

        if (skillButton != null)
        {
            skillButton.interactable = false; // Tắt nút trong lúc hồi chiêu
        }

        // Phát nổ các vị trí
        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 explosionPosition = transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

            // Hiển thị cảnh báo trước khi phát nổ
            StartCoroutine(ShowWarningAndExplode(explosionPosition));

            yield return new WaitForSeconds(delayBetweenExplosions);
        }

        isSkillActive = false;

        // Bắt đầu cooldown
        StartCoroutine(Cooldown());
    }

    private IEnumerator ShowWarningAndExplode(Vector3 position)
    {
        // Hiển thị cảnh báo
        GameObject warning = Instantiate(warningPrefab, position, Quaternion.identity);

        // Phát âm thanh cảnh báo
        if (warningSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(warningSound);
        }

        // Đợi thời gian cảnh báo
        yield return new WaitForSeconds(warningTime);

        // Tạo vụ nổ tại vị trí
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        // Random kích thước vụ nổ
        float randomSize = Random.Range(explosionSizeRange.x, explosionSizeRange.y);
        explosion.transform.localScale = new Vector3(randomSize, randomSize, 1);

        // Phát âm thanh vụ nổ
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Gửi sát thương tới kẻ địch trong vùng nổ
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, randomSize / 2f); // Kích thước vụ nổ ảnh hưởng tới bán kính
        foreach (Collider2D hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Hủy cảnh báo và vụ nổ sau khi kết thúc
        Destroy(warning);
        Destroy(explosion, 2f);
    }
    
    private IEnumerator Cooldown()
    {
        float elapsed = 0f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            currentCooldownTime = cooldownTime - elapsed;

            // Cập nhật lớp phủ
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = currentCooldownTime / cooldownTime;
            }

            yield return null;
        }

        // Kết thúc hồi chiêu
        isCooldown = false;
        currentCooldownTime = 0;

        // Bật nút kỹ năng trở lại
        if (skillButton != null)
        {
            skillButton.interactable = true;
        }

        // Tắt lớp phủ
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
