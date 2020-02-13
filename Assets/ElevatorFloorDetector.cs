using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorFloorDetector : MonoBehaviour
{
    private ElevatorController controller;
    private void Start()
    {
        controller = GetComponentInParent<ElevatorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FloorIndicator")) return;
        var splittedName = other.name.Split('_');
        var currentFloor = int.Parse(splittedName[1]);
        controller.UpdateFloor(currentFloor);
    }
}
