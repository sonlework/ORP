using UnityEngine;

namespace ActionPlatformerKit
{
    public class TurretBotController : MonoBehaviour
    {
        [SerializeField] private Transform playerTf;
        [SerializeField] private float shootRange = 5f;

        [Header("Shooting")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float shootCooldown = 2f;

        private float shootTimer;
        private bool isShooting;

        private void Start()
        {
            if (playerTf == null)
                playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (playerTf == null) return;

            float dist = Vector3.Distance(transform.position, playerTf.position);

            if (dist < shootRange)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }

            if (isShooting)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f)
                {
                    Shoot();
                    shootTimer = shootCooldown;
                }
            }
        }

        private void Shoot()
        {
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet =Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Vector3 shootDir = -firePoint.right;
                bullet.GetComponent<EnemyBullet>().SetDirection(shootDir);
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySFX("shoot");
                }
            }
        }
    }
}