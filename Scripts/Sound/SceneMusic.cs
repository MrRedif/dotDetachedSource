using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public static SceneMusic instance;
    Animator anim;
    AudioClip nextClip;
    AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }


    public void ChangeClip(AudioClip clip)
    {
        if (clip == source.clip)
        {
            return;
        }
        nextClip = clip;
        anim.SetTrigger("Change");
    }

    public void AnimCallClip()
    {
        source.Stop();
        source.clip = nextClip;
        source.Play();
        nextClip = null;
    }
}
