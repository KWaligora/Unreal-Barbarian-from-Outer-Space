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
        base.Attack();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }
}
