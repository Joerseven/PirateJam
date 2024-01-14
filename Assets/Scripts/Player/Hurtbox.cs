using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Hurtbox : MonoBehaviour
{
    public UnityEvent<Collider2D> OnHit;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit.Invoke(other);
    }
}
