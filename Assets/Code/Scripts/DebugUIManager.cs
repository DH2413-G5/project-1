using UnityEngine;
using TMPro;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;

    // Provides a public method to update text content
    public static DebugUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple DebugUIManager instances found! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    // Provides a public method to update text content
    public void UpdateDebugText(string text)
    {
        if (debugText != null)
        {
            debugText.text = text;
        }
    }
}

