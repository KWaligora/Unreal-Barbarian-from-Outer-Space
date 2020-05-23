using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifeTime;
    public float maxSpeed;

    Rigidbody2D myRB;
    int damage;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
        myRB = GetComponent<Rigidbody2D>();
        if (transform.localRotation.z > 0)
            myRB.AddForce(new Vector2(-1, 0) * maxSpeed, ForceMode2D.Impulse);
        else
            myRB.AddForce(new Vector2(1, 0) * maxSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeTrueDamage(damage, transform);
            Destroy(myRB);
            transform.SetParent(collision.transform);
        }
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
