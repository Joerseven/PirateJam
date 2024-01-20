using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI helpText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateHelpText("This is a test");
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

