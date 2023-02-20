using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainMenu : MonoBehaviour
{
    private GameRunner gameRunner;
    public Animator tvCamZoom;
    private PauseGame pauseGame;
    private QuitGame quitGame;
     
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
        pauseGame = FindObjectOfType<PauseGame>();
        quitGame = FindObjectOfType<QuitGame>();

    }


    void StartGame()
    {
        gameRunner.GameComplete();
        tvCamZoom.SetTrigger("ZoomIn");
        FindObjectOfType<AudioManager>().Play("StartGame");
        pauseGame.gameStarted = true;
    }

    void QuitGame()
    {
        tvCamZoom.SetTrigger("ZoomIn");
        quitGame.Quit();
    }



}
