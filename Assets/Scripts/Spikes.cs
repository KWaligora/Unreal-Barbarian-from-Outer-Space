using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    bool canHurt = true;
    public float pushBackForce;
    public int damage;

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (canHurt)
            {
                StartCoroutine(sentDamage(collision));
                StartCoroutine(DisableController(collision));
            }
        }
    }

    IEnumerator sentDamage(Collider2D player)
    {
        canHurt = false;
        player.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        pushBack(player);
        yield return new WaitForSeconds(1);
        canHurt = true;
    }

    void pushBack(Collider2D player)
    {
        Vector2 pushDirection = new Vector2(player.transform.position.x - transform.position.x,
            (player.transform.position.y - transform.position.y)).normalized;
        pushDirection *= pushBackForce;
        Rigidbody2D pushRB = player.gameObject.GetComponent<Rigidbody2D>();
        pushRB.velocity = Vector2.zero;
        pushRB.AddForce(pushDirection, ForceMode2D.Impulse);
    }

    IEnumerator DisableController(Collider2D player)
    {
        player.gameObject.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        if(!player.gameObject.GetComponent<PlayerStats>().isDead())
            player.gameObject.GetComponent<PlayerController>().enabled = true;
    }
}
