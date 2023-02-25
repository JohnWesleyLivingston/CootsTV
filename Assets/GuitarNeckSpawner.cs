using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarNeckSpawner : MonoBehaviour
{
    public GameObject groundTile;
    //Vector3 nextSpawnPoint;
  //  public Vector3 nextSpawnPoint;
    public Transform initialSpawn;

    private bool initalSpawnComplete;

    public void SpawnTile()
    {
        if (initalSpawnComplete)
        {
            initialSpawn.position = gameObject.transform.position;
        }


        GameObject temp = Instantiate(groundTile, initialSpawn.position, Quaternion.identity);
        if (!initalSpawnComplete)
        {
            initialSpawn.position = temp.transform.GetChild(1).transform.position;
        }
    }


    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
        initalSpawnComplete = true;

    }


}
