using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int damageAmount;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float effectTime = 0.2f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.GetComponentInParent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damageAmount);
            }
           
        }
        GameObject bulletEffect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(bulletEffect, effectTime);
    }
}