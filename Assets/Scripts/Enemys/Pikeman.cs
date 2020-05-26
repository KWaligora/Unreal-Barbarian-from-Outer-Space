﻿using System.Collections;
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
            audioSource.PlayOneShot(attackS);
            StartCoroutine(HitSound(attackS.length));
            StartCoroutine(HitSound(attackS.length * 2));

            if (player.tag == "Player")
            {
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), sendDamageDelay, damage));
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), sendDamageDelay * 2, damage));
                StartCoroutine(SendTrueDamage(player.gameObject.GetComponent<PlayerStats>(), sendDamageDelay * 3, damage));
                break;
            }
        }
    }

    IEnumerator HitSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(attackS);
    }
}
