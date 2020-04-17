using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //move var
    public float maxSpeed;

    //jump var
    bool grounded = false;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight = true;
   
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("Grounded", grounded);
        if (grounded && Input.GetAxis("Jump") > 0)
            jump();

        float move = Input.GetAxis("Horizontal");
        Movement(move);        
    }

    void Movement(float move)
    {
        myRB.velocity = new Vector2(maxSpeed * move, myRB.velocity.y);
        myAnim.SetFloat("speed", Mathf.Abs(move));
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
        transform.localScale = theScale;
    }

    void jump()
    {
        myRB.AddForce(new Vector2(0, jumpHeight));
    }
}
