using UnityEngine;
using UnityEngine.UI;

public class vidaboss : MonoBehaviour
{
    [Header("Configuração de Vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI (opcional)")]
    public Image healthBar; // arraste a barra de vida aqui, se tiver

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        Debug.Log("Boss: " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("💀 Boss morreu!");

        // Aqui você pode:
        // - Desativar o jogador
        // - Tocar animação de morte
        // - Reiniciar a cena
        // - Chamar Game Over

        gameObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }
}
