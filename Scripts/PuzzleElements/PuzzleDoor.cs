using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : Interactable
{
    Animator anim;
    public Collider2D coll;
    private void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        anim.SetBool("Active", true);
        coll.isTrigger = true;
    }

    public override void DeInteract()
    {
        anim.SetBool("Active", false);
        coll.isTrigger = false;

    }

}
