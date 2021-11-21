using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLine : MonoBehaviour
{
    LineRenderer ln;
    public bool active;
    PlayerHands hand;
    private void Awake()
    {
        ln = GetComponent<LineRenderer>();
        ln.SetPosition(0, Vector3.zero);
        hand = GetComponentInParent<PlayerHands>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.takeMousePos)
        {
            Vector3 cm = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cm.z = 0;
            Vector3 hld = (transform.position - cm);
            hld = new Vector3(hld.x * Mathf.Sign(transform.lossyScale.x), hld.y, hld.z);
            ln.SetPosition(1,hld);
            ln.SetPosition(1,Vector3.ClampMagnitude(ln.GetPosition(1),3.5f));
            
        }
        else
        {
            ln.SetPosition(1,Vector3.zero);
        }
    }
}
