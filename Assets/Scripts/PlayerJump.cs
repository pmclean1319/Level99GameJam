using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpStrength = 300;
    public bool isEarthBound = true;
    public GameObject feet;
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
            isEarthBound = false;
        }   
    }
    
    public void CheckforFalling()
    {
        if (Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .7f))
        {
            isEarthBound = true;
        }
        else
        {
            isEarthBound = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 &&
            Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .4f))
            
        {
            isEarthBound = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 8 &&
            Physics.SphereCast(feet.transform.position, .2f, transform.up * -1f, out RaycastHit hitInfo, .4f))
        {   
            isEarthBound = true;
        }
    }
}
