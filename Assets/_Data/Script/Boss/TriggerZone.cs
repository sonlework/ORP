using UnityEngine;
using Unity.Cinemachine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private BossController boss;


    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!hasTriggered)
        {
            hasTriggered = true;
            boss.gameObject.SetActive(true);
            boss.StartBossSequence();
            BossHealthController health = boss.GetComponent<BossHealthController>();
            if (health != null) health.Init(health.GetMaxHealth());
            if (AudioManager.HasInstance) AudioManager.Instance.PlayBGM("Boss Theme");
        }
        if (UIManager.HasInstance)
            UIManager.Instance.GamePanel.ActiveBossHealth(true);
    }
}

