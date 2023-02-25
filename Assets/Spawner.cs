using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;

    public float spawnRate;

    private float timer = 0;

    public float heightOffset;



    void Start()
    {
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }

        else
        {
            int r = Random.Range(4, 12);
            spawnRate = r;

            SpawnObject();
            timer = 0;
        }
    }

    void SpawnObject()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        int r = Random.Range(0, objectsToSpawn.Length);

        Instantiate(objectsToSpawn[r], new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), transform.position.z), transform.rotation);
    }


}