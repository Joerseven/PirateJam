using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spurt : MonoBehaviour
{

    private Vector3Int originCell;
    private Vector3Int targetCell;
    private Vector2 spurtDirection;
    private Grid grid;
    private SpriteRenderer sprite;
    private Vector2Int levelSize;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        levelSize = GetComponentInParent<LevelManager>().size;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSpurt(Vector2 direction)
    {
        ResizeSpurt(direction);

    }

    private void ResizeSpurt(Vector2 direction)
    {
        originCell = grid.WorldToCell(transform.position);
        targetCell = GetSpurtTarget(originCell, direction);

        // Hacky way of using the distance between cells, getting the absolute from the unit vector by just
        // multiplying it by itsself. 0, 0, -1 would become 0, 0, 1
        Vector3 testScale = (targetCell - originCell).magnitude *
                            new Vector3(direction.x * direction.x, direction.y * direction.y, 0);
        
        
        print(testScale);
        transform.localScale += new Vector3(testScale.x * transform.localScale.x, testScale.y * transform.localScale.y, testScale.z * transform.localScale.z);
        transform.position += (Vector3)(targetCell - originCell) / 2.0f;
        sprite.enabled = true;
    }

    private Vector3Int GetSpurtTarget(Vector3Int origin, Vector2 direction)
    {
        var furthestPossibleCell = new Vector3Int(
            origin.x + levelSize.x * (int)direction.x, 
            origin.y + levelSize.y * (int)direction.y, 
            0);

        furthestPossibleCell.Clamp(new Vector3Int(levelSize.x / -2, levelSize.y / -2, 0),
            new Vector3Int(levelSize.x / 2, levelSize.y / 2, 0));
        
        // TODO: Add here a loop that calls another function to go along the line until it meets an invalid cell.
        // Unneeded at the moment because it's just a square with no obstacles.

        return furthestPossibleCell;
    }
}
