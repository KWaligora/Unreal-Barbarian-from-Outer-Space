using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;

    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight;
   
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        facingRight = true;
    }

    
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        Movement(move);
    }

    void Movement(float move)
    {
        myRB.velocity = new Vector2(maxSpeed * move, myRB.velocity.y);

        if (move < 0 && facingRight)
            Flip();
        else if (move > 0 && !facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
    }
}
