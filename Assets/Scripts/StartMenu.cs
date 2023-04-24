using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public float maxZoomOffset = 2.5f; // Maximum amount the image can be zoomed in or out 
    public float zoomDuration = 8f; // Duration of the zooming mode in seconds 

    [SerializeField]
    private RectTransform rectTransform;
    private Vector3 startScale;
    private float zoomStartTime = 0f;
    private bool isZoomingIn = true;
    private Vector3 zoomTarget;

    void Start()
    { 
        startScale = rectTransform.localScale;
        zoomTarget = startScale *  maxZoomOffset;
    }

    void Update()
    {
        float zoomPercent = (Time.time - zoomStartTime) / zoomDuration;
        if (zoomPercent >= 1) {
            isZoomingIn = !isZoomingIn;
            zoomStartTime = Time.time;
        } else {
            Vector3 start = isZoomingIn ? startScale : zoomTarget;
            Vector3 end = isZoomingIn ? zoomTarget : startScale;
            rectTransform.localScale = Vector3.Lerp(start, end, zoomPercent);
        }
    }

}