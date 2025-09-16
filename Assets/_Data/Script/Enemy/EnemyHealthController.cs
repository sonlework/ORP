using System.Collections;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] protected int totalHealth = 3;
    [SerializeField] private GameObject deathEffect;

    [Header("Flash Settings")]
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 0.01f;
    [SerializeField] private float flashTime = 0.5f;

    protected int currentHealth;
    private Color originalColor;
    private Vector3 spawnPosition;

    protected virtual void Start()
    {
        if (enemySprite != null)
        {
            originalColor = enemySprite.color;
        }

        currentHealth = totalHealth;
        spawnPosition = transform.position;
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
                GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
                Destroy(effect, 2f);
            }

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX("explosion");
            }
            if (enemySprite != null)
            {
                enemySprite.color = originalColor;
            }
            gameObject.SetActive(false);
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


    public virtual void Respawn()
    {
        currentHealth = totalHealth;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
        if (enemySprite != null)
        {
            enemySprite.color = originalColor;
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => totalHealth;
}