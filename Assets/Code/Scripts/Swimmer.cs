using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{

    [Header("Values")]
    [Tooltip("Determines the speed at which the player swims though the environment.")]
    [SerializeField] private float swimForce = 5f;
    [Tooltip("Determines the drag force of the water, slowing down the player.")]
    [SerializeField] private float dragForce = 1f;
    [Tooltip("Minimum velocity needed for the player to start swimming.")]
    [SerializeField] private float minVelocity = 0.2f;
    [Tooltip("Maximum force applied to rigibody at once.")]
    [SerializeField] private float maxForce = 100f;
    
    private HandVelocityCalculator _calculator;
    private Rigidbody _rigidbody;
    private bool _leftHandSwim = false;
    private bool _rightHandSwim = false;
    
    private void Awake() {
        // Caching the rigidbody.
        _rigidbody = GetComponent<Rigidbody>();
        // Turn off gravity for a floating/swimming effect.
        _rigidbody.useGravity = false;
        // Prevent rotations of the rigidbody to combat motion sickness.
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void Start()
    {
        _calculator = GetComponent<HandVelocityCalculator>();
    }

    private void FixedUpdate()
    {
        var velocities = _calculator.GetWristVelocities();
        Vector3 leftHandVelocity = velocities.leftVelocityVector.magnitude > minVelocity ? velocities.leftVelocityVector : Vector3.zero;
        Debug.Log(velocities.leftVelocityVector.sqrMagnitude);
        Vector3 rightHandVelocity = velocities.rightVelocityVector.magnitude > minVelocity ? velocities.rightVelocityVector : Vector3.zero;
        Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
        
        // Inverting velocity: moving forward by pulling backwards.
        localVelocity *= -1;
        if (_leftHandSwim || _rightHandSwim)
        {
            Vector3 forceToAdd = localVelocity * (localVelocity.sqrMagnitude * swimForce);
            // Clamping the force to ensure it doesn't exceed maxForce
            forceToAdd = Vector3.ClampMagnitude(forceToAdd, maxForce);

            _rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
            
            Debug.Log(localVelocity);
            Debug.Log("Swimming");
        }

        // Apply water drag force if player is moving
        if (_rigidbody.velocity.sqrMagnitude > 0.01f) {
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }
    }
    
    public void LeftHandSwim()
    {
        _leftHandSwim= true;
    }
    
    public void LeftHandStopSwim()
    {
        _leftHandSwim = false;
    }
    
    public void RightHandSwim()
    {
        _rightHandSwim = true;
    }
    
    public void RightHandStopSwim()
    {
        _rightHandSwim = false;
    }
}
