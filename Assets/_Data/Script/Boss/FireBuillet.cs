using UnityEngine;

public class FireBuillet : MonoBehaviour
{
    [Header("Motion")]
    [SerializeField] private float speed;
    [SerializeField] private int dmgAmount;
    
    [SerializeField] private float lifeTime;

    [Header("Puddle")]
    [SerializeField] private GameObject firePuddlePrefab;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float puddleSpawnYOffset;

    private Rigidbody2D rb;
    private float dir = -1f;
    private float timer;
    private float gravityScale;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        timer = 0f;
        gravityScale = Random.Range(0.5f, 1f);
        if (rb != null) rb.gravityScale = gravityScale;
    }

    void FixedUpdate()
    {
        Vector2 v = rb.linearVelocity;
        v.x = dir * speed;
        rb.linearVelocity = v;
    }

    public void SetDirection(float direction)
    {
        dir = Mathf.Sign(direction);

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.GetComponentInParent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(dmgAmount);
            }
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            GameObject bulletEffect = Instantiate(explosionEffect, contactPoint, Quaternion.identity);
            if(AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX("explosion");
            }
            Destroy(bulletEffect, 3f);
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Ground"))
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - puddleSpawnYOffset, transform.position.z);
            GameObject bulletEffect = Instantiate(firePuddlePrefab, spawnPos, Quaternion.identity);
            Destroy(bulletEffect, 3f);
            Destroy(gameObject);
        }
    }
}
