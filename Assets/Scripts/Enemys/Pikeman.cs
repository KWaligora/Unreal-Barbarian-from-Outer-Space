using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikeman : Enemy, IEnemy
{
    protected override void HeavyAttack()
    {      
        myAnim.SetTrigger("Attack2");
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in collider)
        {
            if (player.tag == "Player")
            {
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), delay, damage));
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), delay * 2, damage));
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), delay * 3, damage));
                break;
            }
        }
    }
}
