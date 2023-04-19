using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockDoor : MonoBehaviour
{
    public Animator anim;
    public AudioClip open, close;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.Play("AirlockOpen");
            source.PlayOneShot(open);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        {
            anim.Play("AirlockClose");
            source.PlayOneShot(close);
        }
    }
}
