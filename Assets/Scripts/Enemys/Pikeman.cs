using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikeman : Enemy, IEnemy
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        SetMovement();
    }

    protected override void Attack()
    {
        myAnim.SetTrigger("Attack");
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in collider)
        {
            if (player.tag == "Player")
            {
                StartCoroutine(SendDamage(player.gameObject.GetComponent<PlayerStats>()));
                break;
            }
        }
    }

    protected override void CreateExpBall()
    {
        ExpBall ball = Instantiate(expBall, GetComponentInParent<ExpBall>().transform.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<ExpBall>();
        ball.Init(expValue);
    }
}
