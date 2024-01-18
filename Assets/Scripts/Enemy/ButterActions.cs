//written by ed 2023
//To contain code of behavior of the butter enemy.
//Roam and Charge

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ButterActions : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeCooldown;

    private Grid grid;

    
    private bool hasChargeAvailable;
    private Rigidbody2D _rb;

    private Vector3 enemyPositionOnGrid;
    private Vector3 playerPositionOnGrid;




    private void Start()
    {
        hasChargeAvailable = false;
        _rb = GetComponent<Rigidbody2D>();
        grid = GetComponentInParent<Grid>();
        if (grid == null ) Debug.LogError("Unable to find grid");
    }

    public void InitButter(GameObject t)
    {
        target = t;
        hasChargeAvailable = true;
    }

  
    private void Update()
    {
        if (hasChargeAvailable)
        {
            enemyPositionOnGrid = grid.WorldToCell(transform.position);
            playerPositionOnGrid = grid.WorldToCell(target.transform.position);
            Debug.Log("Enemy at: " + enemyPositionOnGrid + " Player at: " + playerPositionOnGrid);
            StartCoroutine(Charge());
        }
    }

    private Vector2 FindPlayer()
    {
        return (target.transform.position - transform.position).normalized;

    }

    private IEnumerator Charge()
    {
        hasChargeAvailable = false;
        
        var toPlayer = FindPlayer();
        
        yield return new WaitForSeconds(0.5f);

        //_rb.AddForce(toPlayer * chargeSpeed, ForceMode2D.Impulse);
        MoveToPlayerOnGrid();

        StartCoroutine(CoolDown());
        yield return null;

    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        hasChargeAvailable = true;

    }

    private void MoveToPlayerOnGrid()
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
            transform.position = newPos;
        }
    }   
}
