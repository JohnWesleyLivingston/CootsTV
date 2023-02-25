using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarNote : MonoBehaviour
{
    public float moveSpeed = 4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }

    public void KillMe()
    {
        Destroy(gameObject, 3f);
    }

    public void KillMeImmediate()
    {
        Destroy(gameObject);
    }
}
