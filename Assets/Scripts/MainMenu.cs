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
        resumeButton.onClick.AddListener(closeMainMenu);
        optionsButton.onClick.AddListener(launchOptions);
        exitButton.onClick.AddListener(exitGame);

        // Ensure main menu component survives scene transitions.
        DontDestroyOnLoad(gameObject);
        GameState.Instance.Reset += () => Destroy(gameObject); 
    }

    private void Update()
    {
        if (isGameLaunched) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (!canvas.enabled) {
                    openMainMenu();
                } else {
                    closeMainMenu();
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

    void openMainMenu()
    {
        resumeButton.onClick.AddListener(closeMainMenu);
        canvas.enabled = true;
        GameState.Instance.Pause();
    }

    void closeMainMenu()
    {
        canvas.enabled = false;
        GameState.Instance.UnPause();
    }

    void launchOptions()
    {

    }

    void exitGame()
    {
        Application.Quit();
    }
}
