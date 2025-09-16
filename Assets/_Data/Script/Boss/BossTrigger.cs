using UnityEngine;

namespace ActionPlatformerKit
{
    public class BossTrigger : MonoBehaviour
    {   public static void TriggerBoss(BossController boss)
        {
            if (boss == null) return;
            boss.gameObject.SetActive(true);
            boss.StartBossSequence();
            BossHealthController health = boss.GetComponent<BossHealthController>();
            if (health != null) health.Init(health.GetMaxHealth());
            if (UIManager.HasInstance)
                UIManager.Instance.GamePanel.ActiveBossHealth(true);
        }

    }
}
