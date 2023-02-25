using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedNoteManager2 : MonoBehaviour
{
    public GuitarHeroManager2 guitarHeroManager2;
    private GuitarNeckSpawner guitarNeckSpawner;

    private void Start()
    {
        guitarNeckSpawner = FindObjectOfType<GuitarNeckSpawner>();
    }


    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Blocker")
        {
            guitarHeroManager2.MissNote();
            target.GetComponent<GuitarNote>().KillMe();
        }

        if (target.tag == "GuitarNeck")
        {
            guitarNeckSpawner.SpawnTile();
            Destroy(target.gameObject, 2);
        }
    }
}
