using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    const int TEXT_SPEED = 1;
    public TextMeshProUGUI ScreenText;
    public GameObject TerminalCamera;
    public int progressTrack;
    GameObject sceneCamera;
    GameObject player;
    List<Message> messages;
    int textSpeedTrack = 0;
    int messageLinePosition = 0;
    bool displayText = false;
    Message message;

    // Start is called before the first frame update
    void Start()
    {
        string jsonPath = Application.streamingAssetsPath + "/TerminalMessages.json";
        string jsonStr = File.ReadAllText(jsonPath);

        messages = JsonHelper.FromJson<Message>(jsonStr).ToList();

        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
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


        if (displayText)
        {
            if (TEXT_SPEED == textSpeedTrack)
            {
                ScreenText.text = message.messageText.Substring(0, messageLinePosition);
                messageLinePosition++;
                textSpeedTrack = 0;
            }
            else
            {
                textSpeedTrack++;
            }
            if (messageLinePosition == message.messageText.Length + 1)
                displayText = false;
        }

    }
    
    public void StartJournal()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if(sceneCamera == null)
            sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");

        ChangePlayerScriptStatus(player, false);
        sceneCamera.SetActive(false);
        TerminalCamera.SetActive(true);

        TypeOutMessage();
    }

    public void TypeOutMessage()
    {
        message = messages.Where(m => m.messageNumber == progressTrack).Single();
        displayText = true;
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
