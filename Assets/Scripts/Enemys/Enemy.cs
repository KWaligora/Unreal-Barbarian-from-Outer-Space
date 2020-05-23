using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Move_Var
    [Header ("Move")]
    public float maxSpeed;
    public LayerMask groundLayer;
    public Transform wallsCheck;
    protected bool facingLeft = true;
    protected float currentSpeed;
    Collider2D touchingWalls;       
    #endregion

    #region Attack_Var
    [Header ("Attack")]
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int damage;
    public float attackRatio;
    public float delay;
    public float lightPushBackForce;
    public float heavyPushBackForce;
    protected bool canAttack = true;
    #endregion

    #region Health_Var
    [Header ("Health")]
    public int maxHealth;
    int currentHealth;
    #endregion

    #region Other_Var
    [Header ("Other")]
    public int expValue;
    public GameObject expBall;
    protected Animator myAnim;
    Material material;
    Rigidbody2D myRb;    
    #endregion
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        myRb = GetComponent<Rigidbody2D>();
        maxSpeed *= -1;
        currentSpeed = maxSpeed;
    }

    #region Movement

    protected void SetMovement()
    {
        myRb.velocity = new Vector2(currentSpeed, myRb.velocity.y);
        myAnim.SetFloat("Speed", Mathf.Abs(myRb.velocity.x));
        CheckFlip();
    }

    void CheckFlip()
    {
        touchingWalls = Physics2D.OverlapCircle(wallsCheck.position, 0.1f, groundLayer);
        if (touchingWalls && touchingWalls.tag.Equals("Walls"))
            flip();
    }

    void flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        currentSpeed *= -1;
    }

    protected void SetSpeed()
    {
        if (facingLeft)
            currentSpeed = maxSpeed;
        else
            currentSpeed = -maxSpeed;
    }
    #endregion

    #region TakeDamage

   public virtual void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(SetHitTint());
    }

    IEnumerator SetHitTint()
    {
        material.SetColor("_Color1", new Color(2, 2, 2, 1));
        yield return new WaitForSeconds(0.25f);
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
    }

    IEnumerator SetLoadingTint()
    {
        material.SetColor("_Color1", new Color(0.8f, 0.2f, 0.2f, 1));
        yield return new WaitForSeconds(1.0f);
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
    }

    void Die()
    {
        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(myRB);

        this.gameObject.layer = 0;
        myAnim.SetTrigger("Die");
        material.SetColor("_Color1", new Color(1, 1, 1, 1));

        CreateExpBall();        

        Destroy(this);
    }

    void CreateExpBall()
    {
        ExpBall ball = Instantiate(expBall, this.gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<ExpBall>();
        ball.Init(expValue);
    }

    #endregion
    
    #region Attack

    protected virtual void LightAttack()
    {
        StartCoroutine(AttackDelay(attackRatio));

        myAnim.SetTrigger("Attack1");
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in collider)
        {
            if (player.tag == "Player")
            {               
                StartCoroutine(SendDamage(player.gameObject.GetComponent<PlayerStats>(), delay));
                break;
            }
        }
    }       

    protected virtual void HeavyAttack()
    {
        myAnim.SetTrigger("Attack2");
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in collider)
        {
            if (player.tag == "Player")
            {
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), delay, damage * 2));
                break;
            }
        }
    }

    IEnumerator HeavyAttackLoading()
    {
        StartCoroutine(AttackDelay(3.0f));
        StartCoroutine(SetLoadingTint());
        yield return new WaitForSeconds(1.0f);
        HeavyAttack();
    }

    protected IEnumerator AttackDelay(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    protected IEnumerator SendDamage(PlayerStats player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.TakeDamage(damage, transform, lightPushBackForce);
    }

    protected IEnumerator SendTrueDamage(PlayerStats player, float delay, int dmg)
    {
        yield return new WaitForSeconds(delay);
        player.TakeTrueDamage(dmg, transform, heavyPushBackForce);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (canAttack)
            {
                //if player is behind, flip
                float playerXPos = collision.gameObject.transform.position.x;
                if (playerXPos < transform.position.x && !facingLeft) flip();
                else if (playerXPos > transform.position.x && facingLeft) flip();

                if (Random.Range(0, 3) == 2)
                {
                    StartCoroutine(HeavyAttackLoading());
                }
                else
                    LightAttack();               
            }
            currentSpeed = 0;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        SetSpeed();
    }

    #endregion
}
