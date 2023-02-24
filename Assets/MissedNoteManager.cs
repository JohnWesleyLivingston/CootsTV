using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedNoteManager : MonoBehaviour
{
    public GuitarHeroManager guitarHeroManager;


    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Blocker")
        {
            guitarHeroManager.MissNote();
            target.GetComponent<GuitarNote>().KillMe();
        }
    }
}
