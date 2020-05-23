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
        if(isMobile)
         SetMovement();
    }

    protected override void LightAttack()
    {
        base.LightAttack();
    }
}
