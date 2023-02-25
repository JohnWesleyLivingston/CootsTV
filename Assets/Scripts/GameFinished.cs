using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinished : MonoBehaviour
{

    private PauseGame pauseGame;
    private QuitGame quitGame;

    public GameObject transitionScreen;
    public Animator tvCamZoom;

    public GameObject quitOverlay;

    public GameObject finishedText;
    public GameObject loadingText;

    public GameObject creditsScreen;
    public bool inCredits;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Credits;

    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Credits;

    }

    void Start()
    {
        pauseGame = FindObjectOfType<PauseGame>();
        quitGame = FindObjectOfType<QuitGame>();

        tvCamZoom.SetTrigger("ZoomOut");
        FindObjectOfType<AudioManager>().Play("PauseGame");

        pauseGame.gameStarted = false;
        quitOverlay.SetActive(false);

    }

    void Meow()
    {
        if (!inCredits)
        {
            StartCoroutine(RestartDelay());
        }
        else
        {
            creditsScreen.SetActive(false);
            inCredits = false;
        }
    }

    void Hiss()
    {
        if (!inCredits)
        {
            quitGame.Quit();
        }

    }

    void Credits()
    {
        creditsScreen.SetActive(true);
        inCredits = true;

    }

    private IEnumerator RestartDelay()
    {
        FindObjectOfType<AudioManager>().Play("PauseGame");

        quitOverlay.SetActive(true);

        finishedText.SetActive(false);
        loadingText.SetActive(true);

        yield return new WaitForSeconds(2f);

        {
            FindObjectOfType<AudioManager>().Play("Meow");

            yield return new WaitForSeconds(2f);

            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                print("Quit Game");

            }

        }

    }


}