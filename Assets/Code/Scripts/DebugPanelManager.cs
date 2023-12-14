using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class DebugPanelManager : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int tutorialSceneBuildIndex = 0;
    [SerializeField] private int mainSceneBuildIndex = 1;
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private GameObject uiHelper;

    private bool _pressed, _released;
    
    private void Awake()
    {
        Assert.IsNotNull(startPosition);
        Assert.IsNotNull(playerTransform);
        Assert.IsNotNull(uiHelper);
        _pressed = false; 
        _released = false;
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _pressed = true;
        }

        if (_pressed && !OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _released = true;
            _pressed = false;
        }
        
        if (_released)
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
            _released = false;
            uiHelper.SetActive(debugPanel.activeSelf);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(tutorialSceneBuildIndex);
    }

    public void ResetCurrentScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ResetMainScene()
    {
        SceneManager.LoadScene(mainSceneBuildIndex);
    }

    public void ResetPosition()
    {
        playerTransform.position = startPosition.position;
    }
}
