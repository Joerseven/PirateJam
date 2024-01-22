using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unsplurtable : MonoBehaviour
{

    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        // Snap to cell center for easy drag and drop level editing.
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
