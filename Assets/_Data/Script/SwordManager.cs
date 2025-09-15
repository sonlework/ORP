using UnityEngine;

namespace ActionPlatformerKit
{
    public class SwordManager : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private GameObject hitEffect;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = collision.GetComponentInParent<EnemyHealthController>();
                if (enemyHealth != null && hitEffect != null)
                {
                    enemyHealth.DamageEnemy(damageAmount);
                    Vector2 contactPoint = collision.ClosestPoint(transform.position);
                    GameObject swordEffect = Instantiate(hitEffect, contactPoint, Quaternion.identity);
                    Destroy(swordEffect, 0.2f);
                }
            }
        }
    }
}
