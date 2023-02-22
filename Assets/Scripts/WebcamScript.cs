using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamScript : MonoBehaviour
{
    static WebCamTexture webCamTex;

    void Start()
    {
        if(webCamTex == null)
        {
            webCamTex = new WebCamTexture();
        }
        else
        {
            print("No cam");
        }
        GetComponent<Renderer>().material.mainTexture = webCamTex;

        if(!webCamTex.isPlaying)
        {
            webCamTex.Play();
        }
        else
        {
            print("No cam");
        }
    }

}
