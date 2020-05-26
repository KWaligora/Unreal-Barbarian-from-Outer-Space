using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Move_Var
    [Header ("Move")]
    public float maxSpeed;
    float isCrouching = 0.0f;
    CapsuleCollider2D collider;
    #endregion

    #region Jump_Var
    [Header("Jump")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    bool grounded = false;
    bool canJump = true;
    float groundCheckRadius = 0.2f;
    #endregion

    #region Melee_Attack_Var
    [Header ("Meele Attack")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public int meleeAttackDMG;
    public float attackRatio;
    bool attacking = false;
    bool blocking = false;
    int attackNum = 1;
    #endregion

    #region Laser_Var
    [Header ("Laser")]
    public Slider laserSlider;
    public GameObject laserBeam;
    public Transform gunTip;
    int maxLaserCharge = 5;
    int currentLaserCharge;
    #endregion

    #region Other_Var
    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight = true;
    #endregion

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        currentLaserCharge = 0;
        laserSlider.value = 0;
    }

    void Update()
    {
        //attack
        if (!attacking && Input.GetAxis("Fire1") > 0)
            StartCoroutine(MeleeAttack());
        else if (!attacking && Input.GetAxis("Fire2") > 0)
            StartCoroutine(LaserAttack());
        //block
        if (grounded && Input.GetAxis("Fire3") > 0 && isCrouching == 0.0f)
            Block(true);
        else
            Block(false);

        //crouch        
        Crouch();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("Grounded", grounded);
        //jump
        if (grounded && Input.GetAxis("Jump") > 0 && canJump)
            StartCoroutine(Jump());

        //move        
        Movement();
    }

    #region Move
    void Movement()
    {
        float move = Input.GetAxis("Horizontal");
        if (isCrouching >= 0 && !blocking)
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

    IEnumerator Jump()
    {
        canJump = false;
        myRB.AddForce(new Vector2(0, jumpHeight));
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }
    #endregion

    #region Fight
    IEnumerator MeleeAttack()
    {
        attacking = true;
        if (attackNum == 1)
            myAnim.SetTrigger("Attack");
        else
            myAnim.SetTrigger("Attack2");

        Collider2D [] HitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in HitEnemys)
        {
            enemy.GetComponent<IEnemy>().TakeDamage(meleeAttackDMG);
        }
        attackNum *= -1;
        yield return new WaitForSeconds(attackRatio);
        attacking = false;
    }

    IEnumerator LaserAttack()
    {
        if (currentLaserCharge > 0)
        {
            currentLaserCharge--;
            laserSlider.value = currentLaserCharge;

            attacking = true;
            myAnim.SetTrigger("LaserAttack");
            if (facingRight)
                Instantiate(laserBeam, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            else
                Instantiate(laserBeam, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
            yield return new WaitForSeconds(0.25f);
            attacking = false;
        }
    }

    void Block(bool isblocking)
    {
        blocking = isblocking;
        if(blocking)
            myRB.velocity = new Vector2(0, myRB.velocity.y);

        myAnim.SetBool("Block", isblocking);        
    }

    void Crouch()
    {
        isCrouching = Input.GetAxis("Vertical");
        if (grounded && isCrouching < 0.0)
        {
            myAnim.SetBool("Crouch", true);
            myRB.velocity = new Vector2(0, myRB.velocity.y);

            collider.size = new Vector2(collider.size.x, 1.5f);
            collider.offset = new Vector2(collider.offset.x, -0.627f);
        }
        else
        {
            myAnim.SetBool("Crouch", false);
            collider.size = new Vector2(collider.size.x,  2.34f);
            collider.offset = new Vector2(collider.offset.x, -0.212f);
        }
    }

    public bool isBlocking()
    {
        return blocking;
    }

    public void AddLaserCharge()
    {
        if (currentLaserCharge < maxLaserCharge)
        {
            currentLaserCharge++;
            laserSlider.value = currentLaserCharge;
        }
    }
    #endregion
}
