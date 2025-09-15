using UnityEngine;

public class BossHealthController : EnemyHealthController
{

    protected override void Start()
    {
        base.Start();
    }

    public void Init(int maxHealthValue)
    {
        totalHealth = maxHealthValue;
        currentHealth = maxHealthValue;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.ActiveBossHealth(true);
            UIManager.Instance.GamePanel.SetBossMaxHealth(totalHealth);
            UIManager.Instance.GamePanel.UpdateBossHealth(currentHealth);
        }
    }

    public override void DamageEnemy(int damageAmount)
    {
        base.DamageEnemy(damageAmount);

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.UpdateBossHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.GamePanel.ActiveBossHealth(false);
            }
            if(PlatformGameManager.HasInstance)
            {
                PlatformGameManager.Instance.WinGame();
            }
        }
    }
}
