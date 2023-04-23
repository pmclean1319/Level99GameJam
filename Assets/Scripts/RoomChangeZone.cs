using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChangeZone : MonoBehaviour
{
    /// <summary>
    /// True if the player is currently in a zone with oxygen.
    /// </summary>
    public static bool PlayerInOxygenZone = true;

    public bool HasOxygen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInOxygenZone = HasOxygen;
            FindObjectOfType<BreathBar>().SetHasOxygen(HasOxygen);
        }
    }
}
