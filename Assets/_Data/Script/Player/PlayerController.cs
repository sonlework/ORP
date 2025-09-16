using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player Collider")]
    [SerializeField] private CapsuleCollider2D playerColliderTrigger;
    [SerializeField] private CapsuleCollider2D detectorCollider;


    [Header("Movement")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject dustEffect;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpPushForce;

    [Header("Weapon")]
    [SerializeField] private GameObject busterMode;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private GameObject lightSaberMode;
    [SerializeField] private float attackLockTime = 0.3f;
    [SerializeField] private BulletController chargedBulletPrefab;
    [SerializeField] private GameObject shootEffect;
    [SerializeField] private GameObject chargeEffect;
    [SerializeField] private GameObject maxChargeEffect;
    [SerializeField] private float strongBulletThreshold = 2f;

    [Header("Positions")]
    [SerializeField] private Transform shootPos;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private int maxDash = 1;
    [SerializeField] private bool disableJumpAfterDash = false;
    [SerializeField] private bool isAirDash;
    [SerializeField] private bool isDashInvincible;
    private int dashCount;

    [Header("Detector")]
    [SerializeField] private CeilingDetector ceilingDetector;
    [SerializeField] private WallDetector wallDetector;
    [SerializeField] private GroundDetector groundDetector;

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator armAnimator;

    [Header("Arm Settings")]
    [SerializeField] private Transform armSocket;
    [SerializeField] private Transform arm;

    // Animator params
    private readonly int speedParam = Animator.StringToHash("speed");
    private readonly int isOnGroundParam = Animator.StringToHash("isOnGround");
    private readonly int velocityYParam = Animator.StringToHash("velocityY");
    private readonly int isDashingParam = Animator.StringToHash("Dash");
    private readonly int isCrouchingParam = Animator.StringToHash("isCrouching");
    private readonly int isWallSlidingParam = Animator.StringToHash("isWallSliding");
    private readonly int MeleeParam = Animator.StringToHash("Melee");
    private readonly int ShootParam = Animator.StringToHash("Shoot");
    private readonly int SpinSlashParram = Animator.StringToHash("spinSlash");
    // Collider backup
    private float originalHeight;
    private float originalYOffset;
    public float crouchColliderHeight = 1f;
    private float originalCeilingDetectorYOffset;
    private float originalWallDetectorYOffset;
    public float ceilingDetectorYOffset;
    public float wallDetectorYOffset;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer busterSR;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color iceColor = Color.cyan;
    [SerializeField] private Color saberColor = Color.red;

    [SerializeField] private float footstepInterval = 0.4f;
    private float footstepTimer;
    // State
    private float currentSpeed;
    private float dashCounter;
    private float dashCooldownCounter;
    private bool isOnGround;
    private bool isDoubleJump;
    private bool isCrouching;
    private bool isDashing;
    private bool isWallSliding = false;
    private WeaponType currentWeapon = WeaponType.Buster;
    private WeaponType[] weaponList;
    private int weaponIndex = 0;
    private bool isAttacking = false;
    private float attackCounter;
    private float xAxis;
    private float shootChargeTime;
    private GameObject currentChargeEffect;
    private GameObject currentMaxChargeEffect;

    private void OnEnable()
    {
        if (arm != null && armSocket != null)
        {
            arm.SetParent(armSocket);
            arm.localPosition = Vector3.zero;
            arm.localRotation = Quaternion.identity;
        }
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
        if (playerColliderTrigger != null)
        {
            originalHeight = playerColliderTrigger.size.y;
            originalYOffset = playerColliderTrigger.offset.y;
        }

        if (ceilingDetector != null)
            originalCeilingDetectorYOffset = ceilingDetector.cubeCastYOffset;

        if (wallDetector != null)
            originalWallDetectorYOffset = wallDetector.cubeCastYOffset;

        weaponList = new WeaponType[] { WeaponType.Buster, WeaponType.LightSaber };
        SwitchWeapon(weaponList[weaponIndex]);
    }

    private void Update()
    {
        if (dashCooldownCounter > 0)
            dashCooldownCounter -= Time.deltaTime;
        if (isOnGround)
        {
            dashCount = maxDash;
        }
        if (isAttacking)
        {
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0f)
            {
                isAttacking = false;
            }
        }

        HandleMovement();
        HandleAnimations();
        HandleAttack();
        HandleWeaponSwitching();
        PauseGame();
    }

    private void HandleMovement()
    {
        // === DASH ===
        if (Input.GetKeyDown(KeyCode.L) && !isDashing && dashCooldownCounter <= 0 && !isCrouching)
        {
            if (isOnGround || (isAirDash && dashCount > 0))
            {
                isDashing = true;
                dashCounter = dashTime;
                if (!isOnGround && isAirDash) dashCount--;
                if (isDashInvincible) ToggleCapsuleCollider2D(false);
                if (AudioManager.Instance)
                {
                    AudioManager.Instance.PlaySFX("dash");
                }
                ResizeCapsuleCollider2D();
            }
        }

        if (isDashing)
        {
            dashCounter -= Time.deltaTime;
            playerRb.linearVelocity = new Vector2(dashSpeed * transform.localScale.x, 0f);

            if (dashCounter <= 0 || Input.GetKeyUp(KeyCode.L))
            {
                bool touchWall = wallDetector != null && wallDetector.GetDetectorValue();
                bool touchCeiling = ceilingDetector != null && ceilingDetector.GetDetectorValue();
                bool touchGround = groundDetector != null && groundDetector.GetDetectorValue();


                if (touchCeiling && touchGround && !touchWall)
                {

                    dashCounter = dashTime;
                    return;
                }
                else if (touchWall && touchCeiling && touchGround)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                    if (transform.localScale.x < 0) wallDetector.FaceWallDetectorLeft();
                    else wallDetector.FaceWallDetectorRight();

                    dashCounter = dashTime;
                    return;
                }
                else
                {
                    isDashing = false;
                    dashCooldownCounter = dashCooldown;

                    if (isDashInvincible) ToggleCapsuleCollider2D(true);
                    ResetCapsuleCollider2D();
                    if (disableJumpAfterDash)
                        isDoubleJump = false;
                }
            }
            return;
        }

        // === MOVE ===    

        xAxis = Input.GetAxisRaw("Horizontal");
        if ((isCrouching || isAttacking) && isOnGround) xAxis = 0;
        playerRb.linearVelocity = new Vector2(xAxis * currentSpeed, playerRb.linearVelocity.y);

        if (AudioManager.Instance)
        {
            if (xAxis != 0 && isOnGround && !isDashing)
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    AudioManager.Instance.PlaySFX("walk");
                    footstepTimer = footstepInterval;
                }
            }
            else
            {
                footstepTimer = 0f;
            }
        }

        if (playerRb.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            wallDetector.FaceWallDetectorLeft();
        }
        else if (playerRb.linearVelocity.x > 0)
        {
            transform.localScale = Vector3.one;
            wallDetector.FaceWallDetectorRight();
        }


        // === JUMP ===
        isOnGround = groundDetector != null && groundDetector.GetDetectorValue();
        if (Input.GetKeyDown(KeyCode.K) && (isOnGround || isDoubleJump) && !isCrouching && !isAttacking)
        {
            if (isOnGround) isDoubleJump = true;
            else isDoubleJump = false;
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX("jump");
            }
        }

        // === CROUCH ===
        if (Input.GetKey(KeyCode.S))
        {
            if (!isCrouching && isOnGround)
            {
                isCrouching = true;
                ResizeCapsuleCollider2D();

            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                ResetCapsuleCollider2D();

            }
        }

        // === WALL SLIDE ===
        if (!isOnGround && wallDetector != null && wallDetector.GetDetectorValue() && !groundDetector.GetDetectorValue() && playerRb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            dashCount = maxDash;
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, wallSlideSpeed);
            transform.localScale = (wallDetector.cubeCastXOffset < 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            dustEffect.SetActive(true);
        }
        else
        {
            isWallSliding = false;
            dustEffect.SetActive(false);
        }
        // === WALL JUMP ===
        if (isWallSliding && Input.GetKeyDown(KeyCode.K))
        {
            float pushDirection = wallDetector.cubeCastXOffset < 0 ? 1 : -1;

            playerRb.linearVelocity = new Vector2(pushDirection * wallJumpPushForce, jumpForce);

            if (isWallSliding) isDoubleJump = true;
            else isDoubleJump = false;
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX("jump");
            }
        }

    }

    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponIndex--;
            if (weaponIndex < 0)
            {
                weaponIndex = weaponList.Length - 1;
                ShowChargeEffect(false);
                ShowChargeMax(false);
            }
            SwitchWeapon(weaponList[weaponIndex]);
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX("btnClick");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            weaponIndex++;
            if (weaponIndex >= weaponList.Length)
            {
                weaponIndex = 0;
                ShowChargeEffect(false);
                ShowChargeMax(false);
            }
            SwitchWeapon(weaponList[weaponIndex]);
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX("btnClick");
            }
        }
    }

    private void HandleAttack()
    {
        // === BUSTER ===
        if (currentWeapon == WeaponType.Buster && Input.GetKey(KeyCode.J))
        {
            shootChargeTime += Time.deltaTime;
            if (shootChargeTime > 0.5f && shootChargeTime < strongBulletThreshold)
            {
                ShowChargeEffect(true);
                ShowChargeMax(false);
            }
            else if (shootChargeTime >= strongBulletThreshold)
            {
                ShowChargeEffect(false);
                ShowChargeMax(true);
            }
        }
        if (currentWeapon == WeaponType.Buster && Input.GetKeyUp(KeyCode.J))
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX("shoot");
            }
            isAttacking = true;
            attackCounter = attackLockTime;
            ShowChargeMax(false);
            ShowChargeEffect(false);
            ShootBullet();
            PlayShootEffect();
            shootChargeTime = 0f;
        }

        // === LIGHT SABER ===
        if (currentWeapon == WeaponType.LightSaber && Input.GetKey(KeyCode.J) && !isAttacking && !isCrouching)
        {
            isAttacking = true;
            attackCounter = attackLockTime;
            if (AudioManager.Instance && isAttacking)
            {
                AudioManager.Instance.PlaySFX("slash");
            }
            if (isOnGround)
            {
                bodyAnimator.SetTrigger(MeleeParam);
            }
            else
            {
                bodyAnimator.SetTrigger(SpinSlashParram);
                StartCoroutine(SpinSlashLockY(0.1f));
            }
        }
    }

    private IEnumerator SpinSlashLockY(float duration)
    {
        float timer = duration;
        float originalGravity = playerRb.gravityScale;

        // Khóa trục Y
        playerRb.gravityScale = 0f;
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        playerRb.gravityScale = originalGravity;
    }

    private void ShootBullet()
    {
        BulletController bullet;

        if (shootChargeTime >= strongBulletThreshold)
        {
            bullet = BulletManager.Instance.GetChargedBullet();
            bullet.transform.position = shootPos.position;
            bullet.transform.rotation = shootPos.rotation;
        }
        else
        {
            bullet = BulletManager.Instance.GetPooledBullet();
            bullet.transform.position = shootPos.position;
            bullet.transform.rotation = shootPos.rotation;
        }

        bullet.Active(new Vector2(transform.localScale.x, 0));
        bodyAnimator.SetTrigger(ShootParam);
    }

    private void ShowChargeEffect(bool isCharging)
    {

        if (isCharging && currentChargeEffect == null)
        {
            Vector3 offset = new Vector3(0f, -0.1f, 0f);
            currentChargeEffect = Instantiate(chargeEffect, shootPos.position + offset, transform.rotation);
            currentChargeEffect.transform.SetParent(shootPos);
            if (AudioManager.Instance && !AudioManager.Instance.IsSFXPlaying("weapon charge") && !AudioManager.Instance.IsSFXPlaying("max charge"))
            {
                AudioManager.Instance.PlaySFX("weapon charge");
            }
        }
        else if (!isCharging && currentChargeEffect != null)
        {
            Destroy(currentChargeEffect);
            currentChargeEffect = null;
            AudioManager.Instance.StopSFX("weapon charge");

        }
    }

    private void ShowChargeMax(bool chargeMax)
    {
        if (chargeMax && currentMaxChargeEffect == null)
        {
            currentMaxChargeEffect = Instantiate(maxChargeEffect, shootPos.position + new Vector3(0f, 0.1f, 0f), transform.rotation);
            currentMaxChargeEffect.transform.SetParent(shootPos);
            if (AudioManager.Instance && !AudioManager.Instance.IsSFXPlaying("max charge"))
            {
                AudioManager.Instance.PlaySFX("max charge");
            }
        }
        else if (!chargeMax && currentMaxChargeEffect != null)
        {
            Destroy(currentMaxChargeEffect);
            currentMaxChargeEffect = null;
            AudioManager.Instance.StopSFX("max charge");
        }
    }

    private void PlayShootEffect()
    {
        float direction = transform.localScale.x;
        shootEffect.transform.localScale = transform.localScale;
        Vector3 offset = new Vector3(0.1f * direction, 0, 0f);
        GameObject effect = Instantiate(shootEffect, shootPos.position + offset, shootPos.rotation);
        effect.transform.SetParent(shootPos);
        effect.transform.localPosition = Vector3.zero;
        Destroy(effect, 0.5f);
    }

    private void HandleAnimations()
    {
        float speed = Mathf.Abs(playerRb.linearVelocity.x);
        float velocityY = playerRb.linearVelocity.y;

        bodyAnimator.SetFloat(speedParam, speed);
        bodyAnimator.SetBool(isCrouchingParam, isCrouching);
        bodyAnimator.SetBool(isOnGroundParam, isOnGround);
        bodyAnimator.SetFloat(velocityYParam, velocityY);
        bodyAnimator.SetBool(isWallSlidingParam, isWallSliding);
        bodyAnimator.SetBool(isDashingParam, isDashing);
    }

    public void ResetDashes()
    {
        isDashing = false;
        dashCounter = 0;
        dashCooldownCounter = 0;
    }

    private bool isPaused;
    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlatformGameManager.HasInstance)
            {
                if(isPaused)
                {
                    isPaused = false;
                    PlatformGameManager.Instance.ResumeGame();
                }
                else
                {
                    isPaused = true;
                    PlatformGameManager.Instance.PauseGame();
                }
            }

        }
    }

    private void ResizeCapsuleCollider2D()
    {
        playerColliderTrigger.size = new Vector2(playerColliderTrigger.size.x, crouchColliderHeight);
        detectorCollider.size = new Vector2(detectorCollider.size.x, crouchColliderHeight);

        float yOffset = (originalHeight - crouchColliderHeight) / 2;
        playerColliderTrigger.offset = new Vector2(playerColliderTrigger.offset.x, originalYOffset - yOffset);
        detectorCollider.offset = new Vector2(detectorCollider.offset.x, originalYOffset - yOffset);

        ceilingDetector.cubeCastYOffset = ceilingDetectorYOffset;
        wallDetector.cubeCastYOffset = wallDetectorYOffset;
    }

    private void ResetCapsuleCollider2D()
    {
        playerColliderTrigger.size = new Vector2(playerColliderTrigger.size.x, originalHeight);
        detectorCollider.size = new Vector2(detectorCollider.size.x, originalHeight);

        playerColliderTrigger.offset = new Vector2(playerColliderTrigger.offset.x, originalYOffset);
        detectorCollider.offset = new Vector2(detectorCollider.offset.x, originalYOffset);

        ceilingDetector.cubeCastYOffset = originalCeilingDetectorYOffset;
        wallDetector.cubeCastYOffset = originalWallDetectorYOffset;
    }

    public void ToggleCapsuleCollider2D(bool value)
    {
        playerColliderTrigger.enabled = value;
    }

    private void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        if (currentWeapon == WeaponType.Buster)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.GamePanel.UpdateWeaponIcon(WeaponType.Buster);
            }

        }
        else if (currentWeapon == WeaponType.LightSaber)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.GamePanel.UpdateWeaponIcon(WeaponType.LightSaber);
            }
        }
    }

    private void ShootBullet(BulletController prefab)
    {
        if (prefab == null) return;

        BulletController bullet = Instantiate(prefab, shootPos.position, Quaternion.identity);
        bullet.Active(new Vector2(transform.localScale.x, 0f));
    }

}
public enum WeaponType
{
    Buster,
    LightSaber
}