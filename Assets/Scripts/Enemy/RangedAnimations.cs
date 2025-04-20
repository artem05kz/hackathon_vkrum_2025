using UnityEngine;

public class RangedAnimations : MonoBehaviour
{
    public Animator animator;

    public bool isMoving;
    public bool isAggro;
    public bool isIdle;
    public bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAggro", isAggro);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isAttacking", isAttacking);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerDeath()
    {
        animator.SetTrigger("Death");
    }
    public void DeathEnd()
    {
        Destroy(transform.root.gameObject);
    }
    public void TriggerHit()
    {
        animator.SetTrigger("Hit");
    }

}
