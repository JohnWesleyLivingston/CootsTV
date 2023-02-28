using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Painter2 : MonoBehaviour
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
   // public Transform[] horizontalPortrait;

   // public Transform[] verticalPortrait;

    public Transform[] portraitTransforms;
    public Transform[] faceTransforms;

    public int currentPaintMoveTo;


    public Transform transHolder;
    public GameObject vertTrans;
    public Transform easelZoom;
    public Transform camStart;

    [Header("Hand")]
    public GameObject playerHand;
    public bool slideHorizontal = true;
    public float handSpeed = 1;
    public GameObject handVisual;
    public Animator handSlideIn;

    [Header("Art")]
    public bool waitingForPur1 = true;
    public GameObject[] objectToPaint;
    public GameObject artCanvas;
    public int paintCounter;

    private bool resetToggle = true;
    private bool shiftCam;
    private bool shiftCamOut;

    float timeCount = 0.0f;

    public GameObject cootsBody;
    public GameObject paintingBG;

    public SpriteRenderer paintPreview;

    public Animator bobRossAnim;

    private bool paintToggle;

    [Header("End Living Room")]
    public Transform livingRoomPaintingHolder;
    public GameObject sparkles;
    public GameObject livingRoomFrame;
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


            if (!paintToggle)
            {
                if (paintCounter >= 2)
                {
                    currentPaintMoveTo = Random.Range(0, faceTransforms.Length);
                    paintToggle = true;
                }
                else
                {
                    currentPaintMoveTo = Random.Range(0, 5);
                    paintToggle = true;
                }
            }

            if (paintCounter >= 2)
            {
                playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, faceTransforms[currentPaintMoveTo].transform.position, handSpeed * Time.deltaTime);

                if (playerHand.transform.position == faceTransforms[currentPaintMoveTo].transform.position)
                {
                    paintToggle = false;
                }
            }
            else
            {
                playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, portraitTransforms[currentPaintMoveTo].transform.position, handSpeed * Time.deltaTime);

                if (playerHand.transform.position == portraitTransforms[currentPaintMoveTo].transform.position)
                {
                    paintToggle = false;
                }
            }


        }

        else
        {
            playerHand.transform.position = Vector3.MoveTowards(playerHand.transform.position, portraitTransforms[5].transform.position, handSpeed * 2 * Time.deltaTime);

            gameUI.SetActive(false);

            if (playerHand.transform.position == portraitTransforms[5].transform.position && !resetToggle && !gameComplete)
            {
                StartCoroutine(PlacementCountdown());
                resetToggle = true;
            }
        }

        if (shiftCam)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, easelZoom.transform.position, 3 * Time.deltaTime);

            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, easelZoom.rotation, timeCount * 0.03f);
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
        if (startPainting)
        {
            if (waitingForPur1)
            {
 
                objectToPaint[paintCounter].transform.SetParent(artCanvas.transform);
                objectToPaint[paintCounter].SetActive(true);
                handSlideIn.SetTrigger("Paint");
                FindObjectOfType<AudioManager>().Play("Paintbrush");

                ResetPainter();
            }


        }
    }

    public void ResetPainter()
    {
        paintCounter++;

        if (paintCounter == 5)
        {
            StartCoroutine(EndingSequencer());
            startPainting = false;
            waitingForPur1 = false;
            gameComplete = true;
        }
        else
        {
            startPainting = false;
            slideHorizontal = true;

            paintPreview.gameObject.SetActive(false);
        }

        if (paintCounter == 1)
        {
            subtitles.text = "We're starting with a happy little tree, standing tall and proud.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_2");
            StartCoroutine(DialogueDisable());
        }
        if (paintCounter == 2)
        {
            subtitles.text = "Painting a self portrait is like capturing the essence of who you are.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_3");
            StartCoroutine(DialogueDisable());

        }
        if (paintCounter == 3)
        {
            subtitles.text = "They say the eyes are the window to the soul, and I think that's true.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_4");
            StartCoroutine(DialogueDisable());
        }
        if (paintCounter == 4)
        {
            subtitles.text = "Let’s add some definition to that nose.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_5");
            StartCoroutine(DialogueDisable());

        }
        if (paintCounter == 5)
        {
            subtitles.text = "Now let’s bring this delightful little forest scene all together.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_6");
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
            bobRossAnim.SetTrigger("FaceRevealTalk");
            subtitles.text = "Welcome back friends. Today we're going to paint a lovely forest scene.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_1");
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
            bobRossAnim.SetTrigger("FaceRevealTalk");
            subtitles.text = "And there you have it, folks. A lovely forest with yours truly. It's not perfect, but it's me.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_7");
        }
        yield return new WaitForSeconds(1f);
        {
            shiftCam = false;
            shiftCamOut = true;
            handVisual.SetActive(false);
        }
        yield return new WaitForSeconds(6f);
        {
            subtitles.text = "";
        }
        yield return new WaitForSeconds(1f);
        {
            bobRossAnim.SetTrigger("FaceRevealTalk");
            subtitles.text = "And as always, have fun and let your imagination run wild. Happy painting, my friends.";
            FindObjectOfType<AudioManager>().Play("BobRoss2_8");
        }
        yield return new WaitForSeconds(2f);
        {
            bobRossAnim.SetTrigger("Reveal");
            FindObjectOfType<AudioManager>().Play("Meow");
        }
        yield return new WaitForSeconds(4.5f);
        {
            livingRoomFrame.SetActive(true);
            artCanvas.transform.parent = livingRoomPaintingHolder.transform;
            artCanvas.transform.position = livingRoomPaintingHolder.transform.position;
            artCanvas.transform.rotation = livingRoomPaintingHolder.transform.rotation;

            sparkles.SetActive(false);

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


