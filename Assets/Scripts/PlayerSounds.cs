using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWalk))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerFrontCheck))]
public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioClip walkingFootsteps;
    [SerializeField] AudioClip runningFootsteps;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip jumpLandingSound;
    [SerializeField] AudioClip slowBreathing;
    [SerializeField] AudioClip normalBreathing;
    [SerializeField] AudioClip fastBreathing;
    [SerializeField] AudioClip oxygenMusic;
    [SerializeField] AudioClip noOxygenMusic;

    [SerializeField] float musicVolume;
    [SerializeField] float effectsVolume;

    AudioSource footstepsAudioSource;
    AudioSource jumpLandingAudioSource;
    AudioSource breathingAudioSource;
    AudioSource musicAudioSource;
    PlayerFrontCheck playerFrontCheck;
    PlayerWalk playerWalk;
    PlayerJump playerJump;
    List<AudioSource> allAudioSources;
    Dictionary<AudioSource, AudioClip> queuedSound;
    Dictionary<AudioSource, float> fadeDuration;
    Dictionary<AudioSource, float> maxVolume;

    void Start()
    {
        Debug.Log("Its Starting");
        // Populate or reset lists.
        allAudioSources = new List<AudioSource>();
        queuedSound = new Dictionary<AudioSource, AudioClip>();
        fadeDuration = new Dictionary<AudioSource, float>();
        maxVolume = new Dictionary<AudioSource, float>();
        // Setup audio sources.
        jumpLandingAudioSource = createAudioSource("JumpLandingAudio", false, 0f, effectsVolume);
        footstepsAudioSource = createAudioSource("FootstepsAudio", true, 0.15f, effectsVolume);
        breathingAudioSource = createAudioSource("BreathingAudio", true, 0.75f, effectsVolume);
        musicAudioSource = createAudioSource("MusicAudio", true, 1.25f, musicVolume);
        // Setup player movement components.
        playerFrontCheck = GetComponent<PlayerFrontCheck>();
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerJump.Jumped += PlayerJumped;
        playerJump.Landed += PlayerLanded;
        GameState.Instance.Paused += PauseAudio;
        GameState.Instance.UnPaused += UnPauseAudio;
    }

    AudioSource createAudioSource(string name, bool loop, float fadeOutDuration, float sourceMaxVolume)
    {
        GameObject audioContainer = new GameObject(name);
        audioContainer.transform.parent = gameObject.transform;
        AudioSource audioSource = audioContainer.AddComponent<AudioSource>();
        audioSource.loop = loop;
        fadeDuration[audioSource] = fadeOutDuration;
        maxVolume[audioSource] = sourceMaxVolume;
        allAudioSources.Add(audioSource);
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
        ToggleMusic();
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

    void ToggleMusic()
    {
        SetSound(musicAudioSource, RoomChangeZone.PlayerInOxygenZone ? oxygenMusic : noOxygenMusic);
    }

    void SetSound(AudioSource audioSource, AudioClip newSound)
    {
        if (audioSource.clip == null) {
            audioSource.clip = newSound;
            audioSource.Play();
            audioSource.volume = maxVolume[audioSource];
        } else if (audioSource.clip == newSound) {
            if (queuedSound.ContainsKey(audioSource)) {
                // Sound was fading out -- cancel that.
                queuedSound.Remove(audioSource);
                audioSource.volume = maxVolume[audioSource];

            }
        } else { // Current clip is wrong; start fadeout.
            queuedSound[audioSource] = newSound;
        }
    }

    void PauseAudio()
    {
        allAudioSources.ForEach(a => a.Pause());
    }

    void UnPauseAudio()
    {
        allAudioSources.ForEach(a => a.UnPause());
    }

    void UpdateQueuedSounds()
    {
        List<AudioSource> audioSourcesToRemove = new List<AudioSource>();
        foreach (AudioSource audioSource in queuedSound.Keys) {
            float volumeDecrease = Time.deltaTime / fadeDuration[audioSource] / maxVolume[audioSource];
            audioSource.volume -= volumeDecrease;
            if (audioSource.volume <= 0) { 
                audioSourcesToRemove.Add(audioSource);
            }
        }
        foreach (AudioSource audioSource in audioSourcesToRemove) {
            AudioClip newClip;
            queuedSound.Remove(audioSource, out newClip);
            audioSource.volume = maxVolume[audioSource];
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
