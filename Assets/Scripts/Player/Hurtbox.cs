using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Hurtbox : MonoBehaviour
{
    public UnityEvent<Collider2D> OnHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit.Invoke(other);
    }
}
