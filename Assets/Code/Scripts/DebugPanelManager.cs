using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/*
 * Manager Script for the Debug Panel. Can only be triggered used Hand Controllers.
 */

public class DebugPanelManager : MonoBehaviour
{
    // Sets start position for the ResetPosition button
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
        // Check if button to show the DebugPanel is pressed
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _pressed = true;
        }

        // Check for release
        if (_pressed && !OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _released = true;
            _pressed = false;
        }
        
        // Show Debug panel if released
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
