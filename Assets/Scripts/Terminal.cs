using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public TextMeshProUGUI ScreenText;
    public GameObject TerminalCamera;
    public AudioSource SpaceBarAudio;
    public AudioSource KeyPressAudio;
    public int progressTrack;
    public float TypeSpeedMin = .1f;
    public float TypeSpeedMax = .2f;
    public GameObject sceneCamera;
    public GameObject player;
    List<Message> messages;
    float textSpeedTrack = 0;
    float randomTypeSpeed = 0;
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
            displayText = false;
            ScreenText.enabled = false;
            TerminalCamera.SetActive(false);
            sceneCamera.SetActive(true);
            ChangePlayerScriptStatus(player, true);
        }


        if (displayText)
        {
            textSpeedTrack += Time.deltaTime;
            if (textSpeedTrack > randomTypeSpeed)
            {
                ScreenText.text = message.messageText.Substring(0, messageLinePosition);
                if (message.messageText[messageLinePosition] == ' ')
                    SpaceBarAudio.Play();
                else
                    KeyPressAudio.Play();
                messageLinePosition++;
                textSpeedTrack = 0;
                randomTypeSpeed = Random.Range(TypeSpeedMin, TypeSpeedMax);
            }

            if (messageLinePosition == message.messageText.Length)
                displayText = false;
        }

    }
    
    public void StartJournal()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if(sceneCamera == null)
            sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");

        ScreenText.enabled = true;
        ChangePlayerScriptStatus(player, false);
        sceneCamera.SetActive(false);
        TerminalCamera.SetActive(true);

        TypeOutMessage();
    }

    public void TypeOutMessage()
    {
        message = messages.Where(m => m.messageNumber == progressTrack).Single();
        if(messageLinePosition < message.messageText.Length + 1)
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
