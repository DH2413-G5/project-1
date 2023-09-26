using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " exited TriggerArea.");
    }
}
