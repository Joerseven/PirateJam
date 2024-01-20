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

    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite unsqueezed;
    [SerializeField] Sprite squeezed;

    private bool canAttack = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            Vector3 targetPos = Player.Instance.transform.position;
            Vector3 enemyPos = transform.position;
            targetPos.x = targetPos.x - enemyPos.x;
            targetPos.y = targetPos.y - enemyPos.y;
            float angle = Mathf.Atan2(targetPos.x, targetPos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }
        Attack();
    }

    private IEnumerator AttackingRoutine()
    {
        canAttack = false;
        Vector2 targetDir = Player.Instance.transform.position - transform.position;
        GameObject newBullet = Instantiate(enemyProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        newBullet.transform.right = targetDir;
        StartCoroutine(SqueezeBottle());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator SqueezeBottle()
    {
        spriteRenderer.sprite = squeezed;
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = unsqueezed;
    }
}