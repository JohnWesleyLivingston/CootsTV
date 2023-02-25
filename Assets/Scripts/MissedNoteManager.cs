using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedNoteManager : MonoBehaviour
{
    public GuitarHeroManager guitarHeroManager;
    private GuitarNeckSpawner guitarNeckSpawner;

    private void Start()
    {
        guitarNeckSpawner = FindObjectOfType<GuitarNeckSpawner>();
    }


    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Blocker")
        {
            guitarHeroManager.MissNote();
            target.GetComponent<GuitarNote>().KillMe();
        }

        if (target.tag == "GuitarNeck")
        {
            guitarNeckSpawner.SpawnTile();
            Destroy(target.gameObject, 2);
        }
    }
}
