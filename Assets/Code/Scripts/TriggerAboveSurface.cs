using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAboveSurface : MonoBehaviour
{
    AudioSource m_MyAudioSource;

    void OnValidate()
    {
        // Default fog
        RenderSettings.fog = true;  
        //RenderSettings.fogColor = Color.HSVToRGB(202f / 360, 66f / 100f, 76f / 100f);
        //RenderSettings.fogMode = FogMode.ExponentialSquared;
        //RenderSettings.fogDensity = 0.005f;
    }

    void Start()
    {
        // Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Play audio if not already playing
        if (!m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.Play();
        }
        // Deactivate fog
        RenderSettings.fog = false;
    }

    void OnTriggerExit(Collider other)   
    {
        // Stop audio if playing
        if (m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.Stop();
        }
        // Activate fog
        RenderSettings.fog = true;
    }
}