using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    public int playerHealth;
    public string spawnPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Check
        if (GameObject.Find("Player"))
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth( int amt)
    {
        playerHealth += amt;
    }

}
