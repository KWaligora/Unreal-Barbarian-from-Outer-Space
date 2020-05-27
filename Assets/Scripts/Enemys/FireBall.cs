using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int dmg;
    public float delay;
    public float fireballSpeed;

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeTrueDamage(dmg, transform, 5.0f);
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fireballSpeed);
    }
}
