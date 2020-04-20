using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.2f;
    public LayerMask enemyLayer;
    public float lifeTime;
    public float maxSpeed;

    Rigidbody2D myRB;
    int damage;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
        myRB = GetComponent<Rigidbody2D>();
        if(transform.localRotation.z >0)
            myRB.AddForce(new Vector2(-1, 0) * maxSpeed, ForceMode2D.Impulse);
        else
            myRB.AddForce(new Vector2(1, 0) * maxSpeed, ForceMode2D.Impulse);
    }
    
    void FixedUpdate()
    {
        Collider2D[] HitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in HitEnemys)
        {
            Debug.Log("hit enemy:" + enemy.name);
            enemy.GetComponent<IEnemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
