using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTemplate : MonoBehaviour
{
    private GameRunner gameRunner;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
    }


    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
        StartCoroutine(StartSequencer());
    }

    void Update()
    {

    }

    void Meow()
    {
        gameRunner.GameComplete();

    }
    void Hiss()
    {

    }
    void Purr()
    {

    }


    private IEnumerator StartSequencer()
    {

        yield return new WaitForSeconds(1f);
        {

        }

    }
}
