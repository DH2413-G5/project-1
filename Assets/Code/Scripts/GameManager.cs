using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // Drag and drop the child objects of hand tracking in the Inspector
    public GameObject[] handTrackingChildObjects; 
    // Drag and drop the child objects of controller in the Inspector
    public GameObject[] controllerChildObjects; 

    private bool previousControllerState;

    void Start()
    {
        // Initially set the child objects for player2 active and for player inactive, and set the controller state
        ToggleChildObjects(handTrackingChildObjects, false);
        ToggleChildObjects(controllerChildObjects, true);
        previousControllerState = IsOVRControllerConnected();
    }

    void Update()
    {
        bool currentControllerState = IsOVRControllerConnected();

        // Only switch active state of child objects when the controller state changes
        if (currentControllerState != previousControllerState)
        {
            if (currentControllerState)
            {
                // Toggle child objects active state
                ToggleChildObjects(handTrackingChildObjects, false);
                ToggleChildObjects(controllerChildObjects, true);
            }
            else
            {
                // Toggle child objects active state
                ToggleChildObjects(handTrackingChildObjects, true);
                ToggleChildObjects(controllerChildObjects, false);
            }
        }

        previousControllerState = currentControllerState;
    }

    void ToggleChildObjects(GameObject[] objects, bool state)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(state);
        }
    }

    bool IsOVRControllerConnected()
    {
        // Check if Oculus controllers are connected
        return OVRInput.IsControllerConnected(OVRInput.Controller.RTouch) ||
               OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
    }
}