using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

/*
 * Script that controls the UI elements, such as panels, in the tutorial scene.
 */

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private TutorialCanvas TutorialCanvas;
    [SerializeField] private Camera EventCamera;
    [SerializeField] private GameObject UIHelper;
    [SerializeField] private GameObject HandEventSystemObject;
    [SerializeField] private GameObject ControllerEventSystemObject;
    [SerializeField] private GameObject DebugCanvas;
    

    private bool _controllerPanelShown;
    private bool _handsPanelShown;
    private bool _canvasShown;
    private GameObject _nextPanel;

    private EventSystem _controllerEventSystem;
    private EventSystem _handEventSystem;
    
    private void Start()
    {
        Assert.IsNotNull(TutorialCanvas);
        Assert.IsNotNull(UIHelper);
        _handsPanelShown = false;
        _controllerPanelShown = false;
        _nextPanel = TutorialCanvas.StartPanel;
        _canvasShown = true;
        _controllerEventSystem = ControllerEventSystemObject.GetComponent<EventSystem>();
        _handEventSystem = HandEventSystemObject.GetComponent<EventSystem>();
        // We can only have on EventSystem active. Hence we start off with Controller.
        TurnOnController();
        // Show the first panel
        ShowPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((_canvasShown || DebugCanvas.activeSelf) && IsOVRControllerConnected())
        {
            TurnOnController();
        }
        else
        {
            TurnOnHandtracking();
        }

        // If both tutorials have been completed, show the final panel.
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

    private void TurnOnController()
    {
        // Turn on event systems for Controller
        TutorialCanvas.Canvas.worldCamera = EventCamera;
        UIHelper.SetActive(true);
        EventSystem.current = _controllerEventSystem;
        // Turn off Hand-tracking
        if (HandEventSystemObject.activeSelf)
        {
            HandEventSystemObject.SetActive(false);
        }
    }

    private void TurnOnHandtracking()
    {
        // Turn on Hand-tracking
        HandEventSystemObject.SetActive(true);
        TutorialCanvas.Canvas.worldCamera = HandEventSystemObject.GetComponent<Camera>();
        EventSystem.current = _handEventSystem;
        // Turn off Controller
        if (UIHelper.activeSelf)
        {
            UIHelper.SetActive(false);
        }
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
