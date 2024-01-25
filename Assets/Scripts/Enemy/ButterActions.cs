using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
public class ButterActions : MonoBehaviour, IEnemyType
{

    [SerializeField] private GameObject target;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float animationTime = 1.0f;

    private Enemy enemyBase;
    
    private Rigidbody2D _rb;

    private Vector2Int levelSize;
    private LevelManager level;
    
    private Vector3Int enemyPositionOnGrid;
    private Vector3Int playerPositionOnGrid;
    private Vector3 gridTarget;
    private Vector3 originPos;
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
        animator = GetComponentInChildren<Animator>();
        enemyBase.CanDamage += IsDamageable;
        level = GetComponentInParent<LevelManager>();
        levelSize = level.size;
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
            if (nextCell.y >= level.size.y || nextCell.x >= level.size.x || nextCell.y < 0 || nextCell.x < 0)
            {
                break;
            }
            
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

  
    private void FixedUpdate()
    {
        ButterActionsStateController();
        /*Debug.Log("Enemy grid pos: " + enemyPositionOnGrid);
        Debug.Log("Player grid pos: " + playerPositionOnGrid);*/
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
                        coolDown = chargeCooldown;
                        animator.SetTrigger("hasDoneCharging");
                        break;
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
                    break;
                }
                coolDown -= Time.deltaTime;
                
                break;
            
            case ButterState.Searching:
                
                if (GetTargetPos(out var targetPos))
                {
                    state = ButterState.Charging;
                    animator.SetTrigger("charge");
                    gridTarget = targetPos;
                    originPos = transform.position;
                    delta = 0;
                }
                break;
        }
    }

    private Vector2 FindPlayer()
    {
        return (target.transform.position - transform.position).normalized;
    }
    
    
    private bool GetTargetPos(out Vector3 targetPos)
    {
        targetPos = Vector3.zero;
        var currentPos = grid.WorldToCell(transform.position);
        var playerPos = grid.WorldToCell(target.transform.position);
        
        // Mixing vec2s and 3s is not coolio in the hoolio. 
        currentPos.z = 0;
        playerPos.z = 0;
        
        var diff = playerPos - currentPos;
        if (diff.magnitude == 0) return false;
        var unitDiff = new Vector3Int(diff.x / (int)diff.magnitude, diff.y / (int)diff.magnitude, 0);
        var tileToCheck = currentPos;

        if (diff.x != 0 && diff.y != 0) return false;
        
        while (level.CheckBounds(tileToCheck + unitDiff) && level.GetTileInfo(tileToCheck + unitDiff).canCover)
        {
            tileToCheck += unitDiff;
        }

        if (tileToCheck != currentPos && diff.magnitude <= (tileToCheck - currentPos).magnitude)
        {
            targetPos = grid.GetCellCenterWorld(tileToCheck);
            return true;
        }

        return false;
    }

    private void ButterSpurtAction()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.HitPlayer();
        }
    }
    
    private Vector3Int GetMaxChargeDistance(Vector3Int origin, Vector2 direction)
    {
        var furthestPossibleCell = new Vector3Int(
            origin.x + levelSize.x * (int)direction.x, 
            origin.y + levelSize.y * (int)direction.y, 
            0);

        furthestPossibleCell.Clamp(new Vector3Int(0, 0, 0),
            new Vector3Int(levelSize.x - 1, levelSize.y - 1, 0));

        while (!level.CanCover(furthestPossibleCell))
        {
            furthestPossibleCell -= new Vector3Int((int)direction.x, (int)direction.y, 0);
        }
        
        return furthestPossibleCell;
    }

    public void AddSpurtAction(ref SpurtInfo spurt)
    {
    }
}
