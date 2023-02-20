using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainMenu : MonoBehaviour
{
    private GameRunner gameRunner;
    public Animator tvCamZoom;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += StartGame;
        VoiceRecognitionManager.OnHiss += QuitGame;

    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= StartGame;
        VoiceRecognitionManager.OnHiss -= QuitGame;

    }


    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
    }


    void StartGame()
    {
        gameRunner.GameComplete();
        tvCamZoom.SetTrigger("ZoomIn");
        FindObjectOfType<AudioManager>().Play("StartGame");
    }

    void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("StartGame");
        print("Quit Game");
    }

}
