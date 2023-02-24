using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Painter : MonoBehaviour
{
    [Header("Setup")]
    private GameRunner gameRunner;
    public GameObject cam;
    public bool startPainting;
    public GameObject paintCanvas;
    public TextMeshProUGUI subtitles;
    public GameObject gameUI;
    public bool gameComplete;

    [Header("Transforms")]
    public Transform[] horizontalPortrait;

    public Transform[] verticalPortrait;

    public Transform transHolder;
    public GameObject vertTrans;
    public Transform easelZoom;
    public Transform camStart;

    [Header("Hand")]
    public GameObject playerHand;
    public bool slideHorizontal = true;
    private bool goRight = true;
    private bool goDown = true;
    public float handSpeed = 1;
    public GameObject handVisual;
    public Animator handSlideIn;

    [Header("Art")]
    public bool waitingForPur1 = true;
    public bool waitingForPur2 = false;
    public GameObject[] objectToPaint;
    public GameObject artCanvas;
    public int paintCounter;
    public int paintCounterGameEndLock;

    private bool resetToggle = true;
    private bool shiftCam;
    private bool shiftCamOut;

    float timeCount = 0.0f;

    public GameObject cootsBody;
    public GameObject paintingBG;

    public SpriteRenderer paintPreview;

    public Animator bobRossAnim;

    void OnEnable()
    {
        VoiceRecognitionManager.OnPurr += Purr;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnPurr -= Purr;
    }


    void Start()
    {
        vertTrans.transform.SetParent(playerHand.transform);
        objectToPaint[paintCounter].SetActive(false);
        gameRunner = FindObjectOfType<GameRunner>();
        StartCoroutine(StartSequencer());
        handVisual.SetActive(false);
        paintCanvas.SetActive(true);
    }

 

    void Update()
    {
        if (startPainting && !gameComplete)
        {
            gameUI.SetActive(true);

            if (slideHorizontal)
            {
                vertTrans.transform.SetParent(playerHand.transform);

                if (playerHand.transform.position == horizontalPortrait[1].transform.position)
                {
                    goRight = false;
                }
                if (playerHand.transform.position == horizontalPortrait[0].transform.position)
                {
                    goRight = true;
                }

                if (goRight)
                {
                    playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, horizontalPortrait[1].transform.position, handSpeed * Time.deltaTime);
                }
                else
                {
                    playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, horizontalPortrait[0].transform.position, handSpeed * Time.deltaTime);
                }
            }
            else
            {
                vertTrans.transform.SetParent(transHolder);


                if (playerHand.transform.position == verticalPortrait[1].transform.position) //Hit Bottom
                {
                    goDown = false; //Go Up
                }
                if (playerHand.transform.position == verticalPortrait[0].transform.position) //Hit Top
                {
                    goDown = true; //Go Down
                }

                if (goDown)
                {
                    playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, verticalPortrait[1].transform.position, handSpeed * Time.deltaTime);
                }
                else
                {
                    playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, verticalPortrait[0].transform.position, handSpeed * Time.deltaTime);
                }
            }
        }

        else
        {
            playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, horizontalPortrait[0].transform.position, handSpeed * 2 * Time.deltaTime);
            
            gameUI.SetActive(false);

            if (playerHand.transform.position == horizontalPortrait[0].transform.position && !resetToggle && !gameComplete)
            {
                StartCoroutine(PlacementCountdown());
                resetToggle = true;
            }
        }

        if (shiftCam)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, easelZoom.transform.position, 2 * Time.deltaTime);

            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, easelZoom.rotation, timeCount * 0.01f);
            timeCount = timeCount + Time.deltaTime;
        }

        if (shiftCamOut)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camStart.transform.position, 2 * Time.deltaTime);

            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camStart.rotation, timeCount * 0.01f);
            timeCount = timeCount + Time.deltaTime;
        }
    }


    void Purr()
    {
        if(startPainting)
        {
            if (waitingForPur2)
            {

                objectToPaint[paintCounter].transform.SetParent(artCanvas.transform);
                objectToPaint[paintCounter].SetActive(true);
                handSlideIn.SetTrigger("Paint");
                FindObjectOfType<AudioManager>().Play("Paintbrush");

                ResetPainter();

            }

            if (waitingForPur1)
            {
                waitingForPur2 = true;
                slideHorizontal = false;
            }


        }
    }

    public void ResetPainter()
    {
        paintCounter++;

        if (paintCounter == 3)
        {
            StartCoroutine(EndingSequencer());
            startPainting = false;
            waitingForPur1 = false;
            waitingForPur2 = false;
            gameComplete = true;
        }
        else
        {
            startPainting = false;
            slideHorizontal = true;
            waitingForPur1 = false;
            waitingForPur2 = false;
            goRight = true;
            goDown = true;
            paintPreview.gameObject.SetActive(false);
        }

        if(paintCounter == 1)
        {
            subtitles.text = "Look at those happy little strokes.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_2");
            StartCoroutine(DialogueDisable());
        }
        if (paintCounter == 2)
        {
            subtitles.text = "Now, let's give our cat some tender little eyes.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_3");
            StartCoroutine(DialogueDisable());

        }
        if (paintCounter == 3)
        {
            subtitles.text = "We're just bringing this kitty to life.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_4");
            StartCoroutine(DialogueDisable());

        }
    }


    private IEnumerator PlacementCountdown()
    {
        vertTrans.transform.position = playerHand.transform.position;
        vertTrans.transform.SetParent(playerHand.transform);

        yield return new WaitForSeconds(1f);
        {

            paintPreview.sprite = objectToPaint[paintCounter].GetComponent<SpriteRenderer>().sprite;
            paintPreview.gameObject.SetActive(true);

            waitingForPur1 = true;
            startPainting = true;
            resetToggle = false;

        }

    }

    private IEnumerator StartSequencer()
    {
        yield return new WaitForSeconds(2f);
        {
            bobRossAnim.SetTrigger("Talk");
            subtitles.text = "Hello there friends. Today we're going to paint a happy little cat.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_1");
        }

        yield return new WaitForSeconds(6f);
        {
            shiftCam = true;
            subtitles.text = "";

        }

        yield return new WaitForSeconds(1f);
        {
            handVisual.SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        {
            StartCoroutine(PlacementCountdown());
        }
    }

    private IEnumerator EndingSequencer()
    {
        handSlideIn.SetTrigger("Exit");
        paintPreview.gameObject.SetActive(false);


        //Ending anim
        yield return new WaitForSeconds(2f);
        {
            cootsBody.SetActive(true);
            paintingBG.SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        {
            bobRossAnim.SetTrigger("Talk");
            subtitles.text = "And there we have it, a happy little kitty.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_5");
        }
        yield return new WaitForSeconds(5f);
        {
            shiftCam = false;
            shiftCamOut = true;
            handVisual.SetActive(false);


        }
        yield return new WaitForSeconds(3f);
        {
            bobRossAnim.SetTrigger("Talk");
            subtitles.text = "I hope you enjoyed painting with us today.";
            FindObjectOfType<AudioManager>().Play("BobRoss1_6");
            bobRossAnim.SetTrigger("Reveal");
        }

        yield return new WaitForSeconds(5f);
        {

            paintCanvas.SetActive(false);

            gameRunner.GameComplete();

        }
    }

    private IEnumerator DialogueDisable()
    {

        yield return new WaitForSeconds(4f);
        {
            subtitles.text = "";
        }



    }

}









