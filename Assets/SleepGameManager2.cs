using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepGameManager2 : MonoBehaviour
{
    private GameRunner gameRunner;

    public int time = 0;
    public int timeMax = 1500;

    [Header("Coots heads")]
    public GameObject head; //Used for flipping head left/right
    public GameObject headAwake;
    public GameObject headSleep1;
    public GameObject headSleep2;
    public GameObject rightEye;

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

    [Header("Other Settings")]
    //public Animator mouth;

    private bool gameComplete;

    public AudioSource lullabySong;
    public GameObject cootsDream;

    public bool wakeUp;
    public GameObject carDriveBy;
    private bool isRight;
    public int soundCounter;
    public Transform[] camMarkers;
    public Sprite[] dreams;
    public SpriteRenderer currentDream;

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

        headAwake.SetActive(false); //Awake face
        headSleep1.SetActive(false); //Sleep face
        armsRight.SetActive(false);
        armsLeft.SetActive(false);
        armsDownRight.SetActive(false);
        armsDownLeft.SetActive(false);

        armsSleep.SetActive(true);
        head.SetActive(true);

        cootsDream.SetActive(true);
        currentDream.sprite = dreams[soundCounter];

        headSleep2.SetActive(true); //Happy sleep face

        StartCoroutine(CarDriveDelay());

        isRight = true;



        FindObjectOfType<AudioManager>().Play("SleepyCat");
        FindObjectOfType<AudioManager>().Play("AlarmClock");
    }

    void Update()
    {
        if (!gameComplete)
        {
            if (wakeUp)
            {

            }

            /*
            if (time > 100 && time < timeMax)
            {
                //Debug.Log("ZOOM IN");
                headAwake.SetActive(false);
                headSleep1.SetActive(true);
                cam.transform.position = Vector3.Lerp(cam.transform.position, camEnd.transform.position, smooth * Time.deltaTime);

                if (songToggle)
                {
                    cootsSwear.SetActive(false);
                    zoomCam = false;
                    lullabySong.Play();
                    songToggle = false;
                }
            }
            */

            if (soundCounter >= 4) //WIN
            {
                FindObjectOfType<AudioManager>().Play("Purr");

                gameComplete = true;

                headAwake.SetActive(true);
                headSleep1.SetActive(false);
                headSleep2.SetActive(false);
                armsRight.SetActive(false);
                armsLeft.SetActive(false);
                armsDownRight.SetActive(true);
                armsDownLeft.SetActive(false);

                armsSleep.SetActive(true);
                cootsDream.SetActive(true);

                head.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(Countdown());
            }
        }


        cam.transform.position = Vector3.Lerp(cam.transform.position, camMarkers[soundCounter].transform.position, 5 * Time.deltaTime);

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
        soundCounter++;
        StartCoroutine(StopYelling());
    }



    private IEnumerator Countdown()         //Win effect 

    {
        print("FInished!");

        yield return new WaitForSeconds(3f);
        {
            FindObjectOfType<AudioManager>().Stop("AlarmClock");
            FindObjectOfType<AudioManager>().Stop("SleepyCat");

            //Play animation
        }

        yield return new WaitForSeconds(3f);
        {
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



    private IEnumerator StopYelling()      

    {


        FindObjectOfType<AudioManager>().PitchedPlay("CatAngry");

        headSleep2.SetActive(false);
        armsSleep.SetActive(false);

        headSleep1.SetActive(true);
        rightEye.SetActive(true);
        cootsDream.SetActive(false);
        currentDream.sprite = dreams[soundCounter];

        yield return new WaitForSeconds(1f);
        {
            headSleep2.SetActive(true);
            headSleep1.SetActive(false);
            rightEye.SetActive(false);
            cootsDream.SetActive(true);
            FindObjectOfType<AudioManager>().Play("SleepyCat");

            if (!isRight)
            {
                head.transform.localScale = new Vector3(1, 1, 1);
                armsDownRight.SetActive(true);
                armsRight.SetActive(false);
                armsLeft.SetActive(false);
                armsDownLeft.SetActive(false);
            }
            else
            {
                head.transform.localScale = new Vector3(-1, 1, 1);
                armsDownLeft.SetActive(true);
                armsRight.SetActive(false);
                armsLeft.SetActive(false);
                armsDownRight.SetActive(false);
            }
        }
    }

}
