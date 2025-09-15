using System.Runtime.InteropServices;
using UnityEngine;

namespace ActionPlatformerKit
{
    public class ImpactDamage : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float effectTime;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = collision.GetComponentInParent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    enemyHealth.DamageEnemy(damageAmount);
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.PlaySFX("enemy hit");
                    }
                }

            }
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            GameObject effect = Instantiate(hitEffect, contactPoint, transform.rotation);
            Destroy(effect, effectTime);
        }
    }
}
