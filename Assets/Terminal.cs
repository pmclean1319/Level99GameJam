using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public TextMeshProUGUI ScreenText;
    public GameObject TerminalCamera;
    GameObject sceneCamera;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TerminalCamera.SetActive(false);
            sceneCamera.SetActive(true);
            ChangePlayerScriptStatus(player, true);
        }
    }
    
    public void StartJournal()
    {
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        ChangePlayerScriptStatus(player, false);

        //player.SetActive(false);
        sceneCamera.SetActive(false);
        TerminalCamera.SetActive(true);
    }

    void ChangePlayerScriptStatus(GameObject player, bool status)
    {
        player.GetComponent<MouseLook>().enabled = status;
        player.GetComponent<PlayerWalk>().enabled = status;
        player.GetComponent<PlayerJump>().enabled = status;
        player.GetComponent<PlayerFrontCheck>().enabled = status;
        player.GetComponent<PlayerCrouch>().enabled = status;
    }
}
