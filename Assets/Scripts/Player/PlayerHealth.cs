using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        currentHealth = maxHealth;
        HealthBar.Instance.SetHeathBarText($"{currentHealth}/{maxHealth}");
        HealthBar.Instance.SetSliderHealthBarValue(1);
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform) {
        
        if (!canTakeDamage) { return; }
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        HealthBar.Instance.SetHeathBarText($"{currentHealth}/{maxHealth}");
        float ratio = (float) currentHealth / maxHealth;
        HealthBar.Instance.SetSliderHealthBarValue(ratio);
        if (currentHealth <= 0)
        {
            StartCoroutine(EndGame());
            return;
        }
        StartCoroutine(DamageRecoveryRoutine());
    }

    private IEnumerator EndGame()
    {
        PlayerController.Instance.PlayerDie();
        yield return new WaitForSeconds(PlayerController.Instance.GetTimeOfDieAnim());
        PlayerController.Instance.gameObject.SetActive(false);
        print("Game Over");
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
