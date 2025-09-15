using UnityEngine;

public class EnemyFlyBotController : MonoBehaviour
{
    [SerializeField] private Transform playerTf;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float flySpeed = 2f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 2f;
    private float shootTimer;
    private bool isChasing;
    

    private void Start()
    {
        firePoint = this.transform;
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isChasing)
        {
            if (playerTf != null && Vector3.Distance(transform.position, playerTf.position) < chaseRange)
            {
                isChasing = true;
            }
        }
        else
        {
            if (playerTf != null)
            {
                Vector3 direction = playerTf.position - transform.position;

                if (direction.x >= 0f)
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                else
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

                // Move
                transform.position = Vector3.MoveTowards(
                    transform.position ,
                    playerTf.position + new Vector3(0f, 1f, 0f),
                    flySpeed * Time.deltaTime
                );

                // Shooting
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f)
                {
                    ShootAtPlayer();
                    shootTimer = shootCooldown;
                }
            }
        }
    }

    private void ShootAtPlayer()
    {
        if (bulletPrefab != null && firePoint != null && playerTf != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector3 shootDir = (playerTf.position - firePoint.position).normalized;
            bullet.GetComponent<EnemyBullet>().SetDirection(shootDir);
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX("shoot");
            }
        }
    }
}