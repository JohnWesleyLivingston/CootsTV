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


    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
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
        StartCoroutine(RestartDelay());
    }

    void Hiss()
    {
        quitGame.Quit();


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