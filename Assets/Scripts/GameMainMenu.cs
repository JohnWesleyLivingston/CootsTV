using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMainMenu : MonoBehaviour
{
    private GameRunner gameRunner;
    public Animator tvCamZoom;
    private PauseGame pauseGame;
    private QuitGame quitGame;

    private int volumeCounter = 4;
    public Image volumeIcon;
    public Sprite[] volumeSprites;
     
    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += StartGame;
        VoiceRecognitionManager.OnHiss += QuitGame;
        VoiceRecognitionManager.OnPurr += ToggleVolume;

    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= StartGame;
        VoiceRecognitionManager.OnHiss -= QuitGame;
        VoiceRecognitionManager.OnPurr -= ToggleVolume;

    }


    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
        pauseGame = FindObjectOfType<PauseGame>();
        quitGame = FindObjectOfType<QuitGame>();

        volumeCounter = 3;
        AudioListener.volume = 0.67f;
        volumeIcon.sprite = volumeSprites[2];
        
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

    void ToggleVolume()
    {
        volumeCounter++;

        if(volumeCounter == 1)
        {
            AudioListener.volume = 0;
            volumeIcon.sprite = volumeSprites[0];
        }
        if (volumeCounter == 2)
        {
            AudioListener.volume = 0.34f;
            volumeIcon.sprite = volumeSprites[1];
        }
        if (volumeCounter == 3)
        {
            AudioListener.volume = 0.67f;
            volumeIcon.sprite = volumeSprites[2];
        }
        if (volumeCounter == 4)
        {
            AudioListener.volume = 1;
            volumeCounter = 0;
            volumeIcon.sprite = volumeSprites[3];
        }
    }

}
