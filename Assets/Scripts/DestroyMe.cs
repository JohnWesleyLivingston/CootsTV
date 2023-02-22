using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float timeToDestroy = 1f;
    
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }


}
