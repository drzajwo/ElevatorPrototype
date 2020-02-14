using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideElevatorButton : MonoBehaviour
{
    public GameObject elevator;
    public int floor;
    public bool doorActionRequest;

    private ElevatorController m_ElevatorController;

    private void Start()
    {
        m_ElevatorController = elevator.GetComponent<ElevatorController>();
    }

    public void RegisterAction()
    {
        if (doorActionRequest)
        {
            // Open door
            if (floor == 0)
            {
                m_ElevatorController.OpenDoors();
            }
            // Close door
            else if (floor == 1)
            {
                m_ElevatorController.CloseDoors();
            }

            return;
        }

        var up = m_ElevatorController.CalculateFloorsToMove(floor) > 0;
        m_ElevatorController.Call(floor, up);
    }
}