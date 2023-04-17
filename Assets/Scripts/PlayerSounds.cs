using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWalk))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerFrontCheck))]
public class PlayerSounds : MonoBehaviour
{

    [SerializeField] float audioFadeOutDuration = 0.5f;
    [SerializeField] AudioClip walkingFootsteps;
    [SerializeField] AudioClip runningFootsteps;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip jumpLandingSound;
    [SerializeField] AudioClip slowBreathing;
    [SerializeField] AudioClip normalBreathing;
    [SerializeField] AudioClip fastBreathing; 

    AudioSource footstepsAudioSource;
    AudioSource jumpLandingAudioSource;
    AudioSource breathingAudioSource;
    PlayerFrontCheck playerFrontCheck;
    PlayerWalk playerWalk;
    PlayerJump playerJump;

void Start()
    { 
        // Setup audio sources.
        jumpLandingAudioSource = createAudioSource("JumpLandingAudio", false);
        footstepsAudioSource = createAudioSource("FootstepsAudio", true);
        breathingAudioSource = createAudioSource("BreathingAudio", true);
        footstepsAudioSource.name = "catso";
        // Setup player movement components.
        playerFrontCheck = GetComponent<PlayerFrontCheck>();
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerJump.Jumped += PlayerJumped;
        playerJump.Landed += PlayerLanded;
        Debug.LogFormat("{0} {1} {2}", jumpLandingAudioSource, footstepsAudioSource, breathingAudioSource);
    }

    AudioSource createAudioSource(string name, bool loop)
    {
        GameObject audioContainer = new GameObject(name);
        audioContainer.transform.parent = gameObject.transform;
        AudioSource audioSource = audioContainer.AddComponent<AudioSource>();
        audioSource.loop = loop;
        return audioSource;
    }
    void PlayerJumped()
    {
        if (playerFrontCheck.inFront != "wall") { // "Jumping" on a wall = climbing
            jumpLandingAudioSource.Stop();
            jumpLandingAudioSource.clip = jumpSound;
            jumpLandingAudioSource.Play();
        }
    }

    void PlayerLanded(float fallDuration)
    {
        if (fallDuration > 1) { // Ignore trivially short falls; don't play audio for them.
            jumpLandingAudioSource.Stop();
            jumpLandingAudioSource.clip = jumpLandingSound;
            jumpLandingAudioSource.Play();
        }
    }

    void Update()
    {
        ToggleFootstepSounds();
        ToggleBreathingSounds();
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
        SetSound(footstepsAudioSource, newSound);
    }

    void ToggleBreathingSounds()
    {
        AudioClip newSound = null;
        if (playerWalk.isRunning) {
            newSound = fastBreathing;
        } else if (playerWalk.isWalking) {
            newSound = normalBreathing;
        } else  {
            newSound = slowBreathing;
        }
        SetSound(breathingAudioSource, newSound);
    }

    void SetSound(AudioSource audioSource, AudioClip newSound)
    {
        if (newSound != audioSource.clip) {
            Debug.LogFormat("Fading from {0} to {1}", audioSource.clip, newSound);
            audioSource.Stop();
            audioSource.clip = newSound; 
            audioSource.Play();
        }
    }
}
