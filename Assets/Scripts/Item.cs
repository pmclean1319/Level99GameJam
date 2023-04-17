using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Gather(GameObject self)
    {
        GameObject.Destroy(self);
    }

    public void Itemize()
    {
        if (GetComponent<MeshRenderer>() != false)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (GetComponent<Collider>() != false)
        {
            GetComponent<Collider>().enabled = false;
        }

    }
}
