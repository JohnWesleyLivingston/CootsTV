using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarNeckTile : MonoBehaviour
{



    private void FixedUpdate()
    {
        gameObject.transform.position += Vector3.back * 4 * Time.deltaTime;

    }
}
