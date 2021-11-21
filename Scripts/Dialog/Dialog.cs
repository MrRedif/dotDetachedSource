using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
    [ContextMenuItem("UseDefaultSpeed", "UseDefault")]
    public DialogFragment fragment;
    void UseDefault()
    {
        foreach (var part in fragment.dialogParts)
        {
            part.speed = 0.05f;
        }
    }
}

[System.Serializable]
public class DialogFragment
{
    public string talker;
    public int Action;
    public DialogPart[] dialogParts;
    public Dialog nextDialog;

    
}
[System.Serializable]
public class DialogPart
{
    [TextArea(5,10)]
    public string dialog;
    public float speed;


}
