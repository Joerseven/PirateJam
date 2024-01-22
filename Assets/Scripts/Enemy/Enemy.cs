using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{

    public UnityEvent<Enemy> OnEnemyDeath;

    public delegate bool DelCanDamage();
    public DelCanDamage CanDamage;

    public bool IsDead;
    private Collider2D enemyCollider;

    private Grid grid;
    
    private Spurt spurt;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        enemyCollider = GetComponent<Collider2D>();
        spurt = GetComponentInChildren<Spurt>(true);
        
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }

    void StartingLevel()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerSpurt(Vector2 direction)
    {
        spurt.CreateSpurt(direction);
    }

    public void Kill()
    {
        IsDead = true;
        enemyCollider.enabled = false;
    }

    // Takes in slash direction as unit vector
    public void ReceiveSlash(Vector2 slashDirection)
    {
        var damageable = true;
        
        if (CanDamage != null)
        {
            damageable = CanDamage();
        }

        if (!damageable)
        {
            return;
        }

        Kill();
        
        TriggerSpurt(slashDirection);
        OnEnemyDeath.Invoke(this);
        
    }
    
}
