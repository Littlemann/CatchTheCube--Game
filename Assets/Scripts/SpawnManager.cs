using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    Collider[] colliders;

    float minX = -4f;
    float minZ = 0f;
    float maxX = 4f;
    float maxZ = 5f;
    float randomX, randomZ;
    Vector3 spawnPos;
    int randomPrefab;
    
    GameObject objects;
    List<GameObject> spawnList;

    private void Start()
    {
        randomPrefab = Random.Range(0, prefabs.Length);
        SpawnObjects();
    }
    private void Update()
    {

    }

    void SpawnObjects()
    {
        for(int i = 0; i <3 ; i++)
        {
            randomX = Random.Range(minX, maxX);
            randomZ = Random.Range(minZ, maxZ);
            spawnPos = new Vector3(transform.position.x + randomX, 0.3f, transform.position.z + randomZ);
            objects = Instantiate(prefabs[randomPrefab], spawnPos, Quaternion.identity);
            objects.transform.parent = transform;
        }
    }
}
