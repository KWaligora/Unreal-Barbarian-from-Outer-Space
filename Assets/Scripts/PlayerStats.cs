using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //health
    public int maxHealth;
    int currentHealth;

    //other
    Animator myAnim;
    PlayerController playerController;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int dmg)
    {
        if (!playerController.isBlocking())
        {
            currentHealth -= dmg;
            if (currentHealth <= 0)
                Die();
            else
                myAnim.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        myAnim.SetTrigger("Death");
        playerController.enabled = false;
    }

    public bool isDead()
    {
        return currentHealth<=0;
    }
}
