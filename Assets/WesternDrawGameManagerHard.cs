using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
public class WesternDrawGameManagerHard : MonoBehaviour
{
    private GameRunner gameRunner;
    public PostProcessVolume postProcessVolume;

    public Camera mainCam;
    public Transform[] camPos;
    public Transform[] focalPoints;
    public float drawTimeLimit = 6f;

    public Transform gunCam;
    public Transform gunFocalPoint;
    public GameObject cootsShootAnim;

    public int currentlySelectedCam;
    public int currentlySelectedFocalPoint;
    public float camSpeed = 1;

    private bool cinemaOn = false;

    public GameObject cinemaBars;
    public GameObject shootUI;

    public TextMeshProUGUI dialogue;

    public Animator banditAnim;
    public Animator cootsAnim;
    public Animator doorSwing;


    private bool waitForHiss;

    private bool falseWord;

    private bool drawWin;
    private bool drawZoom;
    private bool winZoom;
    private bool doorOpen;

    public GameObject cowboyBoot;
    public Transform bootCam;
    public Transform bootFocal;

    public GameObject bandit;
    public GameObject banditSillouette;

    public GameObject banditHide;
    public Transform banditHideCam;
    public Transform banditHideFocal;

    public Transform endingCam;
    public Transform endingFocalPoint;

    public Transform banditDeadCam;
    public Transform banditDeadFocalPoint;
    public GameObject graveStone;

    public int wordToSay;
    public int timeToDraw;
    private int drawIteration;

    void OnEnable()
    {
        VoiceRecognitionManager.OnHiss += Hiss;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnHiss -= Hiss;
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

        if(doorOpen)
        {
            mainCam.transform.LookAt(focalPoints[0].transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos[5].position, 1f * Time.deltaTime);
        }

        if (drawZoom)
        {
            mainCam.transform.LookAt(focalPoints[1].transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos[6].position, 1.5f * Time.deltaTime);
        }

        if (winZoom)
        {
            mainCam.transform.LookAt(endingFocalPoint.transform);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, endingCam.position, 1f * Time.deltaTime);
        }
    }


    void Hiss()
    {
        if (waitForHiss)
        {
            if (!falseWord)
            {
                StopAllCoroutines();
                StartCoroutine(DrawWin());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FalseDraw());
            }
        }
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
        mainCam.transform.position = camPos[4].transform.position;
        mainCam.transform.LookAt(focalPoints[0].transform);

        yield return new WaitForSeconds(3f);
        {
            FindObjectOfType<AudioManager>().Play("SaloonDoorSwing");
            doorSwing.SetTrigger("Open");
        }

        yield return new WaitForSeconds(1f);
        {
            doorOpen = true;
        }

        yield return new WaitForSeconds(5f);
        {

            mainCam.transform.position = bootCam.transform.position;
            mainCam.transform.LookAt(bootFocal.transform);

            doorOpen = false;
            cowboyBoot.SetActive(true);


            FindObjectOfType<AudioManager>().Play("BootClink");
        }

        yield return new WaitForSeconds(2f);
        {
            FindObjectOfType<AudioManager>().Play("Western2_1");
            dialogue.text = "Well well well, look what the cat dragged in.";     
        }

        yield return new WaitForSeconds(4f);
        {
            cowboyBoot.SetActive(false);
            bandit.SetActive(true);
            banditSillouette.SetActive(false);
            mainCam.transform.position = camPos[6].transform.position;
            mainCam.transform.LookAt(focalPoints[1].transform);
            dialogue.text = "We have a score to settle…";
            banditAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("Western2_2");
        }

        yield return new WaitForSeconds(4f);
        {
            mainCam.transform.position = camPos[5].transform.position;
            mainCam.transform.LookAt(focalPoints[0].transform);
            FindObjectOfType<AudioManager>().Play("Meow");
            cootsAnim.SetTrigger("Talk");
            dialogue.text = "meow.";
        }

        yield return new WaitForSeconds(2f);
        {
            dialogue.text = "Get ready...";
            cinemaOn = true;
            StartCoroutine(StartCamSequencer());
            float timeToDraw = Random.Range(8, 20);

            yield return new WaitForSeconds(timeToDraw);
            {
                FindObjectOfType<AudioManager>().Play("Western2_Paw");
                shootUI.SetActive(true);

                dialogue.text = "PAW!";
                dialogue.fontSize = 30;

                cinemaOn = false;

                waitForHiss = true;
                falseWord = true;

                mainCam.transform.position = camPos[8].transform.position;
                drawZoom = true;

                banditAnim.SetTrigger("Shoot");

                yield return new WaitForSeconds(6f);
                {
                    waitForHiss = false;
                    StartCoroutine(DrawResetWait());
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

        yield return new WaitForSeconds(r);
        {
            StartCoroutine(StartCamSequencer());
        }
    }



    private IEnumerator FalseDraw()
    {
        drawZoom = false;
        waitForHiss = false;
        falseWord = false;

        print("WIN DUEL");
        shootUI.SetActive(false);

        dialogue.text = "";
        dialogue.fontSize = 16;

        mainCam.transform.position = gunCam.transform.position;
        mainCam.transform.LookAt(gunFocalPoint.transform);

        cootsShootAnim.SetActive(true);

        yield return new WaitForSeconds(.7f);
        {
            drawWin = false;

            FindObjectOfType<AudioManager>().Play("PistolFire");
        }
        yield return new WaitForSeconds(1.8f);
        {
            mainCam.transform.position = banditHideCam.transform.position;
            mainCam.transform.LookAt(banditHideFocal.transform);

            cootsShootAnim.SetActive(false);
            bandit.SetActive(false);
            banditHide.SetActive(true);


            FindObjectOfType<AudioManager>().Play("Western2_FalseDraw");
            dialogue.text = "AAA! Not fair, I didn't say draw!";
        }

        yield return new WaitForSeconds(4f);
        {
            mainCam.transform.position = camPos[4].transform.position;
            mainCam.transform.LookAt(focalPoints[0].transform);

            FindObjectOfType<AudioManager>().Play("Hiss");
            cootsAnim.SetTrigger("Talk");

            dialogue.text = "hiss.";
        }

        yield return new WaitForSeconds(2f);
        {
            bandit.SetActive(true);
            banditHide.SetActive(false);

            mainCam.transform.position = camPos[7].transform.position;
            mainCam.transform.LookAt(focalPoints[1].transform);

            banditAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("Western1_Lose2");
            dialogue.text = "Let's try that one more time.";
        }

        yield return new WaitForSeconds(4f);
        {
            StartCoroutine(DrawRandomizer());
        }
    }

    private IEnumerator DrawResetWait()
    {
        dialogue.text = "";
        dialogue.fontSize = 16;
        shootUI.SetActive(false);
        drawZoom = false;

        waitForHiss = false;
        falseWord = false;

        mainCam.transform.position = camPos[6].transform.position;
        mainCam.transform.LookAt(focalPoints[1].transform);

        banditAnim.SetTrigger("Talk");
        FindObjectOfType<AudioManager>().Play("Western2_Wait");
        dialogue.text = "How clever of you…";

        yield return new WaitForSeconds(4f);
        {
            StartCoroutine(DrawRandomizer());
        }
    }

    private IEnumerator DrawRandomizer()
    {
        drawIteration++;
        cinemaOn = true;
        StartCoroutine(StartCamSequencer());
        dialogue.text = "Get ready...";
        drawZoom = false;
        waitForHiss = false;
        falseWord = false;

        timeToDraw = Random.Range(4, 12);

        if(drawIteration >= 2)
        {
            wordToSay = 0;
        }
        else
        {
            wordToSay = Random.Range(0, 4);
        }


        if (wordToSay == 0) // REAL WORD
        {

            yield return new WaitForSeconds(timeToDraw);
            {
                FindObjectOfType<AudioManager>().Play("Western1_Draw");
                shootUI.SetActive(true);
                banditAnim.SetTrigger("Shoot");
                dialogue.text = "DRAW!";
                dialogue.fontSize = 30;

                cinemaOn = false;
                waitForHiss = true;
                falseWord = false;

                mainCam.transform.position = camPos[8].transform.position;
                drawZoom = true;

                yield return new WaitForSeconds(drawTimeLimit);
                {
                    if (!drawWin)
                    {
                        waitForHiss = false;
                        StartCoroutine(DrawLose());
                    }
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(timeToDraw);
            {
                dialogue.fontSize = 30;

                if (wordToSay == 1) // FAKE WORD
                {
                    dialogue.text = "PAW!";
                    FindObjectOfType<AudioManager>().Play("Western2_Paw");
                }
                if (wordToSay == 2) // FAKE WORD
                {
                    dialogue.text = "SHOOT ME!";
                    FindObjectOfType<AudioManager>().Play("Western2_ShootMe");
                }
                if (wordToSay == 3) // FAKE WORD
                {
                    dialogue.text = "Woof!";
                    FindObjectOfType<AudioManager>().Play("Western2_Woof");
                }

                shootUI.SetActive(true);
                cinemaOn = false;
                waitForHiss = true;
                falseWord = true;

                mainCam.transform.position = camPos[8].transform.position;
                drawZoom = true;
                
                banditAnim.SetTrigger("Shoot");

                yield return new WaitForSeconds(drawTimeLimit);
                {
                    if (!drawWin)
                    {
                        waitForHiss = false;
                        StartCoroutine(DrawResetWait());
                    }
                }
            }
        }


    }


    private IEnumerator DrawWin()
    {
        drawZoom = false;
        waitForHiss = false;
        drawWin = true;
        print("WIN DUEL");
        shootUI.SetActive(false);

        dialogue.text = "";
        dialogue.fontSize = 16;

        mainCam.transform.position = gunCam.transform.position;
        mainCam.transform.LookAt(gunFocalPoint.transform);

        cootsShootAnim.SetActive(true);

        yield return new WaitForSeconds(.7f);
        {
            FindObjectOfType<AudioManager>().Play("PistolFire");
        }
        yield return new WaitForSeconds(1.8f);
        {
            mainCam.transform.position = camPos[8].transform.position;
            mainCam.transform.LookAt(focalPoints[1].transform);
            cootsShootAnim.SetActive(false);

            banditAnim.SetTrigger("FuckingDie");
            doorSwing.SetTrigger("Destroy");

            FindObjectOfType<AudioManager>().Play("Western1_Win");
            dialogue.text = "AAA!";
        }
        yield return new WaitForSeconds(2f);
        {
            mainCam.transform.position = banditDeadCam.transform.position;
            mainCam.transform.LookAt(banditDeadFocalPoint.transform);

            FindObjectOfType<AudioManager>().Play("Western1_Win2");
            dialogue.text = "You have bested me stranger...";
            bandit.SetActive(false);
            graveStone.SetActive(true); 
        }
        yield return new WaitForSeconds(4f);
        {
            mainCam.transform.position = camPos[8].transform.position;

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
        FindObjectOfType<AudioManager>().Play("PistolFire");
        cootsAnim.SetTrigger("Lose");
        mainCam.transform.position = camPos[8].transform.position;
        mainCam.transform.LookAt(focalPoints[0].transform);
        shootUI.SetActive(false);

        waitForHiss = false;
        falseWord = false;

        yield return new WaitForSeconds(1f);
        {
            mainCam.transform.position = camPos[6].transform.position;
            mainCam.transform.LookAt(focalPoints[1].transform);

            banditAnim.SetTrigger("Talk");

            FindObjectOfType<AudioManager>().Play("Western2_Lose");
            dialogue.text = "Hahahaha! Might've had a little too much to drink...";
        }

        yield return new WaitForSeconds(4f);
        {
            mainCam.transform.position = camPos[7].transform.position;
            mainCam.transform.LookAt(focalPoints[1].transform);

            banditAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("Western1_Lose2");
            dialogue.text = "Let's try that one more time.";
        }

        yield return new WaitForSeconds(4f);
        {
            StartCoroutine(DrawRandomizer());
        }


    }

}
