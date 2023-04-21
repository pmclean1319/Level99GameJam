using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;

    Canvas canvas;
    bool isGameLaunched = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        resumeButton.gameObject.SetActive(false);
        // Setup button listeners.
        newGameButton.onClick.AddListener(launchNewGame);
        resumeButton.onClick.AddListener(resumeGame);
        optionsButton.onClick.AddListener(launchOptions);
        exitButton.onClick.AddListener(exitGame);

        // Ensure main menu component survives scene transitions.
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isGameLaunched) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (!canvas.enabled) {
                    canvas.enabled = true;
                    Time.timeScale = 0;
                } else {
                    resumeGame();
                }
            }
        }
    }

    void launchNewGame()
    {
        SceneManager.LoadScene("Living Quarters");
        isGameLaunched = true;
        newGameButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        canvas.enabled = false;
    }

    void resumeGame()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
    }

    void launchOptions()
    {

    }

    void exitGame()
    {
        Application.Quit();
    }
}
