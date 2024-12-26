using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private GameObject _endGame;
    private bool canTakeDamage = true;
    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    
    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        _endGame.SetActive(false);
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
        PlayerController.Instance._currentHealth -= damageAmount;
        EventManager.Raise(UIEvent.OnUpdateHealthBar);
        if (PlayerController.Instance._currentHealth <= 0)
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
        yield return new WaitForSeconds(0.5f);
        _endGame.SetActive(true);
        PlayerController.Instance.gameObject.SetActive(false);
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
