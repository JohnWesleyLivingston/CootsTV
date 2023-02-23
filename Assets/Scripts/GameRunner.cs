using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

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

    public PostProcessVolume postProcessTVVolume;

    private void Awake()
    {
        for (int i = 0; i < gameList.Length; i++)
        {
            gameList[i].SetActive(i == 0);
        }
    }

    void Start()
    {
        transitionScreen.SetActive(false);
        instructionText.SetActive(false);

        ResetPostProcess();

        cam.transform.position = camPos.transform.position;
        cam.transform.rotation = camPos.transform.rotation;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameComplete();
        }
    }

    public void ResetPostProcess()
    {

        if (postProcessTVVolume)
        {
            ColorGrading cg;
            if (postProcessTVVolume.sharedProfile.TryGetSettings<ColorGrading>(out cg))
            {
                cg.saturation.value = 20;
                cg.contrast.value = 0;
            }
        }
    }

    public void GameComplete()
    {
        ResetPostProcess();

        StartCoroutine(Countdown());
    }

    private void RunNextGame()
    {
        if (currentGame < gameList.Length - 1)
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

            yield return new WaitForSeconds(2f);
            {
                instructionText.SetActive(false);

            }
        }

    }



}