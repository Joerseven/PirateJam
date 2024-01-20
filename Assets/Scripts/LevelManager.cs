using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform playerTransform;
    private Transform spawnpoint;
    private Grid grid;
    private TileInfo[] tileInfo; 

    public Vector2Int size;

    private List<Enemy> enemies;
    private List<Unsplurtable> unsplurtables;
    // Start is called before the first frame update

    void Start()
    {
        
        playerTransform = GetComponentInChildren<Player>().transform;
        // Reliant on name? Bad practise maybe.
        spawnpoint = transform.Find("Spawnpoint");

        tileInfo = new TileInfo[size.x * size.y];
        for (int i = 0; i < size.x * size.y; i++)
        {
            tileInfo[i] = new TileInfo();
        }
        
        
        grid = GetComponent<Grid>();
        
        RegisterEnemies();
        RegisterUnsplurtables();
        // TODO: Move this function away when adding the choosing 'cards' phase before the level.   
        BeginPlay();
    } 
    void RegisterEnemies()
    {
        enemies = GetComponentsInChildren<Enemy>().ToList();
        foreach (var e in enemies)
        {
            e.OnEnemyDeath.AddListener(CheckEnd);
        }

        var butters = GetComponentsInChildren<ButterActions>().ToList();
        foreach (var b in butters)
        {
            b.InitButter(GetComponentInChildren<Player>().gameObject);
        }
    }

    void RegisterUnsplurtables()
    {
        unsplurtables = GetComponentsInChildren<Unsplurtable>().ToList();
        foreach (var u in unsplurtables)
        {
            // Add to covered for level finish detection.
            var index = IndexTo1D(grid.WorldToCell(u.transform.position));
            tileInfo[index].covered = 1;
            tileInfo[index].canCover = false;
        }
    }

    public bool CanCover(Vector3Int cell)
    {
        if (tileInfo[IndexTo1D(cell)].canCover)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginPlay()
    {
        // TODO: Enable player controller on play begin when choosing cards phase is added
        playerTransform.position = grid.GetCellCenterWorld(grid.WorldToCell(spawnpoint.position));
    }

    public void AddSpurtToLevel(Vector3Int originCell, Vector3Int targetCell)
    {
        var uDelta = ((Vector3)(targetCell - originCell)).normalized;
        for (int i = 0; i <= (int)((targetCell - originCell).magnitude); i++)
        {
            Vector3Int cellSplurt = originCell + new Vector3Int((int)uDelta.x, (int)uDelta.y, 0) * i;
            tileInfo[IndexTo1D(cellSplurt)].covered = 1;
        }

        CheckLevelOver();
    }

    public void CheckLevelOver()
    {
        bool allTrue = true;
        
        foreach (var t in tileInfo)
        {
            if (t.covered != 1)
            {
                allTrue = false;
                break;
            }
        }
        
        if (allTrue)
        {
            print("Level finished");
        }
        
    }

    int IndexTo1D(Vector3Int cell)
    {
        var index = cell.y * size.x + cell.x;
        return index;
    }

    // Called every time an enemy dies, checks if win condition (not yet implemented) is met. 
    private void CheckEnd(Enemy deadEnemy)
    {
        enemies.Remove(deadEnemy);

        if (enemies.Count == 0)
        {
            print("All enemies are dead!");
        }
    }
}

public class TileInfo
{
    public int covered = 0;
    public bool canCover = true;
}
