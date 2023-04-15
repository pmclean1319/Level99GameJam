using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float breathLoss = 0;

        if (PlayerWalkScript.isRunning)
            breathLoss = Time.deltaTime * RunningBreathLossRate;
        else if(PlayerWalkScript.isCrouching)
            breathLoss = Time.deltaTime * CrouchingBreathLossRate;
        else
            breathLoss = Time.deltaTime * BreathLossRate;


        GetComponent<Slider>().value -= breathLoss;
        BreathPercentageText.text = Mathf.Round(GetComponent<Slider>().value * 100 * 4/3).ToString() + "%";
    }
}
