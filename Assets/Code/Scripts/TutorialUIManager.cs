using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private TutorialCanvas TutorialCanvas;
    [SerializeField] private Camera EventCamera;
    [SerializeField] private GameObject UIHelper;
    [SerializeField] private GameObject HandEventSystemObject;
    [SerializeField] private GameObject ControllerEventSystemObject;
    // [SerializeField] private GameObject EventSystem;
    

    private bool _controllerPanelShown;
    private bool _handsPanelShown;
    private bool _canvasShown;
    private bool _changed;
    private GameObject _nextPanel;

    private EventSystem controllerEventSystem;
    private EventSystem handEventSystem;
    
    private void Start()
    {
        Assert.IsNotNull(TutorialCanvas);
        Assert.IsNotNull(UIHelper);
        _handsPanelShown = false;
        _controllerPanelShown = false;
        _nextPanel = TutorialCanvas.StartPanel;
        _canvasShown = true;
        _changed = true;
        controllerEventSystem = ControllerEventSystemObject.GetComponent<EventSystem>();
        handEventSystem = HandEventSystemObject.GetComponent<EventSystem>();
        // Can only have one EventSystem active at a time.
        HandEventSystemObject.SetActive(false);
        ShowPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_canvasShown && IsOVRControllerConnected())
        {
            // Camera cameraComponent = EventSystem.GetComponent<Camera>();
            // if (cameraComponent != null)
            // {
                // Destroy(cameraComponent);
            // }
            
            HandEventSystemObject.SetActive(false);
            TutorialCanvas.Canvas.worldCamera = EventCamera;
            
            
            UIHelper.SetActive(true);
            EventSystem.current = controllerEventSystem;
        }
        else
        {
            UIHelper.SetActive(false);
            HandEventSystemObject.SetActive(true);
            TutorialCanvas.Canvas.worldCamera = HandEventSystemObject.GetComponent<Camera>();
            EventSystem.current = handEventSystem;
        }

        if (_controllerPanelShown && _handsPanelShown)
        {
            _nextPanel = TutorialCanvas.EndPanel;
        }
    }

    public void HidePanels()
    {
        TutorialCanvas.HidePanels();
        _canvasShown = false;
    }

    public void ShowPanel()
    {
        _nextPanel.SetActive(true);
        _canvasShown = true;
    }

    public void SetControllerPanel()
    {
        _nextPanel = TutorialCanvas.ControllerPanel;
        _controllerPanelShown = true;
    }

    public void SetHandPanel()
    {
        _nextPanel = TutorialCanvas.HandsPanel;
        _handsPanelShown = true;
    }

    
    private bool IsOVRControllerConnected()
    {
        // Check if Oculus controllers are connected
        return OVRInput.IsControllerConnected(OVRInput.Controller.RTouch) ||
            OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
    }

    public void Clicked()
    {
        Debug.Log("Clicked");
    }
}
