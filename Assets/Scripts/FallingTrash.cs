using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrash : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // The object to spawn and make it fall

    [SerializeField]
    private Transform groundTransform; // The transform representing the ground

    [SerializeField]
    private float spawnHeight = 10f; // Height from which the object will fall
    [SerializeField]
    private float fallSpeed = 5f; // Speed at which the object falls

    private Vector3 spawnPosition; // The initial spawn position

    private void Start()
    {
        if (objectToSpawn == null || groundTransform == null)
        {
            Debug.LogError("Please assign 'objectToSpawn' and 'groundTransform' in the Inspector.");
            enabled = false;
            return;
        }

        spawnPosition = new Vector3(
            Random.Range(groundTransform.position.x - 5f, groundTransform.position.x + 5f),
            spawnHeight,
            Random.Range(groundTransform.position.z - 5f, groundTransform.position.z + 5f)
        );

        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }

    private void Update()
    {
        // Ensure the object stays on the ground
        if (spawnPosition.y > groundTransform.position.y)
        {
            spawnPosition.y -= fallSpeed * Time.deltaTime;
            objectToSpawn.transform.position = spawnPosition;
        }
        else
        {
            spawnPosition.y = groundTransform.position.y;
        }
    }
}
