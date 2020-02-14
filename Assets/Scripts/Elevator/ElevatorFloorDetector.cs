using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorFloorDetector : MonoBehaviour
{
    private ElevatorController m_Controller;

    private void Start()
    {
        m_Controller = GetComponentInParent<ElevatorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FloorIndicator")) return;
        var splittedName = other.name.Split('_');
        var currentFloor = int.Parse(splittedName[1]);
        m_Controller.UpdateFloor(currentFloor);
    }
}