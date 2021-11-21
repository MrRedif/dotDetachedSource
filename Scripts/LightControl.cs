using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    Animator anim;
    public bool flicker;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        if (flicker)
        {
            Invoke("ActivateFlick", Random.Range(0f,6.5f));
        }
    }

    void ActivateFlick()
    {
        anim.SetBool("Flicker", true);
    }
}
