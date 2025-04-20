using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : Sounds
{
    private Transform playerTransform;

    private NavMeshAgent agent;
    private Rigidbody2D rigidbodyComponent;

    public float attackRange = 1f;
    public float attackCooldown = 1.0f;
    private float attackTimer = 0f;
    public int attackDamage = 10;

    public int maxHp = 15;
    private int hp;
    public bool isRightSide = false;

    private SpiderAnimations animations;

    void Start()
    {
        animations = GetComponentInChildren<SpiderAnimations>();
        if (animations == null)
            UnityEngine.Debug.LogError("SpiderAnimations не найден в потомках объекта SpiderEnemy");
        else
        {
            animations.isSpawn = true;
            animations.isMoving = false;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rigidbodyComponent = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        hp = maxHp;
    }

    void FixedUpdate()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        if (rigidbodyComponent == null)
            UnityEngine.Debug.LogError("Rigidbody2D не найден на объекте SpiderEnemy.");

    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.x < transform.position.x && !isRightSide)
            {
                Spin();
            }
            else if (playerTransform.position.x > transform.position.x && isRightSide)
            {
                Spin();
            }
        }
        attackTimer += Time.deltaTime;
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                if (attackTimer >= attackCooldown)
                {
                    AttackPlayer();
                    attackTimer = 0f;
                }
            }
        }   
    }

    void AttackPlayer()
    {
        PersonScript player = playerTransform.GetComponent<PersonScript>();
        if (player != null)
        {
            PlaySound(sounds[1], 0.4f);
            player.Damage(attackDamage);
        }
    }

    public void Damage(int damageAmount)
    {
        hp -= damageAmount;
        UnityEngine.Debug.Log($"SpiderEnemy получил урон: {damageAmount}. Осталось HP: {hp}");

        if (hp <= 0)
        {
            PlaySound(sounds[0], 0.2f);
            animations.isMoving = false;
            animations.isDead = true;
            agent.isStopped = true;
            this.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
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
