using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy, IEnemy
{
    public GameObject arrow;
    public Transform arrowPlace;

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
        myAnim.SetTrigger("Attack");

        if (facingLeft)
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 180f))).gameObject.GetComponent<Arrow>().SetDamage(damage);
        else
            Instantiate(arrow, arrowPlace.position, Quaternion.Euler(new Vector3(0, 0, 0))).gameObject.GetComponent<Arrow>().SetDamage(damage);        
    }

   
}
