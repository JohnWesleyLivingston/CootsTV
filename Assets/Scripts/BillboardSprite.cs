using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Camera camToFollow;
    void Update()
    {
        transform.LookAt(camToFollow.transform);

    }
}
