using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform playerTransform;
    private Transform spawnpoint;
    private Grid grid;

    public Vector2Int size;

    private List<Enemy> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
        playerTransform = GetComponentInChildren<Player>().transform;
        // Reliant on name? Bad practise maybe.
        spawnpoint = transform.Find("Spawnpoint");
        
        grid = GetComponent<Grid>();
        
        RegisterEnemies();
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
