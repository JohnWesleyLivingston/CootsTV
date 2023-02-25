using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed;

    public float deadZone;

    public Animator topTower;
    public Animator botTower;

    public bool isEndBox;

    // Start is called before the first frame update
    void Start()
    {
        float r1 = Random.Range(0, 1);
        topTower.SetFloat("CycleOffset", r1);
        float r2 = Random.Range(0, 1);
        botTower.SetFloat("CycleOffset", r2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEndBox)
        {
            transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

            if (transform.position.x < deadZone)
            {
                Destroy(gameObject);
            }
        }
        else
        {

            if (transform.position.x > -64)
            {
                transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;
            }
        }
    }


}
