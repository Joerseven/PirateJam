using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Transform playerTransform;
    private Transform spawnpoint;
    private Grid grid;
    private TileInfo[] tileInfo;
    public UnityEvent LevelFailed;

    [SerializeField] TextMeshProUGUI gridText;



    public Vector2Int size;

    private List<Enemy> enemies;
    private List<Unsplurtable> unsplurtables;
    private Player player;
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
        player = GetComponentInChildren<Player>();
        player.PlayerDeathEvent.AddListener(GameOver);
        
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
        return tileInfo[IndexTo1D(cell)].canCover;
    }

    // Update is called once per frame
    void Update()
    {
        ReadGridToUI();
    }

    private void BeginPlay()
    {
        // TODO: Enable player controller on play begin when choosing cards phase is added
        playerTransform.position = grid.GetCellCenterWorld(grid.WorldToCell(spawnpoint.position));
    }

    public void AddSpurtToLevel(Vector3Int originCell, Vector3Int targetCell, SpurtInfo spurtInfo)
    {
        originCell.z = 0;
        targetCell.z = 0;
        var uDelta = ((Vector3)(targetCell - originCell)).normalized;
        for (int i = 0; i <= (int)((targetCell - originCell).magnitude); i++)
        {
            Vector3Int cellSplurt = originCell + new Vector3Int((int)uDelta.x, (int)uDelta.y, 0) * i;
            tileInfo[IndexTo1D(cellSplurt)].covered = 1;
            tileInfo[IndexTo1D(cellSplurt)].spurtInfo = spurtInfo;
        }

        if (CheckLevelOver())
        {
            NextLevel();
        }
    }

    public SpurtInfo GetSplurtInfo(Vector3Int playerCell)
    {
        return tileInfo[IndexTo1D(playerCell)].spurtInfo;
    }

    public bool CheckLevelOver()
    {
        foreach (var t in tileInfo)
        {
            if (t.covered != 1)
            {
                return false;
            }
        }

        return true;
    }

    private void GameOver()
    {
        var scripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (var s in scripts)
        {
            s.enabled = false;
        }
        LevelFailed.Invoke();
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "GameLoop")
        {
            print("Level win conditions have been met.");
            return;
        }


        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public TileInfo GetTileInfo(Vector3Int cell)
    {
        return tileInfo[IndexTo1D(cell)];
    }

    int IndexTo1D(Vector3Int cell)
    {
        var index = cell.y * size.x + cell.x;
        return index;
    }
    
    private void CheckEnd(Enemy deadEnemy)
    {
        enemies.Remove(deadEnemy);
        if (enemies.Count != 0) return;
        if (CheckLevelOver()) return;
        GameOver();

    }

    private void ReadGridToUI()
    {
        string tempString = "";
        int tempCount = 0;
        for (int i = 0; i < tileInfo.Length; i++)
        {
            switch (tileInfo[i].covered)
            {
                case 0:
                    tempString += "O";
                    break;
                case 1:
                    tempString += "X";
                    break;
            }
            tempCount++;
            if (tempCount % size.x == 0) { tempString += "\n"; }
        }
        gridText.text = tempString;
    }
}

public class TileInfo
{
    public int covered = 0;
    public bool canCover = true;
    public SpurtInfo spurtInfo;
}
