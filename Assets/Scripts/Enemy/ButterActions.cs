using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ButterActions : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float animationTime = 1.0f;

    private Enemy enemyBase;
    private Spurt spurt;
    private LevelManager level;
    
    private Rigidbody2D _rb;

    private Vector3 enemyPositionOnGrid;
    private Vector3 gridTarget;
    private Vector3 originPos;
    private Vector3 playerPositionOnGrid;
    private Grid grid;
    private float coolDown;
    private float delta;
    private Animator animator;

    enum ButterState
    {
        Searching,
        Charging,
        Recharging
    }

    private ButterState state;

    private void Awake()
    {
    }

    private void Start()
    {
        grid = GetComponentInParent<Grid>();
        _rb = GetComponent<Rigidbody2D>();
        enemyBase = GetComponent<Enemy>();
        enemyBase.CanDamage += IsDamageable;
        level = GetComponentInParent<LevelManager>();
        animator = GetComponentInChildren<Animator>();
        spurt = GetComponentInChildren<Spurt>();
        spurt.SpurtInfo = new SpurtInfo();
        spurt.SpurtInfo.SpurtAction = OnPlayerRoll;
    }

    private void OnPlayerRoll(Player player)
    {
        var direction = player.Facing;
        var nextCell = grid.WorldToCell(player.transform.position) + new Vector3Int((int)direction.x, (int)direction.y, 0);
        while (level.GetTileInfo(nextCell).spurtInfo == spurt.SpurtInfo)
        {
            print("Go more");
            nextCell += new Vector3Int((int)direction.x, (int)direction.y, 0);
        }
        

    }

    public void InitButter(GameObject t)
    {
        target = t;
    }

    public bool IsDamageable()
    {
        if (state == ButterState.Charging) return false;
        return true;
    }

  
    private void Update()
    {
        ButterActionsStateController();
    }

    private void ButterActionsStateController()
    {
        enemyPositionOnGrid = grid.WorldToCell(transform.position);
        playerPositionOnGrid = grid.WorldToCell(target.transform.position);
        
        
        if (enemyBase.IsDead)
        {
            return;
        }

        switch (state)
        {
            default:
                case ButterState.Charging:
                    
                    var tValue = delta / animationTime;

                    if (tValue >= 1)
                    {
                        transform.position = gridTarget;
                        state = ButterState.Recharging;
                        animator.SetTrigger("hasDoneCharging");
                        coolDown = chargeCooldown;
                    }
            
                    var distanceVec = gridTarget - originPos;
                    var deltaVec = tValue * tValue * tValue * distanceVec;
                    transform.position = originPos + deltaVec;
                    delta += Time.deltaTime;
                    break;
            
            case ButterState.Recharging:
                
                if (coolDown <= 0)
                {
                    state = ButterState.Searching;
                }
                coolDown -= Time.deltaTime;
                
                break;
            
            case ButterState.Searching:
                
                if ((enemyPositionOnGrid - playerPositionOnGrid).magnitude <= 1)
                {
                    state = ButterState.Charging;
                    gridTarget = GetTargetPos();
                    originPos = transform.position;
                    delta = 0;
                    animator.SetTrigger("charge");
                }
                break;
        }
    }

    private Vector2 FindPlayer()
    {
        return (target.transform.position - transform.position).normalized;
    }
    
    
    private Vector3 GetTargetPos()
    {
        Vector3 difference = playerPositionOnGrid - enemyPositionOnGrid;

        if (Mathf.Abs(difference.x) > 0.9f || Mathf.Abs(difference.y) > 0.9f)
        {
            Vector3 newPos = transform.position;
            switch (Mathf.Abs(difference.x) >= Mathf.Abs(difference.y))
            {
                case true:
                    _ = difference.x >= 0 ? newPos.x += 1 : newPos.x -= 1;
                    break;
                case false:
                    _ = difference.y >= 0 ? newPos.y += 1 : newPos.y -= 1;
                    break;
            }
            return newPos;
        }
        return transform.position;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.HitPlayer();
        }
    }
}
