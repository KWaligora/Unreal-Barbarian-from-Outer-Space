using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //health
    public int maxHealth;
    int currentHealth;

    //other
    public float pushBackForce;
    Animator myAnim;
    PlayerController playerController;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int dmg, Transform enemy)
    {
        if (!playerController.isBlocking())
        {
            currentHealth -= dmg;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
                Die();
            else
                myAnim.SetTrigger("Hurt");
            pushBack(enemy);
        }
    }

    void pushBack(Transform enemy)
    {
        StartCoroutine(FreezeController());

        Vector2 pushDirection = new Vector2(transform.position.x - enemy.position.x,
           (transform.position.y - enemy.position.y)).normalized;

        pushDirection *= pushBackForce;

        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = Vector2.zero;
        myRB.AddForce(pushDirection, ForceMode2D.Impulse);
    }

    void Die()
    {
        myAnim.SetTrigger("Death");
        playerController.enabled = false;
    }

    IEnumerator FreezeController()
    {
        playerController.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if(currentHealth>0)
            playerController.enabled = true;
    }
}
