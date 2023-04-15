using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerWalk))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    AudioClip walkingFootsteps;
    [SerializeField]
    AudioClip runningFootsteps;
     
    private AudioSource audioSource;
    private PlayerWalk playerWalk;
    private PlayerJump playerJump;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        AudioClip newSound = null;
        if (playerJump.isEarthBound) {
            if (playerWalk.isWalking) {
                newSound = walkingFootsteps;
            } else if (playerWalk.isRunning) {
                newSound = runningFootsteps;
            }
        }
        if (newSound != audioSource.clip) { 
            if (newSound == null) {
                audioSource.Stop();
                audioSource.clip = null;
            } else {
                audioSource.clip = newSound;
                audioSource.Play();
            }
        }
    }
}
