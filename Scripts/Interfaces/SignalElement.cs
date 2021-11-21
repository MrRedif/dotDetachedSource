using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignalElement:MonoBehaviour
{
    public static List<SignalElement> elements = new List<SignalElement>();
    public Action AddToNetwork;
    public SignalElement source;
    public SignalElement next;
    public Action RemoveFromNetwork;
    public bool active;
    public bool lookForConnection;
    protected LineRenderer ln;
    Color activeColor;
    public Color breakColor;

    protected void Awake()
    {
        ln = GetComponent<LineRenderer>();
        elements.Add(this);
        RemoveFromNetwork += RemoveSelf;

        AddToNetwork += Activate;
        activeColor = ln.startColor;
        
    }
    private void OnDestroy()
    {
        elements.Remove(this);
    }

    private void FixedUpdate()
    {
        if (lookForConnection)
        {
            float limit = 3.75f;
            SignalElement fnd = null;
            foreach (var item in elements)
            {
                if (item == this) continue;
                if (item.active == false)
                {
                    float hld = Vector2.Distance(transform.position, item.transform.position);
                    if (hld < limit)
                    {
                        limit = hld;
                        fnd = item;
                    }
                }
            }
            if (fnd != null)
            {
                lookForConnection = false;
                fnd.AddToNetwork?.Invoke();
                next = fnd;
                fnd.source = this;
            }


        }
        if (active && source != null)
        {
            if (Vector2.Distance(transform.position, source.transform.position) > 3.0f)
            { ln.startColor = breakColor; ln.endColor = breakColor; }
            else
            { ln.startColor = activeColor; ln.endColor = activeColor; }

            if (Vector2.Distance(transform.position,source.transform.position) > 3.75f)
            {
                RemoveFromNetwork?.Invoke();
                ln.SetPosition(1, transform.position);
            }
            else
            {
                ln.SetPosition(1, source.transform.position);
            }
        }
        else
        {
            ln.SetPosition(1, transform.position);
        }
        ln.SetPosition(0, transform.position);
    }


    public void RemoveNext()
    {
        if (next != null)
        {
            next?.RemoveFromNetwork();
        }
        next = null;
    }

    public void RemoveSelf()
    {
        RemoveNext();
        Debug.Log("Removed");
        active = false;
        lookForConnection = false;
        source.next = null;
        source.Activate();
        source = null;
    }
    public void Activate()
    {
        lookForConnection = true;
        active = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3.75f);
    }
}
