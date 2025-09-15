using System.Collections;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float flashTime = 0.1f;
    private float invicCounter;
    private float flashCounter;

    [Header("Components")]
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private Animator bodyAnim;
    [SerializeField] private GameObject playerDeathEffect;

    private readonly int hitParam = Animator.StringToHash("getHit");
    private readonly int dieParam = Animator.StringToHash("die");

    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (invicCounter > 0 && !IsDead)
        {
            invicCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                    sprite.enabled = !sprite.enabled;

                flashCounter = flashTime;
            }

            if (invicCounter <= 0)
            {
                foreach (var sprite in playerSprites)
                    sprite.enabled = true;
                flashCounter = 0;
            }
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invicCounter > 0 || IsDead) return;

        currentHealth -= damageAmount;

        if (bodyAnim != null)
            bodyAnim.SetTrigger(hitParam);

        if (AudioManager.HasInstance)
            AudioManager.Instance.PlaySFX("hit");

        if (UIManager.HasInstance)
            UIManager.Instance.GamePanel.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            invicCounter = invincibilityTime;
        }
    }

    private void Die()
    {
        currentHealth = 0;

        if (bodyAnim != null)
            bodyAnim.SetTrigger(dieParam);

        if (AudioManager.HasInstance)
            AudioManager.Instance.PlaySFX("die");

        if (playerDeathEffect != null)
            Instantiate(playerDeathEffect, transform.position, Quaternion.identity);

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1f);

        if (PlatformGameManager.HasInstance)
            PlatformGameManager.Instance.LooseGame();
        invicCounter = 0;
        foreach (var sprite in playerSprites)
            sprite.enabled = true;
    }

    public void ResetHealth()
    {
        StopAllCoroutines();

        currentHealth = maxHealth;
        invicCounter = 0;
        flashCounter = 0;
        foreach (var sprite in playerSprites)
        {
            sprite.enabled = true;
            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
        }

        if (bodyAnim != null)
        {
            bodyAnim.Rebind();
        }

        if (UIManager.HasInstance)
        {

            UIManager.Instance.GamePanel.SetMaxHealth(maxHealth);
            UIManager.Instance.GamePanel.UpdateHealth(currentHealth);
        }
    }
}
