using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DialogShower : MonoBehaviour
{
    public event Action StartedConversation;
    public event Action EndedConversation;
    public event Action<int> EndedConversationReturn;
    public event Action SkippedDialog;
    public Action Interact;
    Animator anim;
    Dialog before;
    [SerializeField]Text targetText;
    public Dialog currentDialog;
    int currentDialogIndex;
    bool isWriting;
    IEnumerator hld;

    private void Awake()
    {
        targetText.text = "";
        Interact = StartWriting;
        anim = GetComponent<Animator>();
        StartedConversation += TriggerAnimEnter;
        EndedConversation += TriggerAnimExit;
    }
    private void Update()
    {
    }

    void StartWriting()
    {
        if (!isWriting)
        {
            if(currentDialogIndex == 0) StartedConversation?.Invoke();
            hld = WriteDialogToScreen();
            StartCoroutine(hld);
        }
        else
        {
            SkippedDialog?.Invoke();
            StopCoroutine(hld);
            SkipDialog();
        }

    }
    void FinishWriting()
    {
        EndedConversation?.Invoke();
        if (before != null)
        {
            EndedConversationReturn?.Invoke(before.fragment.Action);
        }
        else
        {
            EndedConversationReturn?.Invoke(currentDialog.fragment.Action);
        }
        
        Interact = StartWriting;
    }

    IEnumerator WriteDialogToScreen()
    {
        isWriting = true;
        targetText.text = "";
        char[] hold = currentDialog.fragment.dialogParts[currentDialogIndex].dialog.ToCharArray();
        for (int i = 0; i < hold.Length; i++)
        {
            targetText.text += hold[i];
            yield return new WaitForSeconds(currentDialog.fragment.dialogParts[currentDialogIndex].speed);
        }
        isWriting = false;
        UpdateIndex();
        yield return null;
    }
    void SkipDialog()
    {
        targetText.text = currentDialog.fragment.dialogParts[currentDialogIndex].dialog;
        isWriting = false;
        UpdateIndex();
    }

    void UpdateIndex()
    {
        currentDialogIndex++;
        if (currentDialogIndex >= currentDialog.fragment.dialogParts.Length)
        {
            currentDialogIndex = 0;
            Interact = FinishWriting;
            if (currentDialog.fragment.nextDialog != null)
            {
                before = currentDialog;
                currentDialog = currentDialog.fragment.nextDialog;
            }
                

        }
    }

    void TriggerAnimEnter()
    {
        anim.SetTrigger("Enter");
    }
    void TriggerAnimExit()
    {
        anim.SetTrigger("Exit");
    }
}
