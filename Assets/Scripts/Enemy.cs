using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    public UnityEvent<Enemy> OnEnemyDeath;

    private Grid grid;
    

    private Spurt spurt;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        spurt = GetComponentInChildren<Spurt>(true);
    }

    void StartingLevel()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerSpurt(Vector2 direction)
    {
        spurt.CreateSpurt(direction);
    }

    // Takes in slash direction as unit vector
    public void ReceiveSlash(Vector2 slashDirection)
    {
        OnEnemyDeath.Invoke(this);
        TriggerSpurt(slashDirection);
        // Testing spurt
        //gameObject.SetActive(false);
    }
}
