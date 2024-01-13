using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform player;
    private Transform spawnpoint;
    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        // Reliant on name? Bad practise maybe.
        player = transform.Find("Player");
        spawnpoint = transform.Find("Spawnpoint");
        grid = GetComponent<Grid>();
        BeginPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginPlay()
    {
        player.position = grid.GetCellCenterWorld(grid.WorldToCell(spawnpoint.position));
    }
}
