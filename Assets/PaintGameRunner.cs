using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGameRunner : MonoBehaviour
{




    // Start is called before the first frame update
    void Start()
    {
        // gameRunner.GameComplete();
        StartCoroutine(StartSequencer());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartSequencer()
    {
        yield return new WaitForSeconds(1f);
        {

        }

    }
}
