using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TutorialCanvas : MonoBehaviour
{
    // Panel to be shown at the start of the Tutorial
    [SerializeField] private GameObject startPanel;
    // Panel to be shown after finishing the Controller Tutorial
    [SerializeField] private GameObject controllerPanel;
    // Panel to be shown after finishing the Hand-tracking Tutorial
    [SerializeField] private GameObject handsPanel;
    // Panel to be shown after finishing both Swimming Tutorials.
    [SerializeField] private GameObject endPanel;

    public GameObject StartPanel => startPanel;

    public GameObject ControllerPanel => controllerPanel;

    public GameObject HandsPanel => handsPanel;

    public GameObject EndPanel => endPanel;

    public Canvas Canvas => GetComponent<Canvas>();

    private void Awake()
    {
        // InitializePanels();
        HidePanels();
    }

    public void HidePanels()
    {
        startPanel.SetActive(false);
        controllerPanel.SetActive(false);
        handsPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    private void InitializePanels()
    {
        startPanel.SetActive(true);
        controllerPanel.SetActive(false);
        handsPanel.SetActive(false);
        endPanel.SetActive(false);
    }
}