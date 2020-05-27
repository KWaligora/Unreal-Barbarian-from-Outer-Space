using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBoss : MonoBehaviour, IEnemy
{
    [Header("Health")]
    public int maxHealth;
    int currentHealth;

    [Header("Other")]
    Animator myAnim;
    Material material;
    Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        myRB = GetComponent<Rigidbody2D>();
    }

    #region Movement
    void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion

    #region Take_Damage
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
        else
           StartCoroutine(SetHitTint());
    }

    IEnumerator SetHitTint()
    {
        material.SetColor("_Color1", new Color(2, 2, 2, 1));
        yield return new WaitForSeconds(0.25f);
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
    }

    void Die()
    {
        myAnim.SetTrigger("Die");
        Destroy(this);
    }
    #endregion

    #region Fight

    void FireStorm()
    {

    }

    #endregion
}
