using UnityEngine;


public class GroundDetector : MonoBehaviour
{
    public Collider2D actorCollider;
    public LayerMask groundLayerMask;

    public bool isGrounded = false;

    [Header("Gizmo parameters:")]
    [Range(-2f, 2f)] public float cubeCastYOffset = -0.1f;
    [Range(-2f, 2f)] public float cubeCastXOffset = 0f;
    [Range(0, 2)] public float cubeCastWidth = 1f, cubeCastHeight = 0.2f;
    public Color gizmoColor = Color.yellow;

    private void Awake()
    {
        if (actorCollider == null)
            actorCollider = GetComponent<Collider2D>();
    }

    public void CheckIsGrounded()
    {
        Vector3 origin = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(cubeCastWidth, cubeCastHeight),
                                             0, Vector2.down, 0, groundLayerMask);

        isGrounded = hit.collider != null;
    }

    private void Update()
    {
        CheckIsGrounded();
    }

    private void OnDrawGizmos()
    {
        if (actorCollider == null) return;

        Gizmos.color = isGrounded ? Color.green : gizmoColor;
        Vector3 center = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        Gizmos.DrawWireCube(center, new Vector3(cubeCastWidth, cubeCastHeight, 0));
    }

    public bool GetDetectorValue()
    {
        return isGrounded;
    }
}

