﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : MonoBehaviour, IEnemy
{
    //move
    public Transform pos1, pos2;
    public float maxSpeed;
    float currentSpeed;
    bool facingLeft = true;
    bool canflip = true;

    //attack
    public int damage;
    public float attackRatio;
    public GameObject arrow;
    public Transform arrowPlace;
    bool canAttack = true;
    

    //health
    public int maxHealth;
    int currentHealth;

    //other
    public int expValue;
    Animator myAnim;
    Material material;
    Rigidbody2D myRb;
    public GameObject expBall;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        myRb = GetComponent<Rigidbody2D>();
        maxSpeed *= -1;
        currentSpeed = maxSpeed;

    }

    void Update()
    {
        myRb.velocity = new Vector2(currentSpeed, myRb.velocity.y);
        //movement       
        myAnim.SetFloat("Speed", Mathf.Abs(myRb.velocity.x));

        if (transform.position.x <= pos1.position.x && canflip)
            flip();

        else if (transform.position.x >= pos2.position.x && canflip)
            flip();

        canflip = CheckFlip();
    }


    bool CheckFlip()
    {
        if (transform.position.x > pos1.position.x && transform.position.x < pos2.position.x)
            return true;
        else return false;
    }

    void flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        currentSpeed *= -1;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
        {
            StartCoroutine(SetTint());
            myAnim.SetTrigger("Hit");
        }
    }

    void Die()
    {
        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(myRB);
       
        this.gameObject.layer = 0;
        myAnim.SetTrigger("Die");
        material.SetColor("_Color1", new Color(1, 1, 1, 1));

        ExpBall ball = Instantiate(expBall, GetComponentInParent<BowMan>().transform.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<ExpBall>();
        ball.Init(expValue);

        Destroy(this);
    }

    void Attack()
    {
        myAnim.SetTrigger("Attack");

        if (facingLeft)
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 180f))).gameObject.GetComponent<Arrow>().SetDamage(damage);
        else
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<Arrow>().SetDamage(damage);


    }

    IEnumerator AttackDelay(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    IEnumerator SendDamage(PlayerStats player)
    {
        yield return new WaitForSeconds(0.5f);
        player.TakeDamage(damage, transform);
    }

    IEnumerator SetTint()
    {
        material.SetColor("_Color1", new Color(2, 2, 2, 1));
        yield return new WaitForSeconds(0.25f);
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
    }

    private void OnTriggerStay2D(Collider2D collision)
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentSpeed = maxSpeed;
    }
}