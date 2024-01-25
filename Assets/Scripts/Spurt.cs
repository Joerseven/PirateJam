using System.Collections;
using Array = System.Array;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Spurt : MonoBehaviour
{

    [DoNotSerialize] public Vector3Int originCell;
    [DoNotSerialize] public Vector3Int targetCell;
    private Vector3 originPosition;
    private Vector3 cellDelta;
    private Vector2 spurtDirection;
    private Grid grid;
    private SpriteRenderer sprite;
    private Vector2Int levelSize;
    private LevelManager level;

    public SpurtInfo SpurtInfo;

    [SerializeField]
    private Color splurtColor;

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
        level = GetComponentInParent<LevelManager>();
        levelSize = level.size;
        sprite = GetComponent<SpriteRenderer>();
        elapsed = -1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (elapsed >= 0)
        {
            // More efficient way of doing this?
            elapsed += Time.deltaTime;
            materialProperty.SetFloat("elapsed", elapsed);
            sprite.SetPropertyBlock(materialProperty);
        }
    }

    public void CreateSpurt(Vector2 direction, SpurtInfo info)
    {
        originCell = grid.WorldToCell(transform.position);
        originPosition = transform.position;
        particleOrigin = transform.position;

        info.StartingCell = originCell;
        
        ResizeSpurt(direction, info);
        AnimateSpurt(direction);
        RegisterSpurtOnLevel(info);
    }

    private void ResizeSpurt(Vector2 direction, SpurtInfo info)
    {
        targetCell = GetSpurtTarget(originCell, direction);
        info.EndingCell = targetCell;
        cellDelta = targetCell - originCell;
        var scaleFactor = new Vector3(Mathf.Abs(cellDelta.x), Mathf.Abs(cellDelta.y), Mathf.Abs(cellDelta.z));
        
        transform.localScale += new Vector3(scaleFactor.x * transform.localScale.x, 
            scaleFactor.y * transform.localScale.y, 
            scaleFactor.z * transform.localScale.z);
        
        transform.position += (Vector3)cellDelta / 2.0f;
    }

    private void RegisterSpurtOnLevel(SpurtInfo info)
    {
        level.AddSpurtToLevel(originCell, targetCell, info);
    }
    
    private void AnimateSpurt(Vector2 direction)
    {
        var cellArea = 1 + cellDelta.magnitude;
        particleTargets = new Vector4[10 + (int)cellArea * 15];
        var maxDistance = grid.CellToWorld(targetCell) - originPosition;
        Vector3 direction3 = new Vector3(direction.x, direction.y, 0);

        for (int i = 0; i < particleTargets.Length; i++)
        {
            float xRand = Mathf.Pow(Random.Range(-1.0f, 1.0f), 3.0f) * 0.15f;
            float yRand = Mathf.Pow(Random.Range(-1.0f, 1.0f), 3.0f) * 0.15f;
            
            float spreadTValue = i / (float)particleTargets.Length;
            Vector3 addOn = direction3 * 0.35f;
            
            float distanceSpreadModifier = Mathf.Pow(Random.Range(-1.0f, 1.0f), 3.0f) * 0.45f * (spreadTValue);
            Vector3 cellDeltaNorm = cellDelta.normalized;
            
            particleTargets[i] = new Vector4(originPosition.x + xRand + (cellDelta.x + addOn.x) * (spreadTValue) + distanceSpreadModifier * cellDeltaNorm.y, 
                                            originPosition.y + yRand + (cellDelta.y + addOn.y) * (spreadTValue) + distanceSpreadModifier * cellDeltaNorm.x,
                                            0, 0);
            
        }

        materialProperty = new MaterialPropertyBlock();
        materialProperty.SetVectorArray("particlePos", particleTargets);
        materialProperty.SetFloat("elapsed", elapsed);
        materialProperty.SetVector("targetCell", (Vector3)targetCell);
        materialProperty.SetFloat("cellArea", cellArea);
        materialProperty.SetVector("originPos", originPosition);
        materialProperty.SetColor("color", splurtColor);
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

        furthestPossibleCell.Clamp(new Vector3Int(0, 0, 0),
            new Vector3Int(levelSize.x - 1, levelSize.y - 1, 0));

        while (!level.CanCover(furthestPossibleCell))
        {
            furthestPossibleCell -= new Vector3Int((int)direction.x, (int)direction.y, 0);
        }
        
        return furthestPossibleCell;
    }
}

public class SpurtInfo
{ 
    public delegate void DSpurtAction(Player player);
    public DSpurtAction SpurtAction;
    public Vector3Int StartingCell;
    public Vector3Int EndingCell;

    // public SpurtInfo(Vector3Int startingCell, Vector3Int endingCell)
    // {
    //     StartingCell = startingCell;
    //     EndingCell = endingCell;
    // }
}
