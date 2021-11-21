using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IActivateable : MonoBehaviour
{
    public Action Activate;
    public Action Deactivate;
    public bool isActivated;
}
