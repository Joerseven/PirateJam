using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffects : MonoBehaviour
{
    private Vector3Int originCell;
    private Vector3 originPosition;
    private Vector2 particleOrigin;

    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
