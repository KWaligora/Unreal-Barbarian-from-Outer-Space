using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    public float speed;
    public Transform pos1, pos2;

    Vector3 nextPos;

    void Start()
    {
        nextPos = pos2.position;
    }

    
    void Update()
    {
        if (transform.position == pos1.position)
            nextPos = pos2.position;        

        if (transform.position == pos2.position)        
            nextPos = pos1.position;        

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            collision.collider.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            collision.collider.transform.SetParent(null);
    }
}
