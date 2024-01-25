using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swipeAnimController : MonoBehaviour
{
    [SerializeField]  private Animator _animator;
    [SerializeField] private AnimationClip[] swipeAnimations;
    

    public void playSwing(Vector2 dir)
    {
       
        
        if (dir.y == 0)
        {
            //play going left
            _animator.SetTrigger(dir.x < 0 ? "hori-rev" : "hori-slash");
        }

        if (dir.x == 0)
        {
            //play going left
            _animator.SetTrigger(dir.y > 0 ? "verti-slash" : "verti-rev");
        }
    }
}
