using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    /// <summary>
    /// Event emitted when the player jumps into the air.
    /// </summary>
    public event Action Jumped;

    /// <summary>
    /// Event emitted when the player lands on the ground after jumping or falling. Float value indicates airborne duration.
    /// </summary>
    public event Action<float> Landed;

    public float jumpStrength = 300;
    public bool isEarthBound = true;
    public GameObject feet;

    private float airborneStartTime;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        {
            CheckforFalling();
            JumpBegin();
            //JumpEnd();
        }
    }

    public void JumpBegin()
    {   
        if (Input.GetButtonDown("Jump") && isEarthBound == true) //&& GetComponent<Rigidbody>().velocity.y < 3)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpStrength);
            becomeAirborn();
            Jumped?.Invoke();
        }   
    }
    
    public void CheckforFalling()
    {
        if (Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .7f))
        {
            becomeEarthbound();
        }
        else {
            becomeAirborn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 &&
            Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .4f))
            
        {
            becomeEarthbound();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 8 &&
            Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .4f))
        {
            becomeEarthbound();
        }
    }

    private void becomeAirborn()
    {
        if (isEarthBound) {
            airborneStartTime = Time.time;
        }
        isEarthBound = false;
    }

    private void becomeEarthbound()
    {
        bool wasAirborne = !isEarthBound;
        isEarthBound = true;
        if (wasAirborne) {
            Landed?.Invoke(Time.time - airborneStartTime);
        }
    }
}
