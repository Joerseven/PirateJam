using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swipeAnimController : MonoBehaviour
{
    [SerializeField]  private Animator _animator;
    [SerializeField] private AnimationClip[] swipeAnimations;
    
    void Start()
    {
        // _animator = GetComponent<Animator>();
        //
        // if (_animator == null)
        // {
        //     Debug.LogError("No animator found");
        // }
    }
    

    public void playSwing(Vector2 dir)
    {
        Debug.Log("Gettiong from player" + dir);
        
        if (dir.y == 0)
        {
            if (dir.x < 0)
            {
                _animator.SetTrigger("hori-rev");
            }
            else
            {
                //play going left
                _animator.SetTrigger("hori-slash");
            }
        }

        if (dir.x != 0) return;
        switch (dir.y)
        {
            case < 0:
                _animator.SetTrigger("verti-rev");
                break;
            case > 0:
                _animator.SetTrigger("verti-slash");
                break;
        }
    }
}
