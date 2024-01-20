using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Attacking,
        Patrolling
    }
    
    [SerializeField] private MonoBehaviour enemyType;

    private State state;
    private float attackRange = 2.0f;
    
    private void Awake()
    {
        state = State.Roaming;
        attackRange = (enemyType as IEnemy)?.GetAttackRange() ?? attackRange;
    }

    // Start called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        StateControl();
    }

    
    private void StateControl()
    {
        //State machine is inprogress, needs refining before can be applied to the enemies. ATM it doesn't switch states properly based on the conditions i've set.
        switch (state)
        {
            default:
                case State.Roaming: 
                    Roaming();
                    break;
                case State.Attacking:
                    Attacking();
                    break;
        }
    }

    private void Roaming()
    {
        (enemyType as IEnemy)?.Move();

        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }
    }

    private void Attacking()
    {
        (enemyType as IEnemy)?.Attack();
        
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }
    }
}
