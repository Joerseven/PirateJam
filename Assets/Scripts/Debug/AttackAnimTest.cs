//made by ed 2023 18 jan
//Test script for anim




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimTest : MonoBehaviour
{
  // Start is called before the first frame update

    [SerializeField]
    private Animator attackAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            attackAnimator.SetTrigger("AttackR2L");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            attackAnimator.SetTrigger("AttackL2R");
        }
    }
}
