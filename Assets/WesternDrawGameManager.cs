using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class WesternDrawGameManager : MonoBehaviour
{
    private GameRunner gameRunner;
    public PostProcessVolume postProcessVolume;

    public Camera mainCam;
    public Transform[] camPos;
    public Transform[] focalPoints;

    public int currentlySelectedCam;
    public int currentlySelectedFocalPoint;
    private float camSpeed;

    private bool cinemaOn = false;

    public GameObject cinemaBars;

    public TextMeshProUGUI dialogue;

    public Animator banditAnim;

    private bool waitForHiss;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
    }


    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();

        StartCoroutine(IntroSequencer());

        cinemaBars.SetActive(true);
        if (postProcessVolume)
        {
            ColorGrading cg;
            if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out cg))
            {
                cg.saturation.value = -100;
                cg.contrast.value = 25;
            }
        }
    }

    void Update()
    {
        if (cinemaOn)
        {
            mainCam.transform.LookAt(focalPoints[currentlySelectedFocalPoint].transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos[currentlySelectedCam].position, camSpeed * Time.deltaTime);
        }

  
    }

    void Meow()
    {

    }
    void Hiss()
    {
        if(waitForHiss)
        {
            GameComplete();
            waitForHiss = false;

        }
    }
    void Purr()
    {

    }

    void GameComplete()
    {
        if (postProcessVolume)
        {
            ColorGrading cg;
            if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out cg))
            {
                cg.saturation.value = 20;
                cg.contrast.value = 0;
            }
        }
        cinemaBars.SetActive(false);
        gameRunner.GameComplete();

    }

    private IEnumerator IntroSequencer()
    {
        mainCam.transform.position = camPos[7].transform.position;
        mainCam.transform.LookAt(focalPoints[2].transform);

        //cue clouds drifting

        yield return new WaitForSeconds(4f);
        {
            mainCam.transform.position = camPos[9].transform.position;
            mainCam.transform.LookAt(focalPoints[2].transform);

            //cue tumbleweed

            yield return new WaitForSeconds(4f);
            {
                mainCam.transform.position = camPos[0].transform.position;
                mainCam.transform.LookAt(focalPoints[0].transform);
                banditAnim.SetTrigger("Talk");
                dialogue.text = "Listen, stranger. Didn't you get the idea?";

                yield return new WaitForSeconds(6f);
                {
                    mainCam.transform.position = camPos[1].transform.position;
                    mainCam.transform.LookAt(focalPoints[0].transform);
                    banditAnim.SetTrigger("Talk");
                    dialogue.text = "We don't like to see bad boys like you in town.";

                    yield return new WaitForSeconds(6f);
                    {
                        mainCam.transform.position = camPos[2].transform.position;
                        mainCam.transform.LookAt(focalPoints[1].transform);

                        dialogue.text = "hiss.";

                        yield return new WaitForSeconds(6f);
                        {
                            dialogue.text = "";
                            cinemaOn = true;
                            StartCoroutine(StartCamSequencer());

                            float timeToDraw = Random.Range(8, 20);

                            yield return new WaitForSeconds(timeToDraw);
                            {
                                dialogue.text = "DRAW!";
                                StopAllCoroutines();
                                cinemaOn = false;
                                waitForHiss = true;
                                mainCam.transform.position = camPos[2].transform.position;
                                mainCam.transform.LookAt(focalPoints[0].transform);
                                //StartCoroutine(StartCamSequencer());
                                banditAnim.SetTrigger("Shoot");
                            }
                        }
                    }
                }
            }
        }
    }



    private IEnumerator StartCamSequencer()
    {
        float r = Random.Range(2, 8);

        camSpeed = Random.Range(0.1f, 1f);

        currentlySelectedCam = Random.Range(0, camPos.Length);

        currentlySelectedFocalPoint = Random.Range(0, focalPoints.Length);

        //mainCam.fieldOfView = Random.Range(30, 60);

        yield return new WaitForSeconds(r);
        {
            StartCoroutine(StartCamSequencer());
        }
    }



    



    private IEnumerator EndingSequencer()
    {


        yield return new WaitForSeconds(4);
        {

        }
    }

}
