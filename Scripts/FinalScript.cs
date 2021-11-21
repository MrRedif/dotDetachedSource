using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScript : MonoBehaviour
{
    public bool active = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Arms"))
        {
            collision.isTrigger = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneMaster.instance.ReloadScene();
        }
    }
}
