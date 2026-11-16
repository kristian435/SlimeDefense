using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("TerveysAsetukset")]
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private int moneyWorth = 50;
    private float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoney(moneyWorth);
        }
        Destroy(gameObject);
    }
}
