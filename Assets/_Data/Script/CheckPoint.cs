using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private bool hasPassedCheckPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPassedCheckPoint)
        {
            hasPassedCheckPoint = true;
            if (RespawnManager.HasInstance)
            {
                RespawnManager.Instance.SetRespawnPoint(this.transform);
            }
        }
    }
}