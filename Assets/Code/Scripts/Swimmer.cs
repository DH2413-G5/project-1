using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    private HandVelocityCalculator calculator;
    [Header("Values")]
    [Tooltip("Determines the speed at which the player swims though the environment.")]
    [SerializeField] private float swimForce = 2f;
    [Tooltip("Determines the drag force of the water, slowing down the player.")]
    [SerializeField] private float dragForce = 1f;
    [Tooltip("Minimum force needed for the player to start swimming.")]
    [SerializeField] private float minVelocity = 0.2f;
    [Tooltip("Limits the amount of strokes per second.")]
    private bool leftHandSwim = false;
    private bool rightHandSwim = false;
    private void Start()
    {
        calculator = GetComponent<HandVelocityCalculator>();
    }

    private void Update()
    {
        if (calculator != null)
        {
            var velocities = calculator.GetWristVelocities();
            if(Mathf.Abs(velocities.leftVelocity) > minVelocity)
            {
                Debug.Log($"Left hand velocity: {velocities.leftVelocity}");
            }
            if(Mathf.Abs(velocities.rightVelocity) > minVelocity)
            {
                Debug.Log($"Right hand velocity: {velocities.rightVelocity}");
            }
        }
    }

    public void LeftHandSwim()
    {
        leftHandSwim= true;
    }
    
    public void LeftHandStopSwim()
    {
        leftHandSwim = false;
    }
    
    public void RightHandSwim()
    {
        rightHandSwim = true;
    }
    
    public void RightHandStopSwim()
    {
        rightHandSwim = false;
    }
}
