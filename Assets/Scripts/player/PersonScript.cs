using System.Diagnostics;
using UnityEngine;

public class PersonScript : Sounds
{
    // �������� �������������� ���������
    public bool isRightSide = true;
    public Vector2 speed = new Vector2(5, 5);
    public int maxHp = 100;
    public int hp;
    public bool isEnemy = true;
    public bool isDead = false;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    private float invulnTimer = 0f;
    // ����������� ����� ����� ����������� (� ��������)
    public float damageCooldown = 0.2f;
    // ����� ���������� ��������� �����
    private float lastDamageTime = -Mathf.Infinity;

    // ��������
    private CharacterAnimations animations;

    // ��������
    public Joystick joystickMovement;
    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;
    public Vector2 direction = new Vector2(-1, 0);

    // �������� � ������������
    public Transform shotPrefab;
    public float shootingRate = 0.25f;
    private float shootCooldown;
    public Joystick joystickAttack;
    private Vector2 aimDirection;
    private bool isAiming;      // ������� ������������
    private float aimTime;
    private Vector2 lastAimDirection; // ���������� ����������� ��������
    private float lastAimTime;        // ���������� ������� �������
    public GameObject aimIndicator;   // ��������� ������������


    // UI �������
    public ChargeBar chargeBar;
    public float maxChargeTime = 2f;    // ����� ��� ������� ������

    // ������
    public int maxArrows = 5;
    private int currentArrows;
    public float arrowReplenishTime = 4f;
    private float replenishTimer;
    public Quiver quiver;

    // ����������� ���������
    public bool CanAttack => shootCooldown <= 0f && currentArrows > 0;

    // ���� ��� ���������� �������� �� ����� ��������
    private bool isShooting;

    void Start()
    {
        hp = maxHp;
        shootCooldown = 0f;
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        currentArrows = maxArrows;
        replenishTimer = arrowReplenishTime;
        isAiming = false;
        isShooting = false;
        aimTime = 0f;
        animations = GetComponentInChildren<CharacterAnimations>();
        aimIndicator.SetActive(false);
        if (chargeBar != null)
        {
            chargeBar.Hide();
        }
    }

    void Update()
    {
        // ���� ����������� ������� ��� ��� �������� ��������, ��������� ���� � ��������� �����
        bool blockAttackInput = (currentArrows <= 0) || isShooting;
        if (blockAttackInput)
        {
            // ���� ����� ��� �������, �� �������� UI � ���������� ����
            if (isAiming)
            {
                isAiming = false;
                aimIndicator.SetActive(false);
                if (chargeBar != null)
                    chargeBar.Hide();
            }
        }
        else
        {
            // ��������� ����� �� ������������ (���� �� �����������)
            float inputXAim = joystickAttack.Horizontal;
            float inputYAim = joystickAttack.Vertical;

            if (inputXAim != 0f || inputYAim != 0f)
            {
                aimDirection = new Vector2(inputXAim, inputYAim).normalized;

                // ��������� ������� ���������� ������������
                float distance = 1.2f;
                Vector2 indicatorPosition = (Vector2)transform.position + aimDirection * distance;
                aimIndicator.transform.position = indicatorPosition;
                aimIndicator.SetActive(true);

                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                aimIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                if (!isAiming)
                {
                    isAiming = true;
                    aimTime = 0f;
                    animations.isAiming = true;
                    if (chargeBar != null)
                    {
                        chargeBar.Show();
                    }
                }
                aimTime += Time.deltaTime;
                if (chargeBar != null)
                {
                    float fill = Mathf.Clamp01(aimTime / maxChargeTime);
                    chargeBar.SetCharge(fill);
                }
            }
            else if (isAiming)
            {
                // ���� ����� �������� �������� ����� � ���� �������
                aimIndicator.SetActive(false);
                if (chargeBar != null)
                    chargeBar.Hide();
                isAiming = false;
                lastAimTime = aimTime;
                lastAimDirection = aimDirection;
                if ((aimDirection.x > 0f && !isRightSide) || (aimDirection.x < 0f && isRightSide))
                {
                    Spin();
                }
                PlaySound(sounds[0], 0.15f, p1: 2);
                isShooting = true;
                animations.isShot = true;
                animations.isAiming = false;
            }
        }

        // ��������� �������� ���������� ������, ���� �� ����������� ��������� ��������
        float inputXMove = joystickMovement.Horizontal; // Input.GetAxis("Horizontal")
        float inputYMove = joystickMovement.Vertical; ;

        if (!isShooting)
        {
            movement = new Vector2(speed.x * inputXMove, speed.y * inputYMove);
            animations.isMoving = (inputXMove != 0 || inputYMove != 0);
            if ((inputXMove > 0f && !isRightSide) || (inputXMove < 0f && isRightSide))
            {
                if (inputXMove != 0f)
                {
                    Spin();
                }
            }
        }
        else
        {
            movement = Vector2.zero;
            animations.isMoving = false;
        }

        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }

        if (currentArrows < maxArrows)
        {
            replenishTimer -= Time.deltaTime;
            if (replenishTimer <= 0f)
            {
                currentArrows++;
                replenishTimer = arrowReplenishTime;
                UpdateArrowUI();
            }
        }
    }


    void FixedUpdate()
    {
        if (rigidbodyComponent == null)
            rigidbodyComponent = GetComponent<Rigidbody2D>();
        rigidbodyComponent.linearVelocity = movement;
    }

    void Spin()
    {
        isRightSide = !isRightSide;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Damage(int damageCount)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;
        if (isDead || isInvulnerable) return;

        lastDamageTime = Time.time;
        FindAnyObjectByType<ScreenEffectsController>()?.FlashDamageOverlay();

        hp -= damageCount;
        PlaySound(sounds[1], 0.05f);

        if (hp <= 0 && !isDead)
        {
            isDead = true;
            this.enabled = false;
            animations.isDead = true;

            FindAnyObjectByType<GameOverManager>()?.ShowGameOver(this);
        }
    }


    public void Heal(int amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
        PlaySound(sounds[2], 0.2f);
        UnityEngine.Debug.Log($"����� ����������� {amount} HP, ������� HP = {hp}");
    }

    public void Revive()
    {
        hp = 15;
        isDead = false;
        isInvulnerable = true;
        invulnTimer = invulnerabilityDuration;
        animations.isDead = false;
        this.enabled = true;
        animations.TriggerRevive();
    }

    // ���� ����� ����� ���������� �� �������� ��������
    public void ShotAnimationEnd()
    {
        Attack(isEnemy);
        isShooting = false;
    }

    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;
            currentArrows--;
            UpdateArrowUI();
            
            Transform shotTransform = Instantiate(shotPrefab);
            shotTransform.position = transform.position;
            shotTransform.rotation = Quaternion.LookRotation(Vector3.forward, lastAimDirection);

            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
                shot.damage = CalculateDamage(lastAimTime);
            }

            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = lastAimDirection;
                move.speed = CalculateSpeed(lastAimTime);
            }
        }
    }

    private int CalculateDamage(float time)
    {
        return Mathf.Clamp(Mathf.FloorToInt(time * 35), 10, 70);
    }

    private float CalculateSpeed(float time)
    {
        return Mathf.Clamp(time * 15, 15f, 50f);
    }

    private void UpdateArrowUI()
    {
        if (quiver != null)
        {
            quiver.UpdateArrows(currentArrows);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(shot.damage);
                Destroy(shot.gameObject);
            }
        }
    }
}
