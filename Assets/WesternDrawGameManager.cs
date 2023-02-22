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

    public Transform endingCam;
    public Transform endingFocalPoint;
    public Transform banditDeadFocalPoint;

    public int currentlySelectedCam;
    public int currentlySelectedFocalPoint;
    private float camSpeed;

    private bool cinemaOn = false;

    public GameObject cinemaBars;

    public TextMeshProUGUI dialogue;

    public Animator banditAnim;
    public Animator cootsAnim;

    private bool waitForHiss;
    private bool drawWin;
    private bool drawZoom;
    private bool winZoom;

    public GameObject cootsShootAnim;
    public ParticleSystem introTumbleweed;

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
        if (drawZoom)
        {
            mainCam.transform.LookAt(focalPoints[0].transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos[0].position, 1.5f * Time.deltaTime);
        }
        if (winZoom)
        {
            mainCam.transform.LookAt(focalPoints[1].transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos[2].position, 1.5f * Time.deltaTime);
        }
    }

    void Meow()
    {

    }
    void Hiss()
    {
        if(waitForHiss)
        {
           // StopAllCoroutines();
            StartCoroutine(DrawWin());

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

        yield return new WaitForSeconds(6f);
        {
            mainCam.transform.position = camPos[9].transform.position;
            mainCam.transform.LookAt(focalPoints[3].transform);
            introTumbleweed.Play();

            yield return new WaitForSeconds(6f);
            {
                mainCam.transform.position = camPos[0].transform.position;
                mainCam.transform.LookAt(focalPoints[0].transform);
                banditAnim.SetTrigger("Talk");

                FindObjectOfType<AudioManager>().Play("Western1_1");

                dialogue.text = "Listen, stranger. Didn't you get the idea?";

                yield return new WaitForSeconds(6f);
                {
                    mainCam.transform.position = camPos[1].transform.position;
                    mainCam.transform.LookAt(focalPoints[0].transform);
                    banditAnim.SetTrigger("Talk");

                    FindObjectOfType<AudioManager>().Play("Western1_2");

                    dialogue.text = "We don't like to see bad boys like you in town.";

                    yield return new WaitForSeconds(6f);
                    {
                        mainCam.transform.position = camPos[2].transform.position;
                        mainCam.transform.LookAt(focalPoints[1].transform);
                        dialogue.text = "";

                        yield return new WaitForSeconds(2f);
                        {
                            FindObjectOfType<AudioManager>().Play("Hiss");
                            cootsAnim.SetTrigger("Talk");
                            dialogue.text = "hiss.";
                            
                            yield return new WaitForSeconds(2.5f);
                            {
                                dialogue.text = "";
                                cinemaOn = true;
                                StartCoroutine(StartCamSequencer());

                                float timeToDraw = Random.Range(8, 20);

                                yield return new WaitForSeconds(timeToDraw);
                                {
                                    FindObjectOfType<AudioManager>().Play("Western1_Draw");

                                    dialogue.text = "DRAW!";
                                    dialogue.fontSize = 30;

                                    cinemaOn = false;
                                    waitForHiss = true;
                                    mainCam.transform.position = camPos[2].transform.position;
                                    //mainCam.transform.LookAt(focalPoints[0].transform);
                                    drawZoom = true;
                                    banditAnim.SetTrigger("Shoot");

                                    yield return new WaitForSeconds(8f);
                                    {
                                        if (!drawWin)
                                        {
                                            waitForHiss = false;

                                            StartCoroutine(DrawLose());
                                        }
                                    }
                                }
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



    private IEnumerator DrawWin()
    {
        drawZoom = false;
        waitForHiss = false;
        drawWin = true;
        print("WIN DUEL");

        dialogue.text = "";
        dialogue.fontSize = 16;

        mainCam.transform.position = endingCam.transform.position;
        mainCam.transform.LookAt(endingFocalPoint.transform);

        cootsShootAnim.SetActive(true);
        yield return new WaitForSeconds(.7f);
        {
            FindObjectOfType<AudioManager>().Play("PistolFire");
        }
        yield return new WaitForSeconds(3f);
        {
            mainCam.transform.position = camPos[8].transform.position;
            mainCam.transform.LookAt(focalPoints[0].transform);
            cootsShootAnim.SetActive(false);

            banditAnim.SetTrigger("FuckingDie");

            FindObjectOfType<AudioManager>().Play("Western1_Win");
            dialogue.text = "AAA!";
        }
        yield return new WaitForSeconds(3f);
        {
            mainCam.transform.position = camPos[0].transform.position;
            mainCam.transform.LookAt(banditDeadFocalPoint.transform);

            FindObjectOfType<AudioManager>().Play("Western1_Win2");
            dialogue.text = "You win this time stranger...";
        }
        yield return new WaitForSeconds(5f);
        {
            mainCam.transform.position = camPos[8].transform.position;
            //mainCam.transform.LookAt(focalPoints[1].transform);
            winZoom = true;
            FindObjectOfType<AudioManager>().Play("Whip");
            FindObjectOfType<AudioManager>().Play("Purr");
            cootsAnim.SetTrigger("Talk");

            dialogue.text = "purr.";
        }
        yield return new WaitForSeconds(3f);
        {
            GameComplete();
        }
    }

    private IEnumerator DrawLose()
    {
        drawZoom = false;
        dialogue.text = "";
        dialogue.fontSize = 16;
        //Draw failed
        FindObjectOfType<AudioManager>().Play("PistolFire");

        yield return new WaitForSeconds(3f);
        {
            mainCam.transform.position = camPos[8].transform.position;
            mainCam.transform.LookAt(focalPoints[0].transform);
            cootsShootAnim.SetActive(false);

            banditAnim.SetTrigger("FuckingDie");

            FindObjectOfType<AudioManager>().Play("Western1_Win");
            dialogue.text = "AAA!";
        }

        yield return new WaitForSeconds(1f);
        {
            cinemaOn = true;
            StartCoroutine(StartCamSequencer());
        }


    }




}
