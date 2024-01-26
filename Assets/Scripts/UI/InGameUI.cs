using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI helpText;

    Animator animator;
    private LevelManager levelManager;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("Level").GetComponent<LevelManager>();
        levelManager.LevelFailed.AddListener(ShowLeveFailedScreen);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateHelpText("This is a test");
            animator.SetTrigger("ViewHelp");

        }
    }

    private void ShowLeveFailedScreen()
    {
        animator.SetTrigger("PlayerDied");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LeaveGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHelpText(string text)
    {
        helpText.text = text;
    }

    public void ShowControls()
    {
        animator.SetTrigger("ViewControls");
    }
}

