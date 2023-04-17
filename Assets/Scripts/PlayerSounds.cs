using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWalk))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerFrontCheck))]
public class PlayerSounds : MonoBehaviour
{

    [SerializeField] float audioFadeOutDuration = 0.15f;
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
    Dictionary<AudioSource, AudioClip> queuedSound = new Dictionary<AudioSource, AudioClip>();
    Dictionary<AudioSource, float> fadeDuration = new Dictionary<AudioSource, float>();

    void Start()
    {
        // Setup audio sources.
        jumpLandingAudioSource = createAudioSource("JumpLandingAudio", false, 0f);
        footstepsAudioSource = createAudioSource("FootstepsAudio", true, 0.15f);
        breathingAudioSource = createAudioSource("BreathingAudio", true, 0.75f);
        // Setup player movement components.
        playerFrontCheck = GetComponent<PlayerFrontCheck>();
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerJump.Jumped += PlayerJumped;
        playerJump.Landed += PlayerLanded;
    }

    AudioSource createAudioSource(string name, bool loop, float fadeOutDuration)
    {
        GameObject audioContainer = new GameObject(name);
        audioContainer.transform.parent = gameObject.transform;
        AudioSource audioSource = audioContainer.AddComponent<AudioSource>();
        audioSource.loop = loop;
        fadeDuration[audioSource] = fadeOutDuration;
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
        UpdateQueuedSounds();
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
        AudioClip newSound;
        if (playerWalk.isRunning) {
            newSound = fastBreathing;
        } else if (playerWalk.isWalking) {
            newSound = normalBreathing;
        } else {
            newSound = slowBreathing;
        }
        SetSound(breathingAudioSource, newSound);
    }

    void SetSound(AudioSource audioSource, AudioClip newSound)
    {
        if (audioSource.clip == null) {
            audioSource.clip = newSound;
            audioSource.Play();
        } else if (audioSource.clip == newSound) {
            if (queuedSound.ContainsKey(audioSource)) {
                // Sound was fading out -- cancel that.
                queuedSound.Remove(audioSource);
                audioSource.volume = 1;

            }
        } else { // Current clip is wrong; start fadeout.
            queuedSound[audioSource] = newSound;
        }
    }

    void UpdateQueuedSounds()
    {
        List<AudioSource> audioSourcesToRemove = new List<AudioSource>();
        foreach (AudioSource audioSource in queuedSound.Keys) {
            float volumeDecrease = Time.deltaTime / fadeDuration[audioSource];
            audioSource.volume -= volumeDecrease;
            if (audioSource.volume <= 0) { 
                audioSourcesToRemove.Add(audioSource);
            }
        }
        foreach (AudioSource audioSource in audioSourcesToRemove) {
            AudioClip newClip;
            queuedSound.Remove(audioSource, out newClip);
            audioSource.volume = 1;
            audioSource.clip = newClip;
            audioSource.Play();
        }
    } 
}
