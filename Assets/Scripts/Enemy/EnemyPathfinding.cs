using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not currently used as it moves the enemy in a random direction. Needs to be relative to the grid but I'm off to the gym in a minute and then I'll be drinking... Work for tomorrow!
public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;

    private Rigidbody2D rb;
    private Vector2 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void NewDirection(Vector2 targetPos)
    {
        moveDir = targetPos;
    }
}
