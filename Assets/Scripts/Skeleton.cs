using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IEnemy
{
    //move
    public float maxRangeRight;
    public float maxRangeLeft;

    //attack
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int damage;
    public float attackRatio;
    bool canAttack = true;

    //health
    public int maxHealth;
    int currentHealth;

    //other
    Animator myAnim;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canAttack)
        {
            StartCoroutine(AttackDelay(attackRatio));
            Attack();
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
        {
            myAnim.SetTrigger("Hit");
        }
    }

    void Die()
    {
        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(myRB);
        Destroy(this);
        myAnim.SetTrigger("Die");
    }

    void Attack()
    {
        myAnim.SetTrigger("Attack");
        Collider2D [] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach(Collider2D player in collider)
        {
            if(player.tag == "Player")
            {
                StartCoroutine(SendDamage(player.gameObject.GetComponent<PlayerStats>()));
                break;
            }
        }

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
}
