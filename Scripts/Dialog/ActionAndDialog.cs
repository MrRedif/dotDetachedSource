using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActionAndDialog : MonoBehaviour
{
    PlayableDirector dir;
    bool activated;
    public Dialog dialog;
    public bool useSignal;

    private void Awake()
    {
        dir = FindObjectOfType<PlayableDirector>();
        if(!useSignal)
        GetComponentInParent<IActivateable>().Activate += ActivateAnim;
        else
        {
            GetComponentInParent<PuzzleSignalSender>().ActivateSignal += ActivateAnim;
        }
    }


    void ActivateAnim()
    {
        if (!activated)
        {
            dir.Play();
            if (dialog != null)
            {
                FindObjectOfType<DialogShower>().currentDialog = dialog;
            }
            activated = true;
        }
        
    }

}
