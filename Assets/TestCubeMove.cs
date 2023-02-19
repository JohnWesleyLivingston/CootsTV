using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeMove : MonoBehaviour
{
    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += CubeMove;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= CubeMove;
    }

    void CubeMove()
    {
        transform.Translate(1, 0, 0);
        print("move");
    }


}
