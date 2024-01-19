using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        Player player = other.gameObject.GetComponent<Player>();
        player?.TakeDamage(transform, knockbackAmount);
        Destroy(this.gameObject);
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * (projectileSpeed * Time.deltaTime));
    }
}
