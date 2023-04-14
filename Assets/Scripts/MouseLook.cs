using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;
    public bool rotX, rotY;
    public bool isTurningEnabled = true;
    //public GameObject body;

    private void Start()
    {
        transform.localRotation.Set(-60, 0, 0, 0);
        //body = GameObject.Find("Player");
    }

    void Update()
    {
        //print("Euler = " + transform.eulerAngles.x);
        //print("Rotation = " + transform.rotation.x);
        Look();
        //VerticalLock();
    }

    private void Look()
    {
        if (rotY)
        {
            
                rotation.y += Input.GetAxis("Mouse X");
            
                
        }
        else
        {
            rotation.y += 0;
        }


        if (rotX)
        {
            if (transform.localRotation.x > -.75f)
            {
                if (Input.GetAxis("Mouse Y") > 0)
                {
                    rotation.x += -Input.GetAxis("Mouse Y");
                    //rotation.x += -Input.GetAxis("rStickVertical");
                }
                
            }
            if (transform.localRotation.x < .75f)
            {
                if (Input.GetAxis("Mouse Y") < 0 )
                {
                    rotation.x += -Input.GetAxis("Mouse Y");
                    //rotation.x += -Input.GetAxis("rStickVertical");
                }
            
            }
        }
        else
        {
            rotation.x += 0;
        }
        
        if (isTurningEnabled)
        {
            transform.localEulerAngles = (Vector2)rotation * speed;    
        }
    }

    public void VerticalLock()
    {

        if (transform.rotation.x > 1f) //&& transform.eulerAngles.x < 270)
        {
            transform.eulerAngles = new Vector3(90,
                                                transform.localRotation.y,
                                                transform.localRotation.z);
        }
        if (transform.rotation.x < -1f)
        {
            transform.eulerAngles = new Vector3(-90,
                                                transform.localRotation.y,
                                                transform.localRotation.z);
        }
    }

    public void ToggleTurning(bool toggle)
    {
        isTurningEnabled = toggle;
    }
}
