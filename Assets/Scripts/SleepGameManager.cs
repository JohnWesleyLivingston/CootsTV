using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepGameManager : MonoBehaviour
{
    private GameRunner gameRunner;

    public int time = 0;
    public int timeMax = 1500;

    [Header("Coots heads")]    
    public GameObject head; //Used for flipping head left/right
    public GameObject headAwake;
    public GameObject headSleep1;
    public GameObject headSleep2;

    [Header("Coots Limbs")]
    public GameObject armsRight;
    public GameObject armsLeft;
    public GameObject armsDownRight;
    public GameObject armsDownLeft;
    public GameObject armsSleep;



    [Header("Camera Settings")]
    public Transform camStart;
    public Transform camEnd;
    public GameObject cam;
    public float smooth = 0.1f;
    private bool zoomCam;

    [Header("Other Settings")]
    //public Animator mouth;

    private bool gameComplete;
    //public GameObject sleepBubble;

    public AudioSource lullabySong;
    public GameObject cootsSwear;
    public GameObject cootsDream;

    public bool wakeUp;
    public GameObject carDriveBy;
    private bool songToggle;
    private bool isRight;
    public Animator cootsSleep;
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

        headAwake.SetActive(true); //Awake face
        headSleep1.SetActive(false); //Sleep face
        headSleep2.SetActive(false); //Happy sleep face

        armsRight.SetActive(false);
        armsLeft.SetActive(false);
        armsDownRight.SetActive(true);
        armsDownLeft.SetActive(false);

        head.SetActive(true);
        armsSleep.SetActive(false);

        StartCoroutine(CarDriveDelay());
        zoomCam = false;
        songToggle = true;
        cootsSwear.SetActive(false);
        cootsDream.SetActive(false);
        //     mouth.SetBool("Awake", false);
        isRight = true;

        FindObjectOfType<AudioManager>().Play("SleepyCat");
    }

    private void Update()
    {
        if (zoomCam)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camStart.transform.position, 20 * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (!gameComplete)
        {
            if (wakeUp)
            {
                time = 0;
                //Debug.Log("ZOOM OUT");
                FindObjectOfType<AudioManager>().PitchedPlay("CatAngry");
                FindObjectOfType<AudioManager>().Stop("SleepyCat");
                cootsSleep.SetTrigger("Awake");

                cootsSwear.SetActive(true);
                headAwake.SetActive(true);
                headSleep1.SetActive(false);
                zoomCam = true;
                songToggle = true;
                wakeUp = false;

            }
            else
            {
                time = time + 1;
                //        mouth.SetBool("Awake", false);

            }

            if (time > 75)
            {
                //sleepBubble.SetActive(false);
            }

            // TIME STATES
            if (time > 100 && time < timeMax)
            {
                //Debug.Log("ZOOM IN");
                headAwake.SetActive(false);
                headSleep1.SetActive(true);
                cam.transform.position = Vector3.Lerp(cam.transform.position, camEnd.transform.position, smooth * Time.deltaTime);

                if(songToggle)
                {
                    FindObjectOfType<AudioManager>().Play("SleepyCat");
                    cootsSleep.SetTrigger("Sleep");

                    cootsSwear.SetActive(false);
                    zoomCam = false;
                    lullabySong.Play();
                    songToggle = false;
                }
            }


            if (time == timeMax) //WIN
            {
                FindObjectOfType<AudioManager>().Play("Purr");

                gameComplete = true;

                headSleep1.SetActive(false);
                headSleep2.SetActive(true);
                armsRight.SetActive(false);
                armsLeft.SetActive(false);
                armsDownRight.SetActive(false);
                armsDownLeft.SetActive(false);

                armsSleep.SetActive(true);
                cootsDream.SetActive(true);

                head.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(Countdown());
            }
        }
    }

    void Meow()
    {
        if (!gameComplete)
        {
            print("LEFT");
            armsRight.SetActive(false);
            armsLeft.SetActive(true);
            armsDownRight.SetActive(false);
            armsDownLeft.SetActive(false);

            head.transform.localScale = new Vector3(-1, 1, 1);
            isRight = false;
            WakeUp();
        }
    }
    void Hiss()
    {
        if (!gameComplete)
        {
            print("UP");
            armsRight.SetActive(false);
            armsLeft.SetActive(false);
            armsDownRight.SetActive(false);
            armsDownLeft.SetActive(false);

            if (isRight)
            {
                armsDownRight.SetActive(true);
            }
            else
            {
                armsDownLeft.SetActive(true);
            }

            WakeUp();
        }
    }
    void Purr()
    {
        if (!gameComplete)
        {
            print("RIGHT");
            armsRight.SetActive(true);
            armsLeft.SetActive(false);
            armsDownRight.SetActive(false);
            armsDownLeft.SetActive(false);
            head.transform.localScale = new Vector3(1, 1, 1);
            isRight = true;
            WakeUp();
        }
    }


    void WakeUp()
    {
        wakeUp = true;
        lullabySong.Stop();

    }



    private IEnumerator Countdown()         //Win effect 

    {

        lullabySong.Stop();

        yield return new WaitForSeconds(3f);
        {
            FindObjectOfType<AudioManager>().Stop("SleepyCat");

            gameRunner.GameComplete();
        }
    }


    private IEnumerator CarDriveDelay()         //Win effect 

    {
        yield return new WaitForSeconds(1f);
        {
            carDriveBy.SetActive(true);
        }
        
        yield return new WaitForSeconds(6f);
        {
            carDriveBy.SetActive(false);
        }

        yield return new WaitForSeconds(6f);
        {
            StartCoroutine(CarDriveDelay());
        }
    }

}
