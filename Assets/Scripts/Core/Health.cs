// Assets/Scripts/Core/Health.cs
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Slider healthBar; // опционально

    [SerializeField] private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        if (CompareTag("Player"))
        {
            // Игрок умер → в хаб
            UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
        }
        else if (CompareTag("Enemy"))
        {
            // Враг умер
            Destroy(gameObject);
        }
    }
}