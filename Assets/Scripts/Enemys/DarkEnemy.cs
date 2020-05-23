using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEnemy : Enemy, IEnemy
{
    private void Update()
    {
        if (isMobile)
            SetMovement();
    }
}
