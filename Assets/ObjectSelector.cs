using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public new Camera camera;
    
    private void Update()
    {
        // Ray from camera (player eyes)
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;
        // Getting object
        var objectHit = hit.transform;
        // We are interested only with elevator buttons
        if (!objectHit.CompareTag("ElevatorButton") /* ||  !objectHit.CompareTag("ElevatorInsideButton")*/) return;
        // TODO: Show hint with button or highlight it
        if (!Input.GetKeyDown(KeyCode.E)) return;
        var button = objectHit.gameObject.GetComponent<CallElevatorButton>();
        button.CallElevator();
    }
}
