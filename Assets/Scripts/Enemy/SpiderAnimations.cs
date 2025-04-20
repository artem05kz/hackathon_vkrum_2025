using UnityEngine;

public class SpiderAnimations : MonoBehaviour
{
    private Animator animator;
    public bool isMoving;
    public bool isDead;
    public bool isSpawn;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isSpawn", isSpawn);
    }
    public void DeadAnimationEnd()
    {
        Destroy(transform.root.gameObject);
    }
    public void SpawnAnimationEnd()
    {
        isSpawn = false;
        isMoving = true;
    }
}