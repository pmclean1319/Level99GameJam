using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySpawnpoint : MonoBehaviour
{

    public string pointName;
    public Transform objective;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        GameObject player = GameObject.Find("Player");
        if (player.GetComponent<PlayerVitals>().spawnPoint == pointName)
        {
            player.transform.position = transform.position;
            player.transform.eulerAngles = transform.eulerAngles;
            FindObjectOfType<Arrow>().Objective = objective;
        }
    }
}
