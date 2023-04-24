using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{

    public Action PlayerDied;

    public float OxygenLevel { get; private set; }

    public string spawnPoint;

    public void SetOxygenLevel(float newOxygenLevel)
    {
        OxygenLevel = newOxygenLevel;
        if (OxygenLevel <= 0) {
            TriggerPlayerDeath();
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameState.Instance.Reset += () =>  Destroy(gameObject);
    }

    void TriggerPlayerDeath()
    {
        PlayerDied?.Invoke();
        GameState.Instance.ResetGame();
    }
}
