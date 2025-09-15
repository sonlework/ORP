using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FirePuddle : MonoBehaviour
{
    [Header("Puddle Settings")]
    [SerializeField] private float duration = 6f;
    [SerializeField] private float damagePerSecond = 5f;

    private PlayerHealthController playerInZone;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        if (playerInZone != null)
        {
            playerInZone.DamagePlayer(Mathf.CeilToInt(damagePerSecond * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = other.GetComponent<PlayerHealthController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInZone != null && other.GetComponent<PlayerHealthController>() == playerInZone)
            {
                playerInZone = null;
            }
        }
    }
}
