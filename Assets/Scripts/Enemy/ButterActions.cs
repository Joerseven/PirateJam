//written by ed 2023
//To contain code of behavior of the butter enemy.
//Roam and Charge

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterActions : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeCooldown;
    
    private bool hasChargeAvailable;
    private Rigidbody2D _rb;
    
    
    private void Start()
    {
        hasChargeAvailable = false;
        _rb = GetComponent<Rigidbody2D>();
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
        
        _rb.AddForce(toPlayer * chargeSpeed, ForceMode2D.Impulse);
        StartCoroutine(CoolDown());
        yield return null;

    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        hasChargeAvailable = true;

    }

    
}
