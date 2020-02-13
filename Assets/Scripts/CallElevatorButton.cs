using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallElevatorButton : MonoBehaviour
{
    public BoxCollider floorCollider;
    public GameObject elevator;
    public int floor;
    public bool upDirection;

    private ElevatorController elevatorController;

    private void Start()
    {
        elevatorController = elevator.GetComponent<ElevatorController>();
    }

    public void CallElevator()
    {
        // change button color
        // play animation
        // play sound
        elevatorController.Call(floor, upDirection);
    }
}
