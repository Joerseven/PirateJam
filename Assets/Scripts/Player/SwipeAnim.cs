using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAnim : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySwing(Vector2 dir)
    {
        if (dir.y == 0)
        {
            if (dir.x < 0)
            {
                animator.SetTrigger("hori-rev");
            }
            else
            {
                animator.SetTrigger("hori-slash");
            }
        }

        if (dir.x != 0) return;
        
        switch (dir.y)
        {
            case < 0:
                animator.SetTrigger("verti-rev");
                break;
            case > 0:
                animator.SetTrigger("verti-slash");
                break;
        }
    }
}
