using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blockGameObject;
    public GameObject objectToSpawn;

    private int worldSizeX = 80;
    private int worldSizeZ = 80;

    private int noiseHeight = 5;
    private float gridOffset = 1.1f;

    private List<Vector3> blockpositions = new List<Vector3>();

    void Start()
    {
        for (int x = 0; x < worldSizeX; x++){
            for (int z = 0; z < worldSizeZ; z++){
                Vector3 pos = new Vector3(x * gridOffset,GenerateNoise(x,z,8f) * noiseHeight,z* gridOffset);

                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                blockpositions.Add(block.transform.position);
                block.transform.SetParent(this.transform);
            }
        }
        SpawnObject();
      
    }



    //spawn different objects in sand

    private void SpawnObject ()
    {
        for(int c = 0;c < 20;c++)
        {
            GameObject toPlaceObject = Instantiate(objectToSpawn, 
                ObjectSpawnLocation(), 
                Quaternion.identity);
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockpositions.Count);

        Vector3 newPos = new Vector3(
            blockpositions[rndIndex].x,
            blockpositions[rndIndex].y + 0.5f,
            blockpositions[rndIndex].z);

        blockpositions.RemoveAt(rndIndex);
        return newPos;
    } 

    private float GenerateNoise (int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(xNoise, zNoise);
    }

}
