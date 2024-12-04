using Unity.VisualScripting;
using UnityEngine;

public class KingCrabNormalAttack : MonoBehaviour, IEnemy
{
    [SerializeField] private DropBomBossController _dropBomBossController;
    [SerializeField]
    private int _anglebetween2butlet = 6;
    [SerializeField] private GameObject bulletPrefab;
    public void Attack()
    {
        int bulletCount = 360/_anglebetween2butlet;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // Tính toán hướng bắn
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 bulletDirection = new Vector2(dirX, dirY);

            // Tạo đạn
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.right = bulletDirection;

            // Tăng góc bắn cho viên tiếp theo
            angle += _anglebetween2butlet;
        }
    }

    public void DropBom()
    {
        StartCoroutine(_dropBomBossController.ActivateSkill());
    }
}
