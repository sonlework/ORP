using UnityEngine;


public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed;
    [SerializeField]
    private Vector2 BulletDirection;
    [SerializeField]
    private Rigidbody2D BulletRb;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private int damageAmount = 1;
    [SerializeField] 
    private float deactiveTime = 3f;
    [SerializeField] 
    private float effectTime = 0.2f;
    private float deactiveCounter;
    private bool isActive = false;
    public bool IsActive => isActive;
    void Start()
    {
        deactiveCounter = deactiveTime;
    }
    void FixedUpdate()
    {
        if (!isActive) return;
        BulletRb.linearVelocity = BulletDirection * BulletSpeed;
    }
    public void Active(Vector2 newDirection)
    {
        isActive = true;
        BulletDirection = newDirection;

        if (newDirection.x < 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthController enemyHealth = collision.GetComponent<EnemyHealthController>();
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
        GameObject bulletEffect = Instantiate(impactEffect, contactPoint, transform.rotation);
        Destroy(bulletEffect, effectTime);

        DeActive();
    }

    private void OnBecameInvisible()
    {
        DeActive();
    }
    public void DeActive()
    {

        this.gameObject.SetActive(false);
        isActive = false;
        BulletRb.linearVelocity = Vector2.zero;
        BulletDirection = Vector2.zero;
        deactiveCounter = deactiveTime;
    }

}

