using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Animator animator;
    public bool isMoving;
    public bool isDead;
    public bool isShot;
    public bool isAiming;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isShot", isShot);
        animator.SetBool("isAiming", isAiming);
    }
    public void ShotAnimationEnd()
    {
        isShot = false;
    }
    public void TriggerRevive()
    {
        animator.SetTrigger("Revive");
    }
}