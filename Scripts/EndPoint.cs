using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    bool activated;
    [SerializeField] Transform goPoint;
    [SerializeField] string nextScene;
    public bool useBool = false;
    public SceneMaster.Context cont;
    bool hasHead;
    bool hasArms;
    bool hasLegs;

    private void Update()
    {
        if (useBool)
        {
            SceneMaster.instance.LoadLevel(nextScene);
            useBool = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Head"))
            hasHead = true;

        if (collision.gameObject.CompareTag("Arms"))
            hasArms = true;

        if (collision.gameObject.CompareTag("Player"))
            hasLegs = true;




        if (collision.isTrigger == false  && !activated && (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Arms") || collision.gameObject.CompareTag("Head")))
        {
            activated = true;
            SceneMaster.instance.hasHead = hasHead;
            SceneMaster.instance.hasArms = hasArms;
            SceneMaster.instance.hasLegs = hasLegs;

            SceneMaster.instance.LoadLevel(nextScene,cont);
        }

    }

    public void CallSkip()
    {
        if (!activated)
        {
            activated = true;
            SceneMaster.instance.LoadLevel(nextScene);
        }
    }
}
