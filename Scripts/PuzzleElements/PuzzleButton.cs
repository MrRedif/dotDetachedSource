using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : IActivateable
{
    Animator anim;
    AudioSource source;
    ParticleSystem ps;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        ps = GetComponentInChildren<ParticleSystem>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == false && collision.gameObject.CompareTag("Head") && !isActivated)
        {
            PlayerHead head = collision.gameObject.GetComponent<PlayerHead>();
            StartCoroutine(head.AttachGiven(transform));
            Activate?.Invoke();
            isActivated = true;
            anim.SetBool("Active", true);
            source.Play();
            ps.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Head") && isActivated)
        {
            PlayerHead head = collision.gameObject.GetComponent<PlayerHead>();
            Deactivate?.Invoke();
            isActivated = false;
            anim.SetBool("Active", false);
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
