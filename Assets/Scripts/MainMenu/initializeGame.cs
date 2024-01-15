using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class initializeGame : MonoBehaviour
{
    private AudioManager audioManagerCache;
    
    void Awake()
    {
        audioManagerCache = FindObjectOfType<AudioManager>();
        if(audioManagerCache == null) {
            Debug.LogError("Audio manager is NULL. Check Audio manager init.");
            }
        audioManagerCache.StartMainMenuMusic();
       
    }

   
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
        audioManagerCache.StartGameMusic();
        
    }
    
    
}
