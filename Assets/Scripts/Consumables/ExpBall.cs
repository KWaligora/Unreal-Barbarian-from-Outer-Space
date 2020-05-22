using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : MonoBehaviour
{
    int expValue;

    public void Init(int value)
    {
        expValue = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().AddExp(expValue);
            Destroy(gameObject);
        }
    }
}
