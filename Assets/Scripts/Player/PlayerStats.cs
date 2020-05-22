using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    #region Health_Var
    [Header ("Health")]
    public Slider healthSlider;
    public int maxHealth;
    int currentHealth;
    #endregion

    #region Exp_Var
    [Header ("Exp")]
    public Slider expSlider;
    int currentExp;
    int requiredExp;
    #endregion

    #region Other_Var
    [Header ("Other")]
    public float pushBackForce;
    Animator myAnim;
    PlayerController playerController;
    #endregion
    
    void Start()
    {        
        myAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        //health
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        //exp
        currentExp = 0;
        requiredExp = 100;
        expSlider.value = currentExp;
        expSlider.maxValue = requiredExp;
    }

    #region TakeDamage
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
    #endregion

    #region Exp
    public void AddExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= requiredExp)
        {
            LvlUp(currentExp);
            return;
        }
        expSlider.value = currentExp;
        Debug.Log(currentExp);
    }

    void LvlUp(int currentExp)
    {
        Debug.Log("lvl up");
        currentExp -= requiredExp;
        expSlider.value = currentExp;

        //health up
        maxHealth += 10;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    #endregion

    public void AddHealth(int health)
    {
        currentHealth += health;
        healthSlider.value = currentHealth;
    }
}
