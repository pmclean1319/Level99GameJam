using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        newGameButton.onClick.AddListener(launchNewGame);
        optionsButton.onClick.AddListener(launchOptions);
        exitButton.onClick.AddListener(exitGame);
    }
    void launchNewGame()
    {
        SceneManager.LoadScene("Living Quarters");
    }

    void launchOptions()
    {

    }

    void exitGame()
    {
        Application.Quit();
    }
}
