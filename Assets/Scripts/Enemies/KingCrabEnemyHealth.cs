using UnityEngine;

public class KingCrabEnemyHealth : EnemyHealth
{
    public Transform healthBarForeground;
    
    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float scale = (float) currentHealth / startingHealth;
        healthBarForeground.localScale = new Vector3(scale, 1, 1);
    }
}
