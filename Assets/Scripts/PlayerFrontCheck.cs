using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrontCheck : MonoBehaviour
{
    public GameObject head, chest, waist, feet;
    public string inFront;
    public float checkDist = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check();
    }

    public string Check(Vector3 dir)
    {
        string ObjInFront = "space";
        bool headCheck = Physics.BoxCast(
            head.transform.position,
            new Vector3(.2f,.2f,.2f),
            dir,
            head.transform.rotation,
            checkDist);
        bool chestCheck = Physics.BoxCast(
            chest.transform.position,
            new Vector3(.2f, .2f, .2f),
            dir,
            chest.transform.rotation,
            checkDist);
        bool waistCheck = Physics.BoxCast(
            waist.transform.position,
            new Vector3(.2f, .2f, .2f),
            dir,
            waist.transform.rotation,
            checkDist);
        bool feetCheck = Physics.Raycast(
            feet.transform.position,
            dir,
            checkDist);

        //Check all possibilities 
        if (headCheck == true)
        {
            ObjInFront = "wall";
        }
        if (headCheck == false && 
            chestCheck == true)
        {
            ObjInFront = "ledge";
        }
        if (headCheck == false && 
            chestCheck == false &&
            waistCheck == true)
        {
            ObjInFront = "vaultable";
        }
        if (headCheck == false &&
            chestCheck == false &&
            waistCheck == false &&
            feetCheck == true)
        {
            ObjInFront = "step";
        }

        inFront = ObjInFront;

        return ObjInFront;

    }
}
