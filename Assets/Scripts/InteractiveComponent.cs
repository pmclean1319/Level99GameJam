using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public string interText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(GameObject player)
    {
        Transport(player);
        Gather(player);
    }

    public void Transport(GameObject player)
    {
        if (GetComponent<Transport>() != null)
        {
            GetComponent<Transport>().TransportPlayer(player);
        }
    }

    public void Gather(GameObject player)
    {
        if (GetComponent<Item>() != null)
        {
            GetComponent<Item>().Itemize();
            //player.GetComponent<PlayerInventoryList>().AddToInventory(this.gameObject);
        }
    }
}
