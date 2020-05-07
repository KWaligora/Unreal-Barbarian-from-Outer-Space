using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IEnemy
{
    //move
    public Transform pos1, pos2;
    public float speed;
    bool facingLeft = true;
    bool canflip = true;

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
    Material material;
    Rigidbody2D myRb;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        myRb = GetComponent<Rigidbody2D>();
        speed *= -1;         
    }

    void Update()
    {
        //movement
        myRb.velocity = new Vector2(speed, myRb.velocity.y);
        Debug.Log(myRb.velocity);
        myAnim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

        if (transform.position.x <= pos1.position.x && canflip)
            flip();

        else if (transform.position.x >= pos2.position.x && canflip)
            flip();

        canflip = CheckFlip();    

        //attack
        if (canAttack)
          {
              StartCoroutine(AttackDelay(attackRatio));
              //Attack();
          }
    }


    bool CheckFlip()
    {
        if (transform.position.x > pos1.position.x && transform.position.x < pos2.position.x)
            return true;
        else return false;
    }

    void flip()
    {
        Debug.Log("true");
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        speed *= -1;
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

    IEnumerator SetTint()
    {
        material.SetColor("_Color1", new Color(2, 2, 2, 1));
        yield return new WaitForSeconds(0.25f);
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
    }
}
