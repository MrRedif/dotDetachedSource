using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class AfterDialogEvent : MonoBehaviour
{
    public PlayableAsset cutscene;

    private void Awake()
    {
        FindObjectOfType<DialogShower>().EndedConversationReturn += ChangeAnimAndPlay;
    }

    void ChangeAnimAndPlay(int i)
    {
        if (i == 1)
        {
            Debug.Log("Played");
            FindObjectOfType<PlayableDirector>().Stop();
            FindObjectOfType<PlayableDirector>().Play(cutscene);
        }
       
    }
}
