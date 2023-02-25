using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;

    public float spawnRate;

    private float timer = 0;

    public float heightOffset;

    private bool spawnOverride;
    public GameObject endBox;
    private GameObject[] boxes;
    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnOverride)
        {
            if (timer < spawnRate)
            {
                timer = timer + Time.deltaTime;
            }

            else
            {
                spawnPipe();
                timer = 0;
            }
        }
    }

    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), transform.position.z), transform.rotation);
    }

    public void SpawnEndPipe()
    {
        spawnOverride = true;
        Instantiate(endBox, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
    }

    public void DestroyAllBoxes()
    {
        boxes = GameObject.FindGameObjectsWithTag("Tower");
        for (int i = 0; i < boxes.Length; i++)
        {
            Destroy(boxes[i]);
        }
    }
}
