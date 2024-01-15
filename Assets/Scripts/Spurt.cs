using System.Collections;
using Array = System.Array;
using System.Collections.Generic;
using UnityEngine;


public class Spurt : MonoBehaviour
{

    private Vector3Int originCell;
    private Vector3Int targetCell;
    private Vector3 originPosition;
    private Vector3 cellDelta;
    private Vector2 spurtDirection;
    private Grid grid;
    private SpriteRenderer sprite;
    private Vector2Int levelSize;

    private float elapsed;
    
    [SerializeField]
    private const int PARTICLECOUNT = 50;

    private Vector2 particleOrigin;
    private Vector4[] particleTargets;
    private MaterialPropertyBlock materialProperty;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        levelSize = GetComponentInParent<LevelManager>().size;
        sprite = GetComponent<SpriteRenderer>();
        
        originCell = grid.WorldToCell(transform.position);
        originPosition = transform.position;
        particleOrigin = transform.position;
        elapsed = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsed >= 0)
        {
            // More efficient way of doing this?
            elapsed += Time.deltaTime;
            materialProperty.SetFloat("elapsed", elapsed);
            sprite.SetPropertyBlock(materialProperty);
        }
    }

    public void CreateSpurt(Vector2 direction)
    {
        ResizeSpurt(direction);
        AnimateSpurt();
    }

    private void ResizeSpurt(Vector2 direction)
    {
        targetCell = GetSpurtTarget(originCell, direction);

        cellDelta = targetCell - originCell;
        var scaleFactor = new Vector3(Mathf.Abs(cellDelta.x), Mathf.Abs(cellDelta.y), Mathf.Abs(cellDelta.z));
        
        transform.localScale += new Vector3(scaleFactor.x * transform.localScale.x, 
            scaleFactor.y * transform.localScale.y, 
            scaleFactor.z * transform.localScale.z);
        
        transform.position += (Vector3)cellDelta / 2.0f;
    }

    // Don't touch this code. I don't understand it and I wrote it. 
    private void AnimateSpurt()
    {

        particleTargets = new Vector4[PARTICLECOUNT];
        print(cellDelta);

        for (int i = 0; i < PARTICLECOUNT; i++)
        {
            float xRand = Mathf.Pow(Random.Range(-1.0f, 1.0f), 3.0f) * 0.2f;
            float yRand = Mathf.Pow(Random.Range(-1.0f, 1.0f), 3.0f) * 0.2f;
            
            
            float distanceSpread = i / (float)PARTICLECOUNT;
            
            float spreadModifier = Random.Range(-0.15f, 0.15f) * (distanceSpread + 0.05f);
            
            particleTargets[i] = new Vector4(originPosition.x + xRand + cellDelta.x * distanceSpread + spreadModifier * cellDelta.y, 
                                            originPosition.y + yRand + cellDelta.y * distanceSpread + spreadModifier * cellDelta.x,
                                            0, 0);
            
        }
        
        //print(particleTargets[0] - new Vector4(particleOrigin.x, particleOrigin.y, 0, 0));

        materialProperty = new MaterialPropertyBlock();
        materialProperty.SetVectorArray("particlePos", particleTargets);
        materialProperty.SetFloat("elapsed", elapsed);
        materialProperty.SetVector("targetCell", (Vector3)targetCell);
        sprite.SetPropertyBlock(materialProperty);

        elapsed = 0;
        
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