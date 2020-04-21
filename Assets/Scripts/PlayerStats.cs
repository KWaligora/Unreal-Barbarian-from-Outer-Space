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

    void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        myAnim.SetTrigger("Death");
        playerController.enabled = false;
    }
}
