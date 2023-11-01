using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogColorController : MonoBehaviour
{
    [SerializeField] private float hue;
    private float saturation;
    private float value;
    [SerializeField] private float saturationMin;
    [SerializeField] private float saturationMax;
    [SerializeField] private float valueMin;
    [SerializeField] private float valueMax;

    private GameObject player;
    private float playerZ;
    private float zMax;
    private void OnValidate()
    {
        // Initialize color
        hue = 202f;
        saturation = 66f;
        value = 76f;
        saturationMin = 15f;
        saturationMax = 70f;
        valueMin = 40f;
        valueMax = 80f;

        // Activate default fog
        RenderSettings.fog = true;  
        RenderSettings.fogColor = Color.HSVToRGB(hue / 360, saturation / 100f, value / 100f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.008f;
    }
    void Start()
    {
        // The maximum z-coordinate value (should preferrably be dynamic, hardcoded for now)
        zMax = 400f;
    }

    void Update()
    {
        // Find the player object based on its tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Get the player's z-coordinate
            playerZ = player.transform.position.z;

            // Calculate the color ratio based on the player's clamped z-coordinate (inverse proportional)
            float ratio = 1f - Mathf.Clamp01(playerZ / zMax);

            // Calculate saturation and value from ratio
            saturation = saturationMin + ratio * (saturationMax - saturationMin);
            value = valueMin + ratio * (valueMax - valueMin);

            // Set fog color based on HSV to RGB conversion
            RenderSettings.fogColor = Color.HSVToRGB(hue / 360f, saturation / 100f, value / 100f);
        }
        else
        {
            // Player object is not found
            Debug.LogWarning("Player object not found for setting fog saturation.");
        }
    }

}
