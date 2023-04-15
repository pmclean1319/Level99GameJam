using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    public float walkSpeed;
    public float dashSpeed;
    public float crouchSpeed;
    public float currentSpeed;
    public GameObject playerCamera;
    public float rot;
    public float turnSpeed = 1;
    public float stepSpeed = 1;
    public GameObject camAnimRig;
    public Vector3 hDir, vDir, movement;
    public bool isCrouching;
    public bool isRunning;
    public bool isWalkingEnabled = true;
    public float walkCheckDist = .5f;
    public PlayerFrontCheck pfc;

    private void Start()
    {
        //StartCoroutine(TurnOnGravity());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GameObject.Find("Camera");
        pfc = GetComponent<PlayerFrontCheck>();
        //camAnimRig = GameObject.Find("CameraAnimationRig").GetComponent<CamAnimRig>();
    }

    private void FixedUpdate()
    {
        if (isWalkingEnabled)
        {
            Move();
        }
        
    }

    public void Move()
    {
        
        if (!isCrouching)
        {
            currentSpeed = walkSpeed;
            Dash();
        }
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        hDir = (Input.GetAxisRaw("Horizontal") * transform.right);
        vDir = (Input.GetAxisRaw("Vertical") * transform.forward);

        movement = (hDir + vDir).normalized;

        //if ((!Physics.Raycast(transform.position,movement,.25f,1 << 8)) && ((Physics.SphereCast(transform.position,.02f,transform.up * -1,out RaycastHit hitInfo,1f))))
        //{
        

        //Prevent walking into walls
        Vector3 traveldir = new Vector3(movement.x * currentSpeed, 0, movement.z * currentSpeed);
        Debug.DrawRay(transform.position, traveldir);

        pfc.Check(traveldir);

        if (pfc.inFront == "wall") //Physics.BoxCast(pfc.chest.transform.position, new Vector3(.001f,.2f,.2f), traveldir, transform.rotation, walkCheckDist) == true)
            //&& (pfc.inFront == "wall" || pfc.inFront == "vaultable"))
        {
            movement = new Vector3(0, 0, 0);
        }
        GetComponent<Rigidbody>().velocity = new Vector3(movement.x * currentSpeed, GetComponent<Rigidbody>().velocity.y, movement.z * currentSpeed);




        if (movement != new Vector3(0, 0, 0))
        {

        }

        //Steps, vaults, and ledges
        if ((pfc.inFront == "step") //|| pfc.inFront == "vaultable") 
            && ((Input.GetAxisRaw("Vertical") != 0)
            || (Input.GetAxisRaw("Horizontal") != 0)))
        {
            transform.position += transform.up * stepSpeed;
        }

        if ((pfc.inFront == "vaultable" || pfc.inFront == "ledge" || pfc.inFront == "wall") && Input.GetAxisRaw("Jump") != 0
            && ((Input.GetAxisRaw("Vertical") != 0)
             || (Input.GetAxisRaw("Horizontal") != 0)))
        {
            transform.position += transform.up * (stepSpeed/2);
        }
    }
    public void Dash()
    {
        if (Input.GetAxis("Dash") > 0 &&
            Mathf.Abs(Input.GetAxis("Horizontal")) < 1 &&
            Input.GetAxis("Vertical") > 0)
        {
            currentSpeed = dashSpeed;
            isRunning = true;
        }
        else
        {
            currentSpeed = walkSpeed;
            isRunning = false;
        }
    }
    
    public void ToggleCrouch(bool crouchSet)
    {
        isCrouching = crouchSet;
    }

    public void ToggleWalk(bool walkSet)
    {
        isWalkingEnabled = walkSet;
    }
}
