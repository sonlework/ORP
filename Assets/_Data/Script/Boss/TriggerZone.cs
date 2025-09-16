using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{


    public bool oneShot = false;
    private bool alreadyTriggered = false;
    private bool alreadyExited = false;
    public string collisionTag = "Player"; 
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyTriggered) return;

        if(!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag)) return;


        onTriggerEnter?.Invoke();
        if (oneShot) alreadyTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (alreadyExited) return;

        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag)) return;

        onTriggerExit?.Invoke();

        if (oneShot) alreadyExited = true;
    }
}

