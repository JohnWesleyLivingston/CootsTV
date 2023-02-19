using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRunner : MonoBehaviour
{
    public bool isFirstGame;
    public bool isLastGame;

    public GameObject thisGame;
    public GameObject nextGame;
    public GameObject transitionScreen;
    public GameObject instructionText;
    public GameObject cam;
    public Transform camPos;

    public float transitionTime = 3.5f;

    void Start()
    {
        cam.transform.position = camPos.transform.position;
        cam.transform.rotation = camPos.transform.rotation;

        if (!isFirstGame)
        {
            thisGame.SetActive(true);
        }
        else
        {
            print("first game");
        }
        if (!isLastGame)
        {
            nextGame.SetActive(false);
        }
        else
        {
            print("end screen");
        }

        transitionScreen.SetActive(false);
        instructionText.SetActive(true);
    }

    public void GameComplete()
    {
        StartCoroutine(Countdown());

    }

    private IEnumerator Countdown()
    {
        transitionScreen.SetActive(true);
        if (isLastGame)
        {
            yield return new WaitForSeconds(transitionTime);
            {
                SceneManager.LoadScene(0);
                PlayerPrefs.SetInt("Completed", 2);
            }

        }
        else
        {
            yield return new WaitForSeconds(transitionTime);

            {

                instructionText.SetActive(false);
                nextGame.SetActive(true);
                transitionScreen.SetActive(false);

                thisGame.SetActive(false);


                yield return null;
            }
        }
    }
}