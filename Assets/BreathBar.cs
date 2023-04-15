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
    public TextMeshProUGUI BreathPercentageText;
    public PlayerWalk PlayerWalkScript;
    public PlayerJump PlayerJumpScript;
    public PostProcessVolume ScreenVolume;
    private Vignette ScreenVignette;
    private DepthOfField ScreenDoF;

    // Start is called before the first frame update
    void Start()
    {
        ScreenVignette = ScreenVolume.profile.GetSetting<Vignette>();
        ScreenDoF = ScreenVolume.profile.GetSetting<DepthOfField>();
    }

    // Update is called once per frame
    void Update()
    {
        float breathLoss = 0;

        if (!PlayerJumpScript.isEarthBound)
            breathLoss = Time.deltaTime * ClimbingBreathLossRate;
        else if(PlayerWalkScript.isCrouching)
            breathLoss = Time.deltaTime * CrouchingBreathLossRate;
        else if(PlayerWalkScript.isRunning)
            breathLoss = Time.deltaTime * RunningBreathLossRate;
        else
            breathLoss = Time.deltaTime * BreathLossRate;

        GetComponent<Slider>().value -= breathLoss;
        float breathPercentage = GetComponent<Slider>().value * 100 * 4 / 3;
        BreathPercentageText.text = Mathf.Round(breathPercentage).ToString() + "%";

        if(breathPercentage <= 50)
        {
            ScreenVignette.intensity.value = (50-breathPercentage) / 50;
            ScreenDoF.focusDistance.value = breathPercentage;
        }
    }
}
