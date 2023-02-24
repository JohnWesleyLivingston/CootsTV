using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarNote : MonoBehaviour
{
    public string musicalNote;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.back * 4 * Time.deltaTime;
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
