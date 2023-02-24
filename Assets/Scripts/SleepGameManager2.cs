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
    private bool gameComplete;

    public AudioSource lullabySong;
    public GameObject cootsDream;
    public GameObject cootsSwear;

    public bool wakeUp;
    public GameObject carDriveBy;
    private bool isRight;
    public int soundCounter;
    public Transform[] camMarkers;
    public Sprite[] dreams;
    public SpriteRenderer currentDream;
    public Animator cootsSleep;
    public Animator alarmKick;
    private float camSpeed = 5; 


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
                armsSleep.SetActive(false);


                cootsDream.SetActive(false);
                cootsSwear.SetActive(true);

                head.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(Countdown());
            }
        }


        cam.transform.position = Vector3.Lerp(cam.transform.position, camMarkers[soundCounter].transform.position, camSpeed * Time.deltaTime);

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
        if (soundCounter < 4) 
        {
            StartCoroutine(StopYelling());
        }
    }



    private IEnumerator Countdown()         //Win effect 

    {
        print("Finished!");

        cootsSleep.SetTrigger("Awake");
        FindObjectOfType<AudioManager>().Stop("SleepyCat");
        FindObjectOfType<AudioManager>().Play("Hiss");

        yield return new WaitForSeconds(1f);
        {
            cootsSwear.SetActive(false);
            cootsSleep.SetTrigger("Run");
        }
        yield return new WaitForSeconds(1.55f);
        {
            alarmKick.SetTrigger("AlarmKick");
            FindObjectOfType<AudioManager>().Stop("AlarmClock");
        }
        yield return new WaitForSeconds(4f);
        {
            headSleep2.SetActive(true);
            armsSleep.SetActive(true);
            armsDownRight.SetActive(false);
            headAwake.SetActive(false);
            soundCounter = 0;
            lullabySong.Play();
            cootsDream.SetActive(true);
            currentDream.sprite = dreams[5];
            cootsSleep.SetTrigger("Sleep");
            FindObjectOfType<AudioManager>().Play("Purr");

        }
        yield return new WaitForSeconds(1f);
        {
            camSpeed = 0.5f;
            soundCounter = 5;
        }
        yield return new WaitForSeconds(4f);
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

        cootsSleep.SetTrigger("Awake");

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
            cootsSleep.SetTrigger("Sleep");

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
