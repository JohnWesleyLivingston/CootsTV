using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VespaGameManager : MonoBehaviour
{
    public bool canJump;

    public GameObject vespa;
    public Animator vespaAnim;
    public Animator vespaSpinOutAnim;

    public Transform leftPos;
    public Transform midPos;
    public Transform rightPos;

    private bool isLeft;
    private bool isMiddle;
    private bool isRight;

    public bool isMoving;

    private bool wasRight;
    private bool wasLeft;

    public GameObject vespaAll;

    public Camera mainCam;
    public Transform camPos;
    public Transform camPosEnding;
    public Transform camPosEndingZoom;

    public float vespaSpeed;
    public float vespaSpeedFast = 5f;
    public float vespaSpeedSlow = 0f;
    public bool vespaAccelerate = false;
    private bool ending;
    private bool endingZoom;

    public GameObject endingArm;
    public GameObject endingArmDisable;
    public GameObject endingHead;

    public GameObject vespaCanvas;


    private GameRunner gameRunner;

    private CapsuleCollider myCollider;
    public bool isJumping = false;
    public AudioSource blastOffMeow;

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
        vespaAccelerate = true;
        StartCoroutine(StartSequencer());
        gameRunner = FindObjectOfType<GameRunner>();
        myCollider = GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        if(isLeft)
        {
            MoveLeft();
        }
        if (isMiddle)
        {
            MoveMiddle();
        }
        if (isRight)
        {
            MoveRight();
        }

        vespaAll.transform.position += Vector3.forward * vespaSpeed * Time.deltaTime;



        if(vespaAccelerate)
        {
            if(vespaSpeed <= vespaSpeedFast)
            {
                vespaSpeed = vespaSpeed + 0.1f;
            }
        }
        else
        {
            if (vespaSpeed >= vespaSpeedSlow)
            {
                vespaSpeed = vespaSpeed - 0.1f;
            }
        }

        //CAMERA
        if(ending)
        {
            if (endingZoom)
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPosEnding.position, 0.2f * Time.deltaTime);
                vespa.transform.position = Vector3.Lerp(vespa.transform.position, midPos.position, 3f * Time.deltaTime);
            }
            else
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPosEndingZoom.position, 0.5f * Time.deltaTime);

            }
            vespaSpeed = 0;

        }
        else
        {
            mainCam.transform.position = camPos.transform.position;
            mainCam.transform.rotation = camPos.transform.rotation;
        }

    }

    void Meow()
    {
        if (!ending)
        {
            isLeft = true;
            isMiddle = false;
            isRight = false;

            isMoving = true;

            wasRight = true;
            wasLeft = false;
        }

    }
    void Hiss()
    {
        if (!ending)
        {
            if (!canJump)
            {
                isLeft = false;
                isMiddle = true;
                isRight = false;

                isMoving = true;
            }
            else
            {
                Jump();
                print("Jump Input");
            }
        }
    }
    void Purr()
    {
        if (!ending)
        {
            isLeft = false;
            isMiddle = false;
            isRight = true;

            isMoving = true;

            wasRight = false;
            wasLeft = true;
        }
    }


    void MoveLeft()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, leftPos.position, 3f * Time.deltaTime);

        if(isMoving)
        {
            vespaAnim.SetBool("Left", true);
        }


    }
    void MoveMiddle()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, midPos.position, 3f * Time.deltaTime);

        if (isMoving && wasRight)
        {
            vespaAnim.SetBool("Right", true);
        }
        if (isMoving && wasLeft)
        {
            vespaAnim.SetBool("Left", true);
        }
    }
    void MoveRight()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, rightPos.position, 3f * Time.deltaTime);

        if (isMoving)
        {
            vespaAnim.SetBool("Right", true);
        }

    }

    void Jump()
    {
        if (!isJumping)
        {
            vespaSpinOutAnim.SetTrigger("Jump");
            myCollider.enabled = false;
            isJumping = true;
            StartCoroutine(JumpDelay());
        }
    }


    private IEnumerator StartSequencer()
    {
        FindObjectOfType<AudioManager>().Play("VespaMotorStart");

        yield return new WaitForSeconds(2.5f);
        {
            FindObjectOfType<AudioManager>().Play("VespaMotor");
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "VespaLeft")
        {
            vespaAnim.SetBool("Left", false);
            isMoving = false;
        }
        if (target.tag == "VespaMid")
        {
            vespaAnim.SetBool("Left", false);
            vespaAnim.SetBool("Right", false);
            isMoving = false;
        }
        if (target.tag == "VespaRight")
        {
            vespaAnim.SetBool("Right", false);
            isMoving = false;
        }

        if (target.tag == "Blocker")
        {
            FindObjectOfType<AudioManager>().Play("HitBarrel");

            print("Spin Out");
            vespaSpinOutAnim.SetTrigger("SpinOut");
            FindObjectOfType<AudioManager>().Play("VespaSpinOut");
            FindObjectOfType<AudioManager>().Stop("VespaMotor");
            StopAllCoroutines();
            StartCoroutine(CrashMotorDelay());

            int r = Random.Range(-20, 20);
            target.gameObject.GetComponent<Rigidbody>().AddForce(r, 25, 10, ForceMode.Impulse);
        }

        if (target.tag == "Ending")
        {
            FindObjectOfType<AudioManager>().Stop("VespaMotor");
            StopAllCoroutines();
            vespaSpinOutAnim.SetTrigger("Ending");
            vespaAccelerate = false;
            StartCoroutine(EndingSequencer());
            FindObjectOfType<AudioManager>().Play("TireSkid");
            vespaCanvas.SetActive(false);
        }
    }


    private IEnumerator CrashMotorDelay()
    {
        vespaAccelerate = false;

        yield return new WaitForSeconds(1f);
        {
            StartCoroutine(StartSequencer());
            vespaAccelerate = true;

        }

    }

    private IEnumerator EndingSequencer()
    {

        yield return new WaitForSeconds(1f);
        {
            endingArm.SetActive(true);
            endingArmDisable.SetActive(false);
            endingHead.SetActive(true);
            FindObjectOfType<AudioManager>().PitchedPlay("Meow");
            vespaSpeed = 0;
            ending = true;

            yield return new WaitForSeconds(5f);
            {
                endingZoom = true;


                if (!canJump)
                {

                    yield return new WaitForSeconds(6f);
                    {

                        gameRunner.GameComplete();
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1.5f);
                    {
                        vespaSpinOutAnim.SetTrigger("BlastOff");
                        FindObjectOfType<AudioManager>().Play("BlastOff");
                        blastOffMeow.Play();

                        yield return new WaitForSeconds(4.5f);
                        {

                            gameRunner.GameComplete();
                        }
                    }
                }
            }
        }
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(1.5f);
        {
            isJumping = false;
            myCollider.enabled = true;
            FindObjectOfType<AudioManager>().Play("TireSkid");
        }

    }
}
