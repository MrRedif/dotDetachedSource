using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInfo : MonoBehaviour
{
    public PlayerHead head;
    public PlayerHands hands;
    public PlayerLegs legs;
    public Action detachedHands;
    public Action attachedHands;
    public LayerMask ground;
    public LayerMask lookForLegs;
    public static bool enableControl = true;
    public Material holoMat;
    public Material legMat;
    public Material headMat;
    public Material armMat;
    SpriteRenderer legRender;
    SpriteRenderer headRender;
    SpriteRenderer armRender;
    public bool lockFromEditor;
    public static void ChangeControl(bool inp)
    {
        enableControl = inp;
    }
    public void Awake()
    {
        if (lockFromEditor) enableControl = false;
        holoMat.SetFloat("_Dissolve", 0.85f);
    }

    private void Start()
    {
        legRender = legs.GetComponent<SpriteRenderer>();
        armRender = hands.GetComponent<SpriteRenderer>();
        headRender = head.GetComponent<SpriteRenderer>();


        if (!SceneMaster.instance.hasLegs)
        { 
            legRender.material= holoMat;
        }
        if (!SceneMaster.instance.hasArms)
        {
            armRender.material = holoMat;
        }
        if (!SceneMaster.instance.hasHead)
        {  
            headRender.material = holoMat;
        }
    }


    public IEnumerator HoloAnim()
    {
        holoMat.SetFloat("_Dissolve", 0.85f);
        while (holoMat.GetFloat("_Dissolve") > 0)
        {
            holoMat.SetFloat("_Dissolve", holoMat.GetFloat("_Dissolve") - 0.65f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        if (headRender != null && armRender != null && legRender != null)
        {
            headRender.material = headMat;
            armRender.material = armMat;
            legRender.material = legMat;

        }

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
