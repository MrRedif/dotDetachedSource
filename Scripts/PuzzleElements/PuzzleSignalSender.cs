using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleSignalSender : SignalElement
{
    public Action ActivateSignal;
    public Action DeactivateSignal;
    Animator anim;
    AudioSource Asource;
    ParticleSystem ps;

    private void Awake()
    {
        base.Awake();
        AddToNetwork += fireActive;
        RemoveFromNetwork += fireDeactivate;
        anim = GetComponent<Animator>();
        Asource = GetComponent<AudioSource>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (active && source != null)
        {
            if (Vector2.Distance(transform.position, source.transform.position) > 3.75f)
            {
                RemoveFromNetwork?.Invoke();
                ln.SetPosition(1, transform.position);
            }
            else
            {
                ln.SetPosition(1, source.transform.position);
            }
        }
        else
        {
            ln.SetPosition(1, transform.position);
        }
        ln.SetPosition(0, transform.position);
    }

    void fireActive() { ActivateSignal?.Invoke(); anim.SetBool("Active", true); Asource.Play(); ps.Play(); }
    void fireDeactivate() {DeactivateSignal?.Invoke(); anim.SetBool("Active", false); ps.Stop(true, ParticleSystemStopBehavior.StopEmitting); }

}
