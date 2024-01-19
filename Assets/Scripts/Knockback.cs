using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can be applied to any Gameobject that will be taking damage of some sort.
public class Knockback : MonoBehaviour
{

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void KnockBack(Transform damageSource, float knockBackAmount)
    {
        Vector2 delta = (transform.position - damageSource.position).normalized * knockBackAmount * rb.mass;
        rb.AddForce(delta, ForceMode2D.Impulse);
    }
}
