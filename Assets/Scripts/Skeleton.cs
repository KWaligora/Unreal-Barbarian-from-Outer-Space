using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IEnemy
{
    public int maxHealth;
    int currentHealth;
    Animator myAnim;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
            myAnim.SetTrigger("Hit");
    }

    void Die()
    {
        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(myRB);
        myAnim.SetTrigger("Die");
    }
}
