using UnityEngine;
using UnityEngine.Assertions;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private TutorialCanvas TutorialCanvas;
    [SerializeField] private Camera EventCamera;
    [SerializeField] private GameObject UIHelper;
    [SerializeField] private GameObject HandTrackingEventSystem;

    private bool _controllerShown;
    private bool _handsShown;
    private bool _canvasShown;
    private GameObject _nextPanel;
    private void Start()
    {
        Assert.IsNotNull(TutorialCanvas);
        Assert.IsNotNull(UIHelper);
        _handsShown = false;
        _controllerShown = false;
        _nextPanel = TutorialCanvas.StartPanel;
        _canvasShown = true;
        // Can only have one EventSystem active at a time.
        HandTrackingEventSystem.SetActive(false);
        ShowPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canvasShown && IsOVRControllerConnected())
        {
            UIHelper.SetActive(true);
            Camera cameraComponent =HandTrackingEventSystem.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                Destroy(cameraComponent);
            }
            TutorialCanvas.GetComponent<Canvas>().worldCamera = EventCamera;
            HandTrackingEventSystem.SetActive(false);
        }
        else
        {
            UIHelper.SetActive(false);
            HandTrackingEventSystem.SetActive(true);
        }

        if (_controllerShown && _handsShown)
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
        _controllerShown = true;
    }

    public void SetHandPanel()
    {
        _nextPanel = TutorialCanvas.HandsPanel;
        _handsShown = true;
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
