using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BreathBar : MonoBehaviour
{
    public float CrouchingBreathLossRate = .05f;
    public float BreathLossRate = .1f;
    public float RunningBreathLossRate = .2f;
    public float ClimbingBreathLossRate = .2f;
    public Image Bar;
    public TextMeshProUGUI BreathPercentageText;
    public PlayerWalk PlayerWalkScript;
    public PlayerJump PlayerJumpScript;
    public PostProcessVolume ScreenVolume;
    public bool InRoomWithOxygen = false;
    private Vignette ScreenVignette;
    private DepthOfField ScreenDoF;

    private bool jumpedSinceLastUpdate = false;
    private Grain ScreenGrain;

    // Start is called before the first frame update
    void Start()
    {
        ScreenVignette = ScreenVolume.profile.GetSetting<Vignette>();
        ScreenDoF = ScreenVolume.profile.GetSetting<DepthOfField>();
        ScreenGrain = ScreenVolume.profile.GetSetting<Grain>();

        //Added PJM 4/17/23 
        PlayerWalkScript = FindObjectOfType<PlayerWalk>();
        PlayerJumpScript = FindObjectOfType<PlayerJump>();
        PlayerJumpScript.Jumped += PlayerJumpScript_Jumped;
    }

    private void HasAir()
    {
        InRoomWithOxygen = true;
    }

    private void NoAir()
    {
        InRoomWithOxygen = false;
    }

    private void PlayerJumpScript_Jumped()
    {
        jumpedSinceLastUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InRoomWithOxygen)
        {
            float breathLoss = 0;

            if (!PlayerJumpScript.isEarthBound)
            {
                if (jumpedSinceLastUpdate)
                {
                    breathLoss = Time.deltaTime * ClimbingBreathLossRate;
                    jumpedSinceLastUpdate = false;
                }
                else
                {
                    breathLoss = Time.deltaTime * CrouchingBreathLossRate;
                }
            }
            else if (PlayerWalkScript.isCrouching)
            {
                breathLoss = Time.deltaTime * CrouchingBreathLossRate;
            }
            else if (PlayerWalkScript.isRunning)
            {
                breathLoss = Time.deltaTime * RunningBreathLossRate;
            }
            else if (PlayerWalkScript.isWalking)
            {
                breathLoss = Time.deltaTime * BreathLossRate;
            }
            else
            {
                breathLoss = Time.deltaTime * CrouchingBreathLossRate;
            }


            Bar.fillAmount -= breathLoss;
        }
        else
        {
            if (Bar.fillAmount + Time.deltaTime * .5f > .75f)
                Bar.fillAmount = .75f;
            else
                Bar.fillAmount += Time.deltaTime * .5f;
        }

        float breathPercentage = Bar.fillAmount * 100 * 4 / 3;
        BreathPercentageText.text = Mathf.Round(breathPercentage).ToString() + "%";

        if(breathPercentage <= 50)
        {
            ScreenVignette.intensity.value = (50-breathPercentage) / 50;
            ScreenGrain.intensity.value = (50 - breathPercentage) / 50;
            //ScreenDoF.focusDistance.value = breathPercentage;
        }
        else if(breathPercentage > 50 && ScreenVignette.intensity.value != 0)
        {
            ScreenVignette.intensity.value = 0;
        }
        else if (breathPercentage > 50 && ScreenGrain.intensity.value != 0)
        {
            ScreenGrain.intensity.value = 0;
        }
    }
}
