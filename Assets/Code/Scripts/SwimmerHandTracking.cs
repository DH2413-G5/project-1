using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SwimmerHandTracking : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Determines the speed at which the player swims though the environment.")]
    [SerializeField]
    private float swimForce;

    [Tooltip("Determines the drag force of the water, slowing down the player.")] [SerializeField]
    private float dragForce;

    [Tooltip("Make the player drop naturally in the water.")] [SerializeField]
    private float gravity;

    [Tooltip("Maximum force applied to rigibody at once.")] [SerializeField]
    private float maxForce;

    [Tooltip(
        "When the Angle between the direction of the motion speed and the horizontal direction of the world is less than this Angle, the speed in the vertical direction is reduced.")]
    [SerializeField]
    private float limitedAngle;


    private HandVelocityCalculator _calculator;
    private Rigidbody _rigidbody;
    private bool _leftHandSwim = false;
    private bool _rightHandSwim = false;
    private SwimmerAudioController _audioController;
    
    
    private void Awake()
    {
        // Caching the rigidbody.
        _rigidbody = GetComponent<Rigidbody>();
        // Turn off gravity for a floating/swimming effect.
        _rigidbody.useGravity = false;
        // Prevent rotations of the rigidbody to combat motion sickness.
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        _calculator = GetComponent<HandVelocityCalculator>();
        _audioController=GetComponent<SwimmerAudioController>();
    }

    private void FixedUpdate()
    {
        var velocities = _calculator.GetWristVelocities();
        Debug.Log(velocities);
        Vector3 leftHandVelocity = velocities.leftVelocityVector;
        Vector3 rightHandVelocity = velocities.rightVelocityVector;
        Vector3 localVelocity = leftHandVelocity + rightHandVelocity;

        // Inverting velocity: moving forward by pulling backwards.

        if ((_leftHandSwim || _rightHandSwim) && localVelocity.magnitude > 0)
        {
            // Calculate the angle between the velocity and the horizontal direction
            Vector3 horizontalDirection = new Vector3(localVelocity.x, 0, localVelocity.z);
            float angleBetween = Vector3.Angle(horizontalDirection, localVelocity);
            // Print the velocities to Unity Console
            // Debug.Log("Local Velocity: " + localVelocity);
            localVelocity *= -1;

            if (angleBetween < limitedAngle)
            {
                // Smoothly reduce vertical speed
                localVelocity.y *= (angleBetween / limitedAngle);
            }

            Vector3 forceToAdd = localVelocity * swimForce;
            // Clamping the force to ensure it doesn't exceed maxForce
            forceToAdd = Vector3.ClampMagnitude(forceToAdd, maxForce);

            _rigidbody.AddForce(forceToAdd, ForceMode.Acceleration);
            /*Debug.Log("forceToAdd：" + forceToAdd);
            Debug.Log("Swimming");*/
        }

        // Apply water drag force if player is moving
        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }

        // Apply constant downward force for gravity.
        _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    public void LeftHandSwim()
    {
        _leftHandSwim = true;
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