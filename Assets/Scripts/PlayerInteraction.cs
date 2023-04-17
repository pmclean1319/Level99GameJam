using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject cam;
    public float CheckDist;
    public string interactTag;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        CheckforInteractables();
    }

    public void CheckforInteractables()
    {
        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out RaycastHit hitInfo, CheckDist))
        {
            if (hitInfo.collider.gameObject.tag == interactTag)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    print(hitInfo.collider.gameObject.name);
                    hitInfo.collider.gameObject.GetComponent<Item>().Gather(hitInfo.collider.gameObject);
                }
                
            }
        }        
    }
}
