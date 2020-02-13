using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public GameObject elevator;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public bool doorsClosed = true;

    public int lowestFloor;
    public int currentFloor;
    public int maxFloor;

    private float _transitionSpeed = 2f;
    private int _movingDirection = 0;

    private Animator _doorAnimation;

    private bool _idle;
    private bool _goingUp;
    private int _upCallRequests;
    private int _downCallRequests;

    private Dictionary<int, bool> _upMovementOrders;
    private Dictionary<int, bool> _downMovementOrders;

    // Start is called before the first frame update
    private void Start()
    {
        _doorAnimation = GetComponent<Animator>();
        _idle = true;
        _goingUp = false;
        _upCallRequests = 0;
        _downCallRequests = 0;
        _upMovementOrders = new Dictionary<int, bool>();
        _downMovementOrders = new Dictionary<int, bool>();
        for (var i = lowestFloor; i <= maxFloor; i++)
        {
            // using dictionary because of case with floors with negative number...
            // still access with key is O(1)
            _upMovementOrders.Add(i, false);
            _downMovementOrders.Add(i, false);
        }
    }

    public void UpdateFloor(int floorNo)
    {
        currentFloor = floorNo;
        var container = _goingUp ? _upMovementOrders : _downMovementOrders;
        // if in meantime this floor was called
        if (container[currentFloor])
        {
            if (_goingUp)
            {
                _upCallRequests--;
            }
            else
            {
                _downCallRequests--;
            }
            OpenDoors();
            container[currentFloor] = false;
        }
    }

    void Update()
    {
        // if (_upCallRequests == 0 && _downCallRequests == 0) _idle = true;
        if (_upCallRequests <= 0 && _downCallRequests <= 0) return;
        if (!doorsClosed) return;
        // Elevator action requested!
        _goingUp = _upCallRequests > 0;
        ProceedOrders(_goingUp);
    }

    private void ProceedOrders(bool goingUp)
    {
        // if (!_idle) return;
        // _idle = false;
        var targetFloor = GetNextRequest(goingUp);
        // Debug.Log("Target: " + targetFloor);
        // var floorsToMove = CalculateFloorsToMove(targetFloor);
        Move(targetFloor);
    }

    private int GetNextRequest(bool goingUp)
    {
        // assign reference
        var container = goingUp ? _upMovementOrders : _downMovementOrders;
        foreach (var pair in container.Where(pair => pair.Value))
        {
            return pair.Key;
        }
        return int.MinValue;
    }

    private int CalculateFloorsToMove(int targetFloor)
    {
        return targetFloor - currentFloor;
    }

    public void Call(int floor, bool up)
    {
        // open doors when elevator has no other actions and is on current floor
        if (floor == currentFloor && _idle)
        {
            OpenDoors();
            return;
        }

        Debug.Log("Call up direction: " + up + " to floor: " + floor);
        // Requesting movement
        CloseDoors();
        if (up)
        {
            _upMovementOrders[floor] = true;
            _upCallRequests++;
        }
        else
        {
            _downMovementOrders[floor] = true;
            _downCallRequests++;
        }
    }

    private void Move(int floor)
    {
        _movingDirection = CalculateFloorsToMove(floor);
        var position = elevator.transform.position;
        position.y = floor * 4f;
        elevator.transform.position =
            Vector3.Lerp(elevator.transform.position, position, Time.deltaTime * _transitionSpeed);
    }

    private void OpenDoors()
    {
        if (!doorsClosed) return;
        _doorAnimation.Play("Elevator_door_open");
    }

    private void CloseDoors()
    {
        if (doorsClosed) return;
        _doorAnimation.Play("Elevator_door_close");
    }

    public void SetDoorStatus(int closedIsZero)
    {
        doorsClosed = closedIsZero == 0;
    }
}