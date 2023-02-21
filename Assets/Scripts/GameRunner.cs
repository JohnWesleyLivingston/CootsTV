using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameRunner : MonoBehaviour
{
    public GameObject transitionScreen;

    public TextMeshProUGUI gameInstructions;
    public GameObject instructionText;

    public GameObject[] gameList;
    public int currentGame = 0;

    public GameObject cam;
    public Transform camPos;

    public float transitionTime = 1;

    public MinigameManager minigameManager;



    void Start()
    {
        transitionScreen.SetActive(false);
        instructionText.SetActive(false);

        for (int i = 0; i < gameList.Length; i++)
        {
            gameList[i].SetActive(i == 0);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameComplete();
        }
    }



    public void GameComplete()
    {
        StartCoroutine(Countdown());
    }

    private void RunNextGame()
    {
        if (currentGame < gameList.Length -1)
        {

            gameList[currentGame].SetActive(false);
            currentGame++;
            gameList[currentGame].SetActive(true);

            minigameManager = gameList[currentGame].GetComponent<MinigameManager>();

            gameInstructions.text = minigameManager.gameInstructions;

            camPos = minigameManager.startingCamPos;
            cam.transform.position = camPos.transform.position;
            cam.transform.rotation = camPos.transform.rotation;
        }
        else
        {
            print("FINISHED");
            gameInstructions.text = "FINISHED";
        }
    }

    private IEnumerator Countdown()
    {
        transitionScreen.SetActive(true);
        RunNextGame();

        yield return new WaitForSeconds(0.6f);

        {

            transitionScreen.SetActive(false);
            instructionText.SetActive(true);

            yield return new WaitForSeconds(1.5f);
            {
                instructionText.SetActive(false);

            }
        }

    }
}