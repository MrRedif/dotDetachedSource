using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public IActivateable boundedElement;
    public PuzzleSignalSender signalPower;
    public bool isReverse;
    protected void Awake()
    {
        if (boundedElement != null)
        {
            if (isReverse)
            {
                boundedElement.Activate += DeInteract;
                boundedElement.Deactivate += Interact;
            }
            else
            {
                boundedElement.Activate += Interact;
                boundedElement.Deactivate += DeInteract;
            }
           
        }
        if (signalPower != null)
        {
            signalPower.ActivateSignal += Interact;
            signalPower.DeactivateSignal += DeInteract;
        }
    }

    protected void Start()
    {
        if (isReverse)
        {
            Interact();
        }
       
    }
    public virtual void Interact()
    {

    }
    public virtual void DeInteract()
    {

    }
}
