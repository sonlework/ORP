using System.Runtime.InteropServices;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private GameObject explosionEffect;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            rb.linearVelocity = dir * speed;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;


        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, -transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.linearVelocity = -transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.GetComponent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(3);
            }
        }
        if(AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("explosion");
        }
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(effect, 1f);
    }
}