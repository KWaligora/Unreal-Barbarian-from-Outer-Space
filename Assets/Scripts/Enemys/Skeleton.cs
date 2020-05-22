using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy, IEnemy
{
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        SetMovement();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void CreateExpBall()
    {
        ExpBall ball = Instantiate(expBall, GetComponentInParent<Skeleton>().transform.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<ExpBall>();
        ball.Init(expValue);
    }
}
