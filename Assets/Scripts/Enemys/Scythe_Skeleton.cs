using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe_Skeleton : Enemy, IEnemy
{
    bool isBlocking = false;
    bool canBlock = true;

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
        if (!isBlocking)
        {
            base.Attack();
        }
    }

    public override void TakeDamage(int dmg)
    {
        if (!isBlocking)
        {
            base.TakeDamage(dmg);

            if (Random.Range(0, 2) == 1 && canBlock)
            {                
                StartCoroutine(Block());
            }
        }
    }

    IEnumerator Block()
    {
        StartCoroutine(BlockDelay(5.0f));
        myAnim.SetBool("Block", true);
        isBlocking = true;
        currentSpeed = 0;

        yield return new WaitForSeconds(2.0f);

        myAnim.SetBool("Block", false);
        isBlocking = false;
        Attack();
        SetSpeed();
    }

    IEnumerator BlockDelay(float delay)
    {
        canBlock = false;
        yield return new WaitForSeconds(5.0f);
        canBlock = true;
    }

    
}
