using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script attaches to an enemy that shoot a projectile towards the player. It is very basic at the moment, I'll refine it once the playtesting has been done.
public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private float attackCooldown = 2.0f;

    private bool canAttack = true;

    public void Attack()
    {
        if (!canAttack) { return; }
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            StartCoroutine(AttackingRoutine());
        }
    }

    private void Update()
    {
        Attack();
    }

    private IEnumerator AttackingRoutine()
    {
        canAttack = false;
        Vector2 targetDir = Player.Instance.transform.position - transform.position;
        GameObject newBullet = Instantiate(enemyProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        newBullet.transform.right = targetDir;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

}
