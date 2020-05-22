﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Move_Var
    public float maxSpeed;
    public LayerMask groundLayer;
    public Transform wallsCheck;
    Collider2D touchingWalls;
    float currentSpeed;
    bool facingLeft = true;
    #endregion

    #region Attack_Var
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int damage;
    public float attackRatio;
    bool canAttack = true;
    #endregion

    #region Health_Var
    public int maxHealth;
    int currentHealth;
    #endregion

    #region Other_Var
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
        touchingWalls = Physics2D.OverlapCircle(wallsCheck.position, 0.2f, groundLayer);
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

    #endregion

    #region TakeDamage

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(SetTint());
    }

    IEnumerator SetTint()
    {
        material.SetColor("_Color1", new Color(2, 2, 2, 1));
        yield return new WaitForSeconds(0.25f);
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

    protected virtual void CreateExpBall() { } //Override 

    #endregion
    
    #region Attack

    protected virtual void Attack() { } //Override        

    IEnumerator AttackDelay(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    protected IEnumerator SendDamage(PlayerStats player)
    {
        yield return new WaitForSeconds(0.5f);
        player.TakeDamage(damage, transform);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && canAttack)
        {
            float playerXPos = collision.gameObject.transform.position.x;
            if (playerXPos < transform.position.x && !facingLeft) flip();
            else if (playerXPos > transform.position.x && facingLeft) flip();

            StartCoroutine(AttackDelay(attackRatio));
            Attack();
            currentSpeed = 0;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        currentSpeed = maxSpeed;
    }

    #endregion
}
