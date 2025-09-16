using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private int maxHealth = 100;

    [Header("AI Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float actionCooldown = 2f;

    [Header("Bullets")]
    [SerializeField] private GameObject fireBulletPrefab;
    [SerializeField] private GameObject homingBulletPrefab;
    [SerializeField] private Transform shootPoint;
 

    [Header("Animator")]
    [SerializeField] private Animator bossAnimator;

    [SerializeField] private WallDetector wallDetector;

    private readonly int walkParam = Animator.StringToHash("isWalking");

    private Rigidbody2D rb;
    private BossHealthController healthController;
    private bool isActive = false;
    private bool isPerformingAction = false;

    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthController = GetComponent<BossHealthController>();
        rb.bodyType = RigidbodyType2D.Dynamic;
  

        rb.linearVelocity = Vector2.zero;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartBossSequence()
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(BossIntroRoutine());
    }

    private IEnumerator BossIntroRoutine()
    {

       

        yield return new WaitForSecondsRealtime(1.5f);
        if (AudioManager.HasInstance) AudioManager.Instance.StopBGM();
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.ActiveBossHealth(true);
            UIManager.Instance.GamePanel.SetBossMaxHealth(maxHealth);

            yield return StartCoroutine(FillBossHealth(maxHealth));
        }
        if (healthController != null)
        {
            healthController.Init(maxHealth);
        }
        if (AudioManager.HasInstance) AudioManager.Instance.PlayBGM("Boss Theme");
        isActive = true;
        StartCoroutine(AIBehaviour());
    }

    private IEnumerator FillBossHealth(int maxHealthValue)
    {
        int value = 0;
        while (value < maxHealthValue)
        {
            
            value ++;
            
            UIManager.Instance.GamePanel.UpdateBossHealth(value);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    private IEnumerator AIBehaviour()
    {
        while (isActive)
        {
            if (!isPerformingAction)
            {
                int action = Random.Range(0, 4);
                switch (action)
                {
                    case 0: yield return Walk(); break;
                    case 1: yield return ChasingPlayer(); break;
                    case 2: yield return FireBullet(); break;
                    case 3: yield return HomingShot(); break;
                    
                }
                yield return new WaitForSeconds(actionCooldown);
            }
            yield return null;
        }
    }

    private IEnumerator Walk()
    {
        isPerformingAction = true;
        bossAnimator.SetBool(walkParam, true);

        float dir = Random.value > 0.5f ? 1 : -1;
        float duration = 1f;

        Flip(dir);

        float timer = 0f;
        while (timer < duration)
        {
            MoveWithWallCheck(ref dir, walkSpeed);

            timer += Time.deltaTime;
            yield return null;
        }

        isPerformingAction = false;
    }

    private IEnumerator ChasingPlayer()
    {
        isPerformingAction = true;
        bossAnimator.SetBool(walkParam, true);

        float dir = (player.position.x < transform.position.x) ? -1f : 1f;
        Flip(dir);

        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            MoveWithWallCheck(ref dir, dashSpeed);

            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isPerformingAction = false;
    }

    private IEnumerator FireBullet()
    {
        isPerformingAction = true;
        bossAnimator.SetBool(walkParam, false);
        rb.linearVelocity = Vector2.zero;
        float dir = (player.position.x < transform.position.x) ? -1f : 1f;
        GameObject bulletObj = Instantiate(fireBulletPrefab, shootPoint.position, Quaternion.identity);
       FireBuillet bullet = bulletObj.GetComponent<FireBuillet>();
        if (bullet != null)
        {
            bullet.SetDirection(dir);
        }

        yield return new WaitForSeconds(0.2f);
        isPerformingAction = false;
    }

    private IEnumerator HomingShot()
    {
        isPerformingAction = true;
        bossAnimator.SetBool(walkParam, false);
        rb.linearVelocity = Vector2.zero;
        int count = 1;
        for (int i = 0; i < count; i++)
        {
            GameObject bullet = Instantiate(homingBulletPrefab, transform.position, Quaternion.identity);
            BossBullet h = bullet.GetComponent<BossBullet>();
            if (h != null) h.SetTarget(player);

            yield return new WaitForSeconds(0.5f);
        }

        isPerformingAction = false;
    }
    private void MoveWithWallCheck(ref float dir, float speed)
    {
        if (wallDetector != null && wallDetector.GetDetectorValue())
        {
            dir *= -1;
            Flip(dir); 
        }
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }
    private void Flip(float dir)
    {
        Vector3 scale = transform.localScale;

        if (dir > 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            if (wallDetector != null) wallDetector.FaceWallDetectorRight();
        }
        else if (dir < 0)
        {
            scale.x = Mathf.Abs(scale.x);
            if (wallDetector != null) wallDetector.FaceWallDetectorLeft();
        }

        transform.localScale = scale;
    }
    public void ResetBoss()
    {
        isActive = false;
        isPerformingAction = false;
        rb.linearVelocity = Vector2.zero;
        bossAnimator.SetBool(walkParam, false);
    }
}
