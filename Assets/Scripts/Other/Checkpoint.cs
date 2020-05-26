using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    LvlManager lvlManager;

    private void Start()
    {
        lvlManager = GameObject.FindGameObjectWithTag("LvlManager").GetComponent<LvlManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            lvlManager.lastCheckpoint = transform.position;
            Debug.Log(lvlManager.lastCheckpoint);
        }
    }
}
