using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public Collider2D actorCollider;
    public LayerMask wallLayerMask;

    public bool isTouchingWall = false;

    [Header("Gizmo parameters:")]
    [Range(-2f, 2f)] public float cubeCastXOffset = 0.5f;
    [Range(-2f, 2f)] public float cubeCastYOffset = 0f;
    [Range(0, 2f)] public float cubeCastWidth = 0.2f, cubeCastHeight = 1f;
    public Color gizmoColor = Color.blue;

    private Vector2 castDirection = Vector2.right;
    private void Awake()
    {
        if (actorCollider == null)
            actorCollider = GetComponent<Collider2D>();
    }

    public void FaceWallDetectorLeft()
    {
        castDirection = Vector2.left;
        cubeCastXOffset = -Mathf.Abs(cubeCastXOffset);
    }

    public void FaceWallDetectorRight()
    {
        castDirection = Vector2.right;
        cubeCastXOffset = Mathf.Abs(cubeCastXOffset);
    }

    public void CheckIsTouchingWall()
    {
        Vector3 origin = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(cubeCastWidth, cubeCastHeight),
                                             0, castDirection, 0, wallLayerMask);

        isTouchingWall = hit.collider != null;
    }

    private void Update()
    {
        CheckIsTouchingWall();
    }

    private void OnDrawGizmos()
    {
        if (actorCollider == null) return;

        Gizmos.color = isTouchingWall ? Color.green : gizmoColor;
        Vector3 center = actorCollider.transform.position + new Vector3(cubeCastXOffset, cubeCastYOffset, 0);
        Gizmos.DrawWireCube(center, new Vector3(cubeCastWidth, cubeCastHeight, 0));
    }

    public bool GetDetectorValue() => isTouchingWall;
}