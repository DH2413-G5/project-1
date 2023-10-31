using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawn : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] public int numberOfFish = 10;
    [SerializeField] private Vector3 spawnBounds;

    public GameObject[] allFish { get; set; }
    List<Vector3> usedPositions = new List<Vector3>(); // Store used positions to avoid collisions.


    void Start()
    {
        SpawnFish();
    }

    void SpawnFish()
    {
     
        allFish = new GameObject[numberOfFish];
        
        for (int i = 0; i < numberOfFish; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            float height = (float)i / (float)(numberOfFish - 1) * 2.0f - 1.0f;

            randomVector = new Vector3(randomVector.x * spawnBounds.x, height * spawnBounds.y, randomVector.z * spawnBounds.z);


            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            //var rotation = Quaternion.identity;
            allFish[i] = Instantiate(fishPrefab, spawnPosition, rotation);
        }
    }


}
