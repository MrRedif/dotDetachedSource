using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkerScript : MonoBehaviour
{
    bool canTalk;
    bool startedTalk;
    DialogShower shower;
    SpriteRenderer sp;
    [SerializeField]GameObject pressE;
    private void Awake()
    {
        shower = GameObject.FindObjectOfType<DialogShower>();
        sp = GetComponentInChildren<SpriteRenderer>();
        shower.EndedConversation += FinishTalk;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger == false && (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Arms")))
        {
            canTalk = false;
            pressE.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger == false && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Arms")))
        {
            canTalk = true;
            pressE.SetActive(true);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canTalk)
            {
                if (!startedTalk)
                {
                    StartTalk();
                }
                else
                {
                    shower.Interact();
                }
            }
           
        }     
    }


    void StartTalk()
    {
        startedTalk = true;
        shower.Interact();
        PlayerInfo.enableControl = false;
    }

    void FinishTalk()
    {
        startedTalk = false;
        PlayerInfo.enableControl = true;
    }

}
