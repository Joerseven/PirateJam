using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//Class to manager Audio for the entirety of the game.
//TODO: Add player sound effects 
//TODO: Change the sound effects and music to the final songs and sounds
//</summary>

public class AudioManager : MonoBehaviour
{
    
    [Header("Music")]
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource gameMusic;
    private bool mainMenuPlaying    = false;
    private bool gameMusicPlaying   = false;

    [SerializeField] private AudioSource attackSoundSource;

    [SerializeField] private AudioClip[] attackClips;

    // [Header("Player Sounds")]
    // [SerializeField] private AudioSource
    

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //testing code
        // this.StartMainMenuMusic();
    }

    
    void Update()
    {
        //testing code
        if(Input.GetKeyDown("space")){
            Debug.Log("Space Pressed");
            this.StartGameMusic();
        }
        if(Input.GetKeyDown(KeyCode.E)){
            this.PlayAttackSound(0);
        }
         if(Input.GetKeyDown(KeyCode.Q)){
            this.PlayAttackSound(1);
        }
    }

//<summary>
//call to play main menu music. Stops gameMusic. Not generalized.
//</summary>
    public void StartMainMenuMusic(){
        Debug.Log("mmm");
        if(!mainMenuPlaying)
        {
            mainMenuMusic.Play();
            gameMusic.Stop();

            gameMusicPlaying = false;
            mainMenuPlaying = true;
        }
    }
//<summary>
//call to play game music. Stops title music. Not generalized.
//</summary>

    public void StartGameMusic(){
        if(!gameMusicPlaying)
        {
            mainMenuMusic.Stop();
            gameMusic.Play();

            gameMusicPlaying = true;
            mainMenuPlaying = false;
        }
    }

//<summary>
//Method To play Attack Sounds
//@params int of current weapon held. To see what that is look at AttackClips array above
//</summary>

    public void PlayAttackSound(int curWeapon){
        attackSoundSource.PlayOneShot(attackClips[curWeapon]);
    }
    
}


