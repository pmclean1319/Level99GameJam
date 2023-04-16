using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWalk))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioClip walkingFootsteps;
    [SerializeField] AudioClip runningFootsteps;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip jumpLandingSound;

    AudioSource footstepsAudioSource;
    AudioSource jumpLandingAudioSource;
    PlayerWalk playerWalk;
    PlayerJump playerJump;

    void Start()
    {
        // Setup audio sources.
        footstepsAudioSource = gameObject.AddComponent<AudioSource>();
        footstepsAudioSource.loop = true;
        jumpLandingAudioSource = gameObject.AddComponent<AudioSource>();
        // Setup player movement components.
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerJump.Jumped += PlayerJumped;
        playerJump.Landed += PlayerLanded;
    }

    void Update()
    {
        ToggleFootstepSounds();
    }

    void ToggleFootstepSounds()
    {
        AudioClip newSound = null;
        if (playerJump.isEarthBound) {
            if (playerWalk.isWalking) {
                newSound = walkingFootsteps;
            } else if (playerWalk.isRunning) {
                newSound = runningFootsteps;
            }
        }
        if (newSound != footstepsAudioSource.clip) {
            if (newSound == null) {
                footstepsAudioSource.Stop();
                footstepsAudioSource.clip = null;
            } else {
                footstepsAudioSource.clip = newSound;
                footstepsAudioSource.Play();
            }
        }
    }
    void PlayerJumped()
    {
        jumpLandingAudioSource.Stop();
        jumpLandingAudioSource.clip = jumpSound;
        jumpLandingAudioSource.Play();
    }

    void PlayerLanded(float fallDuration)
    {
        if (fallDuration > 1) { // Ignore trivially short falls; don't play audio for them.
            jumpLandingAudioSource.Stop();
            jumpLandingAudioSource.clip = jumpLandingSound;
            jumpLandingAudioSource.Play();
        }
    }
}
