using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAboveSurfaceAudio : MonoBehaviour
{
    AudioSource m_MyAudioSource;

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
            Debug.Log("Above surface audio played");
        }
        Debug.Log("Entered " + other.name);
    }

    void OnTriggerExit(Collider other)   
    {
        // Stop audio if playing
        if (m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.Stop();
            Debug.Log("Above surface audio stopped");
        }
    }
}
