using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool gamePaused = false;
    public bool gameStarted;

    public GameObject pauseScreen;

    private QuitGame quitGame;

    public GameObject transitionScreen;

    public float currentVol;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
        VoiceRecognitionManager.OnPause += Paws;
        VoiceRecognitionManager.OnCoots += Coots;


    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
        VoiceRecognitionManager.OnPause -= Paws;
        VoiceRecognitionManager.OnCoots -= Coots;

    }

    void Start()
    {
        pauseScreen.SetActive(false);
        quitGame = FindObjectOfType<QuitGame>();
    }

    void Update()
    {
        
    }

    void Paws()
    {
        if (gameStarted)
        {
            gamePaused = !gamePaused;
            StartCoroutine(TVStatic());

            if (gamePaused)
            {
                currentVol = AudioListener.volume;
                AudioListener.volume = 0.1f;

                pauseScreen.SetActive(true);
                FindObjectOfType<AudioManager>().Play("PauseGame");
                Time.timeScale = 0;

            }
            else
            {
                AudioListener.volume = currentVol;

                pauseScreen.SetActive(false);
                FindObjectOfType<AudioManager>().Play("StartGame");
                Time.timeScale = 1;

            }
        }
    }

    void Meow()
    {

    }

    void Hiss()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
           // pauseScreen.SetActive(false);

            quitGame.Quit();
        }
    }

    void Purr()
    {

    }

    void Coots()
    {

    }

    private IEnumerator TVStatic()
    {
        transitionScreen.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        {
            transitionScreen.SetActive(false);
        }

    }
}
