using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFocusPoint : MonoBehaviour
{
    CinemachineVirtualCamera cmCam;

    private void Awake()
    {
        cmCam = FindObjectOfType<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Arms"))
        {
            cmCam.m_Follow = transform;
        }
    }
}
