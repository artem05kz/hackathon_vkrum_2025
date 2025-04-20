using System;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Sounds
{
    [Header("Параметры врага")]
    public int maxHp = 50;
    public int hp;
    public int attackDamage = 25;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public float aggroRange = 8f;
    public float chaseSpeed = 4f;
    public bool isRightSide = true;
    public GameObject heartPrefab;

    private float attackTimer = 0f;
    public bool isAggro = false;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform playerTransform;
    public MeleeAnimations animations;
    private TimeBody timeBody;
    private HealthBarUIEnemy healthBar;
    private GameController gameController;

    void Start()
    {
        GameObject gcObj = GameObject.FindGameObjectWithTag("GameController");
        if (gcObj != null)
        {
            gameController = gcObj.GetComponent<GameController>();
            if (gameController != null)
            {
                gameController.RegisterEnemy(this);
            }
        }
        healthBar = GetComponentInChildren<HealthBarUIEnemy>();
        if (healthBar != null)
            healthBar.SetHealth(hp, maxHp);

        timeBody = GetComponent<TimeBody>();
        hp = maxHp;
        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = chaseSpeed;

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            playerTransform = playerGO.transform;

        animations = GetComponentInChildren<MeleeAnimations>();
        if (animations != null)
            animations.isIdle = true;
    }

    void Update()
    {
        if (timeBody != null && (timeBody.IsRewinding || timeBody.IsFrozen))
        {
            if (animations != null && animations.animator != null)
            {
                animations.animator.speed = 0f;
            }
            return;
        }
        else
        {
            if (animations != null && animations.animator != null && animations.animator.speed == 0f)
            {
                animations.animator.speed = 1f;
            }
        }
        if (playerTransform != null)
        {
            if (playerTransform.position.x < transform.position.x && isRightSide)
            {
                Spin();
            }
            else if (playerTransform.position.x > transform.position.x && !isRightSide)
            {
                Spin();
            }
        }

        // Агро-зона
        float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (!isAggro && distToPlayer <= aggroRange)
        {
            isAggro = true;
            animations.isAggro = true;
        }

        // Пока не агро или уже в атаке — ничего не делаем
        if (!isAggro || isAttacking) return;

        attackTimer += Time.deltaTime;

        if (distToPlayer > attackRange)
        {
            // Бежим к игроку
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
            animations.isMoving = true;
            animations.isIdle = false;
        }
        else
        {
            // В зоне атаки
            agent.isStopped = true;
            animations.isMoving = false;
            animations.isIdle = true;

            if (attackTimer >= attackCooldown)
            {
                StartAttack();
                attackTimer = 0f;
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        agent.isStopped = true;                  
        animations.TriggerAttack();              
    }

    public void OnAttackAnimationEnd()
    {
        PlaySound(sounds[1], 0.2f);
        if (playerTransform != null &&
            Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            var player = playerTransform.GetComponent<PersonScript>();
            if (player != null)
                player.Damage(attackDamage);
        }

        isAttacking = false;
        agent.isStopped = false;
    }

    public void Damage(int dmg)
    {
        PlaySound(sounds[0], 0.6f);
        animations.TriggerHit();
        hp -= dmg;
        isAggro = true;
        animations.isAggro = true;

        if (healthBar != null)
            healthBar.SetHealth(hp, maxHp);

        if (hp <= 0)
        {
            if (UnityEngine.Random.value <= 0.35f && heartPrefab != null)
            {
                Instantiate(heartPrefab, transform.position, Quaternion.identity);
            }
            gameController.UnregisterEnemy(this);
            animations.TriggerDeath();
            agent.isStopped = true;
            this.enabled = false;
        }

    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        PersonScript player = otherCollider.GetComponent<PersonScript>();
        if (player != null && !player.isEnemy)
        {
            player.Damage(5);
        }
        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (!shot.isEnemyShot)
            {
                Damage(shot.damage);
                Destroy(shot.gameObject);
            }
        }
    }

    void Spin()
    {
        isRightSide = !isRightSide;
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * (isRightSide ? 1 : -1);
        transform.localScale = scale;
    }
}
