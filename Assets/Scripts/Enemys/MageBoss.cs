using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBoss : MonoBehaviour, IEnemy
{
    [Header("Health")]
    public int maxHealth;
    int currentHealth;

    [Header("Fight")]
    public GameObject fireball;
    public Transform fireballStartPos;

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

        FireStorm();
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
        myRB.gravityScale = 1.0f;
        myAnim.SetTrigger("Die");
        Destroy(this);
    }
    #endregion

    #region Fight

    void FireStorm()
    {
        StartCoroutine(SpellLoading());

        float offset = 1.5f;
        for (int i = 0; i < 19; i++)
        {
            fireballStartPos.position = 
                new Vector3(fireballStartPos.position.x + offset, 
                fireballStartPos.position.y,
                fireballStartPos.position.z);

            Instantiate(fireball, fireballStartPos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    IEnumerator SpellLoading()
    {
        myAnim.SetBool("SpellLoading", true);
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("SpellLoading", false);

    }

    #endregion
}
