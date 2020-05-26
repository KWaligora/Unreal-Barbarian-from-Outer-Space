using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    #region Health_Var
    [Header ("Health")]
    public Slider healthSlider;
    public int maxHealth;
    int currentHealth;
    bool dead = false;
    #endregion

    #region Exp_Var
    [Header ("Exp")]
    public Slider expSlider;
    int currentExp;
    int requiredExp;
    #endregion

    #region Audio_Var
    [Header("Audio")]
    public AudioClip pickUpS;
    public AudioClip playerHitS;
    public AudioClip playerDeathS;
    AudioSource audioSource;
    #endregion

    #region Other_Var
    [Header("Other")]
    public MainCamera camera;
    Animator myAnim;
    PlayerController playerController;
    LvlManager lvlManager;
    #endregion
    
    void Start()
    {        
        myAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        //health
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        //exp
        currentExp = 0;
        requiredExp = 100;
        expSlider.value = currentExp;
        expSlider.maxValue = requiredExp;

        //CheckPoint
        lvlManager = GameObject.FindGameObjectWithTag("LvlManager").GetComponent<LvlManager>();
        transform.position = lvlManager.lastCheckpoint;
    }

    #region TakeDamage
    public void TakeDamage(int dmg, Transform enemyTransform, float pushBackForce)
    {
        if (currentHealth > 0)
        {
            if (!playerController.isBlocking())
            {
                TakeHealth(dmg);
                PushBack(enemyTransform, pushBackForce);
                myAnim.SetTrigger("Hurt");
            }

            else
            {
                dmg = Mathf.RoundToInt(dmg / 8.0f);
                TakeHealth(dmg);                
                PushBack(enemyTransform, pushBackForce / 2.0f);
            }
            
        }
    }

    public void TakeTrueDamage(int dmg, Transform enemyTransform, float pushBackForce)
    {
        if(currentHealth > 0) {
            TakeHealth(dmg);
            myAnim.SetTrigger("Hurt");
            PushBack(enemyTransform, pushBackForce);        
        }
    }

    void TakeHealth(int dmg)
    {
        currentHealth -= dmg;
        healthSlider.value = currentHealth;        

        if (currentHealth <= 0 && !dead)
            StartCoroutine(Die());            
    }

    void PushBack(Transform enemy, float pushForce)
    {
        StartCoroutine(FreezeController());

        Vector2 pushDirection = new Vector2(transform.position.x - enemy.position.x,
           (transform.position.y - enemy.position.y)).normalized;

        pushDirection *= pushForce;

        Rigidbody2D myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = Vector2.zero;
        myRB.AddForce(pushDirection, ForceMode2D.Impulse);
    }

    IEnumerator Die()
    {
        dead = true;
        camera.Stop();
        audioSource.PlayOneShot(playerDeathS);
        myAnim.SetTrigger("Death");
        playerController.enabled = false;
        yield return new WaitForSeconds(playerDeathS.length);
        Respawn();
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
        audioSource.PlayOneShot(pickUpS);
        currentExp += exp;
        if (currentExp >= requiredExp)
        {
            LvlUp(currentExp);
            return;
        }
        expSlider.value = currentExp;    }

    void LvlUp(int currentExp)
    {       
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
        audioSource.PlayOneShot(pickUpS);
        currentHealth += health;
        healthSlider.value = currentHealth;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
