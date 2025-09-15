using System.Collections;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] protected int totalHealth = 3;   // Máu tối đa
    [SerializeField] private GameObject deathEffect;

    [Header("Flash Settings")]
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 0.01f;
    [SerializeField] private float flashTime = 0.5f;

    protected int currentHealth;
    private Color originalColor;
    private Vector3 spawnPosition;  // Lưu vị trí gốc để respawn

    protected virtual void Start()
    {
        if (enemySprite != null)
        {
            originalColor = enemySprite.color;
        }

        currentHealth = totalHealth;
        spawnPosition = transform.position; // Lưu vị trí ban đầu
    }

    public virtual void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (enemySprite != null)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX("explosion");
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator FlashEffect()
    {
        float elapsed = 0f;
        while (elapsed < flashTime)
        {
            enemySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            enemySprite.color = originalColor;
            yield return new WaitForSeconds(flashDuration);

            elapsed += flashDuration * 2f;
        }
    }

    // Hàm để reset lại enemy
    public virtual void Respawn()
    {
        currentHealth = totalHealth;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => totalHealth;
}