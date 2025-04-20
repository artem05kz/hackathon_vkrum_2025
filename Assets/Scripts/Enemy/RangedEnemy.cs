using System;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Sounds
{
    [Header("ѕараметры врага")]
    public int maxHp = 40;
    public int hp;
    public float aggroRange = 10f;
    public float attackCooldown = 2f;
    public int attackDamage = 20;
    public float stepAwayDistance = 2f; // Ќа сколько враг отходит после выстрела
    public float minDistance = 4f;      // не подходить ближе этой дистанции
    public bool isRightSide = true;
    public GameObject heartPrefab;


    private float attackTimer = 0f;
    public bool isAggro = false;
    private bool isAttacking = false; // чтобы блокировать повторные выстрелы

    //  омпоненты
    private NavMeshAgent agent;
    private Transform playerTransform;
    public RangedAnimations animations; 
    public Transform shotPrefab;
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
                gameController.RegisterEnemy(this); // регистрируем себ€
            }
        }
        healthBar = GetComponentInChildren<HealthBarUIEnemy>();
        if (healthBar != null)
            healthBar.SetHealth(hp, maxHp);

        timeBody = GetComponent<TimeBody>();
        hp = maxHp;
        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        animations = GetComponentInChildren<RangedAnimations>();
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

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (!isAggro && dist <= aggroRange)
        {
            isAggro = true;
            animations.isAggro = true;
        }

        if (!isAggro || isAttacking) return;

        attackTimer += Time.deltaTime;

        if (dist < minDistance)
        {
            // уходим от игрока
            Vector3 fleeDir = (transform.position - playerTransform.position).normalized;
            Vector3 target = transform.position + fleeDir * stepAwayDistance;
            agent.isStopped = false;
            agent.SetDestination(target);
            animations.isMoving = true;
            animations.isIdle = false;
        }
        else if (attackTimer < attackCooldown)
        {
            // держим позицию, готовимс€
            agent.isStopped = true;
            animations.isMoving = false;
            animations.isIdle = true;
        }
        else
        {
            // стрел€ем
            StartRangedAttack();
            attackTimer = 0f;
        }
    }

    void StartRangedAttack()
    {
        isAttacking = true;
        animations.TriggerAttack();
    }

    // Ётот метод вызываетс€ по окончании анимации атаки
    public void OnRangedShot()
    {
        PlaySound(sounds[1], 0.2f);
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        Transform shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        shot.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        var sh = shot.GetComponent<ShotScript>();
        if (sh != null) { sh.isEnemyShot = true; sh.damage = attackDamage; }

        var mv = shot.GetComponent<MoveScript>();
        if (mv != null) mv.direction = dir;

        isAttacking = false;
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void Damage(int damageAmount)
    {
        PlaySound(sounds[0], 0.6f);
        animations.TriggerHit();
        hp -= damageAmount;
        isAggro = true;
        if (animations != null)
            animations.isAggro = true;

        if (healthBar != null)
            healthBar.SetHealth(hp, maxHp);

        if (hp <= 0)
        {
            if (UnityEngine.Random.value <= 0.1f && heartPrefab != null)
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
