using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBoss : MonoBehaviour, IEnemy
{
    #region Health_Var
    [Header("Health")]
    public int maxHealth;
    int currentHealth;
    #endregion

    #region Movement_Var
    [Header("Movement")]
    public Transform[] positions;
    bool facingLeft = true;
    bool changingPos = false;
    #endregion

    #region Fight_Var
    [Header("Fight")]
    public GameObject fireball;
    public Transform fireballStartPos;
    bool canAttack = true;
    bool canDealDamage = false;
    #endregion

    #region Other_Var
    [Header("Other")]
    Animator myAnim;
    Material material;
    Rigidbody2D myRB;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        myRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canAttack)
            StartCoroutine(LoadNextSpell());
    }

    #region Movement

    IEnumerator ChangePosition()
    {
        changingPos = true;
        yield return new WaitForSeconds(2.0f);
        FindNextPosition();
        changingPos = false;
    }

    void FindNextPosition()
    {
        int nextPosition = Random.Range(0, 4);
        if (transform.position == positions[nextPosition].position)           
        {
            if (nextPosition == 3) nextPosition = 0;

            else
                nextPosition++;            
        }
        StartCoroutine(Teleport(nextPosition));
    }

    IEnumerator Teleport(int nextPosition)
    {
        StartCoroutine(SpellLoading());
        yield return new WaitForSeconds(1.0f);
        transform.position = positions[nextPosition].position;
        CheckFlip(nextPosition);
    }

    void CheckFlip(int nextPosition)
    {
        if (nextPosition % 2 == 0 && !facingLeft)
            flip();
        else if (nextPosition % 2 != 0 && facingLeft)
            flip();
    }

    void flip()
    {
        if (facingLeft)
            facingLeft = false;
        else
            facingLeft = true;

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

    IEnumerator LoadNextSpell()
    {
        canAttack = false;
        int nextSpell = Random.Range(0, 2);
        if (nextSpell == 1)
            FireStorm();
        else
            FlameThrower();
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(ChangePosition());
        canAttack = true;

    }

    void FireStorm()
    {
        StartCoroutine(SpellLoading());
        Vector3 nextPosition = fireballStartPos.position;

        float offset = 1.5f;
        for (int i = 0; i < 19; i++)
        {
            nextPosition = 
                new Vector3(fireballStartPos.position.x + offset, 
                fireballStartPos.position.y,
                fireballStartPos.position.z);

            Instantiate(fireball, nextPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
            offset += 1.5f;
        }
    }

    void FlameThrower()
    {
        myAnim.SetTrigger("Attack");
    }

    IEnumerator SpellLoading()
    {
        myAnim.SetBool("SpellLoading", true);
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("SpellLoading", false);

    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            canDealDamage = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            canDealDamage = false;
    }
}
