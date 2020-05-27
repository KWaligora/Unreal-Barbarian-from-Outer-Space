using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerStats PlayerStats;
    public Animator playerAnim;
    public MageBoss mageboss;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim.SetBool("Grounded", true);
        playerController.enabled = false;
        PlayerStats.enabled = false;
        mageboss.enabled = false;
    }

    public void Run()
    {
        playerController.enabled = true;
        PlayerStats.enabled = true;
        mageboss.enabled = true;
    }
}
