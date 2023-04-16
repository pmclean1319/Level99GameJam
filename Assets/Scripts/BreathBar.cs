using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BreathBar : MonoBehaviour
{
    public float CrouchingBreathLossRate = .05f;
    public float BreathLossRate = .1f;
    public float RunningBreathLossRate = .2f;
    public float ClimbingBreathLossRate = .2f;
    public float AudioFadeOutDuration = 0.5f;
    public TextMeshProUGUI BreathPercentageText;
    public PlayerWalk PlayerWalkScript;
    public PlayerJump PlayerJumpScript;
    public PostProcessVolume ScreenVolume;
    private Vignette ScreenVignette;
    private DepthOfField ScreenDoF;
    private AudioSource audioSource;

    public AudioClip slowBreathing;
    public AudioClip normalBreathing; 
    public AudioClip fastBreathing;
    /// <summary>Indicates which breathing sound is currently playing (or slated to start playing after the fade-out).</summary>
    private AudioClip currentAudio;
    /// <summary>Indicates whether an audio fade-out is in progress. Prevents multiple coroutines from running simultaneously.</summary>
    private bool isFading;
    private bool jumpedSinceLastUpdate = false;
    private Grain ScreenGrain;

    // Start is called before the first frame update
    void Start()
    {
        ScreenVignette = ScreenVolume.profile.GetSetting<Vignette>();
        ScreenDoF = ScreenVolume.profile.GetSetting<DepthOfField>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        ScreenGrain = ScreenVolume.profile.GetSetting<Grain>();

        PlayerJumpScript.Jumped += PlayerJumpScript_Jumped;
    }

    private void PlayerJumpScript_Jumped()
    {
        jumpedSinceLastUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        float breathLoss = 0;
        AudioClip breathingAudio = null;

        if (!PlayerJumpScript.isEarthBound) {
            if (jumpedSinceLastUpdate)
            {
                breathLoss = Time.deltaTime * ClimbingBreathLossRate;
                breathingAudio = fastBreathing;
                jumpedSinceLastUpdate = false;
            }
            else
            {
                breathLoss = Time.deltaTime * CrouchingBreathLossRate;
                breathingAudio = slowBreathing;
            }
        } else if (PlayerWalkScript.isCrouching) {
            breathLoss = Time.deltaTime * CrouchingBreathLossRate;
            breathingAudio = slowBreathing;
        } else if (PlayerWalkScript.isRunning) {
            breathLoss = Time.deltaTime * RunningBreathLossRate;
            breathingAudio = fastBreathing;
        } else if (PlayerWalkScript.isWalking){
            breathLoss = Time.deltaTime * BreathLossRate;
            breathingAudio = normalBreathing;
        }else {
            breathLoss = Time.deltaTime * CrouchingBreathLossRate;
            breathingAudio = slowBreathing;
        }

        GetComponent<Slider>().value -= breathLoss;
        float breathPercentage = GetComponent<Slider>().value * 100 * 4 / 3;
        BreathPercentageText.text = Mathf.Round(breathPercentage).ToString() + "%";

        if(breathPercentage <= 50)
        {
            ScreenVignette.intensity.value = (50-breathPercentage) / 50;
            ScreenGrain.intensity.value = (50 - breathPercentage) / 50;
            //ScreenDoF.focusDistance.value = breathPercentage;
        }

        if (breathingAudio != currentAudio) {
            currentAudio = breathingAudio;
            StartCoroutine(FadeOutAndSwitchAudio());
        }
    }

    /// <summary>
    /// Switches the audio by fading out the currently-playing clip and then starting the new clip.
    /// </summary>
    private IEnumerator FadeOutAndSwitchAudio()
    {
        if (!isFading) {
            isFading = true;
            float startVolume = audioSource.volume;

            // Fade out
            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / AudioFadeOutDuration;
                yield return null;
            }
            // Stop and switch the audio clip
            audioSource.Stop();
            audioSource.clip = currentAudio;
            audioSource.volume = startVolume;
            audioSource.Play();

            isFading = false;
        }
    }
}
