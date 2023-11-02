using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // Drag and drop the child objects of hand tracking in the Inspector
    public GameObject[] handTrackingChildObjects; 
    // Drag and drop the child objects of controller in the Inspector
    public GameObject[] controllerChildObjects;
    private bool _previousControllerState;

    void Start()
    {
        InitializeInputDevice();   
    }

    void Update()
    {
        UpdateInputDevice();
    }

    /**
     * Function used to initialize the input devices. 
     */
    void InitializeInputDevice()
    {
        // This will force the first UpdateInputDevice function, thus initializing the input devices.
        _previousControllerState = !IsOVRControllerConnected();
    }

    /**
     * Function used to check and update if the input devices change.
     */
    void UpdateInputDevice()
    {
        bool currentControllerState = IsOVRControllerConnected();
        // Only switch active state of child objects when the controller state changes
        if (currentControllerState != _previousControllerState)
        {
            if (currentControllerState)
            {
                // Toggle child objects active state
                ToggleChildObjects(handTrackingChildObjects, false);
                ToggleChildObjects(controllerChildObjects, true);
                _previousControllerState = false;
            }
            else
            {
                // Toggle child objects active state
                ToggleChildObjects(handTrackingChildObjects, true);
                ToggleChildObjects(controllerChildObjects, false);
                _previousControllerState = true;
            }

            _previousControllerState = currentControllerState;
        }
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