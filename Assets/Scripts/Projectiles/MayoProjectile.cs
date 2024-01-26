using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MayoProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float knockbackAmount = 5.0f;

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out _))
        {
            return;
        }
        
        if (other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(transform, knockbackAmount);
            player.HitPlayer();
        }
        
        Destroy(this.gameObject);
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * (projectileSpeed * Time.deltaTime));
    }
}
