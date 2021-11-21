using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PuzzleButtonContact : IActivateable
{
    Animator anim;
    public AudioClip open;
    public AudioClip close;
    AudioSource source;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Arms"))) && !isActivated && collision.isTrigger == false))
        {
            source.clip = open;
            source.Play();
            Activate?.Invoke();
            isActivated = true;
            anim.SetBool("Active", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Arms"))) && isActivated))
        {
            source.clip = close;
            source.Play();
            Deactivate?.Invoke();
            isActivated = false;
            anim.SetBool("Active", false);
        }
    }
}
