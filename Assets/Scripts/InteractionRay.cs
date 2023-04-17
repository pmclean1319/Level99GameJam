using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionRay : MonoBehaviour
{
    float rayDist = 2;
    public TextMeshProUGUI dispField;
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    public void CheckForInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit info, rayDist))
        {
            if (info.collider.gameObject.GetComponent<InteractiveComponent>() != null)
            {
                if (dispField != null)
                {
                    dispField.text = info.collider.gameObject.GetComponent<InteractiveComponent>().interText;
                }

                if (Input.GetButton("Interact"))
                {
                    CallActivation(info.collider.gameObject);
                }
                
            }
            else
            {
                if (dispField != null)
                dispField.text = "";
            }
        }
        else
        {
            if (dispField != null)
                dispField.text = "";
        }
    }

    public void CallActivation(GameObject target)
    {
        target.GetComponent<InteractiveComponent>().Activate(playerObject);
    }
}
