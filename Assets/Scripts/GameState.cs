using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GameState
{
    static public event Action Paused;
    static public event Action UnPaused;

    static public bool IsPaused {  get; private set; }

    static public void Pause()
    {
        if (!IsPaused) {
            IsPaused = true;
            Paused?.Invoke();
        }
    }
    static public void UnPause()
    {
        if (IsPaused) {
            IsPaused = false;
            UnPaused?.Invoke();
        }
    }
}
