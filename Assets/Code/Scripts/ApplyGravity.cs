using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    private Collider _collider;
    private List<Rigidbody> _objectList;
    
    private void Awake()
    {
        // Ensure that the collider IsTrigger is set to true.
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _objectList = new List<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = FindRigidbody(other);
        _objectList.Add(rb);
    }

    private void FixedUpdate()
    {
        AddGravity();
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = FindRigidbody(other);
        _objectList.Remove(rb);
        // TODO (maybe) Add extra dragforce once it hits the water again.
    }

    private void AddGravity()
    {
       foreach (var obj in _objectList)
       {
           obj.AddForce(2*9.81f*Vector3.down, ForceMode.Acceleration);
       }
    }

    private Rigidbody FindRigidbody(Collider other)
    {
        // Find Rigidbody in current object, if not, find it in parent objects. Otherwise, look at children.
        Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>() ?? other.gameObject.GetComponentInChildren<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody on object found: " + other.name);
        }
        return rb;
    }
}
