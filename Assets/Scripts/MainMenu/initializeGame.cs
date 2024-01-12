using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class initializeGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Holy shit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // <summary>
    // Method used to init main game. Functionality can be Added above.
    // Mono Behaviour Script so cannot init memory stuff here 
    // </summary>
    public void InitMainGame()
    {
        SceneManager.LoadScene("GameLoop");
    }
    
    
}
