using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState
{
    static public GameState Instance = new GameState();

    public event Action Paused;
    public event Action UnPaused;
    public event Action Reset;

    public bool IsPaused {  get; private set; }

    public void Pause()
    {
        if (!IsPaused) {
            Time.timeScale = 0;
            IsPaused = true;
            Paused?.Invoke();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void UnPause()
    {
        if (IsPaused) {
            Time.timeScale = 1;
            IsPaused = false;
            UnPaused?.Invoke();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting");
        Reset?.Invoke();
        IsPaused = false;
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Instance = new GameState();
        SceneManager.LoadScene("StartMenu");
    } 
}
