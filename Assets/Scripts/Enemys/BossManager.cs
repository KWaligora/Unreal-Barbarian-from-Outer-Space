using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public MageBoss mageboss;
    public DialogueTrigger dt;
    bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        mageboss.enabled = false;     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player") && active)
        {
            active = false;
            mageboss.enabled = true;
        }     
    }
}
