using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI helpText;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
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

    public void RestartGame()
    {
        SceneManager.LoadScene("GameLoop");
    }

    public void LeaveGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHelpText(string text)
    {
        helpText.text = text;
    }
}

