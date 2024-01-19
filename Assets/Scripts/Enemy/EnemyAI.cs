using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Shooter shooter;
    private enum State
    {
        Roaming,
        Attacking
    }
    
    [SerializeField] private float roamingTime = 0.0f;
    [SerializeField] private float roamDirChangeTime = 2.0f;
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private MonoBehaviour enemyType;

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 roamPosition;

    private bool canAttack = true;
    
    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        shooter = GetComponent<Shooter>();
        state = State.Roaming;
    }

    // Start called before the first frame update
    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    private void Update()
    {
         Debug.Log(state);
    }

    private void MovementStateControl()
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
        roamingTime += Time.fixedDeltaTime;
        enemyPathfinding.NewDirection(roamPosition);
            Debug.Log("we in this bitch roaming around causing chaos");

        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (roamingTime > roamDirChangeTime)
        {
            roamPosition = GetRoamingPosition();
        }

    }

    private void Attacking()
    {
        if (canAttack)
        {
            canAttack = false;
            /*
             * I'm in the process of setting up an interface for the enemies so that later on we can have multiple enemies with an "attack" function that can be called.
             */
            //(enemyType as IEnemy).Attack();
            shooter.Attack();
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
    }
}
