using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AOE_HotSauce : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 0.1f;
    [SerializeField] private float GrowthTime = 2.0f;
    [SerializeField] private float DestroyObjWaitTime = 5.0f;
    [SerializeField] private float knockbackAmount = 2.0f;
    private Vector3 originalScale;
    private float timeElapsed = 0.0f;

    
    // Start is called before the first frame update
    private void Start()
    {
        originalScale = transform.localScale;
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed <= GrowthTime)
        {
            var tVal = scaleFactor / GrowthTime;
            scaleFactor += tVal * Time.deltaTime * originalScale.x ;
            transform.localScale = new Vector3(scaleFactor, scaleFactor);
        }

        if (timeElapsed >= DestroyObjWaitTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(transform, knockbackAmount);
        }
    }

}
