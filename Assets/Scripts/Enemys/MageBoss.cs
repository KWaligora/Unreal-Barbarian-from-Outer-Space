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
    #endregion

    #region Fight_Var
    [Header("Fight")]
    public GameObject fireball;
    public GameObject flameThrower;
    public Transform fireballStartPos;
    public Transform flameThrowerPos;
    bool canAttack = true;
    bool canDealDamage = false;
    #endregion

    #region Audio_Var
    [Header("Audio")]
    public AudioClip deathS;
    public AudioClip hitS;
    public AudioClip fireStormS;
    public AudioClip flameThrowerS;
    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canAttack)
            StartCoroutine(LoadNextSpell());
    }

    #region Movement

    IEnumerator ChangePosition()
    {
        yield return new WaitForSeconds(2.0f);
        FindNextPosition();
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
        audioSource.PlayOneShot(hitS);
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
        audioSource.PlayOneShot(deathS);
        myRB.gravityScale = 1.0f;
        myAnim.SetTrigger("Die");
        material.SetColor("_Color1", new Color(1, 1, 1, 1));
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
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(ChangePosition());
        canAttack = true;
    }

    void FireStorm()
    {
        audioSource.PlayOneShot(fireStormS);
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
        audioSource.PlayOneShot(flameThrowerS);
        myAnim.SetTrigger("Attack");

        if (facingLeft)
            Instantiate(flameThrower, flameThrowerPos.position, Quaternion.Euler(new Vector3(0, 0, 180f)));        
        else
            Instantiate(flameThrower, flameThrowerPos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    IEnumerator SpellLoading()
    {
        myAnim.SetBool("SpellLoading", true);
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("SpellLoading", false);

    }

    #endregion
}
