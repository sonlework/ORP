using UnityEngine;

public class CeilingDetector : MonoBehaviour
{
    public Collider2D actorCollider;
    public LayerMask ceilingLayerMask;

    public bool isTouchingCeiling = false;

    [Header("Gizmo parameters:")]
    [Range(-2f, 2f)] public float cubeCastYOffset = 0.5f;
    [Range(-2f, 2f)] public float cubeCastXOffset = 0f;
    [Range(0, 2f)] public float cubeCastWidth = 1f, cubeCastHeight = 0.2f;
    public Color gizmoColor = Color.red;

    public Vector3 hitNormal;
    public float angleBetweenNormal = 0;

    private void Awake()
    {
        if (actorCollider == null)
            actorCollider = GetComponent<Collider2D>();
    }

    public void CheckIsTouchingCeiling()
    {
        Vector3 origin = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(cubeCastWidth, cubeCastHeight),
                                             0, Vector2.up, 0, ceilingLayerMask);

        if (hit.collider != null)
        {
            isTouchingCeiling = true;
            hitNormal = hit.normal;
            angleBetweenNormal = Vector2.SignedAngle(Vector2.down, hitNormal);
        }
        else
        {
            isTouchingCeiling = false;
            angleBetweenNormal = 0;
        }
    }

    private void Update()
    {
        CheckIsTouchingCeiling();
    }

    private void OnDrawGizmos()
    {
        if (actorCollider == null) return;

        Gizmos.color = isTouchingCeiling ? Color.green : gizmoColor;
        Vector3 center = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        Gizmos.DrawWireCube(center, new Vector3(cubeCastWidth, cubeCastHeight, 0));

        if (isTouchingCeiling)
            Gizmos.DrawRay(center, hitNormal);
    }

    public bool GetDetectorValue() => isTouchingCeiling;
}
