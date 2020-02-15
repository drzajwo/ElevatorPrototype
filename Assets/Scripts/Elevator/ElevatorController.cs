using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elevator
{
    public class ElevatorController : MonoBehaviour
    {
        public GameObject elevator;
        // statuses - doors
        [HideInInspector]
        public bool doorsClosed = true;
        [HideInInspector]
        public bool doorsClosing = false;
        [HideInInspector]
        public bool doorsOpening = false;
        [HideInInspector]
        public bool doorsOccupied = false;
        // statuses actions
        [HideInInspector]
        public bool inMove = false;

        public int lowestFloor;
        [HideInInspector]
        public int currentFloor;
        public int maxFloor;

        private readonly float _transitionSpeed = 2f;
        private readonly float _timeToCloseDoors = 4f;

        private Animator m_DoorAnimation;

        private bool m_GoingUp;
        private int m_UpCallRequests;
        private int m_DownCallRequests;

        private Dictionary<int, bool> m_UpMovementOrders;

        private Dictionary<int, bool> m_DownMovementOrders;

        // animation variable used for reversing
        private static readonly int Direction = Animator.StringToHash("direction");

        private void Start()
        {
            m_DoorAnimation = GetComponent<Animator>();
            m_GoingUp = false;
            m_UpCallRequests = 0;
            m_DownCallRequests = 0;
            m_UpMovementOrders = new Dictionary<int, bool>();
            m_DownMovementOrders = new Dictionary<int, bool>();
            for (var i = lowestFloor; i <= maxFloor; i++)
            {
                // using dictionary because of case with floors with negative number...
                // still access with key is O(1)
                m_UpMovementOrders.Add(i, false);
                m_DownMovementOrders.Add(i, false);
            }
        }

        private void Update()
        {
            if (m_UpCallRequests <= 0 && m_DownCallRequests <= 0) return;
            if (!doorsClosed) return;
            // Elevator action requested!
            m_GoingUp = m_UpCallRequests > 0;
            ProceedOrders(m_GoingUp);
        }

        // ORDERS HANDELING

        private void ProceedOrders(bool goingUp)
        {
            var targetFloor = GetNextRequest(goingUp);
            Move(targetFloor);
        }

        public void UpdateFloor(int floorNo)
        {
            currentFloor = floorNo;
            var container = m_GoingUp ? m_UpMovementOrders : m_DownMovementOrders;
            // if in meantime this floor was called
            if (container[currentFloor])
            {
                if (m_GoingUp)
                {
                    m_UpCallRequests--;
                }
                else
                {
                    m_DownCallRequests--;
                }

                OpenDoors();
                container[currentFloor] = false;
            }
        }

        public void Call(int floor, bool up)
        {
            // open doors when elevator has no other actions and is on current floor
            if (floor == currentFloor)
            {
                OpenDoors();
                return;
            }

            // Debug.Log("Call up direction: " + up + " to floor: " + floor);
            // Requesting movement
            CloseDoors();
            if (up)
            {
                if (m_UpMovementOrders[floor]) return;
                m_UpMovementOrders[floor] = true;
                m_UpCallRequests++;
            }
            else
            {
                if (m_DownMovementOrders[floor]) return;
                m_DownMovementOrders[floor] = true;
                m_DownCallRequests++;
            }
        }

        // ELEVATOR MOVEMENT

        private void Move(int floor)
        {
            inMove = true;
            var position = elevator.transform.position;
            position.y = floor * 4f;
            elevator.transform.position =
                Vector3.Lerp(elevator.transform.position, position, Time.deltaTime * _transitionSpeed);
        }

        // ELEVATOR DOORS

        public void OpenDoors()
        {
            inMove = false;
            if (!doorsClosed && !doorsClosing) return;
            if (doorsClosing)
            {
                m_DoorAnimation.SetFloat(Direction, -1f);
                Invoke(nameof(CloseDoors), _timeToCloseDoors / 3);
                return;
            }

            m_DoorAnimation.Play("Elevator_door_open");
            Invoke(nameof(CloseDoors), _timeToCloseDoors);
        }

        public void CloseDoors()
        {
            if (doorsClosed) return;
            // If player stands between doors then reset counter
            if (doorsOccupied)
            {
                Invoke(nameof(CloseDoors), _timeToCloseDoors);
                return;
            }

            m_DoorAnimation.SetFloat(Direction, 1f);
            m_DoorAnimation.Play("Elevator_door_close");
        }

        public void SetDoorStatus(int status)
        {
            doorsClosing = status == 2;
            doorsOpening = status == 3;
            doorsClosed = status == 0;
        }

        // HELPER METHODS
        private int GetNextRequest(bool goingUp)
        {
            // assign reference
            var container = goingUp ? m_UpMovementOrders : m_DownMovementOrders;
            foreach (var pair in container.Where(pair => pair.Value))
            {
                return pair.Key;
            }

            return int.MinValue;
        }

        public int CalculateFloorsToMove(int targetFloor)
        {
            return targetFloor - currentFloor;
        }
    }
}