using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAnim : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySwing(Vector2 dir)
    {
       
        
        if (dir.y == 0)
        {
            //play going left
            animator.SetTrigger(dir.x < 0 ? "hori-rev" : "hori-slash");
        }

        if (dir.x == 0)
        {
            //play going left
            animator.SetTrigger(dir.y > 0 ? "verti-slash" : "verti-rev");
        }
    }
}
