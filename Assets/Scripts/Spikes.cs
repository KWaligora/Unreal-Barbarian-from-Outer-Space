using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    bool canHurt = true;
    public int damage;

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (canHurt)
            {
                StartCoroutine(sentDamage(collision));
            }
        }
        if (collision.tag == "Enemy")
            collision.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
    }

    IEnumerator sentDamage(Collider2D player)
    {
        canHurt = false;
        player.gameObject.GetComponent<PlayerStats>().TakeDamage(damage, transform);
        yield return new WaitForSeconds(1);
        canHurt = true;
    }
}
