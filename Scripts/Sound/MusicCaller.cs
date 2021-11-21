using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCaller : MonoBehaviour
{
    public AudioClip clip;

    private void Start()
    {
        SceneMusic.instance.ChangeClip(clip);
    }
}
