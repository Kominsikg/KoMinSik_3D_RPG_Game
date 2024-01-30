using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject Help;
    [SerializeField] GameObject notice;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Help.SetActive(false);
        }
    }
    public void StartButton()
    {
        SceneManager.LoadScene("Main");
        notice .SetActive(true);
    }
    public void HelpOffbutton()
    {
        Help.SetActive(false);
    }
    public void HelpOnbutton()
    {
        Help.SetActive(true);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
