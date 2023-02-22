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

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
        VoiceRecognitionManager.OnPause += Paws;


    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
        VoiceRecognitionManager.OnPause -= Paws;

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
                //Time.timeScale = 0;
                pauseScreen.SetActive(true);
                FindObjectOfType<AudioManager>().Play("PauseGame");

            }
            else
            {
                //Time.timeScale = 1;
                pauseScreen.SetActive(false);
                FindObjectOfType<AudioManager>().Play("StartGame");
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
            quitGame.Quit();
        }
    }

    void Purr()
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
