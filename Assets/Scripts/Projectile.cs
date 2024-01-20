using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float knockbackAmount = 5.0f;
    private UnityEvent playerDeathEvent;
    private Player player;

    private void Awake()
    {
        playerDeathEvent = new UnityEvent();
    }

    private void Start()
    {
        player = GetComponent<Player>();
        playerDeathEvent.AddListener(delegate { Player.Instance.PlayerDeath(); });
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            player?.TakeDamage(transform, knockbackAmount);
            playerDeathEvent.Invoke();
        }
        Destroy(this.gameObject);
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * (projectileSpeed * Time.deltaTime));
    }
}
