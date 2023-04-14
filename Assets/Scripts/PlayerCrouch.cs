using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float shrinkLevel;
    public float shrinkRate;
    public float defaultHeight;
    public Collider ownCollider;
    public PlayerWalk playerWalk;
    // Start is called before the first frame update
    void Start()
    {
        ownCollider = GetComponent<Collider>();
        defaultHeight = gameObject.transform.localScale.y;
        playerWalk = GetComponent<PlayerWalk>();
    }

    // Update is called once per frame
    void Update()
    {
        CrouchHandler();
    }

    public void Crouch()
    {
        if (ownCollider.transform.localScale.y > shrinkLevel)
        ownCollider.transform.localScale -= new Vector3(0,shrinkRate,0);

        playerWalk.ToggleCrouch(true);
        
    }

    public void Uncrouch()
    {
        if (ownCollider.transform.localScale.y < defaultHeight)
        ownCollider.transform.localScale += new Vector3(0,shrinkRate,0);

        playerWalk.ToggleCrouch(false);
    }

    public void CrouchHandler()
    {
        if (Input.GetButton("Crouch"))
        {
            Crouch();
        }
        else
        {
            Uncrouch();
        }
    }
}
