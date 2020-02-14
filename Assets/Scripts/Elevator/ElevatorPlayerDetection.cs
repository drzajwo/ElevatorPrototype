using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlayerDetection : MonoBehaviour
{
    private ElevatorController m_Controller;

    private void Start()
    {
        m_Controller = GetComponentInParent<ElevatorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_Controller.doorsOccupied = true;
        if (m_Controller.doorsClosing)
        {
            m_Controller.OpenDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_Controller.doorsOccupied = false;
    }
}