using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpMovement : MonoBehaviour
{
    [SerializeField] private float rotationFrequency = 10f;
    [SerializeField] private float rotationAmplitude = 1.0f;
    [SerializeField] private float rotationSpeed = .1f;
    private float timeCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        float randomRotationAngle = Random.Range(0, 360);
        Quaternion randomRotation = Quaternion.Euler(0f, randomRotationAngle, 0f);
        transform.rotation = randomRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = Mathf.Sin(timeCounter * rotationFrequency) * rotationAmplitude;
        transform.Rotate(Vector3.right, rotationAngle * Time.deltaTime);
        transform.Rotate(Vector3.back, rotationAngle * Time.deltaTime);
        timeCounter += Time.deltaTime * rotationSpeed;
    }
}
