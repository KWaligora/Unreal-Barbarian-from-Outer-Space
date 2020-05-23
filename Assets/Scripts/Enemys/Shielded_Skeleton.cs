using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielded_Skeleton : Enemy, IEnemy
{
    bool isBlocking = false;
    bool canBlock = true;

    private void Update()
    {
        SetMovement();
    }

    protected override void LightAttack()
    {
        if (!isBlocking)
        {
            StartCoroutine(AttackDelay(attackRatio));

            myAnim.SetTrigger("Attack1");
            Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
            foreach (Collider2D player in collider)
            {
                if (player.tag == "Player")
                {
                    StartCoroutine(SendDamage(player.gameObject.GetComponent<PlayerStats>(), delay));
                    StartCoroutine(SendDamage(player.gameObject.GetComponent<PlayerStats>(), delay * 2.0f));
                    break;
                }
            }
        }
    }

    protected override void HeavyAttack()
    {
        if(!isBlocking)
            base.HeavyAttack();
    }

    public override void TakeDamage(int dmg)
    {
        if (!isBlocking)
        {
            base.TakeDamage(dmg);

            if (Random.Range(0, 2) == 1 && canBlock)
            {                
                StartCoroutine(Block());                
            }
        }
    }

    IEnumerator Block()
    {        
        myAnim.SetBool("Block", true);
        isBlocking = true;
        currentSpeed = 0;

        yield return new WaitForSeconds(2.0f);

        myAnim.SetBool("Block", false);
        isBlocking = false;
        StopAllCoroutines();
        StartCoroutine(BlockDelay());
        LightAttack();
    }

    IEnumerator BlockDelay()
    {
        canBlock = false;
        yield return new WaitForSeconds(3.0f);
        canBlock = true;
    }

    
}
