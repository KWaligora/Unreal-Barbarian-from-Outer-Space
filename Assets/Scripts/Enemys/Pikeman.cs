using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikeman : Enemy, IEnemy
{
    void Update()
    {
        SetMovement();
    }
}
