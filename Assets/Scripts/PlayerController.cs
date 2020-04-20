using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //move var
    public float maxSpeed;

    //jump var
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    bool grounded = false;
    float groundCheckRadius = 0.2f;    

    //attack var
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    bool attackEnable = true;
    bool isAttacking = false;
    int attackNum = 1;    

    //crouch var
    float isCrouching = 0.0f;

    //other
    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight = true;
   
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //attack
        if (!isAttacking && Input.GetAxis("Fire1") > 0)
            StartCoroutine(MeleeAttack());
        else if (!isAttacking && Input.GetAxis("Fire2") > 0)
            StartCoroutine(LaserAttack());

        //crouch        
        Crouch();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("Grounded", grounded);
        //jump
        if (grounded && Input.GetAxis("Jump") > 0)
            Jump();

        //move        
        Movement();
    }

    void Movement()
    {
        float move = Input.GetAxis("Horizontal");
        if (isCrouching>=0)
        {
            myRB.velocity = new Vector2(maxSpeed * move, myRB.velocity.y);
            myAnim.SetFloat("Speed", Mathf.Abs(move));

            if (move < 0 && facingRight)
                Flip();
            else if (move > 0 && !facingRight)
                Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Jump()
    {
        myRB.AddForce(new Vector2(0, jumpHeight));
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        if (attackNum == 1)
            myAnim.SetTrigger("Attack");
        else
            myAnim.SetTrigger("Attack2");

        Collider2D [] HitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in HitEnemys)
        {
            Debug.Log("hit enemy:" + enemy.name);
            enemy.GetComponent<IEnemy>().TakeDamage(1);
        }
        attackNum *= -1;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator LaserAttack()
    {
        isAttacking = true;
        myAnim.SetTrigger("LaserAttack");
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }

    void Crouch()
    {
        isCrouching = Input.GetAxis("Vertical");
        if (grounded && isCrouching < 0.0)
        {
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            myAnim.SetBool("Crouch", true);
        }
        else
            myAnim.SetBool("Crouch", false);        
    }
}
