using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    Animator anim;
    bool started;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerInfo.enableControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !started)
        {
            started = true;
            PlayerInfo.enableControl = true;
            anim.SetTrigger("Start");
            SceneMusic.instance.ChangeClip(clip);
        }
    }
}
