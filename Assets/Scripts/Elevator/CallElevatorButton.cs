using UnityEngine;

namespace Elevator
{
    public class CallElevatorButton : MonoBehaviour
    {
        public BoxCollider floorCollider;
        public GameObject elevator;
        public int floor;
        public bool upDirection;

        private ElevatorController m_ElevatorController;
        
        private void Start()
        {
            m_ElevatorController = elevator.GetComponent<ElevatorController>();
        }

        public void CallElevator()
        {
            // change button color
            // play animation
            // play sound
            m_ElevatorController.Call(floor, upDirection);
        }
    }
}