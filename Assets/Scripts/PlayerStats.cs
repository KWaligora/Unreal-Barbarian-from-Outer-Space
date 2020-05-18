using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //health
    public int maxHealth;
    int currentHealth;

    //other
    public float pushBackForce;
    Animator myAnim;
    PlayerController playerController;

    //HUD
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int dmg, Transform enemy)
    {
        if (currentHealth > 0)
        {
            if (!playerController.isBlocking())
            {
                currentHealth -= dmg;
                healthSlider.value = currentHealth;
                if (currentHealth <= 0)
                    Die();
                else
                    myAnim.SetTrigger("Hurt");
                pushBack(enemy, pushBackForce);
            }
            else
            {                
                currentHealth -= Mathf.RoundToInt(dmg/8.0f);
                healthSlider.value = currentHealth;
                if (currentHealth <= 0)
                    Die();
                else
                pushBack(enemy, pushBackForce / 2.0f);
            }
        }
    }

    void pushBack(Transform enemy, float pushForce)
    {
        StartCoroutine(FreezeController());

        Vector2 pushDirection = new Vector2(transform.position.x - enemy.position.x,
           (transform.position.y - enemy.position.y)).normalized;

        pushDirection *= pushForce;

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
