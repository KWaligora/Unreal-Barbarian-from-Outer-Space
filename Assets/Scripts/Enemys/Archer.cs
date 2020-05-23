using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy, IEnemy
{
    public GameObject arrow;
    public Transform arrowPlace;

    private void Update()
    {
        SetMovement();
    }

    protected override void LightAttack()
    {
        StartCoroutine(AttackDelay(attackRatio));
        myAnim.SetTrigger("Attack1");

        if (facingLeft)
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 180f))).gameObject.GetComponent<Arrow>().SetDamage(damage, lightPushBackForce);
        else
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<Arrow>().SetDamage(damage, lightPushBackForce);        
    }
    protected override void HeavyAttack()
    {
        LightAttack();       
    }


}
