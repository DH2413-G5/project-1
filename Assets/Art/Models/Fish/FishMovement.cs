using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private float swimSpeed = 2.0f;
    [SerializeField] private float turnSpeed = 20;
    [SerializeField] private float rotationFrequency = 5f;
    [SerializeField] private float rotationAmplitude = 30.0f;

    [Header("Detection")]
    [SerializeField] private float detectionDistance = 2.0f;

    [Header("Turning")]
    [SerializeField] private float rotationSpeed = .1f;
    

    private Vector3 bounds = new Vector3(20, 20, 20);
    private int boundSize = 10;

    private float timeCounter = 0.0f;
    private Vector3 initialPosition;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * swimSpeed;
        initialPosition = transform.position;
    }
    private void Update()
    {
        Swim();
    }

    private void Swim()
    {
        

        if (DetectObstacle() || !IsWithinBounds())
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);   
        }
       
            // Continue swimming
            rb.velocity = transform.forward * swimSpeed;
            float rotationAngle = Mathf.Sin(timeCounter * rotationFrequency) * rotationAmplitude;
            transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);
            transform.Rotate(Vector3.left, rotationAngle * Time.deltaTime);
            timeCounter += Time.deltaTime * rotationSpeed;
        


    }


    private bool DetectObstacle()
    {
        // Cast a ray in the forward direction and check for obstacles
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionDistance))
        {
            return true; // Obstacle detected
        }
        return false; // No obstacle detected
    }

    bool isOutsideBounds()
    {
        return (transform.position.x > (initialPosition.x + bounds.x)) || (transform.position.x < (initialPosition.x - bounds.x)) ||
            (transform.position.z > (initialPosition.z) + bounds.z) || (transform.position.z < (initialPosition.z - bounds.z));
    }

    private bool IsWithinBounds()
    {
        Vector3 position = transform.position;
        Vector3 halfBounds = bounds * 0.5f;

        return position.x >= initialPosition.x - halfBounds.x && position.x <= initialPosition.x + halfBounds.x &&
               position.y >= initialPosition.y - halfBounds.y && position.y <= initialPosition.y + halfBounds.y &&
               position.z >= initialPosition.z - halfBounds.z && position.z <= initialPosition.z + halfBounds.z;
    }

}